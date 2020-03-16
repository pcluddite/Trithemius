// =====
//
// Copyright (c) 2013-2020 Timothy Baxendale
//
// =====
using System;
using System.Collections;
using System.Collections.Generic;

namespace Monk.Collections.Immutable
{
    public sealed class ImmutableSet<T> : ISet<T>
    {
        private readonly HashSet<T> _hashSet;

        internal ImmutableSet(IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            _hashSet = new HashSet<T>(items);
            _hashSet.TrimExcess();
        }

        public int  Count      => _hashSet.Count;
        public bool IsReadOnly => true;

        public bool Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            return _hashSet.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _hashSet.CopyTo(array, arrayIndex);
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _hashSet.GetEnumerator();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new NotSupportedException();
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return _hashSet.IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return _hashSet.IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return _hashSet.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return _hashSet.IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return _hashSet.Overlaps(other);
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return _hashSet.SetEquals(other);
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotSupportedException();
        }

        public void UnionWith(IEnumerable<T> other)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}