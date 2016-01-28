using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Trithemius
{
    public class BinaryList : IList<bool>
    {
        private List<bool> bits = new List<bool>();

        public BinaryList()
        {
        }

        public BinaryList(IEnumerable<byte> data)
        {
            foreach(byte b in data) {
                AddRange(b);
            }
        }

        public bool this[int index]
        {
            get {
                return bits[index];
            }
            set {
                bits[index] = value;
            }
        }

        public int Count
        {
            get {
                return bits.Count;
            }
        }

        public bool IsReadOnly
        {
            get {
                return false;
            }
        }

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
            foreach(bool bit in octet) {
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

        /// <summary>
        /// Converts each set of eight bits into a bytes
        /// </summary>
        /// <param name="invert">whether or not to invert bytes</param>
        /// <returns></returns>
        public IEnumerable<byte> ToBytes(bool invert = false)
        {
            List<byte> data;
            if (IsValidBytes()) {
                data = new List<byte>(bits.Count / 8);
            }
            else {
                data = new List<byte>(bits.Count / 8 + 1);
            }

            BinaryOctet curr = new BinaryOctet();

            int bit = 0;
            foreach(bool b in this) {
                curr[bit++] = invert ? !b : b;
                if (bit > 7) {
                    data.Add(curr.ToByte());
                    curr = new BinaryOctet();
                    bit = 0;
                }
            }

            if (bit > 0)
                data.Add(curr.ToByte()); // the last remaining bits

            return data;
        }

        /// <summary>
        /// Sets each bit to the opposite of its current value
        /// </summary>
        public void Invert()
        {
            for(int index = 0; index < bits.Count; ++index) {
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
            foreach(bool b in this) {
                sb.Append(b ? '1' : '0');
            }
            return sb.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return bits.GetEnumerator();
        }
    }
}
