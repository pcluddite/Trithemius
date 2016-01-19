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
                BinaryOctet bin = b;
                foreach(bool bit in bin) {
                    Add(bit);
                }
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
            BinaryOctet bin = b;
            foreach(bool bit in bin) {
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
            List<byte> data = new List<byte>();

            BinaryOctet curr = new BinaryOctet();

            int bit = 0;
            foreach(bool b in this) {
                curr[bit++] = b;
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
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
