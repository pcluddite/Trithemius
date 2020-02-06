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
    public class ArithmeticProgression : InfiniteSequence<int>
    {
        private readonly int[] diffs;
        private readonly ArrayProgression<int> diffSequence;
        private int current;
        private int v;

        public int Start { get; }
        public IEnumerable<int> Differences => diffs;

        public override int Position { get => base.Position; set => throw new NotSupportedException(); }

        public ArithmeticProgression(int start, int diff)
        {
            Start = start;
            diffs = new int[] { diff };
            diffSequence = new ArrayProgression<int>(diffs);
            current = Start;
        }

        public ArithmeticProgression(int start, IEnumerable<int> diffs)
        {
            Start = start;
            this.diffs = diffs.ToArray();
            diffSequence = new ArrayProgression<int>(this.diffs);
            current = Start;
        }

        public ArithmeticProgression(int start, Seed seed)
            : this(start, seed, 0)
        {
        }

        public ArithmeticProgression(int start, Seed seed, int offset) 
        {
            Start = start;
            diffs = new int[seed.Count];
            var seq = seed.GetSequence();
            seq.Position = offset;
            for (int idx = 0; idx < seed.Count; ++idx)
                diffs[idx] = seq.Next() + 1;
            diffSequence = new ArrayProgression<int>(diffs);
            current = Start;
        }

        public override int Next()
        {
            current += diffSequence.Next();
            ++base.Position;
            return current;
        }

        public override int Peek()
        {
            return current + diffSequence.Peek();
        }
    }
}
