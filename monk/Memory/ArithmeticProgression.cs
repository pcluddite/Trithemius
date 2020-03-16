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
        private int _position;

        private readonly int[] _diffs;
        private int _current;

        public int Start { get; }
        public IEnumerable<int> Differences => _diffs;

        public int Position { get => _position; set => throw new NotSupportedException(); }

        public ArithmeticProgression(int start, int diff)
        {
            Start    = start;
            _diffs   = new[] {diff};
            _current = Start;
        }

        public ArithmeticProgression(int start, IEnumerable<int> diffs)
        {
            Start    = start;
            _diffs   = diffs.ToArray();
            _current = Start;
        }

        public ArithmeticProgression(int start, IEnumerable<ushort> seed)
        {
            Start = start;
            List<ushort> seedlist = new List<ushort>(seed);

            if (seedlist.Count == 0) {
                _diffs = new[] {0};
            } else {
                _diffs = new int[seedlist.Count];

                for (int idx = 0; idx < seedlist.Count; ++idx)
                    _diffs[idx] = seedlist[idx] + 1;
            }

            _current = Start;
        }

        public int Next()
        {
            return _current += _diffs[_position++ % _diffs.Length];
        }

        public int Peek()
        {
            return _current += _diffs[_position % _diffs.Length];
        }
    }
}
