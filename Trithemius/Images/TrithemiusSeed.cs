using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trithemius
{
    public struct TrithemiusSeed : IList<byte>
    {
        private byte[] seed;

        public int Count
        {
            get {
                ConstructSeedIfNull();
                return seed.Length;
            }
        }

        public bool IsReadOnly
        {
            get {
                return false;
            }
        }

        public byte this[int index]
        {
            get {
                ConstructSeedIfNull();
                return seed[index];
            }

            set {
                if (value > 9) {
                    throw new ArgumentOutOfRangeException("Value must be less than 10");
                }
                ConstructSeedIfNull();
                seed[index] = value;
            }
        }

        private static void ThrowIfBad(byte[] _seed)
        {
            if (_seed.Length > 10) {
                throw new ArgumentException("Seed cannot be greater than 10 integers");
            }
            foreach (int i in _seed) {
                if (i < 0 || i > 9) {
                    throw new ArgumentException("Integers in the seed must be between 0 and 10");
                }
            }
        }
        
		public TrithemiusSeed(ICollection<byte> _seed)
		{
			this.seed = new byte[_seed.Count];
			int i = 0;
			foreach (byte b in _seed) {
				this.seed[i++] = b;
			}
            ThrowIfBad(seed);
		}

		public TrithemiusSeed(string _seed)
		{
			seed = new byte[_seed.Length];
			int i = 0;
			foreach (char c in _seed) {
				seed[i++] = (byte)(c - '0');
			}
            ThrowIfBad(seed);
		}

		public static TrithemiusSeed RandomSeed(int size)
        {
            if (size > 10 || size < 1)
                throw new ArgumentOutOfRangeException("size");

            Random rand = new Random();
            byte[] seed = new byte[size];

            for (int i = 0; i < seed.Length; ++i)
                seed[i] = (byte)rand.Next(10);

            return new TrithemiusSeed(seed);
        }

        public int IndexOf(byte item)
        {
            ConstructSeedIfNull();
            return ((IList<byte>)seed).IndexOf(item);
        }

        public void Insert(int index, byte item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Add(byte item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(byte item)
        {
            ConstructSeedIfNull();
            return ((IList<byte>)seed).Contains(item);
        }

        public void CopyTo(byte[] array, int arrayIndex)
        {
            ConstructSeedIfNull();
            seed.CopyTo(array, arrayIndex);
        }

        public bool Remove(byte item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<byte> GetEnumerator()
        {
            ConstructSeedIfNull();
            return ((IList<byte>)seed).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            ConstructSeedIfNull();
            return seed.GetEnumerator();
        }

        private void ConstructSeedIfNull()
        {
            if (seed == null) 
                seed = new byte[1];
        }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach(byte b in seed)
				sb.Append(b);
			return sb.ToString();
		}

		public static TrithemiusSeed FromString(string _seed)
		{
			return new TrithemiusSeed(_seed);
		}
    }
}
