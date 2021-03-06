﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static CSF.Collections.CommonCollectionEqualityComparisonFunctions;

namespace CSF.Collections
{
    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> which compares <see cref="IEnumerable{T}"/> objects using bag equality.
    /// Objects are considered equal if they contain equal items, in the same number, but where the order of the
    /// items is irrelevant.
    /// </summary>
    /// <typeparam name="TItem">The type of item within the collections</typeparam>
    public class BagEqualityComparer<TItem> : IEqualityComparer, IEqualityComparer<IEnumerable<TItem>>
    {
        readonly IEqualityComparer<TItem> itemEqComparer;
        readonly IComparer<TItem> itemComparer;

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

            var setResult = TryCompareAsSets(x, y);
            if (setResult.HasValue) return setResult.Value;

            if (DoFiniteCollectionCountsDiffer(x, y))
                return false;

            if(typeof(TItem).GetTypeInfo().IsSubclassOf(typeof(IComparable)))
                return AreCollectionsOfComparableItemsEqual(x, y);

            return !DoCollectionsDifferByElementEquality(x, y);
        }

        bool AreCollectionsOfComparableItemsEqual(IEnumerable<TItem> x, IEnumerable<TItem> y)
        {
            var coll1 = x.OrderBy(i => i, itemComparer);
            var coll2 = y.OrderBy(i => i, itemComparer);

            return coll1.SequenceEqual(coll2, itemEqComparer);
        }

        bool? TryCompareAsSets(IEnumerable<TItem> x, IEnumerable<TItem> y)
        {
            var set1 = x as HashSet<TItem>;
            var set2 = y as HashSet<TItem>;

            if (set1 != null
                && set2 != null
                && set1.Comparer.Equals(set2.Comparer)
                && set1.Comparer.Equals(itemComparer))
            {
                var comparer = new SetEqualityComparer<TItem>(itemEqComparer);
                return comparer.Equals(set1, set2);
            }

            return null;
        }

        bool DoFiniteCollectionCountsDiffer(IEnumerable<TItem> x, IEnumerable<TItem> y)
        {
            var finiteCollectionComparer = new FiniteCollectionCountComparer<TItem>();
            return !finiteCollectionComparer.Equals(x as ICollection<TItem>, y as ICollection<TItem>);
        }

        bool DoCollectionsDifferByElementEquality(IEnumerable<TItem> first, IEnumerable<TItem> second)
        {
            int firstNullCount;
            int secondNullCount;

            var firstElementCounts = GetCountsOfDistinctItems(first, out firstNullCount);
            var secondElementCounts = GetCountsOfDistinctItems(second, out secondNullCount);

            // An optimization to avoid a full comparison, if the counts of contents do not match
            if (firstNullCount != secondNullCount || firstElementCounts.Count != secondElementCounts.Count)
                return true;

            foreach (var kvp in firstElementCounts)
            {
                var firstElementCount = kvp.Value;
                int secondElementCount;
                secondElementCounts.TryGetValue(kvp.Key, out secondElementCount);

                if (firstElementCount != secondElementCount)
                    return true;
            }

            return false;
        }

        Dictionary<TItem, int> GetCountsOfDistinctItems(IEnumerable<TItem> enumerable, out int countOfNullItems)
        {
            var dictionary = new Dictionary<TItem, int>(itemEqComparer);
            countOfNullItems = 0;

            foreach (var element in enumerable)
            {
                if (ReferenceEquals(element, null))
                {
                    countOfNullItems ++;
                    continue;
                }

                int timesThisElementSeen;
                dictionary.TryGetValue(element, out timesThisElementSeen);
                timesThisElementSeen++;
                dictionary[element] = timesThisElementSeen;
            }

            return dictionary;
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

            var counts = new Dictionary<object, long>();
            foreach(object current in obj)
            {
                var item = current ?? new NullObject();
                if (!counts.ContainsKey(item)) counts.Add(item, 0);
                counts[item] = counts[item] + 1;
            }

            return counts.Aggregate(0, (acc, next) =>
            {
                unchecked
                {
                    var itemHash = next.Key is TItem item ? GetItemHashCode(item, itemEqComparer) : 31;
                    return acc ^ itemHash ^ next.Value.GetHashCode();
                }
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BagEqualityComparer{T}"/> class.
        /// </summary>
        public BagEqualityComparer() : this(null, null) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="BagEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="itemEqComparer">An equality comparer by which to compare items within collections.</param>
        public BagEqualityComparer(IEqualityComparer<TItem> itemEqComparer) : this(itemEqComparer, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BagEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="itemComparer">An item comparer used to determine an order of items within collections.</param>
        public BagEqualityComparer(IComparer<TItem> itemComparer) : this(null, itemComparer) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BagEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="itemEqComparer">An equality comparer by which to compare items within collections.</param>
        /// <param name="itemComparer">An item comparer used to determine an order of items within collections.</param>
        public BagEqualityComparer(IEqualityComparer<TItem> itemEqComparer, IComparer<TItem> itemComparer)
        {
            this.itemEqComparer = itemEqComparer ?? EqualityComparer<TItem>.Default;
            this.itemComparer = itemComparer ?? Comparer<TItem>.Default;
        }

        /// <summary>
        /// This object is used to represent nulls when getting a hash code.  This is because an actual null is not permitted
        /// as a dictionary key.
        /// </summary>
        class NullObject
        {
            public override bool Equals(object obj) => obj is NullObject;

            public override int GetHashCode() => 31;
        }
    }
}