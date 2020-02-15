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
using System.Collections.Generic;
using System.Linq;

namespace Monk.Memory
{
    internal class ArithmeticProgression
    {
        private int position;

        private readonly int[] diffs;
        private int current;

        public int Start { get; }
        public IEnumerable<int> Differences => diffs;

        public int Position
        {
            get => position;
            set => throw new NotSupportedException();
        }

        public ArithmeticProgression(int start, int diff)
        {
            Start = start;
            diffs = new int[] { diff };
            current = Start;
        }

        public ArithmeticProgression(int start, IEnumerable<int> diffs)
        {
            Start = start;
            this.diffs = diffs.ToArray();
            current = Start;
        }

        public ArithmeticProgression(int start, IEnumerable<ushort> seed)
        {
            Start = start;
            List<ushort> seedlist = new List<ushort>(seed);
            if (seedlist.Count == 0) {
                diffs = new int[] { 0 };
            }
            else {
                diffs = new int[seedlist.Count];
                for (int idx = 0; idx < seedlist.Count; ++idx)
                    diffs[idx] = seedlist[idx] + 1;
            }
            current = Start;
        }

        public int Next()
        {
            return current += diffs[position++ % diffs.Length];
        }

        public int Peek()
        {
            return current += diffs[(position + 1) % diffs.Length];
        }
    }
}
