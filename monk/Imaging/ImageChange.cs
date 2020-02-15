// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System.Drawing;

namespace Monk.Imaging
{
    public struct ImageChange
    {
        public Point Point { get; private set; }
        public byte OldColor { get; private set; }
        public byte NewColor { get; private set; }

        public ImageChange(int x, int y, byte oldColor, byte newColor)
        {
            Point = new Point(x, y);
            OldColor = oldColor;
            NewColor = newColor;
        }
    }
}
