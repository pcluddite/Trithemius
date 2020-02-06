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
using System.Text;

namespace Monk.Memory
{
    public struct Seed : IList<int>
    {
        public static readonly Seed DefaultSeed = new Seed(0);
        public static readonly long MaxValue = (long)Math.Pow(10, 10) - 1;
        public static readonly long MinValue = 1;

        private static readonly Random random = new Random();

        public long Value { get; }
        public int Count { get; }

        public bool IsReadOnly => true;

        public int this[int index]
        {
            get {
                if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException(nameof(index));
                return (int)(Value / Math.Pow(10, Count - index - 1)) % 10;
            }
            set => throw new InvalidOperationException();
        }

        public Seed(string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (str.Length > 10) throw new ArgumentException("seed is too long", nameof(str));
            Count = Math.Max(str.Length, 1);
            Value = 0;
            for (int idx = 0; idx < str.Length; ++idx) {
                char c = str[idx];
                if (!char.IsDigit(c)) throw new ArgumentException("input string should only contain numbers and must be 10 digits or less");
                Value += (c - '0') * (long)Math.Pow(10, Count - idx - 1);
            }
        }

        public Seed(long seed)
        {
            if (seed < MinValue || seed > MaxValue) throw new ArgumentException("seed is too long", nameof(seed));
            Count = MathUtil.CountDigits(seed);
            Value = seed;
        }

        private Seed(long seed, int size)
        {
            Count = size;
            Value = seed;
        }

        public int IndexOf(int item)
        {
            long pow = (long)Math.Pow(10, Count - 1);
            for (int index = 0; index < Count; ++index) {
                if (Value / pow % 10 == item) return index;
                pow /= 10;
            }
            return -1;
        }

        public bool Contains(int item)
        {
            return IndexOf(item) > -1;
        }

        public void CopyTo(int[] array, int arrayIndex)
        {
            for (int i = 0; arrayIndex < Count; ++arrayIndex, ++i) {
                array[arrayIndex] = this[i];
            }
        }

        public IEnumerator<int> GetEnumerator()
        {
            long pow = (long)Math.Pow(10, Count - 1);
            for (int index = 0; index < Count; ++index) {
                yield return (int)(Value / pow % 10);
                pow /= 10;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public InfiniteSequence<int> GetSequence()
        {
            return new SeedSequence(this);
        }

        public override string ToString()
        {
            int n = MathUtil.CountDigits(Value);
            if (Count > n) {
                StringBuilder sb = new StringBuilder(Count);
                sb.Append('0', Count - n);
                return sb.Append(Value).ToString();
            }
            else {
                return Value.ToString();
            }
        }

        public static Seed Random(int size)
        {
            if (size > 10 || size < 1) throw new ArgumentOutOfRangeException(nameof(size));
            long min = (long)Math.Pow(10, size - 1);
            long max = (long)Math.Pow(10, size);
            return new Seed((long)(random.NextDouble() * (max - min) + min), size);
        }

        public override bool Equals(object obj)
        {
            if (obj is Seed s) return Equals(s);
            return false;
        }

        public bool Equals(Seed other)
        {
            return other.Value == Value && other.Count == Count;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(Seed left, Seed right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Seed left, Seed right)
        {
            return !(left == right);
        }

        void IList<int>.Insert(int index, int item)
        {
            throw new NotImplementedException();
        }

        void IList<int>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        void ICollection<int>.Add(int item)
        {
            throw new NotImplementedException();
        }

        void ICollection<int>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<int>.Remove(int item)
        {
            throw new NotImplementedException();
        }

        private class SeedSequence : InfiniteSequence<int>
        {
            private readonly Seed seed;

            public SeedSequence(Seed seed)
            {
                this.seed = seed;
            }

            public override int Next()
            {
                return seed[Position++ % seed.Count];
            }

            public override int Peek()
            {
                return seed[(Position + 1) % seed.Count];
            }
        }
    }
}