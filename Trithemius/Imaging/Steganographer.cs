/**
 *  Trithemius
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
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using Trithemius.Bittwiddling;

namespace Trithemius.Imaging
{
    public class Steganographer : IDisposable
    {
        public Seed Seed { get; set; }

        private int lsb = 1;
        public int LeastSignificantBits
        {
            get {
                return lsb;
            }
            set {
                if (lsb < 1 || lsb > 4) {
                    throw new ArgumentException("Least significant bits must be between 1 and 4");
                }
                lsb = value;
            }
        }

        public Bitmap BitmapImage { get; set; }
        public PixelColor Color { get; set; }
        public bool InvertBits { get; set; }
        public bool Disposed { get; private set; }

        public Steganographer(Bitmap image)
        {
            BitmapImage = image;
        }

        private bool MessageFitsImage(ICollection<byte> message)
        {
            return GetRequiredSize(message) < GetMaximumSize();
        }

        private static int PixelNumber(int x, int y, int width)
        {
            return y * width + x;
        }

        private static Point PixelCoord(int number, int width)
        {
            return new Point(number % width, (number - number % width) / width);
        }

        public Dictionary<int, byte> Encode(byte[] message, string savePath)
        {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(message.Length));
            data.AddRange(message);

            if (!MessageFitsImage(data)) {
                throw new ArgumentException("Message is too big to encode in the image.");
            }

            BinaryList bits = new BinaryList(data);

            if (InvertBits)
                bits.Invert();

            LockedBitmap lockedBmp = new LockedBitmap(BitmapImage);
            lockedBmp.LockBits();

            Dictionary<int, byte> changes = new Dictionary<int, byte>();

            int bitIndex = 0;
            for (int pixelIndex = Seed[0]; pixelIndex <= BitmapImage.Height * BitmapImage.Width && bitIndex < bits.Count; pixelIndex += Seed[bitIndex % Seed.Count] + 1) {
                byte[] pixelValue = lockedBmp.GetPixelArgb(pixelIndex);

                BinaryOctet octet = pixelValue[(int)Color];
                for (int currBit = 0; currBit < lsb; ++currBit)
                    octet = octet.SetBit(currBit, bits[bitIndex++]);

                if (octet != pixelValue[(int)Color]) {
                    changes.Add(pixelIndex, pixelValue[(int)Color]);
                    pixelValue[(int)Color] = octet;
                }

                lockedBmp.SetPixel(PixelCoord(pixelIndex, BitmapImage.Width), pixelValue);
            }

            lockedBmp.UnlockBits();

            BitmapImage.Save(savePath, ImageFormat.Png);

            return changes;
        }

        public int CheckSize()
        {
            int size = BitConverter.ToInt32(ReadBits(sizeof(int)).ToArray(), 0);

            if (size <= 0 || size > GetMaximumSize())
                return -1; // size was invalid, there probably isn't a message

            return size;
        }

        public byte[] Decode()
        {
            int size = CheckSize();

            if (size < 0)
                return null; // no message

            IEnumerable<byte> data = ReadBits(sizeof(int) + size);
            return data.Skip(sizeof(int)).ToArray();
        }

        private IEnumerable<byte> ReadBits(int byteCount)
        {
            BinaryList data = new BinaryList();

            LockedBitmap lockedBmp = new LockedBitmap(BitmapImage);
            lockedBmp.LockBits();

            int bitIndex = 0;
            for (int pixelIndex = Seed[0]; bitIndex < byteCount * 8; pixelIndex += Seed[bitIndex % Seed.Count] + 1) {
                BinaryOctet octet = lockedBmp.GetPixelArgb(pixelIndex)[(int)Color];

                for (byte currBit = 0; currBit < lsb; ++currBit, ++bitIndex)
                    data.Add(octet[currBit]);
            }

            lockedBmp.UnlockBits();

            return data.ToBytes(InvertBits);
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
            return BitmapImage.Width * BitmapImage.Width * LeastSignificantBits;
        }

        public void Dispose()
        {
            if (BitmapImage != null && !Disposed) {
                Disposed = true;
                BitmapImage.Dispose();
            }
        }
    }

    public enum PixelColor : int
    {
        Alpha = 0, Red = 1, Green = 2, Blue = 3
    }
}

