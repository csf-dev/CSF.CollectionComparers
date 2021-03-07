using System.Collections.Generic;

namespace CSF.Collections
{
    /// <summary>
    /// Extension methods for collections, in order to quickly perform various equality tests.
    /// </summary>
    public static class CollectionComparisonExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the collections are equal, making use of set equality.
        /// </summary>
        /// <seealso cref="SetEqualityComparer{TItem}"/>
        /// <param name="first">The first collection.</param>
        /// <param name="second">The second collection.</param>
        /// <param name="itemComparer">An optional comparer used to determine if items are equal.</param>
        /// <typeparam name="T">The type of the items in the collections.</typeparam>
        /// <returns><c>true</c> if the collections are equal (using set equality); <c>false</c> otherwise.</returns>
        public static bool SetEquals<T>(this IEnumerable<T> first,
                                        IEnumerable<T> second,
                                        IEqualityComparer<T> itemComparer = null)
            => new SetEqualityComparer<T>(itemComparer ?? EqualityComparer<T>.Default).Equals(first, second);

        /// <summary>
        /// Gets a value indicating whether the collections are equal, making use of bag equality.
        /// </summary>
        /// <seealso cref="BagEqualityComparer{TItem}"/>
        /// <param name="first">The first collection.</param>
        /// <param name="second">The second collection.</param>
        /// <param name="itemComparer">An optional comparer used to determine if items are equal.</param>
        /// <typeparam name="T">The type of the items in the collections.</typeparam>
        /// <returns><c>true</c> if the collections are equal (using bag equality); <c>false</c> otherwise.</returns>
        public static bool BagEquals<T>(this IEnumerable<T> first,
                                        IEnumerable<T> second,
                                        IEqualityComparer<T> itemComparer = null)
            => new BagEqualityComparer<T>(itemComparer ?? EqualityComparer<T>.Default).Equals(first, second);

        /// <summary>
        /// Gets a value indicating whether the collections are equal, making use of list/sequence equality.
        /// </summary>
        /// <seealso cref="ListEqualityComparer{TItem}"/>
        /// <param name="first">The first collection.</param>
        /// <param name="second">The second collection.</param>
        /// <param name="itemComparer">An optional comparer used to determine if items are equal.</param>
        /// <typeparam name="T">The type of the items in the collections.</typeparam>
        /// <returns><c>true</c> if the collections are equal (using list equality); <c>false</c> otherwise.</returns>
        public static bool ListEquals<T>(this IEnumerable<T> first,
                                         IEnumerable<T> second,
                                         IEqualityComparer<T> itemComparer = null)
            => new ListEqualityComparer<T>(itemComparer ?? EqualityComparer<T>.Default).Equals(first, second);
    }
}