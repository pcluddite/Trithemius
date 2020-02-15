// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System;

namespace Monk.Memory
{
    internal static class MathExtensions
    {
        /// <summary>
        /// Performs integer division but rounds up
        /// </summary>
        public static int DivideUp(this int dividend, int divisor)
        {
            return dividend / divisor + Math.Min(1, dividend % divisor);
        }

        /// <summary>
        /// Count the number of digits in an integer
        /// </summary>
        public static int CountDigits(this long value)
        {
            if (value == 0) return 1;
            return (int)Math.Floor(Math.Log10(value) + 1);
        }
    }
}
