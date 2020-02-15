// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
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
            return current += diffs[position % diffs.Length];
        }
    }
}
