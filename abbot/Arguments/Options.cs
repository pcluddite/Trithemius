/**
 *  Abbot
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
using CommandLine;
using Monk.Imaging;
using Monk.Memory.Bittwiddling;

namespace Abbot.Arguments
{
    internal class Options
    {
        [Option("image", Required = true, HelpText = "path to the image this is being encoded/decoded")]
        public string Path { get; set; }

        [Option("seed", Required = false, HelpText = "encoding seed")]
        public uint Seed { get; set; } = 0;

        [Option("key", Required = false, HelpText = "encryption key for message or file")]
        public string Key { get; set; } = string.Empty;

        [Option('i', "invert", Required = false, HelpText = "invert bits when being encoded", Default = false)]
        public bool Invert { get; set; } = false;

        [Option('a', "alpha", Required = false, HelpText = "Use the alpha value of the pixels to encode/decode")]
        public bool Alpha { get; set; }

        [Option('r', "red", Required = false, HelpText = "Use the red value of the pixels to encode/decode")]
        public bool Red { get; set; }

        [Option('g', "green", Required = false, HelpText = "Use the green value of the pixels to encode/decode")]
        public bool Green { get; set; }

        [Option('b', "blue", Required = false, HelpText = "Use the blue value of the pixels to encode/decode")]
        public bool Blue { get; set; }

        [Option('l', "lsb", Required = false, HelpText = "the least significant bits to use", Default = 1)]
        public int LeastSignificantBits { get; set; } = 1;

        [Option("order", Required = false, HelpText = "the order the bits for each byte are encoded", Default = EndianMode.LittleEndian)]
        public EndianMode Order { get; set; }

        [Option("offset", Required = false, HelpText = "Start index of the pixel to begin reading or writing", Default = 0)]
        public int Offset { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Show more detailed information", Default = false)]
        public bool Verbose { get; set; }

        public virtual Steganographer BuildTrithemius()
        {
            Steganographer trithemius = new Steganographer(Path)
            {
                LeastSignificantBits = LeastSignificantBits,
                InvertDataBits = Invert,
                InvertPrefixBits = Invert,
                Endianness = Order,
                Offset = Offset
            };

            if (Alpha) trithemius.Colors.Add(PixelColor.Alpha);
            if (Red) trithemius.Colors.Add(PixelColor.Red);
            if (Green) trithemius.Colors.Add(PixelColor.Green);
            if (Blue) trithemius.Colors.Add(PixelColor.Blue);

            return trithemius;
        }
    }
}
