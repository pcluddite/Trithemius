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
        protected Bitmap Bitmap { get; set; }
        protected BitmapData BitmapData { get; set; }
        protected int Stride => BitmapData.Stride;
        protected int Size => Height * Stride;
        protected IntPtr Scan0 => BitmapData.Scan0;

        public int Width => Bitmap.Width;
        public int Height => Bitmap.Height;
        public bool Locked => BitmapData != null;

        public int BytesPerPixel => Depth / 8;
        public abstract int Depth { get; }

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

        public abstract byte GetPixelColor(int x, int y, PixelColor color);

        public abstract void SetPixel(int x, int y, Color color);

        public abstract void SetPixelColor(int x, int y, byte value, PixelColor color);

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
                throw new ArgumentException($"unsupported color depth {depth}bpp");
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
                return Color.FromArgb(Marshal.ReadInt32(Scan0, PointToOffset(x, y)));
            }

            public override byte GetPixelColor(int x, int y, PixelColor color)
            {
                int offset = PointToOffset(x, y);
                if (color == PixelColor.Blue) {
                    return Marshal.ReadByte(Scan0, offset);
                }
                else if (color == PixelColor.Green) {
                    return Marshal.ReadByte(Scan0, offset + 1);
                }
                else if (color == PixelColor.Red) {
                    return Marshal.ReadByte(Scan0, offset + 2);
                }
                else if (color == PixelColor.Alpha) {
                    return Marshal.ReadByte(Scan0, offset + 3);
                }
                else {
                    throw new ArgumentException(nameof(color));
                }
            }

            public override void SetPixel(int x, int y, Color color)
            {
                Marshal.WriteInt32(Scan0, PointToOffset(x, y), color.ToArgb());
            }

            public override void SetPixelColor(int x, int y, byte value, PixelColor color)
            {
                int offset = PointToOffset(x, y);
                if (color == PixelColor.Blue) {
                    Marshal.WriteByte(Scan0, offset, value);
                }
                else if (color == PixelColor.Green) {
                    Marshal.WriteByte(Scan0, offset + 1, value);
                }
                else if (color == PixelColor.Red) {
                    Marshal.WriteByte(Scan0, offset + 2, value);
                }
                else if (color == PixelColor.Alpha) {
                    Marshal.WriteByte(Scan0, offset + 3, value);
                }
                else {
                    throw new ArgumentException(nameof(color));
                }
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
                    byte* lpPixel = (byte*)Scan0.ToPointer() + offset;
                    lpPixel[0] = color.B;
                    lpPixel[1] = color.G;
                    lpPixel[2] = color.R;
                }
            }

            public override void SetPixelColor(int x, int y, byte value, PixelColor color)
            {
                int offset = PointToOffset(x, y);
                if (color == PixelColor.Blue) {
                    Marshal.WriteByte(Scan0, offset, value);
                }
                else if (color == PixelColor.Green) {
                    Marshal.WriteByte(Scan0, offset + 1, value);
                }
                else if (color == PixelColor.Red) {
                    Marshal.WriteByte(Scan0, offset + 2, value);
                }
                else {
                    throw new ArgumentException(nameof(color));
                }
            }
        }

        private class LockedBitmap8bpp : LockedBitmap
        {
            public override int Depth => 24;

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
                if (color == PixelColor.Blue) {
                    return Marshal.ReadByte(Scan0, PointToOffset(x, y));
                }
                else {
                    throw new ArgumentException(nameof(color));
                }
            }

            public override void SetPixel(int x, int y, Color color)
            {
                Marshal.WriteByte(Scan0, PointToOffset(x, y), (byte)color.ToArgb());
            }

            public override void SetPixelColor(int x, int y, byte value, PixelColor color)
            {
                int offset = PointToOffset(x, y);
                if (color == PixelColor.Blue) {
                    Marshal.WriteByte(Scan0, offset, value);
                }
                else {
                    throw new ArgumentException(nameof(color));
                }
            }
        }
    }
}
