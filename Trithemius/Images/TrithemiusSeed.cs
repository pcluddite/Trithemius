﻿/**
 *  Trithemius
 *  Copyright (C) 2012-2016 Timothy Baxendale
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
                return true;
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
            foreach (byte b in seed)
                sb.Append(b);
            return sb.ToString();
        }

        public static TrithemiusSeed FromString(string _seed)
        {
            return new TrithemiusSeed(_seed);
        }
    }
}