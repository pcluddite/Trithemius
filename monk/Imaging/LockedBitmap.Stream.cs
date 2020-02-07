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
using System;
using System.IO;

using Monk.Memory;

namespace Monk.Imaging
{
    public partial class LockedBitmap
    {
        public ByteStream GetStream()
        {
            return GetStream(0);
        }

        public ByteStream GetStream(int pixelOffset)
        {
            if (pixelOffset < 0 || pixelOffset >= Size) throw new ArgumentOutOfRangeException(nameof(pixelOffset));
            return GetStream(pixelOffset, Seed.DefaultSeed, SupportedColors);
        }

        private sealed class LockedBitmapStream : ByteStream
        {
            public UnmanagedBuffer RawData => LockedBitmap.RawData;
            public LockedBitmap LockedBitmap { get; }
            public override int IntLength => LockedBitmap.RawData.Length;

            public LockedBitmapStream(LockedBitmap bitmap, int pixelIndex)
            {
                LockedBitmap = bitmap;
                bitmap.LockBits();
                IntPosition = bitmap.PixelOffsetToByteOffset(pixelIndex);
            }

            public override byte Peek()
            {
                if (IntPosition >= IntLength) throw new EndOfStreamException();
                return RawData[IntPosition];
            }

            public override byte ReadNext()
            {
                if (IntPosition >= IntLength) throw new EndOfStreamException();
                return RawData[IntPosition++];
            }

            public override int ReadByte()
            {
                if (IntPosition >= IntLength) return -1;
                return RawData[IntPosition++];
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (Position >= IntPosition) return -1;
                count = Math.Min(count, RawData.Length - IntPosition);
                ReadOnlySpan<byte> src = RawData.ReadOnlySlice(IntPosition, count);
                Span<byte> dest = new Span<byte>(buffer, offset, count);
                src.CopyTo(dest);
                IntPosition += count;
                return count;

            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                if (count + IntPosition > IntLength) throw new EndOfStreamException();
                Span<byte> dest = RawData.Slice(IntPosition, count);
                Span<byte> src = new Span<byte>(buffer, offset, count);
                src.CopyTo(dest);
                IntPosition += count;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing) {
                    LockedBitmap.UnlockBits();
                }
                base.Dispose(disposing);
            }
        }
    }
}