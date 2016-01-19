using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Trithemius
{
    public class Trithemius : IDisposable
	{
		public TrithemiusSeed Seed { get; set; }

        private int lsb = 1;
        public int LeastSignificantBits {
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

		public Trithemius(Bitmap image)
		{
            BitmapImage = image;
		}

        private bool MessageFitsImage(ICollection<byte> message)
        {
            return GetRequiredSize(message) < GetMaximumSize();
        }

        public static int PixelNumber(int x, int y, int width)
        {
            return (y * width) + x;
        }

        public static Point PixelCoord(int number, int width)
        {
            return new Point(number % width, (number - (number % width)) / width);
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

            LockedBitmap lockedBmp = new LockedBitmap(BitmapImage);
            lockedBmp.LockBits();

            Dictionary<int, byte> changes = new Dictionary<int, byte>();

            int bitIndex = 0;
            for (int pixelIndex = Seed[0]; pixelIndex <= (BitmapImage.Height * BitmapImage.Width) && bitIndex < bits.Count; pixelIndex += (Seed[bitIndex % Seed.Count] + 1)) {
                byte[] pixelValue = lockedBmp.GetPixelArgb(pixelIndex);

                BinaryOctet octet = pixelValue[(int)Color];
                for (int currBit = 0; currBit < lsb; ++currBit)
                    octet[currBit] = bits[bitIndex++];

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
            int size = BitConverter.ToInt32(ReadBits(sizeof(int)), 0);

            if (size <= 0 || size > GetMaximumSize())
                return -1; // size was invalid, there probably isn't a message

            return size;
        }

        public byte[] Decode()
        {
            int size = CheckSize();

            if (size < 0) 
                return null; // no message
            
            byte[] data = ReadBits(sizeof(int) + size);
            return data.Skip(sizeof(int)).ToArray();
        }

        private byte[] ReadBits(int byteCount)
        {
            BinaryList data = new BinaryList();

            LockedBitmap lockedBmp = new LockedBitmap(BitmapImage);
            lockedBmp.LockBits();

            int bitIndex = 0;
            for (int pixelIndex = Seed[0]; bitIndex < byteCount * 8; pixelIndex += (Seed[bitIndex % Seed.Count] + 1)) {
                BinaryOctet octet = lockedBmp.GetPixelArgb(pixelIndex)[(int)Color];

                for (byte currBit = 0; currBit < lsb; ++currBit, ++bitIndex)
                    data.Add(octet[currBit]);
            }

            lockedBmp.UnlockBits();

            return data.ToByteArray();
        }

        public int GetRequiredSize(ICollection<byte> message)
        {
            int size = message.Count + sizeof(int);
            int newSize = 0;
            for(int i = 0; i < size; ++i) {
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
            if (BitmapImage != null) {
                BitmapImage.Dispose();
            }
        }
    }

	public enum PixelColor : int
	{
		Alpha = 0, Red = 1, Green = 2, Blue = 3
	}
}

