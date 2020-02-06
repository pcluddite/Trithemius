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
using System.Text;

namespace Monk.Memory
{
    public abstract class SeededStream : Stream
    {
        protected abstract Stream BaseStream { get; }
        public virtual Seed Seed { get; }

        public override bool CanSeek => true;

        public override bool CanRead => BaseStream.CanRead;
        public override bool CanWrite => BaseStream.CanWrite;

        private long? len;

        public override long Length => len ?? (len = SequencedSize(Seed, BaseStream.Length)).Value;

        public override void Flush()
        {
            BaseStream?.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public static long SequencedSize(Seed seed, long totalSize)
        {
            long size = 0;
            long pos = 0;
            for(; pos < totalSize; pos += seed[(int)(size % seed.Count)] + 1) {
                ++size;
            }
            return size;
        }
    }
}
