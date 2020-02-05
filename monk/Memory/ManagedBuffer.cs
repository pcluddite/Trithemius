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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Monk.Memory
{
    internal class ManagedBuffer<T> : IList<T> where T : struct
    {
        private T[] buffer;

        public int Length => buffer.Length;
        public Span<T> Span => new Span<T>(buffer);
        public Span<byte> ByteSpan => MemoryMarshal.AsBytes(Span);

        int ICollection<T>.Count { get; }
        bool ICollection<T>.IsReadOnly { get; }

        public T this[int index]
        {
            get => buffer[index];
            set => buffer[index] = value;
        }

        public ManagedBuffer(int length)
        {
            buffer = new T[length];
        }

        public Span<T> GetSpan(int length)
        {
            return GetSpan(0, length);
        }

        public Span<T> GetSpan(int offset, int length)
        {
            return new Span<T>(buffer, offset, length);
        }

        public Span<byte> GetByteSpan(int length)
        {
            return GetByteSpan(0, length);
        }

        public Span<byte> GetByteSpan(int offset, int length)
        {
            return ByteSpan.Slice(offset, length);
        }

        public void Resize(int newLength)
        {
            Array.Resize(ref buffer, newLength);
        }

        public void Clear()
        {
            Array.Clear(buffer, 0, Length);
        }

        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(buffer, 0, array, arrayIndex, buffer.Length);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int idx = 0; idx < Length; ++idx) {
                yield return buffer[idx];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return Array.IndexOf(buffer, item);
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }
    }
}
