using System;
using System.Collections;
using System.Collections.Generic;

namespace CSF.Collections
{
    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> which considers finite <see cref="ICollection{T}"/> objects to
    /// be equal simply if their <see cref="ICollection{T}.Count"/> is the same.
    /// </summary>
    public class FiniteCollectionCountComparer<TItem> : IEqualityComparer<ICollection<TItem>>, IEqualityComparer
    {
        /// <summary>
        /// Determines whether the two collection objects are equal.
        /// </summary>
        /// <returns><c>true</c> if the two objects are equal; <c>false</c> otherwise</returns>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The first object to compare.</param>
        public bool Equals(ICollection<TItem> x, ICollection<TItem> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.Count == y.Count;
        }

        /// <summary>
        /// Gets a hash code for the given collection object.
        /// </summary>
        /// <returns>The hash code.</returns>
        /// <param name="obj">The object for which to get a hash code.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="obj"/> is <c>null</c>.</exception>
        public int GetHashCode(ICollection<TItem> obj) => obj?.Count ?? throw new ArgumentNullException(nameof(obj));

        bool IEqualityComparer.Equals(object x, object y)
        {
            return Equals(x as ICollection<TItem>, y as ICollection<TItem>);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            return GetHashCode(obj as ICollection<TItem>);
        }
    }
}