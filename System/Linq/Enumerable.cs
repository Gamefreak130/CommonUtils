namespace System.Linq
{
    using System.Collections.Generic;

    static partial class Enumerable
    {
        private sealed class Grouping<K, V> : List<V>, IGrouping<K, V>
        {
            internal Grouping(K key)
            {
                Key = key;
            }

            public K Key { get; private set; }
        }

        private static class Futures<T>
        {
            public static readonly Func<T> Default = () => default(T);
            public static readonly Func<T> Undefined = () => { throw new InvalidOperationException(); };
        }

        private static class Sequence<T>
        {
            public static readonly IEnumerable<T> Empty = new T[0];
        }

        /// <summary>
        /// Returns an empty <see cref="IEnumerable{T}"/> that has the 
        /// specified type argument.
        /// </summary>

        public static IEnumerable<TResult> Empty<TResult>()
        {
            return Sequence<TResult>.Empty;
        }

        /// <summary>
        /// Base implementation of First operator.
        /// </summary>

        private static TSource FirstImpl<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource> empty)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            //Debug.Assert(empty != null);

            var list = source as IList<TSource>;    // optimized case for lists
            if (list != null)
                return list.Count > 0 ? list[0] : empty();

            using (var e = source.GetEnumerator())  // fallback for enumeration
                return e.MoveNext() ? e.Current : empty();
        }

        /// <summary>
        /// Returns the first element of a sequence.
        /// </summary>

        public static TSource First<TSource>(
            this IEnumerable<TSource> source)
        {
            return source.FirstImpl(Futures<TSource>.Undefined);
        }

        /// <summary>
        /// Returns the first element in a sequence that satisfies a specified condition.
        /// </summary>

        public static TSource First<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return First(source.Where(predicate));
        }

        /// <summary>
        /// Returns the first element of a sequence, or a default value if 
        /// the sequence contains no elements.
        /// </summary>

        public static TSource FirstOrDefault<TSource>(
            this IEnumerable<TSource> source)
        {
            return source.FirstImpl(Futures<TSource>.Default);
        }

        /// <summary>
        /// Returns the first element of the sequence that satisfies a 
        /// condition or a default value if no such element is found.
        /// </summary>

        public static TSource FirstOrDefault<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return FirstOrDefault(source.Where(predicate));
        }

        /// <summary>
        /// Base implementation of Last operator.
        /// </summary>

        private static TSource LastImpl<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource> empty)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var list = source as IList<TSource>;    // optimized case for lists
            if (list != null)
                return list.Count > 0 ? list[list.Count - 1] : empty();

            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                    return empty();

                var last = e.Current;
                while (e.MoveNext())
                    last = e.Current;

                return last;
            }
        }

        /// <summary>
        /// Returns the last element of a sequence.
        /// </summary>
        public static TSource Last<TSource>(
            this IEnumerable<TSource> source)
        {
            return source.LastImpl(Futures<TSource>.Undefined);
        }

        /// <summary>
        /// Returns the last element of a sequence that satisfies a 
        /// specified condition.
        /// </summary>

        public static TSource Last<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return Last(source.Where(predicate));
        }

        /// <summary>
        /// Returns the last element of a sequence, or a default value if 
        /// the sequence contains no elements.
        /// </summary>

        public static TSource LastOrDefault<TSource>(
            this IEnumerable<TSource> source)
        {
            return source.LastImpl(Futures<TSource>.Default);
        }

        /// <summary>
        /// Returns the last element of a sequence that satisfies a 
        /// condition or a default value if no such element is found.
        /// </summary>

        public static TSource LastOrDefault<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return LastOrDefault(source.Where(predicate));
        }

        /// <summary>
        /// Base implementation of Single operator.
        /// </summary>

        private static TSource SingleImpl<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource> empty)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            using (var e = source.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    var single = e.Current;
                    if (!e.MoveNext())
                        return single;

                    throw new InvalidOperationException();
                }

                return empty();
            }
        }

        /// <summary>
        /// Returns the only element of a sequence, and throws an exception 
        /// if there is not exactly one element in the sequence.
        /// </summary>

        public static TSource Single<TSource>(
            this IEnumerable<TSource> source)
        {
            return source.SingleImpl(Futures<TSource>.Undefined);
        }

        /// <summary>
        /// Returns the only element of a sequence that satisfies a 
        /// specified condition, and throws an exception if more than one 
        /// such element exists.
        /// </summary>

        public static TSource Single<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return Single(source.Where(predicate));
        }

        /// <summary>
        /// Returns the only element of a sequence, or a default value if 
        /// the sequence is empty; this method throws an exception if there 
        /// is more than one element in the sequence.
        /// </summary>

        public static TSource SingleOrDefault<TSource>(
            this IEnumerable<TSource> source)
        {
            return source.SingleImpl(Futures<TSource>.Default);
        }

        /// <summary>
        /// Returns the only element of a sequence that satisfies a 
        /// specified condition or a default value if no such element 
        /// exists; this method throws an exception if more than one element 
        /// satisfies the condition.
        /// </summary>

        public static TSource SingleOrDefault<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return SingleOrDefault(source.Where(predicate));
        }

        /// <summary>
        /// Returns the element at a specified index in a sequence.
        /// </summary>

        public static TSource ElementAt<TSource>(
            this IEnumerable<TSource> source,
            int index)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (index < 0)
                throw new ArgumentOutOfRangeException("index", index, null);

            var list = source as IList<TSource>;
            if (list != null)
                return list[index];

            try
            {
                return source.SkipWhile((item, i) => i < index).First();
            }
            catch (InvalidOperationException) // if thrown by First
            {
                throw new ArgumentOutOfRangeException("index", index, null);
            }
        }

        /// <summary>
        /// Returns the element at a specified index in a sequence or a 
        /// default value if the index is out of range.
        /// </summary>

        public static TSource ElementAtOrDefault<TSource>(
            this IEnumerable<TSource> source,
            int index)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (index < 0)
                return default(TSource);

            var list = source as IList<TSource>;
            if (list != null)
                return index < list.Count ? list[index] : default(TSource);

            return source.SkipWhile((item, i) => i < index).FirstOrDefault();
        }

        /// <summary>
        /// Determines whether all elements of a sequence satisfy a condition.
        /// </summary>

        public static bool All<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            foreach (var item in source)
                if (!predicate(item))
                    return false;

            return true;
        }

        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// </summary>

        public static bool Any<TSource>(
            this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            using (var e = source.GetEnumerator())
                return e.MoveNext();
        }

        /// <summary>
        /// Determines whether any element of a sequence satisfies a 
        /// condition.
        /// </summary>

        public static bool Any<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return source.Where(predicate).Any();
        }

        /// <summary>
        /// Determines whether a sequence contains a specified element by 
        /// using the default equality comparer.
        /// </summary>

        public static bool Contains<TSource>(
            this IEnumerable<TSource> source,
            TSource value)
        {
            return source.Contains(value, /* comparer */ null);
        }

        /// <summary>
        /// Determines whether a sequence contains a specified element by 
        /// using a specified <see cref="IEqualityComparer{T}" />.
        /// </summary>

        public static bool Contains<TSource>(
            this IEnumerable<TSource> source,
            TSource value,
            IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (comparer == null)
            {
                var collection = source as ICollection<TSource>;
                if (collection != null)
                    return collection.Contains(value);
            }

            comparer = comparer ?? EqualityComparer<TSource>.Default;
            return source.Any(item => comparer.Equals(item, value));
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the 
        /// elements by using the default equality comparer for their type.
        /// </summary>

        public static bool SequenceEqual<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            return first.SequenceEqual(second, /* comparer */ null);
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing their 
        /// elements by using a specified <see cref="IEqualityComparer{T}" />.
        /// </summary>

        public static bool SequenceEqual<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException("frist");
            if (second == null)
                throw new ArgumentNullException("second");

            comparer = comparer ?? EqualityComparer<TSource>.Default;

            using (IEnumerator<TSource> lhs = first.GetEnumerator(),
                                        rhs = second.GetEnumerator())
            {
                do
                {
                    if (!lhs.MoveNext())
                        return !rhs.MoveNext();

                    if (!rhs.MoveNext())
                        return false;
                }
                while (comparer.Equals(lhs.Current, rhs.Current));
            }

            return false;
        }

        /// <summary>
        /// Creates a <see cref="List{T}"/> from an <see cref="IEnumerable{T}"/>.
        /// </summary>

        public static List<TSource> ToList<TSource>(
            this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new List<TSource>(source);
        }

        /// <summary>
        /// Creates an array from an <see cref="IEnumerable{T}"/>.
        /// </summary>

        public static TSource[] ToArray<TSource>(
            this IEnumerable<TSource> source)
        {
            return source.ToList().ToArray();
        }

        /// <summary>
        /// Creates a <see cref="Lookup{TKey,TElement}" /> from an 
        /// <see cref="IEnumerable{T}" /> according to a specified key 
        /// selector function.
        /// </summary>

        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return ToLookup(source, keySelector, e => e, /* comparer */ null);
        }

        /// <summary>
        /// Creates a <see cref="Lookup{TKey,TElement}" /> from an 
        /// <see cref="IEnumerable{T}" /> according to a specified key 
        /// selector function and a key comparer.
        /// </summary>

        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer)
        {
            return ToLookup(source, keySelector, e => e, comparer);
        }

        /// <summary>
        /// Creates a <see cref="Lookup{TKey,TElement}" /> from an 
        /// <see cref="IEnumerable{T}" /> according to specified key 
        /// and element selector functions.
        /// </summary>

        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
        {
            return ToLookup(source, keySelector, elementSelector, /* comparer */ null);
        }

        /// <summary>
        /// Creates a <see cref="Lookup{TKey,TElement}" /> from an 
        /// <see cref="IEnumerable{T}" /> according to a specified key 
        /// selector function, a comparer and an element selector function.
        /// </summary>

        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");

            var lookup = new Lookup<TKey, TElement>(comparer);

            foreach (var item in source)
            {
                var key = keySelector(item);

                var grouping = (Grouping<TKey, TElement>)lookup.Find(key);
                if (grouping == null)
                {
                    grouping = new Grouping<TKey, TElement>(key);
                    lookup.Add(grouping);
                }

                grouping.Add(elementSelector(item));
            }

            return lookup;
        }

        /// <summary>
        /// Creates a <see cref="Dictionary{TKey,TValue}" /> from an 
        /// <see cref="IEnumerable{T}" /> according to a specified key 
        /// selector function.
        /// </summary>

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.ToDictionary(keySelector, /* comparer */ null);
        }

        /// <summary>
        /// Creates a <see cref="Dictionary{TKey,TValue}" /> from an 
        /// <see cref="IEnumerable{T}" /> according to a specified key 
        /// selector function and key comparer.
        /// </summary>

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer)
        {
            return source.ToDictionary(keySelector, e => e, comparer);
        }

        /// <summary>
        /// Creates a <see cref="Dictionary{TKey,TValue}" /> from an 
        /// <see cref="IEnumerable{T}" /> according to specified key 
        /// selector and element selector functions.
        /// </summary>

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector)
        {
            return source.ToDictionary(keySelector, elementSelector, /* comparer */ null);
        }

        /// <summary>
        /// Creates a <see cref="Dictionary{TKey,TValue}" /> from an 
        /// <see cref="IEnumerable{T}" /> according to a specified key 
        /// selector function, a comparer, and an element selector function.
        /// </summary>

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");

            var dict = new Dictionary<TKey, TElement>(comparer);

            foreach (var item in source)
            {
                //
                // ToDictionary is meant to throw ArgumentNullException if
                // keySelector produces a key that is null and 
                // Argument exception if keySelector produces duplicate keys 
                // for two elements. Incidentally, the doucmentation for
                // IDictionary<TKey, TValue>.Add says that the Add method
                // throws the same exceptions under the same circumstances
                // so we don't need to do any additional checking or work
                // here and let the Add implementation do all the heavy
                // lifting.
                //

                dict.Add(keySelector(item), elementSelector(item));
            }

            return dict;
        }
    }
}
