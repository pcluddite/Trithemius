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
using System.Linq;

namespace Monk.Bittwiddling
{
    /// <summary>
    /// A list of binary values 1 or 0, this class is more flexible than BitArray
    /// </summary>
    public class BinaryList : IList<bool>
    {
        private List<bool> bits = new List<bool>();

        public BinaryList()
        {
        }

        public BinaryList(IEnumerable<byte> data)
        {
            foreach (byte b in data) {
                AddRange(b);
            }
        }

        public bool this[int index]
        {
            get => bits[index];
            set => bits[index] = value;
        }

        public int Count => bits.Count;

        public void Add(bool item)
        {
            bits.Add(item);
        }

        public void AddRange(byte b)
        {
            AddRange(new BinaryOctet(b));
        }

        public void AddRange(BinaryOctet octet)
        {
            foreach (bool bit in octet) {
                Add(bit);
            }
        }

        public void Clear()
        {
            bits.Clear();
        }

        public bool Contains(bool item)
        {
            return bits.Contains(item);
        }

        public void CopyTo(bool[] array, int arrayIndex)
        {
            bits.CopyTo(array, arrayIndex);
        }

        public IEnumerator<bool> GetEnumerator()
        {
            return bits.GetEnumerator();
        }

        public int IndexOf(bool item)
        {
            return bits.IndexOf(item);
        }

        public void Insert(int index, bool item)
        {
            bits.Insert(index, item);
        }

        public bool Remove(bool item)
        {
            return bits.Remove(item);
        }

        public void RemoveAt(int index)
        {
            bits.RemoveAt(index);
        }

        public IEnumerable<byte> ToBytes()
        {
            return ToBytes(invert: false);
        }

        /// <summary>
        /// Converts each set of eight bits into a bytes
        /// </summary>
        /// <param name="invert">whether or not to invert bytes</param>
        /// <returns></returns>
        public IEnumerable<byte> ToBytes(bool invert)
        {
            int idx = 0;
            for(; idx < bits.Count; ++idx) {
                BinaryOctet curr = new BinaryOctet();
                int n = 0;
                do {
                    curr = curr.SetBit(n, invert ? !bits[idx] : bits[idx]);
                } while (++n < BinaryOctet.OCTET && ++idx < bits.Count);
                yield return curr;
            }
        }

        /// <summary>
        /// Sets each bit to the opposite of its current value
        /// </summary>
        public void Invert()
        {
            for (int index = 0; index < bits.Count; ++index) {
                bits[index] = !bits[index];
            }
        }

        /// <summary>
        /// Determines if the bits can be evenly turned into bytes (octets), without remaining bits
        /// </summary>
        /// <returns></returns>
        public bool IsValidBytes()
        {
            return bits.Count % 8 == 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(bits.Count);
            foreach (bool b in this) {
                sb.Append(b ? '1' : '0');
            }
            return sb.ToString();
        }

        bool ICollection<bool>.IsReadOnly => false;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
