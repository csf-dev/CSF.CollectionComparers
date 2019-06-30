using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static CSF.Collections.CommonCollectionEqualityComparisonFunctions;

namespace CSF.Collections
{
    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> which compares <see cref="IEnumerable{T}"/> objects using list equality.
    /// Objects are considered equal if they contain equal items, with the same counts and in the same order.
    /// </summary>
    /// <typeparam name="TItem">The type of item within the collections</typeparam>
    public class ListEqualityComparer<TItem> : IEqualityComparer, IEqualityComparer<IEnumerable<TItem>>
    {
        readonly IEqualityComparer<TItem> itemComparer;

        bool IEqualityComparer.Equals(object x, object y) => Equals<TItem>(x, y, Equals);

        int IEqualityComparer.GetHashCode(object obj) => GetHashCode<TItem>(obj, GetHashCode);

        bool DoFiniteCollectionCountsDiffer(IEnumerable<TItem> x, IEnumerable<TItem> y)
        {
            var finiteCollectionComparer = new FiniteCollectionCountComparer<TItem>();
            return !finiteCollectionComparer.Equals(x as ICollection<TItem>, y as ICollection<TItem>);
        }

        /// <summary>
        /// Determines whether the two enumerable objects are equal.
        /// </summary>
        /// <returns><c>true</c> if the two objects are equal; <c>false</c> otherwise</returns>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The first object to compare.</param>
        public bool Equals(IEnumerable<TItem> x, IEnumerable<TItem> y)
        {
            if(ReferenceEquals(x, y)) return true;
            if(ReferenceEquals(x, null)) return false;
            if(ReferenceEquals(y, null)) return false;

            if(DoFiniteCollectionCountsDiffer(x, y))
                return false;

            return x.SequenceEqual(y, itemComparer);
        }

        /// <summary>
        /// Gets a hash code for the given enumerable object.
        /// </summary>
        /// <returns>The hash code.</returns>
        /// <param name="obj">The object for which to get a hash code.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="obj"/> is <c>null</c>.</exception>
        public int GetHashCode(IEnumerable<TItem> obj)
        {
            if(ReferenceEquals(obj, null))
                throw new ArgumentNullException(nameof(obj));

            unchecked
            {
                return obj.Aggregate(19, (acc, next) => acc * 31 + GetItemHashCode(next, itemComparer));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListEqualityComparer{T}"/> class.
        /// </summary>
        public ListEqualityComparer() : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="itemComparer">An equality comparer by which to compare items within collections.</param>
        public ListEqualityComparer(IEqualityComparer<TItem> itemComparer)
        {
            this.itemComparer = itemComparer ?? EqualityComparer<TItem>.Default;
        }
    }
}