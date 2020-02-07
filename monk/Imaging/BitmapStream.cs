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
using Monk.Memory;
using System;
using System.Collections.Generic;
using System.IO;

namespace Monk.Imaging
{
    internal class BitmapStream : Stream
    {
        private int[] indices;

        public LockedBitmap Bitmap { get; }

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => true;

        private int position = 0;

        public int IntPosition {
            get => position;
            set {
                if (value < 0 || value >= IntLength) throw new IndexOutOfRangeException();
                position = value;
            }
        }

        public override long Position {
            get => IntPosition;
            set => IntPosition = checked((int)value);
            
        }

        public int IntLength => indices.Length;
        public override long Length => IntLength;

        public BitmapStream(LockedBitmap bitmap, Seed seed, int startPixel, int sizeInBytes)
            : this(bitmap, seed, bitmap.SuportedColors, startPixel, sizeInBytes)
        {
        }

        public BitmapStream(LockedBitmap bitmap, Seed seed, PixelColor color, int startPixel, int sizeInBytes)
            : this(bitmap, seed, new PixelColor[] { color }, startPixel, sizeInBytes)
        {
        }

        public BitmapStream(LockedBitmap bitmap, Seed seed, IEnumerable<PixelColor> colors, int startPixel, int sizeInBytes)
        {
            Bitmap = bitmap;
            bitmap.LockBits();
            CachePixels(bitmap, seed, colors, startPixel, sizeInBytes);
        }

        private void CachePixels(LockedBitmap bitmap, Seed seed, IEnumerable<PixelColor> colors, int startPixel, int length)
        {
            int w = bitmap.Width, h = bitmap.Height;
            int imageArea = w * h;
            int position = 0;
            ISet<PixelColor> pixelColors = new HashSet<PixelColor>(colors);

            indices = new int[Math.Min(length, imageArea * pixelColors.Count)];
            
            if (seed.Count == 0) seed = Seed.DefaultSeed;
            ArithmeticProgression pixelIndices = new ArithmeticProgression(startPixel, seed);

            for (int pixelIndex = pixelIndices.Start; pixelIndex < imageArea && position < length; pixelIndex = pixelIndices.Next()) {
                foreach (PixelColor color in pixelColors) {
                    indices[position++] = bitmap.GetBufferIndex(pixelIndex, color);
                    if (position >= indices.Length) break;
                }
            }
        }

        public override void Flush()
        {
        }

        public byte Peek()
        {
            if (position >= IntLength) throw new EndOfStreamException();
            return Bitmap.GetByteAt(indices[position]);
        }

        public override int ReadByte()
        {
            if (position >= IntLength) return -1;
            return Bitmap.GetByteAt(indices[position++]);
        }

        public byte ReadNext()
        {
            if (position >= IntLength) throw new EndOfStreamException();
            return Bitmap.GetByteAt(indices[position++]);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (position >= IntLength) return -1;
            count = Math.Min(count, IntLength - position);
            for (int i = 0; i < count; ++i) {
                buffer[i] = Bitmap.GetByteAt(indices[position++]);
            }
            return count;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin) {
                Position = offset;
            }
            else if (origin == SeekOrigin.Current) {
                Position += offset;
            }
            else if (origin == SeekOrigin.End) {
                Position = Length + offset;
            }
            return Position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void WriteByte(byte value)
        {
            if (position >= IntLength) throw new EndOfStreamException();
            Bitmap.SetByteAt(indices[position++], value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (count + position > IntLength) throw new EndOfStreamException();
            count = Math.Min(count, IntLength - position);
            for (int i = 0; i < count; ++i) {
                Bitmap.SetByteAt(indices[position++], buffer[i]);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Bitmap.Locked) {
                Flush();
                Bitmap.UnlockBits();
                indices = null;
            }
            base.Dispose(disposing);
        }
    }
}
