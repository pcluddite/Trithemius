// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using Monk.Memory;

namespace Monk.Imaging
{
    public abstract partial class LockedBitmap : IDisposable
    {
        internal const int ALPHA_SHIFT   = 0x18;
        internal const int RED_SHIFT     = 0x10;
        internal const int GREEN_SHIFT   = 0x08;
        internal const int BLUE_SHIFT    = 0x00;

        public Bitmap Bitmap { get; protected set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public int BytesPerPixel => Depth / 8;
        public int Size => Height * Width;

        public abstract int Depth { get; }
        public abstract ISet<PixelColor> SupportedColors { get; }

        public virtual bool Locked => BitmapData != null;

        protected BitmapData BitmapData { get; set; }
        protected int Stride { get; set; }

        internal UnmanagedBuffer RawData { get; set; }

        public virtual void LockBits()
        {
            Rectangle rect = new Rectangle(0, 0, Bitmap.Width, Bitmap.Height);
            BitmapData = Bitmap.LockBits(rect, ImageLockMode.ReadWrite, Bitmap.PixelFormat);
            Stride = BitmapData.Stride;
            RawData = new UnmanagedBuffer(BitmapData.Scan0, Stride * BitmapData.Height);
        }

        public virtual void UnlockBits()
        {
            Bitmap.UnlockBits(BitmapData);
            BitmapData = null;
        }

        public abstract int GetPixel(int pixelIndex);

        public virtual int GetPixel(int x, int y)
        {
            return GetPixel(PointToPixelOffset(x, y));
        }

        public virtual byte GetPixelColor(int pixelIndex, PixelColor color)
        {
            if (!SupportedColors.Contains(color)) ThrowHelper.ColorUnsupported(nameof(color), color);
            int value = GetPixel(pixelIndex);
            return (byte)((value >> GetShift(color)) & 0xFF);
        }

        public virtual byte GetPixelColor(int x, int y, PixelColor color)
        {
            return GetPixelColor(PointToPixelOffset(x, y), color);
        }

        public abstract void SetPixel(int pixelIndex, int argb);

        public virtual void SetPixel(int x, int y, int argb)
        {
            SetPixel(PointToPixelOffset(x, y), argb);
        }

        public virtual void SetPixelColor(int x, int y, byte value, PixelColor color)
        {
            SetPixelColor(PointToPixelOffset(x, y), value, color);
        }

        public virtual void SetPixelColor(int pixelOffset, byte value, PixelColor color)
        {
            if (!SupportedColors.Contains(color)) ThrowHelper.ColorUnsupported(nameof(color), color);
            int argb = GetPixel(pixelOffset) & ~(0xFF << GetShift(color));
            SetPixel(pixelOffset, argb | (value << GetShift(color)));
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

        protected int PointToByteOffset(int x, int y)
        {
            EnsureState();
            if ((uint)x >= (uint)Width) throw new ArgumentOutOfRangeException(nameof(x));
            if ((uint)y >= (uint)Height) throw new ArgumentOutOfRangeException(nameof(y));
            return (y * Stride) + (x * BytesPerPixel);
        }

        protected int PointToPixelOffset(int x, int y)
        {
            EnsureState();
            if ((uint)x >= (uint)Width) throw new ArgumentOutOfRangeException(nameof(x));
            if ((uint)y >= (uint)Height) throw new ArgumentOutOfRangeException(nameof(y));
            return (y * Width) + x;
        }

        protected int PixelOffsetToByteOffset(int pixelOffset)
        {
            if ((uint)pixelOffset >= (uint)Size) throw new ArgumentOutOfRangeException(nameof(pixelOffset));
            int x = pixelOffset % Width;
            int y = (pixelOffset - x) / Width;
            return (y * Stride) + (x * BytesPerPixel);
        }

        protected unsafe byte* PixelAt(int pixelOffset)
        {
            return RawData.UnsafePtrAt(PixelOffsetToByteOffset(pixelOffset));
        }

        protected unsafe byte* PixelAt(int x, int y)
        {
            return RawData.UnsafePtrAt(PointToByteOffset(x, y));
        }

        internal virtual int GetBufferIndex(int pixelIndex, PixelColor color)
        {
            throw new NotSupportedException();
        }

        internal void SetByteAt(int byteIndex, byte value)
        {
            RawData[byteIndex] = value;
        }

        internal byte GetByteAt(int byteIndex)
        {
            return RawData[byteIndex];
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
            switch(bitmap.PixelFormat) {
                case PixelFormat.Format32bppArgb: return new LockedBitmap32Bpp(bitmap);
                case PixelFormat.Format24bppRgb: return new LockedBitmap24Bpp(bitmap);
                case PixelFormat.Format8bppIndexed: return new LockedBitmap8Bpp(bitmap);
                default: throw new ArgumentException($"{bitmap.PixelFormat} is not currently supported");
            }
        }

        private static int GetShift(PixelColor color)
        {
            switch(color) { 
                case PixelColor.Alpha: return ALPHA_SHIFT;
                case PixelColor.Red: return RED_SHIFT;
                case PixelColor.Green: return GREEN_SHIFT;
                case PixelColor.Blue: return BLUE_SHIFT;
                default: throw new InvalidOperationException();
            }
        }

        private static class ThrowHelper
        {
            public static void ColorUnsupported(string arg, PixelColor color)
            {
                throw new ArgumentException("unsupported color " + color.ToString(), arg);
            }

            public static void ColorUnsupported(string arg, ISet<PixelColor> colors, ISet<PixelColor> supported)
            {
                ISet<PixelColor> unsupported = new HashSet<PixelColor>(supported);
                unsupported.ExceptWith(colors);
                throw new ArgumentException($"unsupported colors {string.Join(",", unsupported)}", arg);
            }
        }
    }
}
