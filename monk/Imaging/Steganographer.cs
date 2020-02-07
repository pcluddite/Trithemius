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
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using Monk.Memory;
using Monk.Memory.Bittwiddling;

namespace Monk.Imaging
{
    public class Steganographer : IDisposable
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

        private LockedBitmap lockedBitmap;

        public int Depth => lockedBitmap.Depth;
        public Bitmap Image => lockedBitmap.Bitmap;

        public bool InvertPrefixBits { get; set; } = false;
        public bool InvertDataBits { get; set; } = false;
        public bool ZeroBasedSize { get; set; } = false;
        public EndianMode Endianness { get; set; } = EndianMode.LittleEndian;
        public ISet<PixelColor> Colors { get; } = new HashSet<PixelColor>();
        public Seed Seed { get; set; } = Seed.DefaultSeed;
        public int Offset { get; set; } = 0;

        public bool Disposed { get; private set; }

        public Steganographer(string filename)
            : this(new Bitmap(filename))
        {
        }

        public Steganographer(Bitmap image)
        {
            lockedBitmap = LockedBitmap.CreateLockedBitmap(image);
        }

        private bool MessageFitsImage(ICollection<byte> message)
        {
            return GetRequiredSize(message) < GetMaximumSize();
        }

        public void Encode(byte[] data)
        {
            Encode(data, prefixSize: true);
        }

        public void Encode(byte[] data, bool prefixSize)
        {
            EnsureState();
            if (!MessageFitsImage(data)) {
                throw new ArgumentException("Message is too big to encode in the image.");
            }

            BinaryList bits = CreateBinaryList(data, prefixSize);

            int lsb = LeastSignificantBits;
            int bytesNeeded = MathUtil.DivideUp(bits.Count, lsb);
            int bitIndex = 0;

            using (ByteStream stream = lockedBitmap.GetStream(Offset, Seed, Colors)) {
                while(bitIndex < bits.Count) {
                    byte b = stream.Peek();
                    for (int currBit = 0; currBit < lsb; ++currBit)
                        b = Twiddler.SetBit(b, currBit, bits[bitIndex++]);
                    stream.WriteByte(b);
                }
            }
        }

        private BinaryList CreateBinaryList(byte[] data, bool prefixSize)
        {
            EndianMode dataEndianMode = Endianness;
            BinaryList bits;
            if (prefixSize) {
                int bitCount = (sizeof(int) + data.Length) * Twiddler.CHAR_BIT;
                bits = new BinaryList(BitConverter.GetBytes(data.Length), bitCount, EndianMode.LittleEndian);
                if (InvertPrefixBits) {
                    bits.Not();
                }
            }
            else {
                bits = new BinaryList(data.Length * Twiddler.CHAR_BIT, dataEndianMode);
            }
            if (InvertDataBits) {
                BinaryList dataBits = new BinaryList(data, dataEndianMode);
                dataBits.Not();
                bits.AddRange(dataBits);
            }
            else if (bits.Endianness == dataEndianMode) {
                bits.AddRange(data);
            }
            else {
                bits.AddRange(new BinaryList(data, dataEndianMode));
            }

            return bits;
        }

        public int CheckSize()
        {
            int size = BitConverter.ToInt32(ReadBits(sizeof(int), InvertPrefixBits, Offset, Endianness).ToArray(), 0);

            if (size <= 0 || size > GetMaximumSize())
                return -1; // size was invalid, there probably isn't a message

            return size;
        }

        public byte[] Decode()
        {
            EnsureState();
            int size = CheckSize();

            if (size < 0) return null; // no message
            if (ZeroBasedSize) size += 1;

            IEnumerable<byte> data = ReadBits(sizeof(int) + size, InvertDataBits, Offset, Endianness);
            return data.Skip(sizeof(int)).ToArray();
        }

        public byte[] Decode(int size)
        {
            EnsureState();
            return ReadBits(size, InvertDataBits, Offset, Endianness).ToArray();
        }

        private IEnumerable<byte> ReadBits(int byteCount, bool invert, int offset, EndianMode endianness)
        {            
            int lsb = LeastSignificantBits;
            int bitIndex = 0;
            int bitsToRead = byteCount * Twiddler.CHAR_BIT;
            int bytesNeeded = MathUtil.DivideUp(bitsToRead, lsb);

            BinaryList data = new BinaryList(bitsToRead);
            using (ByteStream stream = lockedBitmap.GetStream(offset, Seed, Colors)) {
                while (bitIndex < bitsToRead) {
                    for (int currBit = 0; currBit < lsb; ++currBit, ++bitIndex) {
                        byte pixelByte = stream.ReadNext();
                        data.Add(Twiddler.GetBit(pixelByte, currBit));
                    }
                }
            }

            if (invert) data.Not();

            return data.ToBytes(endianness);
        }

        public int GetRequiredSize(ICollection<byte> message)
        {
            int size = message.Count + sizeof(int);
            int newSize = 0;
            for (int i = 0; i < size; ++i) {
                newSize += Seed[i % Seed.Count] + 1;
            }
            return newSize;
        }

        public int GetMaximumSize()
        {
            return (Image.Width * Image.Height * LeastSignificantBits) / Twiddler.CHAR_BIT;
        }

        public void SetLegacyOptions()
        {
            Endianness = EndianMode.BigEndian;
            LeastSignificantBits = 1;
            ZeroBasedSize = true;
            InvertPrefixBits = false;
            InvertDataBits = false;
            Offset = 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) {
                if (lockedBitmap != null) {
                    lockedBitmap.Dispose();
                    lockedBitmap = null;
                }
                Disposed = true;
            }
        }

        private void EnsureState()
        {
            if (Disposed) throw new ObjectDisposedException(nameof(Steganographer));
            if (lockedBitmap == null || lockedBitmap.Bitmap == null) throw new InvalidOperationException("No Bitmap has been specified");
            if (Seed.Count == 0) throw new InvalidOperationException("Seed cannot be 0 length");
            if (Offset < 0 || Offset >= lockedBitmap.Height * lockedBitmap.Width) throw new InvalidOperationException("Offset cannot be less than 0 or greater than the image area");
            if (Colors.Count == 0) throw new InvalidOperationException("At least one color must be specified");
            if (!Colors.IsSubsetOf(lockedBitmap.SupportedColors)) throw new InvalidOperationException("One or more colors is not supported by the image format");
        }
    }
}

