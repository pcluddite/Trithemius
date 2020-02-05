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

namespace Monk.Imaging
{
    internal struct CachedPixel
    {
        private byte value;

        public int X { get; }
        public int Y { get; }
        public PixelColor Color { get; }
        public bool Changed { get; private set; }

        public byte Value { 
            get => value; 
            set {
                if (value != this.value) Changed = true;
                this.value = value;
            }
        }

        public CachedPixel(int x, int y, byte value, PixelColor color)
        {
            X = x;
            Y = y;
            Color = color;
            this.value = value;
            Changed = false;
        }
    }
}
