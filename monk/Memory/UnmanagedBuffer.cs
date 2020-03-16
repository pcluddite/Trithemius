// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Monk.Memory
{
    /// <summary>
    /// Semi-safe handle to an unmanaged memory block. This is extremely dangerous and should be used with caution.
    /// </summary>
    internal unsafe class UnmanagedBuffer : IDisposable, IList<byte>
    {
        private byte* _lpBlock;

        public bool Owned { get; }

        public int Length { get; }
        public IntPtr Pointer => new IntPtr(_lpBlock);
        public bool Freed => _lpBlock == null;

        public byte this[int index]
        {
            get {
                if ((uint)index >= (uint)Length) throw new ArgumentOutOfRangeException(nameof(index));
                return _lpBlock[index];
            }
            set {
                if ((uint)index >= (uint)Length) throw new ArgumentOutOfRangeException(nameof(index));
                _lpBlock[index] = value;
            }
        }

        public UnmanagedBuffer(int length)
        {
            if (length < 1) throw new ArgumentOutOfRangeException(nameof(length));
            Owned = true;
            Length = length;
            _lpBlock = (byte*)Marshal.AllocHGlobal(Length).ToPointer();
        }

        public UnmanagedBuffer(IntPtr ptr, int length)
            : this((byte*)ptr.ToPointer(), length)
        {
        }

        public UnmanagedBuffer(byte* ptr, int length)
        {
            Length = length;
            Owned = false;
            _lpBlock = ptr;
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
                    _lpBlock = null;
                }
            }
        }

        int ICollection<byte>.Count => Length;
        bool ICollection<byte>.IsReadOnly => false;

        public int IndexOf(byte item)
        {
            byte* lpPtr = _lpBlock;
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
            if ((uint)arrayIndex >= (uint)array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if ((uint)(array.Length - arrayIndex) >= (uint)Length) throw new ArgumentException(nameof(array));
            fixed (byte* lpArr = array) {
                for (int i = 0, len = Length; i < len; ++i) {
                    lpArr[arrayIndex + i] = lpArr[i];
                }
            }
        }

        public void Clear()
        {
            for(int i = 0, len = Length; i < len; ++i) {
                _lpBlock[i] = 0;
            }
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

        public Stream GetStream()
        {
            return new UnmanagedMemoryStream(_lpBlock, Length);
        }

        public Stream GetStream(int offset)
        {
            var stream = GetStream();
            stream.Position = offset;
            return stream;
        }

        public byte* UnsafePtrAt(int index)
        {
            return &_lpBlock[index];
        }
    }
}
