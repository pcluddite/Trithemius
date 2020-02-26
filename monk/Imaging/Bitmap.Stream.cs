// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System;
using System.Collections.Generic;
using System.IO;

using Monk.Memory;

namespace Monk.Imaging
{
    public partial class Bitmap
    {
        public ByteStream GetStream()
        {
            return GetStream(0);
        }

        public ByteStream GetStream(int pixelOffset)
        {
            if (pixelOffset < 0 || pixelOffset >= Size) throw new ArgumentOutOfRangeException(nameof(pixelOffset));
            return new BitmapStream(this, pixelOffset);
        }

        private sealed class BitmapStream : ByteStream
        {
            public Bitmap Bitmap { get; }
            public override int IntLength => Bitmap.buffer.Length;

            public BitmapStream(Bitmap bitmap, int pixelIndex)
            {
                Bitmap = bitmap;
                IntPosition = bitmap.PixelOffsetToByteOffset(pixelIndex);
            }

            public override byte Peek()
            {
                if (IntPosition >= IntLength) throw new EndOfStreamException();
                return Bitmap.buffer[IntPosition];
            }

            public override byte ReadNext()
            {
                if (IntPosition >= IntLength) throw new EndOfStreamException();
                return Bitmap.buffer[IntPosition++];
            }

            public override int ReadByte()
            {
                if (IntPosition >= IntLength) return -1;
                return Bitmap.buffer[IntPosition++];
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if ((uint)Position >= (uint)IntPosition) return -1;
                if ((uint)offset >= (uint)buffer.Length) throw new ArgumentOutOfRangeException(nameof(offset));
                count = Math.Min(count, Bitmap.buffer.Length - IntPosition);
                Array.Copy(Bitmap.buffer, IntPosition, buffer, offset, count);
                IntPosition += count;
                return count;

            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                if ((uint)(count + IntPosition) > (uint)IntLength) throw new EndOfStreamException();
                if ((uint)offset >= (uint)buffer.Length) throw new ArgumentOutOfRangeException(nameof(offset));
                if ((uint)(count + offset) >= (uint)buffer.Length) throw new ArgumentOutOfRangeException(nameof(count));
                Array.Copy(buffer, offset, Bitmap.buffer, IntPosition, count);
                IntPosition += count;
            }
        }
    }
}