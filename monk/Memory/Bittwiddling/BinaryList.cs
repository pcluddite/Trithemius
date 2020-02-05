﻿/**
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

namespace Monk.Memory.Bittwiddling
{
    /// <summary>
    /// A list of binary values 1 or 0, this class is designed to be slightly more flexible than BitArray
    /// </summary>
    public class BinaryList : IList<bool>
    {
        private const int CHAR_BIT = Twiddler.CHAR_BIT;
        private const int ELEMENT_BITS = sizeof(int) * CHAR_BIT;
        private const int DEFAULT_CAPACITY = ELEMENT_BITS;

        private Buffer<int> buffer;

        /// <summary>
        /// This determines the order the bits are read from a byte. The default is technically platform specific
        /// but is universally LittleEndian in practice
        /// </summary>
        public EndianMode Endianness { get; }

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

        public int Count { get; private set; } = 0;
        public int Capacity => buffer.Length * ELEMENT_BITS;
        public int ByteCount => MathUtil.DivideUp(Count, CHAR_BIT);

        public BinaryList()
            : this(Twiddler.ImplementationEndianness)
        {
        }

        public BinaryList(int capacity)
            : this(capacity, Twiddler.ImplementationEndianness)
        {
        }

        public BinaryList(byte[] data)
            : this(data, Twiddler.ImplementationEndianness)
        {
        }

        public BinaryList(IEnumerable<byte> data)
            : this(data, Twiddler.ImplementationEndianness)
        {
        }

        public BinaryList(byte[] data, int capacity)
            : this(data, capacity, Twiddler.ImplementationEndianness)
        {
        }

        public BinaryList(EndianMode endianness)
        {
            buffer = new Buffer<int>(0);
            Endianness = endianness;
        }

        public BinaryList(int capacity, EndianMode endianness)
        {
            buffer = new Buffer<int>(ArrayLength(capacity));
            Endianness = endianness;
        }

        public BinaryList(byte[] data, EndianMode endianness)
            : this(data.Length * CHAR_BIT, endianness)
        {
            AddRange(data);
        }

        public BinaryList(IEnumerable<byte> data, EndianMode endianness)
            : this(DEFAULT_CAPACITY, endianness)
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
            if (Count % ELEMENT_BITS == 0) {
                Span<int> source = binaryList.buffer.Span;
                Span<int> dest = buffer.GetSpan(Count / ELEMENT_BITS, source.Length);
                source.CopyTo(dest);
                Count += binaryList.Count;
            }
            else if (Count % CHAR_BIT == 0) {
                Span<byte> source = binaryList.buffer.ByteSpan;
                Span<byte> dest = buffer.GetByteSpan(Count / CHAR_BIT, source.Length);
                source.CopyTo(dest);
                Count += binaryList.Count;
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
            foreach (byte b in data) {
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
            int len = data.Length;
            if (Count % CHAR_BIT == 0) {
                int byteOffset = Count / CHAR_BIT;
                Span<byte> source = new Span<byte>(data);
                Span<byte> dest = buffer.GetByteSpan(byteOffset, len);
                source.CopyTo(dest);
                if (Endianness != Twiddler.ImplementationEndianness) {
                    for (int idx = 0; idx < len; ++idx) {
                        dest[idx] = Twiddler.FlipBits(dest[idx]);
                    }
                }
                Count += len * CHAR_BIT;
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
            buffer = new Buffer<int>(0);
            Count = 0;
        }

        public bool Contains(bool item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(bool[] array, int arrayIndex)
        {
            for (int idx = 0; arrayIndex < Count; ++arrayIndex, ++idx) {
                array[arrayIndex] = this[idx];
            }
        }

        public IEnumerator<bool> GetEnumerator()
        {
            for (int idx = 0; idx < Count; ++idx) {
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
            if (endianness == Twiddler.ImplementationEndianness) {
                foreach (byte b in ToBytes()) {
                    yield return b;
                }
            }
            else {
                foreach (byte b in ToBytes()) {
                    yield return Twiddler.FlipBits(b);
                }
            }
        }

        /// <summary>
        /// Sets each bit to the opposite of its current value
        /// </summary>
        public void Not()
        {
            if (buffer == null || buffer.Length == 0) return;
            Span<int> span = buffer.Span;
            for (int i = 0, len = span.Length; i < len; ++i) {
                span[i] = ~span[i];
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
            buffer.Resize(ArrayLength(Count));
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

        private byte[] CreateByteArray()
        {
            return buffer.GetByteSpan(ByteCount).ToArray();
        }

        private void EnsureCapacity()
        {
            EnsureCapacity(1);
        }

        private void EnsureCapacity(int bitsNeeded)
        {
            int capacity = Capacity;
            if (capacity == 0) {
                buffer.Resize(ArrayLength(Math.Max(bitsNeeded, DEFAULT_CAPACITY)));
            }
            else if (capacity < Count + bitsNeeded) {
                int count = Count;
                while (capacity < count + bitsNeeded)
                    capacity = checked(capacity * 2);
                buffer.Resize(ArrayLength(capacity));
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

        private void Set(int index, bool value)
        {
            if (value) {
                buffer[IndexAtBit(index)] |= 1 << index % ELEMENT_BITS;
            }
            else {
                buffer[IndexAtBit(index)] &= ~(1 << index % ELEMENT_BITS);
            }
        }

        private bool Get(int index)
        {
            return (buffer[IndexAtBit(index)] & 1 << index % ELEMENT_BITS) != 0;
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