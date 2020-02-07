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

namespace Monk.Memory
{
    public abstract class ByteStream : Stream
    {
        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => true;

        private int position = 0;

        public int IntPosition
        {
            get => position;
            set {
                if (value < 0 || value >= IntLength) throw new ArgumentOutOfRangeException();
                position = value;
            }
        }

        public override long Position
        {
            get => IntPosition;
            set => IntPosition = checked((int)value);

        }

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
    }
}
