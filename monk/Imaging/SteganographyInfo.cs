// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System;
using System.Collections.Generic;

using Monk.Memory.Bittwiddling;

namespace Monk.Imaging
{
    public class SteganographyInfo
    {
        private int lsb = 1;
        public int LeastSignificantBits
        {
            get => lsb;
            set {
                if (value < 1 || value > 4) throw new ArgumentOutOfRangeException("Least significant bits must be between 1 and 4");
                lsb = value;
            }
        }

        private IList<ushort> seed = new ushort[] { 0 };
        public IList<ushort> Seed
        {
            get => seed;
            set {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (value.Count == 0) throw new ArgumentException(nameof(value));
                seed = value;
            }
        }

        public bool InvertPrefixBits { get; set; } = false;
        public bool InvertDataBits { get; set; } = false;
        public bool ZeroBasedSize { get; set; } = false;
        public EndianMode Endianness { get; set; } = EndianMode.LittleEndian;
        public ISet<PixelColor> Colors { get; } = new HashSet<PixelColor>();
        public int Offset { get; set; } = 0;
    }
}
