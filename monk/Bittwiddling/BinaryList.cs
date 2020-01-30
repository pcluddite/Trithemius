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
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System;

namespace Monk.Bittwiddling
{
    /// <summary>
    /// A list of binary values 1 or 0, this class is more flexible than BitArray
    /// </summary>
    public class BinaryList : IList<bool>
    {
        private const int DEFAULT_CAPACITY = sizeof(int) * BinaryOctet.OCTET;
        private readonly BitArray bits;

        public bool this[int index]
        {
            get => bits[index];
            set => bits[index] = value;
        }

        public int Count { get; private set; }

        public int ByteCount => Count / BinaryOctet.OCTET + (IsValidBytes() ? 0 : 1);

        public BinaryList()
        {
            bits = new BitArray(0);
            Count = 0;
        }

        public BinaryList(int capacity)
        {
            bits = new BitArray(capacity);
            Count = 0;
        }

        public BinaryList(byte[] data)
        {
            bits = new BitArray(data);
            Count = bits.Length;
        }

        public BinaryList(IEnumerable<byte> data)
        {
            bits = new BitArray(DEFAULT_CAPACITY);
            AddRange(data);
        }

        public BinaryList(byte[] data, int capacity)
            : this(capacity)
        {
            AddRange(data);
        }

        public void Add(bool item)
        {
            EnsureCapacity();
            bits[Count++] = item;
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

        public void AddRange(IEnumerable<BinaryOctet> data)
        {
            foreach (BinaryOctet b in data) {
                AddRange(b);
            }
        }

        public void AddRange(bool[] bits)
        {
            EnsureCapacity(bits.Length);
            for (int i = 0; i < bits.Length; ++i) {
                this.bits[Count++] = bits[i];
            }
        }

        public void AddRange(byte[] data)
        {
            EnsureCapacity(data.Length * BinaryOctet.OCTET);
            for(int i = 0; i < data.Length; ++i) {
                BinaryOctet octet = data[i];
                for(int j = 0; j < BinaryOctet.OCTET; ++j) {
                    bits[Count++] = octet.GetBit(i);
                }
            }
        }

        public void AddRange(BinaryOctet[] data)
        {
            EnsureCapacity(data.Length * BinaryOctet.OCTET);
            for (int i = 0; i < data.Length; ++i) {
                for (int j = 0; j < BinaryOctet.OCTET; ++j) {
                    bits[Count++] = data[i].GetBit(j);
                }
            }
        }

        public void AddRange(byte b)
        {
            AddRange((BinaryOctet)b);
        }

        public void AddRange(BinaryOctet octet)
        {
            EnsureCapacity(BinaryOctet.OCTET);
            for (int i = 0; i < BinaryOctet.OCTET; ++i) {
                bits[Count++] = octet[i];
            }
        }

        public void Clear()
        {
            bits.Length = 0;
        }

        public bool Contains(bool item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(bool[] array, int arrayIndex)
        {
            bits.CopyTo(array, arrayIndex);
        }

        public IEnumerator<bool> GetEnumerator()
        {
            for(int idx = 0; idx < Count; ++idx) {
                yield return bits[idx];
            }
        }

        public int IndexOf(bool item)
        {
            for (int idx = 0; idx < Count; ++idx) {
                if (bits[idx] == item) {
                    return idx;
                }
            }
            return -1;
        }

        /// <summary>
        /// Converts each set of eight bits into a bytes
        /// </summary>
        /// <param name="invert">whether or not to invert bytes</param>
        /// <returns></returns>
        public IEnumerable<byte> ToBytes(EndianMode endianness)
        {
            int idx = 0;
            for(; idx < Count; ++idx) {
                BinaryOctet curr = new BinaryOctet();
                if (endianness == EndianMode.BigEndian) {
                    int n = 0;
                    do {
                        curr = curr.SetBit(n, bits[idx]);
                    } while (++n < BinaryOctet.OCTET && ++idx < Count);
                }
                else {
                    int n = BinaryOctet.OCTET - 1;
                    do {
                        curr = curr.SetBit(n, bits[idx]);
                    } while (--n >= 0 && ++idx < Count);
                }
                yield return curr;
            }
        }

        /// <summary>
        /// Sets each bit to the opposite of its current value
        /// </summary>
        public void Invert()
        {
            bits.Not();
        }

        /// <summary>
        /// Determines if the bits can be evenly turned into bytes (octets), without remaining bits
        /// </summary>
        /// <returns></returns>
        public bool IsValidBytes()
        {
            return Count % 8 == 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(bits.Count);
            int idx = 0;
            foreach (BinaryOctet octet in ToBytes(EndianMode.BigEndian)) {
                foreach(bool bit in octet) {
                    sb.Append(bit ? '1' : '0');
                    if (++idx == Count) {
                        break;
                    }
                }
                sb.Append(' ');

            }
            return sb.ToString();
        }

        private void EnsureCapacity()
        {
            EnsureCapacity(1);
        }

        private void EnsureCapacity(int bitsNeeded)
        {
            if (bits.Count == 0) {
                bits.Length = Math.Max(bitsNeeded, DEFAULT_CAPACITY);
            }
            else {
                int size = bits.Count;
                while (size <= Count + bitsNeeded)
                    size *= 2;
                bits.Length = size;
            }
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
