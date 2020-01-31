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
using System.Text;

namespace Monk.Bittwiddling
{
    /// <summary>
    /// A list of binary values 1 or 0, this class is designed to be slightly more flexible than BitArray
    /// </summary>
    public class BinaryList : IList<bool>
    {
        private const int CHAR_BIT = Twiddler.CHAR_BIT;
        private const int ELEMENT_BITS = sizeof(int) * CHAR_BIT;
        private const int DEFAULT_CAPACITY = ELEMENT_BITS;

        private int[] array;

        /// <summary>
        /// This determines the order the bits are read from a byte. The default is technically platform specific
        /// but is universally LittleEndian in practice
        /// </summary>
        public EndianMode Endianness { get; } = Twiddler.ImplementationEndianness;

        public bool this[int index]
        {
            get {
                if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException(nameof(index));
                return Get(index);
            }
            set {
                if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException(nameof(index));
                Set(index, value);
            }
        }

        public int Count { get; private set; }
        public int Capacity => array.Length * ELEMENT_BITS;
        public int ByteCount => MathUtil.DivideUp(Count, CHAR_BIT);

        public BinaryList()
        {
            array = new int[0];
            Count = 0;
        }

        public BinaryList(int capacity)
        {
            array = new int[ArrayLength(capacity)];
            Count = 0;
        }

        public BinaryList(int capacity, EndianMode endianness)
            : this(capacity)
        {
            Endianness = endianness;
        }

        public BinaryList(byte[] data)
        {
            AddRange(data);
        }

        public BinaryList(byte[] data, EndianMode endianness)
            : this(data)
        {
            Endianness = endianness;
        }

        public BinaryList(IEnumerable<byte> data)
            : this(DEFAULT_CAPACITY)
        {
            AddRange(data);
        }

        public BinaryList(byte[] data, int capacity)
            : this(capacity)
        {
            AddRange(data);
        }

        public BinaryList(byte[] data, int capacity, EndianMode endianness)
            : this(capacity, endianness)
        {
            AddRange(data);
        }

        public void Add(bool item)
        {
            EnsureCapacity();
            Set(Count++, item);
        }

        public void AddRange(BinaryList binaryList)
        {
            EnsureCapacity(binaryList.Count);
            int destBits = Count;
            int sourceBits = binaryList.Count;

            if (destBits % ELEMENT_BITS == 0) {
                int len = binaryList.ArrayLength(sourceBits);
                Array.Copy(binaryList.array, 0, array, Count, len);
                Count += sourceBits;
            }
            else if (destBits % CHAR_BIT == 0) {
                int sourceBytes = sourceBits / CHAR_BIT;
                if (sourceBits % CHAR_BIT != 0) {
                    ++sourceBytes;
                }
                unsafe {
                    fixed (int* lpwSrc = binaryList.array, lpwDst = array) {
                        AppendBytes(lpwDst, destBits / CHAR_BIT, lpwSrc, sourceBytes);
                    }
                }
                Count += sourceBits;
            }
            else {
                // not aligned, can't use fancy tricks
                for (int srcIndex = 0; srcIndex < binaryList.Count; ++srcIndex) {
                    Set(Count++, binaryList.Get(srcIndex));
                }
            }
            
        }

        public void AddRange(IEnumerable<bool> bits)
        {
            foreach (bool bit in bits) {
                Add(bit);
            }
        }

        public void AddRange(IEnumerable<byte> data)
        {
            foreach(byte b in data) {
                AddRange(b);
            }
        }

        public void AddRange(bool[] bits)
        {
            EnsureCapacity(bits.Length);
            for (int i = 0; i < bits.Length; ++i) {
                Set(Count++, bits[i]);
            }
        }

        public void AddRange(byte[] data)
        {
            EnsureCapacity(data.Length * CHAR_BIT);
            int offset = IndexAtBit(Count);
            int len = data.Length;
            if (Count % CHAR_BIT == 0) {
                unsafe {
                    fixed (byte* lpSrc = data)
                    fixed (int* lpIntArray = array) {
                        byte* lpDst = (byte*)&lpIntArray[offset];
                        if (Endianness == Twiddler.ImplementationEndianness) {
                            AppendBytes(lpDst, lpSrc, len);
                        }
                        else {
                            for (int idx = 0; idx < len; ++idx) {
                                lpDst[idx] = Twiddler.FlipBits(lpSrc[idx]);
                            }
                        }
                    }
                }
                Count += len;
            }
            else {
                for (int byteIdx = 0; byteIdx < len; ++byteIdx) {
                    for (int bitIdx = 0; bitIdx < CHAR_BIT; ++bitIdx) {
                        Set(Count++, Twiddler.GetBit(data[byteIdx], bitIdx, Endianness));
                    }
                }
            }
        }

        public void AddRange(byte b)
        {
            EnsureCapacity(CHAR_BIT);
            for (int i = 0; i < CHAR_BIT; ++i) {
                Set(Count++, Twiddler.GetBit(b, i, Endianness));
            }
        }

        public void Clear()
        {
            array = new int[0];
            Count = 0;
        }

        public bool Contains(bool item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(bool[] array, int arrayIndex)
        {
            for(int idx = 0; arrayIndex < Count; ++arrayIndex, ++idx) {
                array[arrayIndex] = this[idx];
            }
        }

        public IEnumerator<bool> GetEnumerator()
        {
            for(int idx = 0; idx < Count; ++idx) {
                yield return Get(idx);
            }
        }

        public int IndexOf(bool item)
        {
            for (int idx = 0; idx < Count; ++idx) {
                if (Get(idx) == item) {
                    return idx;
                }
            }
            return -1;
        }

        /// <summary>
        /// Converts each set of eight bits into a byte using the implementation dependent bit layout
        /// </summary>
        public IEnumerable<byte> ToBytes()
        {
            return CreateByteArray();
        }

        public IEnumerable<byte> ToBytes(EndianMode endianness)
        {
            foreach(byte b in ToBytes()) {
                if (endianness == Twiddler.ImplementationEndianness) {
                    yield return b;
                }
                else {
                    yield return Twiddler.FlipBits(b);
                }
            }
        }

        /// <summary>
        /// Sets each bit to the opposite of its current value
        /// </summary>
        public unsafe void Not()
        {
            if (array == null || array.Length == 0) return;
            int len = ByteCount;

            fixed(int* lpArr = array) {
                for(int i = 0; i < len; ++i) {
                    lpArr[i] = ~lpArr[i];
                }
            }
        }

        /// <summary>
        /// Determines if the bits can be evenly turned into bytes (octets), without remaining bits
        /// </summary>
        /// <returns></returns>
        public bool IsValidBytes()
        {
            return Count % CHAR_BIT == 0;
        }

        public void TrimExcess()
        {
            Array.Resize(ref array, ArrayLength(Count));
        }

        public override string ToString()
        {
            int len = Count;
            StringBuilder sb = new StringBuilder(len + ByteCount); // add byte count for spaces
            for (int idx = 0; idx < len; ++idx) {
                if (idx > 0 && idx % CHAR_BIT == 0) { // add space every 8 characters
                    sb.Append(' ');
                }
                sb.Append(Get(idx) ? '1' : '0');
            }
            return sb.ToString();
        }

        private unsafe byte[] CreateByteArray()
        {
            byte[] byteArray = new byte[ByteCount];
            fixed (int* lpIntArray = array) {
                Marshal.Copy(new IntPtr(lpIntArray), byteArray, 0, byteArray.Length);
            }
            return byteArray;
        }

        private void EnsureCapacity()
        {
            EnsureCapacity(1);
        }

        private void EnsureCapacity(int bitsNeeded)
        {
            int capacity = Capacity;
            if (capacity == 0) {
                int len = ArrayLength(Math.Max(bitsNeeded, DEFAULT_CAPACITY));
                array = new int[len];
            }
            else if (capacity < Count + bitsNeeded) {
                int count = Count;
                while (capacity < count + bitsNeeded)
                    capacity = checked(capacity * 2);
                Array.Resize(ref array, ArrayLength(capacity));
            }
        }

        private int ArrayLength(int bitCount)
        {
            return MathUtil.DivideUp(bitCount, ELEMENT_BITS);
        }

        private int IndexAtBit(int bitIndex)
        {
            return bitIndex / ELEMENT_BITS;
        }

        private static unsafe void AppendBytes(byte* lpDst, byte* lpSrc, int nLen)
        {
            for (int nIdx = 0; nIdx < nLen; ++nIdx) {
                *lpDst++ = lpSrc[nIdx];
            }
        }

        private static unsafe void AppendBytes(int* lpwDst, int nDstByteOffset, int* lpwSrc, int nSrcBytes)
        {
            byte* lpDst = ((byte*)lpwDst) + nDstByteOffset;
            byte* lpSrc = (byte*)lpwSrc;
            AppendBytes(lpDst, lpSrc, nSrcBytes);
        }

        private void Set(int index, bool value)
        {
            if (value) {
                array[IndexAtBit(index)] |= 1 << (index % ELEMENT_BITS);
            }
            else {
                array[IndexAtBit(index)] &= ~(1 << (index % ELEMENT_BITS));
            }
        }

        private bool Get(int index)
        {
            return (array[IndexAtBit(index)] & (1 << (index % ELEMENT_BITS))) != 0;
        }

        void IList<bool>.Insert(int index, bool item)
        {
            throw new InvalidOperationException();
        }

        bool ICollection<bool>.Remove(bool item)
        {
            throw new InvalidOperationException();
        }

        void IList<bool>.RemoveAt(int index)
        {
            throw new InvalidOperationException();
        }

        bool ICollection<bool>.IsReadOnly => false;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
