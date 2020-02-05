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
    /// <summary>
    /// Semi-safe handle to an unmanaged memory block. This is extremely dangerous and should be used with caution.
    /// </summary>
    internal unsafe class UnmanagedBuffer : IDisposable, IList<byte>
    {
        private void* lpBlock;

        public bool Owned { get; }

        public int Length { get; }
        public IntPtr Pointer => new IntPtr(lpBlock);
        public bool Freed => lpBlock == null;
        public Span<byte> Span => new Span<byte>(lpBlock, Length);
        public ReadOnlySpan<byte> ReadOnlySpan => new ReadOnlySpan<byte>(lpBlock, Length);

        public byte this[int index]
        {
            get => Span[index];
            set => Span[index] = value;
        }

        public UnmanagedBuffer(int length)
        {
            if (length < 1) throw new ArgumentOutOfRangeException(nameof(length));
            Owned = true;
            Length = length;
            lpBlock = Marshal.AllocHGlobal(Length).ToPointer();
        }

        public UnmanagedBuffer(IntPtr ptr, int length)
            : this((byte*)ptr.ToPointer(), length)
        {
        }

        public UnmanagedBuffer(byte* ptr, int length)
        {
            Length = length;
            Owned = false;
            lpBlock = ptr;
        }

        public T Read<T>(int offset) where T : struct
        {
            return MemoryMarshal.Read<T>(ReadOnlySlice(offset, Marshal.SizeOf<T>()));
        }

        public ReadOnlySpan<T> CastAs<T>() where T : struct
        {
            return MemoryMarshal.Cast<byte, T>(ReadOnlySpan);
        }

        public void Write<T>(int index, T value) where T : struct
        {
            MemoryMarshal.Write(Slice(index), ref value);
        }

        public Span<byte> Slice(int start)
        {
            return Slice(start, Length - start);
        }

        public Span<byte> Slice(int start, int length)
        {
            return new Span<byte>((byte*)lpBlock + start, length);
        }

        public ReadOnlySpan<byte> ReadOnlySlice(int start)
        {
            return ReadOnlySlice(start, Length - start);
        }

        public ReadOnlySpan<byte> ReadOnlySlice(int start, int length)
        {
            return new ReadOnlySpan<byte>((byte*)lpBlock + start, length);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) {
                if (Owned && !Freed) {
                    Marshal.FreeHGlobal(Pointer);
                    lpBlock = null;
                }
            }
        }

        int ICollection<byte>.Count => Length;
        bool ICollection<byte>.IsReadOnly => false;

        public int IndexOf(byte item)
        {
            byte* lpPtr = (byte*)lpBlock;
            for (int idx = 0; idx < Length; ++idx) {
                if (lpPtr[idx] == item) return idx;
            }
            return -1;
        }

        public bool Contains(byte item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(byte[] array, int arrayIndex)
        {
            Span<byte> dest = new Span<byte>(array, arrayIndex, array.Length - arrayIndex);
            ReadOnlySpan.CopyTo(dest);
        }

        public void Clear()
        {
            Span.Fill(0);
        }

        public IEnumerator<byte> GetEnumerator()
        {
            IntPtr ptr = Pointer;
            for(int idx = 0; idx < Length; ++idx) {
                yield return Marshal.ReadByte(ptr, idx);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void IList<byte>.Insert(int index, byte item)
        {
            throw new NotSupportedException();
        }

        void IList<byte>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void ICollection<byte>.Add(byte item)
        {
            throw new NotSupportedException();
        }

        bool ICollection<byte>.Remove(byte item)
        {
            throw new NotSupportedException();
        }
    }
}
