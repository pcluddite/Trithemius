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
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Monk.Imaging
{
    public partial class LockedBitmap : IDisposable
    {
        private Bitmap bitmap;
        private BitmapData bitmapData;
        private int stride;
        private int size;
        
        public int Depth { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public bool Locked { get => bitmapData != null; }

        public int BytesPerPixel { get => Depth / 8; }

        public LockedBitmap(Bitmap bitmap)
        {
            this.bitmap = bitmap;
            Depth = Image.GetPixelFormatSize(bitmap.PixelFormat);
            Width = bitmap.Width;
            Height = bitmap.Height;
        }

        public void LockBits()
        {
            Rectangle rect = new Rectangle(0, 0, Width, Height);
            bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            stride = bitmapData.Stride;
            size = Height * stride;
        }

        public void UnlockBits()
        {
            bitmap.UnlockBits(bitmapData);
            bitmapData = null;
        }

        public Color GetPixel(int x, int y)
        {
            return FromBytes(GetPixelArgb(x, y));
        }

        public byte[] GetPixelArgb(int x, int y)
        {
            Color color = GetPixelArgb((y * stride + x) * BytesPerPixel);
            switch(BytesPerPixel) {
                case 4: return new byte[] { color.A, color.R, color.G, color.B };
                case 3: return new byte[] { color.R, color.G, color.B };
                case 1: return new byte[] { color.B };
                default: throw UnsupportedColorDepth();
            }
        }

        private Color GetPixelArgb(int idx)
        {
            if (idx < 0 || idx > size - BytesPerPixel)
                throw new IndexOutOfRangeException();

            switch(BytesPerPixel) {
                case 4:
                    return Color.FromArgb(
                        GetPixelColor(idx, PixelColor.Alpha),
                        GetPixelColor(idx, PixelColor.Red),
                        GetPixelColor(idx, PixelColor.Green),
                        GetPixelColor(idx, PixelColor.Blue));
                case 3:
                    return Color.FromArgb(
                        GetPixelColor(idx, PixelColor.Red),
                        GetPixelColor(idx, PixelColor.Green),
                        GetPixelColor(idx, PixelColor.Blue));
                case 1:
                    byte color = GetPixelColor(idx, PixelColor.Blue);
                    return Color.FromArgb(color, color, color);
                default:
                    throw UnsupportedColorDepth();
            }
        }


        public byte GetPixelColor(int x, int y, PixelColor color)
        {
            return GetPixelColor((y * stride + x) * BytesPerPixel, color);
        }

        private byte GetPixelColor(int idx, PixelColor color)
        {
            return Marshal.ReadByte(bitmapData.Scan0, idx + (3 - (int)color));
        }

        public void SetPixel(int x, int y, Color color)
        {
            SetPixel((y * stride + x) * BytesPerPixel, color);
        }

        private void SetPixel(int idx, Color color)
        {
            switch (BytesPerPixel) {
                case 4: 
                    SetPixelColor(idx, color.B, PixelColor.Blue);
                    SetPixelColor(idx, color.G, PixelColor.Green);
                    SetPixelColor(idx, color.R, PixelColor.Red);
                    SetPixelColor(idx, color.A, PixelColor.Alpha);
                    break;
                case 3:
                    SetPixelColor(idx, color.B, PixelColor.Blue);
                    SetPixelColor(idx, color.G, PixelColor.Green);
                    SetPixelColor(idx, color.R, PixelColor.Red);
                    break;
                case 1:
                    SetPixelColor(idx, color.B, PixelColor.Blue);
                    break;
                default: throw UnsupportedColorDepth();
            }
        }

        public void SetPixelColor(int x, int y, byte value, PixelColor color)
        {
            SetPixelColor((y * stride + x) * BytesPerPixel, value, color);
        }

        private void SetPixelColor(int idx, byte value, PixelColor color)
        {
            if (idx < 0 || idx > size - BytesPerPixel)
                throw new IndexOutOfRangeException();
            Marshal.WriteByte(bitmapData.Scan0, idx + (3 - (int)color), value);
        }

        public void SetPixelArgb(int x, int y, byte[] data)
        {
            SetPixel(x, y, FromBytes(data));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) {
                if (Locked) {
                    UnlockBits();
                }
                bitmap.Dispose();
            }
        }

        private Color FromBytes(byte[] data)
        {
            switch (BytesPerPixel) {
                case 4: return Color.FromArgb(data[0], data[1], data[2], data[3]);
                case 3: return Color.FromArgb(data[0], data[1], data[2]);
                case 1: return Color.FromArgb(data[0], data[0], data[0]);
                default: throw UnsupportedColorDepth();
            }
        }

        private Exception UnsupportedColorDepth()
        {
            return new ArgumentException("unsupported color depth " + Depth + "bpp");
        }
    }
}
