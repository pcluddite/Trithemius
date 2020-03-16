// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Monk.Collections.Immutable;

namespace Monk.Imaging
{
    public partial class LockedBitmap
    {
        private class LockedBitmap32Bpp : LockedBitmap
        {
            public override int Depth => 32;

            public override ISet<PixelColor> SupportedColors { get; } =
                ImmutableSet.Create(PixelColor.Alpha, PixelColor.Red, PixelColor.Green, PixelColor.Blue);

            public LockedBitmap32Bpp(Bitmap bitmap)
            {
                Bitmap = bitmap;
                Width  = bitmap.Width;
                Height = bitmap.Height;
            }

            public override unsafe int GetPixel(int pixelOffset)
            {
                return Marshal.ReadInt32(new IntPtr(PixelAt(pixelOffset)));
            }

            public override unsafe void SetPixel(int pixelOffset, int argb)
            {
                Marshal.WriteInt32(new IntPtr(PixelAt(pixelOffset)), argb);
            }

            internal override int GetBufferIndex(int pixelIndex, PixelColor color)
            {
                return PixelOffsetToByteOffset(pixelIndex) + GetShift(color) / 8;
            }
        }

        private class LockedBitmap24Bpp : LockedBitmap
        {
            public override int Depth => 24;
            public override ISet<PixelColor> SupportedColors { get; } = ImmutableSet.Create(PixelColor.Red, PixelColor.Green, PixelColor.Blue);

            public LockedBitmap24Bpp(Bitmap bitmap)
            {
                Bitmap = bitmap;
                Width  = bitmap.Width;
                Height = bitmap.Height;
            }

            public override unsafe int GetPixel(int pixelOffset)
            {
                byte* pixel = PixelAt(pixelOffset);
                int value = 0xff << ALPHA_SHIFT;
                value |= pixel[2] << RED_SHIFT;
                value |= pixel[1] << GREEN_SHIFT;
                value |= pixel[0] << BLUE_SHIFT;
                return value;
            }

            public override unsafe void SetPixel(int pixelOffset, int argb)
            {
                byte* pixel = PixelAt(pixelOffset);
                pixel[0] = (byte)((argb >> BLUE_SHIFT) & 0xff);
                pixel[1] = (byte)((argb >> GREEN_SHIFT) & 0xff);
                pixel[2] = (byte)((argb >> RED_SHIFT) & 0xff);
            }

            internal override int GetBufferIndex(int pixelIndex, PixelColor color)
            {
                if (color == PixelColor.Alpha) ThrowHelper.ColorUnsupported(nameof(color), color);
                return PixelOffsetToByteOffset(pixelIndex) + GetShift(color) / 8;
            }
        }

        private class LockedBitmap8Bpp : LockedBitmap
        {
            public override int Depth => 8;
            public override ISet<PixelColor> SupportedColors { get; } = ImmutableSet.Create(PixelColor.Blue);

            public LockedBitmap8Bpp(Bitmap bitmap)
            {
                Bitmap = bitmap;
                Width  = bitmap.Width;
                Height = bitmap.Height;
            }

            public override unsafe int GetPixel(int pixelOffset)
            {
                return PixelAt(pixelOffset)[0];
            }

            public override unsafe void SetPixel(int pixelOffset, int argb)
            {
                PixelAt(pixelOffset)[0] = (byte)argb;
            }

            internal override int GetBufferIndex(int pixelIndex, PixelColor color)
            {
                if (color != PixelColor.Blue) ThrowHelper.ColorUnsupported(nameof(color), color);
                return PixelOffsetToByteOffset(pixelIndex);
            }
        }
    }
}
