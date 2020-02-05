﻿/**
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
using System.Collections.Generic;
using System.IO;

namespace Monk.Imaging
{
    public abstract class LockedBitmap : IDisposable
    {
        protected BitmapData BitmapData { get; set; }
        protected int Stride => BitmapData.Stride;
        protected int Size => Height * Stride;
        protected IntPtr Scan0 => BitmapData.Scan0;

        public Bitmap Bitmap { get; protected set; }
        public int Width => BitmapData.Width;
        public int Height => BitmapData.Height;
        public int BytesPerPixel => Depth / 8;

        public abstract int Depth { get; }
        public abstract ISet<PixelColor> SuportedColors { get; }

        public virtual bool Locked => BitmapData != null;
        
        public virtual void LockBits()
        {
            Rectangle rect = new Rectangle(0, 0, Bitmap.Width, Bitmap.Height);
            BitmapData = Bitmap.LockBits(rect, ImageLockMode.ReadWrite, Bitmap.PixelFormat);
        }

        public virtual void UnlockBits()
        {
            Bitmap.UnlockBits(BitmapData);
            BitmapData = null;
        }

        public abstract int GetPixel(int x, int y);

        public virtual byte GetPixelColor(int x, int y, PixelColor color)
        {
            if (!SuportedColors.Contains(color)) throw new ArgumentException("unsupported color", nameof(color));
            int value = GetPixel(x, y);
            return (byte)((value >> (int)color) & 0xFF);
        }

        public abstract void SetPixel(int x, int y, int argb);

        public virtual void SetPixelColor(int x, int y, byte value, PixelColor color)
        {
            if (!SuportedColors.Contains(color)) throw new ArgumentException("unsupported color", nameof(color));
            int argb = GetPixel(x, y) & ~(0xFF << (int)color);
            SetPixel(x, y, argb | (value << (int)color));
        }

        public virtual Color[,] ToColorMatrix()
        {
            Color[,] colors = new Color[Height, Width];
            for (int y = 0; y < Height; ++y) {
                for (int x = 0; x < Width; ++x) {
                    colors[y, x] = Color.FromArgb(GetPixel(x, y));
                }
            }
            return colors;
        }

        protected int PointToOffset(int x, int y)
        {
            EnsureState();
            if (x >= Width || x < 0) throw new ArgumentOutOfRangeException(nameof(x));
            if (y >= Height || y < 0) throw new ArgumentOutOfRangeException(nameof(y));
            return (y * Stride) + (x * BytesPerPixel);
        }

        protected unsafe internal byte* PtrAt(int x, int y)
        {
            int offset = PointToOffset(x, y);
            return (byte*)Scan0.ToPointer() + offset;
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

        protected void EnsureState()
        {
            if (!Locked) throw new InvalidOperationException();
        }

        public virtual void Save(string filename)
        {
            using (Stream stream = File.OpenWrite(filename)) {
                Save(stream);
            }
        }

        public virtual void Save(Stream stream)
        {
            Bitmap.Save(stream, ImageFormat.Png);
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
            public override ISet<PixelColor> SuportedColors { get; } = new HashSet<PixelColor>() { PixelColor.Alpha, PixelColor.Red, PixelColor.Green, PixelColor.Blue };

            public LockedBitmap32bpp(Bitmap bitmap)
            {
                Bitmap = bitmap;
            }

            public override unsafe int GetPixel(int x, int y)
            {
                byte* lpPxl = PtrAt(x, y);
                return *(int*)lpPxl;
            }

            public override unsafe void SetPixel(int x, int y, int argb)
            {
                byte* lpPxl = PtrAt(x, y);
                *(int*)lpPxl = argb;
            }

            public override unsafe void SetPixelColor(int x, int y, byte value, PixelColor color)
            {
                byte* lpPxl = PtrAt(x, y);
                lpPxl[(int)color/8] = value;
            }
        }

        private class LockedBitmap24bpp : LockedBitmap
        {
            public override int Depth => 24;
            public override ISet<PixelColor> SuportedColors { get; } = new HashSet<PixelColor>() { PixelColor.Red, PixelColor.Green, PixelColor.Blue };

            public LockedBitmap24bpp(Bitmap bitmap)
            {
                Bitmap = bitmap;
            }

            public override unsafe int GetPixel(int x, int y)
            {
                byte* lpPxl = PtrAt(x, y);
                int value = 0xff << (int)PixelColor.Alpha;
                value |= lpPxl[2] << (int)PixelColor.Red;
                value |= lpPxl[1] << (int)PixelColor.Green;
                value |= lpPxl[0] << (int)PixelColor.Blue;
                return value;
            }

            public override unsafe void SetPixel(int x, int y, int argb)
            {
                byte* lpPxl = PtrAt(x, y);
                byte* lpArgbBytes = (byte*)&argb;
                for (int i = 0; i < BytesPerPixel; ++i) {
                    lpPxl[i] = lpArgbBytes[i + 1];
                }
            }
        }

        private class LockedBitmap8bpp : LockedBitmap
        {
            public override int Depth => 8;
            public override ISet<PixelColor> SuportedColors { get; } = new HashSet<PixelColor>() { PixelColor.Blue };

            public LockedBitmap8bpp(Bitmap bitmap)
            {
                Bitmap = bitmap;
            }

            public override unsafe int GetPixel(int x, int y)
            {
                byte* lpPtr = PtrAt(x, y);
                return *lpPtr;
            }

            public override unsafe void SetPixel(int x, int y, int argb)
            {
                byte* lpPtr = PtrAt(x, y);
                int value = argb & 0xFF;
                *lpPtr = (byte)value;
            }
        }
    }
}