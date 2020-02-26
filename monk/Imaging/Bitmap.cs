// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System;
using System.Collections.Generic;

using Monk.Collections.Immutable;

namespace Monk.Imaging
{
    public partial class Bitmap
    {
        internal const int ALPHA_SHIFT = 0x18;
        internal const int RED_SHIFT = 0x10;
        internal const int GREEN_SHIFT = 0x08;
        internal const int BLUE_SHIFT = 0x00;

        private readonly byte[] buffer;

        public int Width { get; }
        public int Height { get; }
        public int Size => Height * Width;
        public ISet<PixelColor> SupportedColors { get; } = ImmutableSet.Create(PixelColor.Alpha, PixelColor.Red, PixelColor.Green, PixelColor.Blue);
        public int BytesPerPixel { get; } = sizeof(int);

        public byte GetPixelColor(int x, int y, PixelColor color)
        {
            return GetPixelColor(PointToPixelOffset(x, y), color);
        }

        public byte GetPixelColor(int pixelOffset, PixelColor color)
        {
            return buffer[PixelOffsetToByteOffset(pixelOffset) + (int)color];
        }

        public int GetPixel(int x, int y)
        {
            return GetPixel(PointToPixelOffset(x, y));
        }

        public int GetPixel(int pixelOffset)
        {
            int buffIdx = PixelOffsetToByteOffset(pixelOffset);
            int value = buffer[buffIdx + 3] << ALPHA_SHIFT;
            value |= buffer[buffIdx + 2] << RED_SHIFT;
            value |= buffer[buffIdx + 1] << GREEN_SHIFT;
            value |= buffer[buffIdx + 0] << BLUE_SHIFT;
            return value;
        }

        public void SetPixel(int x, int y, int argb)
        {
            SetPixel(PointToPixelOffset(x, y), argb);
        }

        public void SetPixelColor(int x, int y, byte value, PixelColor color)
        {
            SetPixelColor(PointToPixelOffset(x, y), value, color);
        }

        public void SetPixelColor(int pixelOffset, byte value, PixelColor color)
        {
            buffer[PixelOffsetToByteOffset(pixelOffset) + (int)color] = value;
        }

        public void SetPixel(int pixelOffset, int argb)
        {
            int buffIdx = PixelOffsetToByteOffset(pixelOffset);
            buffer[buffIdx + 0] = (byte)((argb >> BLUE_SHIFT) & 0xff);
            buffer[buffIdx + 1] = (byte)((argb >> GREEN_SHIFT) & 0xff);
            buffer[buffIdx + 2] = (byte)((argb >> RED_SHIFT) & 0xff);
            buffer[buffIdx + 3] = (byte)((argb >> ALPHA_SHIFT) & 0xff);
        }

        protected int PixelOffsetToByteOffset(int pixelOffset)
        {
            if ((uint)pixelOffset >= (uint)Size) throw new ArgumentOutOfRangeException(nameof(pixelOffset));
            int x = pixelOffset % Width;
            int y = (pixelOffset - x) / Width;
            return (y * Width) + (x * BytesPerPixel);
        }

        protected int PointToPixelOffset(int x, int y)
        {
            if ((uint)x >= (uint)Width) throw new ArgumentOutOfRangeException(nameof(x));
            if ((uint)y >= (uint)Height) throw new ArgumentOutOfRangeException(nameof(y));
            return (y * Width) + x;
        }

        internal int GetBufferIndex(int pixelIndex, PixelColor color)
        {
            return buffer[PixelOffsetToByteOffset(pixelIndex) + (int)color];
        }

        internal void SetByteAt(int byteIndex, byte value)
        {
            buffer[byteIndex] = value;
        }

        internal byte GetByteAt(int byteIndex)
        {
            return buffer[byteIndex];
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
