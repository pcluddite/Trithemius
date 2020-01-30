/**
 *  Monk
 *  Copyright (C) Timothy Baxendale
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License along
 *  with this program; if not, write to the Free Software Foundation, Inc.,
 *  51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
**/
using Monk.Bittwiddling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Monk.Imaging
{
    internal class BitmapStream : Stream
    {
        private IntPtr[] cachedPtrs;
        private byte[] cachedData;

        public LockedBitmap Bitmap { get; }
        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => true;

        private int streamPos = 0;

        public int IntPosition {
            get => streamPos;
            set {
                if (value < 0 || value >= IntLength) throw new IndexOutOfRangeException();
                streamPos = value;
            }
        }

        public override long Position {
            get => IntPosition;
            set {
                checked {
                    IntPosition = (int)value;
                }
            }
        }

        public int IntLength => cachedData.Length;
        public override long Length => IntLength;

        public BitmapStream(LockedBitmap bitmap, Seed seed, int dataLength)
            : this(bitmap, seed, bitmap.SuportedColors, dataLength)
        {
        }

        public BitmapStream(LockedBitmap bitmap, Seed seed, PixelColor color, int dataLength)
            : this(bitmap, seed, new PixelColor[] { color }, dataLength)
        {
        }

        public BitmapStream(LockedBitmap bitmap, Seed seed, IEnumerable<PixelColor> colors, int sizeInBytes)
        {
            Bitmap = bitmap;
            bitmap.LockBits();
            CachePixels(bitmap, seed, colors, sizeInBytes);
        }

        private void CachePixels(LockedBitmap bitmap, Seed seed, IEnumerable<PixelColor> colors, int length)
        {
            int w = bitmap.Width, h = bitmap.Height;
            int imageArea = w * h;
            int bitIndex = 0;
            int bits = length * BinaryOctet.OCTET;
            cachedData = new byte[Math.Min(bits, imageArea * colors.Count())];
            cachedPtrs = new IntPtr[cachedData.Length];
            for (int pixelIndex = seed[0]; pixelIndex < imageArea && bitIndex < bits; pixelIndex += seed[bitIndex % seed.Count] + 1) {
                int x = pixelIndex % w;
                int y = (pixelIndex - x) / w;
                foreach (PixelColor color in colors) {
                    cachedPtrs[bitIndex++] = bitmap.GetPointerToColor(x, y, color);
                }
            }

            unsafe {
                int len = cachedData.Length;
                fixed (byte* lpBytes = cachedData)
                fixed (IntPtr* lpPtrs = cachedPtrs) {
                    for (int idx = 0; idx < len; ++idx) {
                        lpBytes[idx] = *(byte*)lpPtrs[idx];
                    }
                }
            }
        }

        public unsafe override void Flush()
        {
            int len = cachedData.Length;
            fixed (byte* lpBytes = cachedData)
            fixed (IntPtr* lpPtrs = cachedPtrs) {
                for (int idx = 0; idx < len; ++idx) {
                    *(byte*)lpPtrs[idx] = lpBytes[idx];
                }
            }
        }

        public byte Peek()
        {
            if (streamPos >= IntLength) throw new EndOfStreamException();
            return cachedData[streamPos];
        }

        public override int ReadByte()
        {
            if (streamPos >= IntLength) return -1;
            return cachedData[streamPos++];
        }

        public byte ReadNext()
        {
            if (streamPos >= IntLength) throw new EndOfStreamException();
            return cachedData[streamPos++];
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (streamPos >= IntLength) return -1;
            count = Math.Min(count, IntLength - streamPos);
            Array.Copy(cachedData, streamPos, buffer, offset, count);
            streamPos += count;
            return count;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            int iOffset = checked((int)offset);
            if (origin == SeekOrigin.Begin) {
                IntPosition = iOffset;
            }
            else if (origin == SeekOrigin.Current) {
                IntPosition += iOffset;
            }
            else if (origin == SeekOrigin.End) {
                IntPosition = checked(IntLength + iOffset);
            }
            return IntPosition;
        }

        public override void SetLength(long value)
        {
            throw new InvalidOperationException();
        }

        public override void WriteByte(byte value)
        {
            if (streamPos >= IntLength) throw new EndOfStreamException();
            cachedData[streamPos++] = value;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (count + streamPos > IntLength) throw new EndOfStreamException();
            count = Math.Min(count, IntLength - streamPos);
            Array.Copy(buffer, offset, cachedData, streamPos, count);
            streamPos += count;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Bitmap.Locked) {
                Flush();
                Bitmap.UnlockBits();
                cachedData = null;
                cachedPtrs = null;
            }
            base.Dispose(disposing);
        }
    }
}
