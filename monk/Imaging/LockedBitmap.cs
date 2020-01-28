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
    public abstract class LockedBitmap : IDisposable
    {
        protected BitmapData BitmapData { get; set; }
        protected int Stride => BitmapData.Stride;
        protected int Size => Height * Stride;
        protected IntPtr Scan0 => BitmapData.Scan0;

        public Bitmap Bitmap { get; protected set; }
        public int Width => Bitmap.Width;
        public int Height => Bitmap.Height;
        public int BytesPerPixel => Depth / 8;

        public abstract int Depth { get; }

        public virtual bool Locked => BitmapData != null;

        public virtual void LockBits()
        {
            Rectangle rect = new Rectangle(0, 0, Width, Height);
            BitmapData = Bitmap.LockBits(rect, ImageLockMode.ReadWrite, Bitmap.PixelFormat);
        }

        public virtual void UnlockBits()
        {
            Bitmap.UnlockBits(BitmapData);
            BitmapData = null;
        }

        public abstract Color GetPixel(int x, int y);

        public virtual byte GetPixelColor(int x, int y, PixelColor color)
        {
            if (!Locked) throw new InvalidOperationException();
            if (x >= Width || x < 0) throw new ArgumentOutOfRangeException(nameof(x));
            if (y >= Height || y < 0) throw new ArgumentOutOfRangeException(nameof(y));

            Color pixel = GetPixel(x, y);
            if (color == PixelColor.Blue) {
                return pixel.B;
            }
            else if (color == PixelColor.Green) {
                return pixel.G;
            }
            else if (color == PixelColor.Red) {
                return pixel.R;
            }
            else if (color == PixelColor.Alpha) {
                return pixel.A;
            }
            else {
                throw new ArgumentException(nameof(color));
            }
        }

        public abstract void SetPixel(int x, int y, Color color);

        public abstract void SetPixelColor(int x, int y, byte value, PixelColor color);

        public virtual void CopyTo(byte[] buffer, int startIndex, int count)
        {
            if (count >= Size)
                throw new ArgumentOutOfRangeException(nameof(count));
            Marshal.Copy(Scan0, buffer, startIndex, count);
        }

        public virtual Color[,] ToColorMatrix()
        {
            Color[,] colors = new Color[Height, Width];
            for(int y = 0; y < Height; ++y) {
                for(int x = 0; x < Width; ++x) {
                    colors[y, x] = GetPixel(x, y);
                }
            }
            return colors;
        }

        protected int PointToOffset(int x, int y)
        {
            return (y * Stride) + (x * BytesPerPixel);
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
                Bitmap.Dispose();
            }
        }

        public static LockedBitmap CreateLockedBitmap(Bitmap bitmap)
        {
            int depth = Image.GetPixelFormatSize(bitmap.PixelFormat);
            if (depth == 32) {
                return new LockedBitmap32bpp(bitmap);
            }
            else if (depth == 24) {
                return new LockedBitmap24bpp(bitmap);
            }
            else if (depth == 8) {
                return new LockedBitmap8bpp(bitmap);
            }
            else {
                throw new ArgumentException($"Image format Bitmap-{depth}bpp is unsuported");
            }
        }

        private class LockedBitmap32bpp : LockedBitmap
        {
            public override int Depth => 32;

            public LockedBitmap32bpp(Bitmap bitmap)
            {
                Bitmap = bitmap;
            }

            public override Color GetPixel(int x, int y)
            {
                if (!Locked) throw new InvalidOperationException();
                if (x >= Width || x < 0) throw new ArgumentOutOfRangeException(nameof(x));
                if (y >= Height || y < 0) throw new ArgumentOutOfRangeException(nameof(y));

                unsafe {
                    int stride = Stride;
                    byte* lpRaw = (byte*)Scan0.ToPointer();
                    int* lpValues = (int*)&lpRaw[stride * y];
                    return Color.FromArgb(lpValues[x]);
                }
            }

            public override void SetPixel(int x, int y, Color color)
            {
                if (!Locked) throw new InvalidOperationException();
                if (x >= Width || x < 0) throw new ArgumentOutOfRangeException(nameof(x));
                if (y >= Height || y < 0) throw new ArgumentOutOfRangeException(nameof(y));

                unsafe {
                    int stride = Stride;
                    byte* lpRaw = (byte*)Scan0.ToPointer();
                    int* lpValues = (int*)&lpRaw[stride * y];
                    lpValues[x] = color.ToArgb();
                }
            }

            public override void SetPixelColor(int x, int y, byte value, PixelColor color)
            {
                int offset = PointToOffset(x, y);
                if (color == PixelColor.Green) {
                    offset += 1;
                }
                else if (color == PixelColor.Red) {
                    offset += 2;
                }
                else if (color == PixelColor.Alpha) {
                    offset += 3;
                }
                else if (color != PixelColor.Blue) {
                    throw new ArgumentException(nameof(color));
                }
                unsafe {
                    byte* lpPixel = (byte*)Scan0.ToPointer();
                    lpPixel[offset] = value;
                }
            }

            public override Color[,] ToColorMatrix()
            {
                if (!Locked) throw new InvalidOperationException();
                int h = Height, w = Width, stride = Stride;
                Color[,] colors = new Color[h, w];
                unsafe {
                    int* lpScan0 = (int*)Scan0.ToPointer();
                    for (int y = 0; y < h; ++y) {
                        int* lpRow = &lpScan0[y * (stride / sizeof(int))];
                        for(int x = 0; x < w; ++x) {
                            colors[y, x] = Color.FromArgb(lpRow[x]);
                        }
                    }
                }
                return colors;
            }
        }

        private class LockedBitmap24bpp : LockedBitmap
        {
            public override int Depth => 24;

            public LockedBitmap24bpp(Bitmap bitmap)
            {
                Bitmap = bitmap;
            }

            public override Color GetPixel(int x, int y)
            {
                int offset = PointToOffset(x, y);
                unsafe {
                    byte* lpPixel = (byte*)Scan0.ToPointer() + offset;
                    return Color.FromArgb(lpPixel[2], lpPixel[1], lpPixel[0]);
                }
            }

            public override byte GetPixelColor(int x, int y, PixelColor color)
            {
                Color c = GetPixel(x, y);
                if (color == PixelColor.Blue) {
                    return c.B;
                }
                else if (color == PixelColor.Green) {
                    return c.G;
                }
                else if (color == PixelColor.Red) {
                    return c.R;
                }
                else {
                    throw new ArgumentException(nameof(color));
                }
            }

            public override void SetPixel(int x, int y, Color color)
            {
                int offset = PointToOffset(x, y);
                unsafe {
                    byte* lpPixel = (byte*)Scan0.ToPointer();
                    lpPixel[offset] = color.B;
                    lpPixel[offset + 1] = color.G;
                    lpPixel[offset + 2] = color.R;
                }
            }

            public override void SetPixelColor(int x, int y, byte value, PixelColor color)
            {
                int offset = PointToOffset(x, y);
                if (color == PixelColor.Green) {
                    offset = offset + 1;
                }
                else if (color == PixelColor.Red) {
                    offset = offset + 2;
                }
                else if (color != PixelColor.Blue) {
                    throw new ArgumentException(nameof(color));
                }
                unsafe {
                    byte* lpPixel = (byte*)Scan0.ToPointer();
                    lpPixel[offset] = value;
                }
            }

            public override Color[,] ToColorMatrix()
            {
                if (!Locked) throw new InvalidOperationException();
                int h = Height, w = Width, stride = Stride;
                Color[,] colors = new Color[h, w];
                unsafe {
                    byte* scan0 = (byte*)Scan0.ToPointer();
                    for (int y = 0; y < h; ++y) {
                        for (int x = 0; x < w; ++x) {
                            byte* lpPixelStart = &scan0[(y * stride) + x];
                            colors[y, x] = Color.FromArgb(lpPixelStart[2], lpPixelStart[1], lpPixelStart[0]);
                        }
                    }
                }
                return colors;
            }
        }

        private class LockedBitmap8bpp : LockedBitmap
        {
            public override int Depth => 8;

            public LockedBitmap8bpp(Bitmap bitmap)
            {
                Bitmap = bitmap;
            }

            public override Color GetPixel(int x, int y)
            {
                return Color.FromArgb(GetPixelColor(x, y, PixelColor.Blue));
            }

            public override byte GetPixelColor(int x, int y, PixelColor color)
            {
                int offset = PointToOffset(x, y);
                unsafe {
                    byte* lpPixel = (byte*)Scan0.ToPointer();
                    return lpPixel[offset];
                }
            }

            public override void SetPixel(int x, int y, Color color)
            {
                byte value = (byte)color.ToArgb();
                int offset = PointToOffset(x, y);
                unsafe {
                    byte* lpPixel = (byte*)Scan0.ToPointer();
                    lpPixel[offset] = value;
                }
            }

            public override void SetPixelColor(int x, int y, byte value, PixelColor color)
            {
                int offset = PointToOffset(x, y);
                unsafe {
                    byte* lpPixel = (byte*)Scan0.ToPointer();
                    lpPixel[offset] = value;
                }
            }
        }
    }
}
