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

namespace Monk.Memory.Bittwiddling
{
    public static class Twiddler
    {
        public const int CHAR_BIT = 8;
        public static readonly EndianMode ImplementationEndianness = BitConverter.IsLittleEndian ? EndianMode.LittleEndian : EndianMode.BigEndian;

        public static bool GetBit(byte b, int bit)
        {
            if (bit < 0 || bit >= CHAR_BIT) throw new ArgumentOutOfRangeException(nameof(bit));
            return (b & 1 << bit) != 0;
        }

        public static bool GetBit(byte b, int bit, EndianMode endianMode)
        {
            if (ImplementationEndianness == endianMode) {
                return GetBit(b, bit);
            }
            else {
                return GetBit(b, CHAR_BIT - bit - 1);
            }
        }

        public static byte SetBit(byte b, int bit, bool value)
        {
            if (bit < 0 || bit >= CHAR_BIT) throw new ArgumentOutOfRangeException(nameof(bit));
            if (value) {
                return (byte)(b | 1 << bit);
            }
            else {
                return (byte)(b & ~(1 << bit));
            }
        }

        public static byte SetBit(byte b, int bit, bool value, EndianMode endianMode)
        {
            if (ImplementationEndianness == endianMode) {
                return SetBit(b, bit, value);
            }
            else {
                return SetBit(b, CHAR_BIT - bit - 1, value);
            }
        }

        public static byte FlipBits(byte b)
        {
            b = (byte)((b & 0xF0) >> 4 | (b & 0x0F) << 4);
            b = (byte)((b & 0xCC) >> 2 | (b & 0x33) << 2);
            b = (byte)((b & 0xAA) >> 1 | (b & 0x55) << 1);
            return b;
        }
    }
}
