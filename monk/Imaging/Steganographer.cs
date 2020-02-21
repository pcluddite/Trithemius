// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

using Monk.Memory;
using Monk.Memory.Bittwiddling;

namespace Monk.Imaging
{
    public class Steganographer : IDisposable
    {
        private LockedBitmap lockedBitmap;
        private readonly SteganographyInfo info;

        public int Depth => lockedBitmap.Depth;
        public Bitmap Image => lockedBitmap.Bitmap;

        public bool InvertPrefixBits
        {
            get => info.InvertPrefixBits;
            set => info.InvertPrefixBits = value;
        }

        public bool InvertDataBits
        {
            get => info.InvertDataBits;
            set => info.InvertDataBits = value;
        }

        public bool ZeroBasedSize
        {
            get => info.ZeroBasedSize;
            set => info.ZeroBasedSize = value;
        }

        public EndianMode Endianness
        {
            get => info.Endianness;
            set => info.Endianness = value;
        }

        public ISet<PixelColor> Colors => info.Colors;

        public IList<ushort> Seed
        {
            get => info.Seed;
            set => info.Seed = value;
        }

        public int Offset
        {
            get => info.Offset;
            set => info.Offset = value;
        }

        public int LeastSignificantBits
        {
            get => info.LeastSignificantBits;
            set => info.LeastSignificantBits = value;
        }

        public bool Disposed { get; private set; }

        public Steganographer(string filename)
            : this(new Bitmap(filename), new SteganographyInfo())
        {
        }

        public Steganographer(Bitmap image)
            : this(image, new SteganographyInfo())
        {
        }

        public Steganographer(string filename, SteganographyInfo info)
            : this(new Bitmap(filename), info)
        {
        }

        public Steganographer(Bitmap image, SteganographyInfo info)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            if (info == null) throw new ArgumentNullException(nameof(info));
            lockedBitmap = LockedBitmap.CreateLockedBitmap(image);
            this.info = info;
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
            int bytesNeeded = bits.Count.DivideUp(lsb);
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
            int pixelsNeeded = bitsToRead.DivideUp(lsb);

            BinaryList data = new BinaryList(bitsToRead);
            using (ByteStream stream = lockedBitmap.GetStream(offset, pixelsNeeded, Seed, Colors)) {
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

        public void SaveImage(string filename)
        {
            lockedBitmap.Save(filename);
        }

        public void SaveImage(Stream stream)
        {
            lockedBitmap.Save(stream);
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
            if (lockedBitmap == null || lockedBitmap.Bitmap == null) throw new InvalidImageOptionException("No Bitmap has been specified", nameof(Image));
            if (Seed.Count == 0) throw new InvalidImageOptionException("Seed cannot be 0 length", nameof(Seed));
            if (Offset < 0 || Offset >= lockedBitmap.Height * lockedBitmap.Width) throw new InvalidImageOptionException("Offset cannot be less than 0 or greater than the image area", nameof(Offset));
            if (Colors.Count == 0) throw new InvalidImageOptionException("At least one color must be specified", nameof(Colors));
            if (!Colors.IsSubsetOf(lockedBitmap.SupportedColors)) throw new InvalidImageOptionException("One or more colors is not supported by the image format", nameof(Colors));
        }
    }
}

