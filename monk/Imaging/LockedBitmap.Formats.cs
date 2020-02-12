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
using System.Collections.Generic;

using Monk.Collections.Immutable;
using System.Runtime.InteropServices;

namespace Monk.Imaging
{
    public partial class LockedBitmap
    {
        private class LockedBitmap32bpp : LockedBitmap
        {
            public override int Depth => 32;
            public override ISet<PixelColor> SupportedColors { get; } = ImmutableHashSet.Create(PixelColor.Alpha, PixelColor.Red, PixelColor.Green, PixelColor.Blue);

            public LockedBitmap32bpp(Bitmap bitmap)
            {
                Bitmap = bitmap;
                Width = bitmap.Width;
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

        private class LockedBitmap24bpp : LockedBitmap
        {
            public override int Depth => 24;
            public override ISet<PixelColor> SupportedColors { get; } = ImmutableHashSet.Create(PixelColor.Red, PixelColor.Green, PixelColor.Blue);

            public LockedBitmap24bpp(Bitmap bitmap)
            {
                Bitmap = bitmap;
                Width = bitmap.Width;
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

        private class LockedBitmap8bpp : LockedBitmap
        {
            public override int Depth => 8;
            public override ISet<PixelColor> SupportedColors { get; } = ImmutableHashSet.Create(PixelColor.Blue);

            public LockedBitmap8bpp(Bitmap bitmap)
            {
                Bitmap = bitmap;
                Width = bitmap.Width;
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
