// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
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
            return new LockedBitmapStream(this, pixelOffset);
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

            public override unsafe int Read(byte[] buffer, int offset, int count)
            {
                if ((uint)Position >= (uint)IntPosition) return -1;
                if ((uint)offset >= (uint)buffer.Length) throw new ArgumentOutOfRangeException(nameof(offset));
                count = Math.Min(count, RawData.Length - IntPosition);
                byte* src = RawData.UnsafePtrAt(IntPosition);
                fixed(byte* dest = &buffer[offset]) {
                    for (int i = 0; i < count; ++i) {
                        dest[i] = src[i];
                    }
                }
                IntPosition += count;
                return count;

            }

            public override unsafe void Write(byte[] buffer, int offset, int count)
            {
                if ((uint)(count + IntPosition) > (uint)IntLength) throw new EndOfStreamException();
                if ((uint)offset >= (uint)buffer.Length) throw new ArgumentOutOfRangeException(nameof(offset));
                if ((uint)(count + offset) >= (uint)buffer.Length) throw new ArgumentOutOfRangeException(nameof(count));
                byte* src = RawData.UnsafePtrAt(IntPosition);
                fixed (byte* dest = &buffer[offset]) {
                    for (int i = 0; i < count; ++i) {
                        src[i] = dest[i];
                    }
                }
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