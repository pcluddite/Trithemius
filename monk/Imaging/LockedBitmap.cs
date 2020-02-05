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
using System.Collections.Generic;
using System.IO;
using Monk.Memory;

namespace Monk.Imaging
{
    public abstract class LockedBitmap : IDisposable
    {
        internal const int ALPHA_SHIFT   = 0x18;
        internal const int RED_SHIFT     = 0x10;
        internal const int GREEN_SHIFT   = 0x08;
        internal const int BLUE_SHIFT    = 0x00;

        protected BitmapData BitmapData { get; set; }

        public Bitmap Bitmap { get; protected set; }
        public int Width => Bitmap.Width;
        public int Height => Bitmap.Height;
        public int BytesPerPixel => Depth / 8;

        public abstract int Depth { get; }
        public abstract ISet<PixelColor> SuportedColors { get; }

        public virtual bool Locked => BitmapData != null;

        protected int Stride => BitmapData.Stride;
        protected int Size => BitmapData.Height * Stride;

        internal UnmanagedBuffer RawData { get; set; }

        public virtual void LockBits()
        {
            Rectangle rect = new Rectangle(0, 0, Bitmap.Width, Bitmap.Height);
            BitmapData = Bitmap.LockBits(rect, ImageLockMode.ReadWrite, Bitmap.PixelFormat);
            RawData = new UnmanagedBuffer(BitmapData.Scan0, Size);
        }

        public virtual void UnlockBits()
        {
            Bitmap.UnlockBits(BitmapData);
            BitmapData = null;
        }

        public abstract int GetPixel(int x, int y);

        public virtual byte GetPixelColor(int x, int y, PixelColor color)
        {
            if (!SuportedColors.Contains(color)) ThrowHelper.ColorUnsupported(nameof(color), color);
            int value = GetPixel(x, y);
            return (byte)((value >> GetShift(color)) & 0xFF);
        }

        public abstract void SetPixel(int x, int y, int argb);

        public virtual void SetPixelColor(int x, int y, byte value, PixelColor color)
        {
            if (!SuportedColors.Contains(color)) ThrowHelper.ColorUnsupported(nameof(color), color);
            int argb = GetPixel(x, y) & ~(0xFF << GetShift(color));
            SetPixel(x, y, argb | (value << GetShift(color)));
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

        protected Span<byte> PixelAt(int x, int y)
        {
            return RawData.Slice(PointToOffset(x, y), BytesPerPixel);
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

        private static int GetShift(PixelColor color)
        {
            return color switch
            {
                PixelColor.Alpha => ALPHA_SHIFT,
                PixelColor.Red => RED_SHIFT,
                PixelColor.Green => GREEN_SHIFT,
                PixelColor.Blue => BLUE_SHIFT,
                _ => throw new InvalidOperationException(),
            };
        }

        private class LockedBitmap32bpp : LockedBitmap
        {
            public override int Depth => 32;
            public override ISet<PixelColor> SuportedColors { get; } = new HashSet<PixelColor>() { PixelColor.Alpha, PixelColor.Red, PixelColor.Green, PixelColor.Blue };

            public LockedBitmap32bpp(Bitmap bitmap)
            {
                Bitmap = bitmap;
            }

            public override int GetPixel(int x, int y)
            {
                return RawData.Read<int>(PointToOffset(x, y));
            }

            public override unsafe void SetPixel(int x, int y, int argb)
            {
                RawData.Write(PointToOffset(x, y), argb);
            }

            public override void SetPixelColor(int x, int y, byte value, PixelColor color)
            {
                Span<byte> pixel = PixelAt(x, y);
                pixel[(int)color/8] = value;
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

            public override int GetPixel(int x, int y)
            {
                Span<byte> pixel = PixelAt(x, y);
                int value = 0xff << ALPHA_SHIFT;
                value |= pixel[2] << RED_SHIFT;
                value |= pixel[1] << GREEN_SHIFT;
                value |= pixel[0] << BLUE_SHIFT;
                return value;
            }

            public override void SetPixel(int x, int y, int argb)
            {
                Span<byte> pixel = PixelAt(x, y);
                pixel[0] = (byte)((argb >> BLUE_SHIFT) & 0xff);
                pixel[1] = (byte)((argb >> GREEN_SHIFT) & 0xff);
                pixel[2] = (byte)((argb >> RED_SHIFT) & 0xff);
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

            public override int GetPixel(int x, int y)
            {
                return PixelAt(x, y)[0];
            }

            public override void SetPixel(int x, int y, int argb)
            {
                PixelAt(x, y)[0] = (byte)argb;
            }
        }

        private static class ThrowHelper
        {
            public static void ColorUnsupported(string arg, PixelColor color)
            {
                throw new ArgumentException("unsupported color " + color.ToString(), arg);
            }
        }
    }
}
