// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System;
using System.IO;

namespace Monk.Memory
{
    public abstract class ByteStream : Stream
    {
        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => true;

        public int IntPosition { get; set; } = 0;

        public override long Position { get => IntPosition; set => IntPosition = checked((int)value); }

        public virtual int IntLength => throw new NotSupportedException();
        public override long Length => IntLength;

        public abstract byte ReadNext();
        public abstract byte Peek();

        public override void Flush()
        {
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin) {
                Position = offset;
            } else if (origin == SeekOrigin.Current) {
                Position += offset;
            } else if (origin == SeekOrigin.End) {
                Position = Length + offset;
            }

            return Position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
    }
}
