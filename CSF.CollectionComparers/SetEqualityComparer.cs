using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static CSF.Collections.CommonCollectionEqualityComparisonFunctions;

namespace CSF.Collections
{
    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> which compares <see cref="IEnumerable{T}"/> objects using set equality.
    /// Objects are considered equal if they contain equal items, but where the order of the items and the presence of
    /// any duplicate items is irrelevant.
    /// </summary>
    /// <typeparam name="TItem">The type of item within the collections</typeparam>
    public class SetEqualityComparer<TItem> : IEqualityComparer, IEqualityComparer<IEnumerable<TItem>>
    {
        readonly IEqualityComparer<TItem> itemComparer;

        bool IEqualityComparer.Equals(object x, object y) => Equals<TItem>(x, y, Equals);

        int IEqualityComparer.GetHashCode(object obj) => GetHashCode<TItem>(obj, GetHashCode);

        /// <summary>
        /// Determines whether the two enumerable objects are equal.
        /// </summary>
        /// <returns><c>true</c> if the two objects are equal; <c>false</c> otherwise</returns>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The first object to compare.</param>
        public bool Equals(IEnumerable<TItem> x, IEnumerable<TItem> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            var setOne = GetSet(x);

            return setOne.SetEquals(y);
        }

        /// <summary>
        /// Gets a hash code for the given enumerable object.
        /// </summary>
        /// <returns>The hash code.</returns>
        /// <param name="obj">The object for which to get a hash code.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="obj"/> is <c>null</c>.</exception>
        public int GetHashCode(IEnumerable<TItem> obj)
        {
            if (ReferenceEquals(obj, null))
                throw new ArgumentNullException(nameof(obj));

            var set = GetSet(obj);
            return set.Aggregate(0, (acc, next) => acc ^ GetItemHashCode(next, itemComparer));
        }

        ISet<TItem> GetSet(IEnumerable<TItem> collection)
        {
            var set = collection as HashSet<TItem>;
            if (set != null && set.Comparer.Equals(itemComparer))
                return set;

            return new HashSet<TItem>(collection, itemComparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetEqualityComparer{T}"/> class.
        /// </summary>
        public SetEqualityComparer()
        {
            itemComparer = EqualityComparer<TItem>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="itemComparer">An equality comparer by which to compare items within collections.</param>
        public SetEqualityComparer(IEqualityComparer<TItem> itemComparer)
        {
            if (itemComparer == null) throw new ArgumentNullException(nameof(itemComparer));
            this.itemComparer = itemComparer;
        }
    }
}