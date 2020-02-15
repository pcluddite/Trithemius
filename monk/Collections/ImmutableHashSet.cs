// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System.Collections.Generic;

namespace Monk.Collections.Immutable
{
    public static class ImmutableHashSet
    {
        public static ImmutableHashSet<T> Create<T>(params T[] items)
        {
            return new ImmutableHashSet<T>(items);
        }

        public static ImmutableHashSet<T> Create<T>(IEnumerable<T> items)
        {
            return new ImmutableHashSet<T>(items);
        }
    }
}
