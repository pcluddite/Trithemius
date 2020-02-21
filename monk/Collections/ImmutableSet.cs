// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System.Collections.Generic;

namespace Monk.Collections.Immutable
{
    public static class ImmutableSet
    {
        public static ImmutableSet<T> Create<T>(params T[] items)
        {
            return new ImmutableSet<T>(items);
        }

        public static ImmutableSet<T> Create<T>(IEnumerable<T> items)
        {
            return new ImmutableSet<T>(items);
        }
    }
}
