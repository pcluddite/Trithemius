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
using System.Runtime.InteropServices;
#if DEBUG
using System.IO;
#endif

namespace Monk.Imaging
{
    public abstract class LockedBitmap : IDisposable
    {
        protected BitmapData BitmapData { get; set; }
        protected int Stride => BitmapData.Stride;
        protected int Size => Height * Stride;

        public Bitmap Bitmap { get; protected set; }
        public int Width => BitmapData.Width;
        public int Height => BitmapData.Height;
        public int BytesPerPixel => Depth / 8;

        public abstract int Depth { get; }
        public abstract IEnumerable<PixelColor> SuportedColors { get; }

        public virtual bool Locked => BitmapData != null;

        protected IntPtr Scan0 => BitmapData.Scan0;
        
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

        public abstract Color GetPixel(int x, int y);

        public virtual byte GetPixelColor(int x, int y, PixelColor color)
        {
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

        public virtual void SetPixelColor(int x, int y, byte value, PixelColor color)
        {
            Color c = GetPixel(x, y);
            switch (color) {
                case PixelColor.Alpha:
                    c = Color.FromArgb(value, c.A, c.G, c.B);
                    break;
                case PixelColor.Red:
                    c = Color.FromArgb(c.A, value, c.G, c.B);
                    break;
                case PixelColor.Green:
                    c = Color.FromArgb(c.A, c.R, value, c.B);
                    break;
                case PixelColor.Blue:
                    c = Color.FromArgb(c.A, c.R, c.G, value);
                    break;
            }
            SetPixel(x, y, c);
        }

        public virtual Color[,] ToColorMatrix()
        {
            Color[,] colors = new Color[Height, Width];
            for (int y = 0; y < Height; ++y) {
                for (int x = 0; x < Width; ++x) {
                    colors[y, x] = GetPixel(x, y);
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

        protected abstract int GetColorOffset(PixelColor color);

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

#if DEBUG
        internal virtual void Dump(Stream stream, PixelColor pixelColor)
        {
            using (StreamWriter sw = new StreamWriter(stream)) {
                Color[,] colors = ToColorMatrix();
                sw.WriteLine("width: {0}", colors.GetUpperBound(1) + 1);
                sw.WriteLine("height: {0}", colors.GetUpperBound(0) + 1);
                sw.WriteLine("depth: {0}bpp", Depth);
                sw.WriteLine("matrix:");
                for (int y = 0; y <= colors.GetUpperBound(0); ++y) {
                    for (int x = 0; x <= colors.GetUpperBound(1); ++x) {
                        Color color = colors[y, x];
                        byte value = 0;
                        switch(pixelColor) {
                            case PixelColor.Alpha: value = color.A; break;
                            case PixelColor.Red: value = color.R; break;
                            case PixelColor.Green: value = color.G; break;
                            case PixelColor.Blue: value = color.B; break;
                        }
                        sw.Write(Convert.ToString(value, 2).PadLeft(8, '0'));
                        sw.Write(' ');
                    }
                    sw.WriteLine();
                }
            }
        }
#endif

        private class LockedBitmap32bpp : LockedBitmap
        {
            public override int Depth => 32;
            public override IEnumerable<PixelColor> SuportedColors => new PixelColor[] { PixelColor.Alpha, PixelColor.Red, PixelColor.Green, PixelColor.Blue };

            public LockedBitmap32bpp(Bitmap bitmap)
            {
                Bitmap = bitmap;
            }

            public override unsafe Color GetPixel(int x, int y)
            {
                byte* lpPxl = PtrAt(x, y);
                return Color.FromArgb(lpPxl[3], lpPxl[2], lpPxl[1], lpPxl[0]);
            }

            public override unsafe void SetPixel(int x, int y, Color color)
            {
                byte* lpPxl = PtrAt(x, y);
                lpPxl[0] = color.B;
                lpPxl[1] = color.G;
                lpPxl[2] = color.R;
                lpPxl[3] = color.A;
            }

            public override unsafe void SetPixelColor(int x, int y, byte value, PixelColor color)
            {
                byte* lpPxl = PtrAt(x, y);
                lpPxl[3 - (int)color] = value;
            }

            protected override int GetColorOffset(PixelColor color)
            {
                switch(color) {
                    case PixelColor.Blue: return 0;
                    case PixelColor.Green: return 1;
                    case PixelColor.Red: return 2;
                    case PixelColor.Alpha: return 3;
                }
                return 0;
            }
        }

        private class LockedBitmap24bpp : LockedBitmap
        {
            public override int Depth => 24;
            public override IEnumerable<PixelColor> SuportedColors => new PixelColor[] { PixelColor.Red, PixelColor.Green, PixelColor.Blue };

            public LockedBitmap24bpp(Bitmap bitmap)
            {
                Bitmap = bitmap;
            }

            public override unsafe Color GetPixel(int x, int y)
            {
                byte* lpPxl = PtrAt(x, y);
                return Color.FromArgb(lpPxl[2], lpPxl[1], lpPxl[0]);
            }

            public override unsafe void SetPixel(int x, int y, Color color)
            {
                byte* lpPxl = PtrAt(x, y);
                lpPxl[0] = color.B;
                lpPxl[1] = color.G;
                lpPxl[2] = color.R;
            }

            public override unsafe void SetPixelColor(int x, int y, byte value, PixelColor color)
            {
                int nColorOffset;
                switch(color) {
                    case PixelColor.Blue: nColorOffset = 0; break;
                    case PixelColor.Green: nColorOffset = 1; break;
                    case PixelColor.Red: nColorOffset = 2; break;
                    default: throw new ArgumentException(nameof(color));
                }
                byte* lpPxl = PtrAt(x, y);
                lpPxl[nColorOffset] = value;
            }

            protected override int GetColorOffset(PixelColor color)
            {
                switch (color) {
                    case PixelColor.Blue: return 0;
                    case PixelColor.Green: return 1;
                    case PixelColor.Red: return 2;
                }
                return 0;
            }
        }

        private class LockedBitmap8bpp : LockedBitmap
        {
            public override int Depth => 8;
            public override IEnumerable<PixelColor> SuportedColors => new PixelColor[] { PixelColor.Blue };

            public LockedBitmap8bpp(Bitmap bitmap)
            {
                Bitmap = bitmap;
            }

            public override unsafe Color GetPixel(int x, int y)
            {
                byte* lpPtr = PtrAt(x, y);
                return Color.FromArgb(*lpPtr);
            }

            public override void SetPixel(int x, int y, Color color)
            {
                SetPixelColor(x, y, (byte)color.ToArgb(), PixelColor.Blue);
            }

            public override unsafe void SetPixelColor(int x, int y, byte value, PixelColor color)
            {
                byte* lpPtr = PtrAt(x, y);
                *lpPtr = value;
            }

            protected override int GetColorOffset(PixelColor color)
            {
                return 0;
            }
        }
    }
}
