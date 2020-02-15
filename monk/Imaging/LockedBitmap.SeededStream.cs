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
using System.Collections.Generic;
using System.IO;

using Monk.Memory;

namespace Monk.Imaging
{
    public partial class LockedBitmap
    {
        public ByteStream GetStream(int pixelIndex, IEnumerable<ushort> seed)
        {
            return GetStream(pixelIndex, seed, SupportedColors);
        }

        public ByteStream GetStream(int pixelIndex, ISet<PixelColor> colors)
        {
            return GetStream(pixelIndex, new ushort[] { 0 }, colors);
        }

        public ByteStream GetStream(int pixelIndex, IEnumerable<ushort> seed, ISet<PixelColor> colors)
        {
            return GetStream(pixelIndex, Size - pixelIndex, seed, colors);
        }

        public ByteStream GetStream(int pixelIndex, int pixelCount, IEnumerable<ushort> seed, ISet<PixelColor> colors)
        {
            if (pixelIndex < 0 || pixelIndex >= Size) throw new ArgumentOutOfRangeException(nameof(pixelIndex));
            if (!SupportedColors.IsSupersetOf(colors)) ThrowHelper.ColorUnsupported(nameof(colors), colors, SupportedColors);
            if (colors.Count == 0) throw new ArgumentException("you must specify at least one color", nameof(colors));
            if (pixelCount < 1 || pixelIndex + pixelCount > Size) throw new ArgumentOutOfRangeException(nameof(pixelCount));
            return new SeededBitmapStream(this, seed, colors, pixelIndex, pixelCount);
        }

        private sealed class SeededBitmapStream : ByteStream
        {
            private int[] indices;

            public LockedBitmap Bitmap { get; }

            public override int IntLength => indices.Length;

            public SeededBitmapStream(LockedBitmap bitmap, IEnumerable<ushort> seed, IEnumerable<PixelColor> colors, int pixelIndex, int pixelCount)
            {
                Bitmap = bitmap;
                bitmap.LockBits();
                CachePixels(bitmap, seed, colors, pixelIndex, pixelCount);
            }

            private void CachePixels(LockedBitmap bitmap, IEnumerable<ushort> seed, IEnumerable<PixelColor> colors, int pixelIndex, int pixelCount)
            {
                int imageArea = bitmap.Size;

                ISet<PixelColor> pixelColors = new SortedSet<PixelColor>(colors);
                indices = new int[Math.Min(pixelCount, imageArea) * pixelColors.Count];

                ArithmeticProgression pixelIndices = new ArithmeticProgression(pixelIndex, seed);

                for (int buffIdx = 0; pixelIndex < imageArea && buffIdx < indices.Length; pixelIndex = pixelIndices.Next()) {
                    foreach (PixelColor color in pixelColors) {
                        indices[buffIdx++] = bitmap.GetBufferIndex(pixelIndex, color);
                        if (buffIdx >= indices.Length) break;
                    }
                }
            }

            public override byte Peek()
            {
                if (IntPosition >= IntLength) throw new EndOfStreamException();
                return Bitmap.GetByteAt(indices[IntPosition]);
            }

            public override int ReadByte()
            {
                if (IntPosition >= IntLength) return -1;
                return Bitmap.GetByteAt(indices[IntPosition++]);
            }

            public override byte ReadNext()
            {
                if (IntPosition >= IntLength) throw new EndOfStreamException();
                return Bitmap.GetByteAt(indices[IntPosition++]);
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (IntPosition >= IntLength) return -1;
                count = Math.Min(count, IntLength - IntPosition);
                for (int i = 0; i < count; ++i) {
                    buffer[i] = Bitmap.GetByteAt(indices[IntPosition++]);
                }
                return count;
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            public override void WriteByte(byte value)
            {
                if (IntPosition >= IntLength) throw new EndOfStreamException();
                Bitmap.SetByteAt(indices[IntPosition++], value);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                if (count + IntPosition > IntLength) throw new EndOfStreamException();
                count = Math.Min(count, IntLength - IntPosition);
                for (int i = 0; i < count; ++i) {
                    Bitmap.SetByteAt(indices[IntPosition++], buffer[i]);
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
}
