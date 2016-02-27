using CeMaS.Common.Identity;
using CeMaS.Common.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace CeMaS.Common.Collections
{
    /// <summary>
    /// Helps with collections.
    /// </summary>
    public static class CollectionHelper
    {
        #region Items with identifiers

        /// <summary>
        /// Gets identifiers from <paramref name="items"/>.
        /// </summary>
        /// <typeparam name="T">Item identifier type.</typeparam>
        /// <param name="items">Items.</param>
        /// <returns>Identifiers if <paramref name="items"/> is not null, otherwise null.</returns>
        public static IEnumerable<T> Ids<T>(this IEnumerable<IId<T>> items)
        {
            return items == null ?
                null :
                items.
                    Select(i => i.Id);
        }

        public static IEnumerable<TItem> With<TId, TItem>(this IEnumerable<TItem> items, IEnumerable<TId> ids)
            where TItem : IId<TId>
        {
            return items.
                Where(i => ids.Contains(i.Id));
        }

        public static int IndexOf<TId, TItem>(this INotifyList<TId, TItem> items, TId id)
        {
            TItem item = items.GetItem(id);
            return items.IndexOf(item);
        }

        public static TItem TryGetItemOrDefault<TId, TItem>(
            this IItemLookUp<TId, TItem> items,
            TId id,
            TItem defaultValue = default(TItem)
            )
        {
            items.ValidateNonNull("items");
            return items.
                TryGetItem(id).
                ValueOrDefault(defaultValue);
        }
        public static IEnumerable<TItem> GetItems<TId, TItem>(
            this IItemLookUp<TId, TItem> items,
            IEnumerable<TId> ids
            )
        {
            items.ValidateNonNull("items");
            ids.ValidateNonNull("ids");
            return ids.
                Select(id => items.GetItem(id));
        }
        public static IEnumerable<Optional<TItem>> TryGetItems<TId, TItem>(
            this IItemLookUp<TId, TItem> items,
            IEnumerable<TId> ids
            )
        {
            items.ValidateNonNull("items");
            ids.ValidateNonNull("ids");
            return ids.
                Select(id => items.TryGetItem(id));
        }

        #endregion

        #region Enumerable

        /// <summary>
        /// Whether <paramref name="items"/> is null or empty.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="items">Items.</param>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
        {
            return
                items == null ||
                !items.Any();
        }
        /// <summary>
        /// Whether <paramref name="items"/> has single only item.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="items">Items.</param>
        public static bool HasSingleItem<T>(this IEnumerable<T> items)
        {
            using (var enumerator = items.GetEnumerator())
            {
                return
                    // first item
                    enumerator.MoveNext() &&
                    // no second item
                    !enumerator.MoveNext();
            }
        }
        /// <summary>
        /// Whether <paramref name="items"/> has more than one item.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="items">Items.</param>
        public static bool HasMoreThanOneItem<T>(this IEnumerable<T> items)
        {
            return items.Skip(1).Any();
        }
        /// <summary>
        /// Gets empty enumerable if <paramref name="items"/> is null, otherwise <paramref name="items"/>.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="items">Items.</param>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> items)
        {
            return items == null ?
                Enumerable.Empty<T>() :
                items;
        }
        /// <summary>
        /// Filters non-null <paramref name="items"/>.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="items">Items. Can be null.</param>
        /// <returns>Non-null <paramref name="items"/> if it is not null, otherwise null.</returns>
        public static IEnumerable<T> NonNull<T>(this IEnumerable<T> items)
            where T : class
        {
            return items == null ?
                null :
                items.
                    Where(i => i != null);
        }
        /// <summary>
        /// Converts <paramref name="item"/> to enumerable by enumerating it <paramref name="count"/> times.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="item">Item.</param>
        /// <param name="count">Number of <paramref name="item"/>s to enumerate.</param>
        public static IEnumerable<T> ToEnumerable<T>(this T item, int count = 1)
        {
            return Enumerable.Repeat<T>(item, count);
        }

        /// <summary>
        /// Adds <paramref name="item"/> at the end of <paramref name="items"/>.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="items">Items. Can be null.</param>
        /// <param name="item">Item.</param>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> items, T item)
        {
            var itemEnum = item.ToEnumerable();
            return items == null ?
                itemEnum :
                items.Concat(itemEnum);
        }
        /// <summary>
        /// Adds <paramref name="item"/> at the beginning of <paramref name="items"/>.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="items">Items. Can be null.</param>
        /// <param name="item">Item.</param>
        public static IEnumerable<T> Concat<T>(this T item, IEnumerable<T> items)
        {
            var itemEnum = item.ToEnumerable();
            return items == null ?
                itemEnum :
                itemEnum.Concat(items);
        }
        /// <summary>
        /// Removes <paramref name="item"/> from <paramref name="items"/>.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="items">Items. Can be null.</param>
        /// <param name="item">Item.</param>
        /// <param name="comparer">Optional item equality comparer.</param>
        /// <returns><paramref name="items"/> without <paramref name="item"/> if <paramref name="items"/> is not null, otherwise null.</returns>
        public static IEnumerable<T> Except<T>(
            this IEnumerable<T> items,
            T item,
            IEqualityComparer<T> comparer = null
            )
        {
            return items == null ?
                null :
                comparer == null ?
                    items.Except(item.ToEnumerable()) :
                    items.Except(item.ToEnumerable(), comparer);
        }
        /// <summary>
        /// Converts <paramref name="items"/> to array.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="items">Items.</param>
        /// <returns>Item array if <paramref name="items"/> is not null, otherwise null.</returns>
        public static T[] ToArrayOrNull<T>(this IEnumerable<T> items)
        {
            return items == null ?
                null :
                items.ToArray();
        }
        /// <summary>
        /// Converts <paramref name="items"/> to array.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="items">Items.</param>
        /// <returns>Item array if <paramref name="items"/> is not null, otherwise null.</returns>
        public static List<T> ToListOrNull<T>(this IEnumerable<T> items)
        {
            return items == null ?
                null :
                items.ToList();
        }

        #endregion

        #region Dictionary

        public static Optional<V> GetValue<K, V>(this IDictionary<K, V> items, K key)
        {
            ValidateNonNull(items);
            V value;
            return items.TryGetValue(key, out value) ?
                value :
                Optional<V>.None;
        }
        public static V GetValue<K, V>(this IDictionary<K, object> items, K key, V defaultValue = default(V))
        {
            object value = items.GetValue(key).ValueOrDefault();
            return value is V ?
                (V)value :
                defaultValue;
        }
        public static V GetValue<V>(this IDictionary<string, object> items, string key, V defaultValue = default(V))
        {
            return items.GetValue<string, V>(key, defaultValue);
        }

        #endregion

        #region Changes

        #region Add

        public static void AddIfNew<T>(this INotifyCollection<T> items, T item)
        {
            if (!items.Contains(item))
                items.Add(item);
        }
        public static void AddIfNew<T>(this INotifyCollection<T> items, IEnumerable<T> itemsToAdd)
        {
            items.ValidateNonNull("items");
            itemsToAdd.ValidateNonNull("itemsToAdd");
            foreach (var item in itemsToAdd)
                items.AddIfNew(item);
        }
        public static void AddIfNew<TId, TItem>(this INotifyCollection<TId, TItem> items, TItem item)
            where TItem : IId<TId>
        {
            if (!items.Contains(item.Id))
                items.Add(item);
        }
        public static void AddIfNew<TId, TItem>(this INotifyCollection<TId, TItem> items, IEnumerable<TItem> itemsToAdd)
            where TItem : IId<TId>
        {
            items.ValidateNonNull("items");
            itemsToAdd.ValidateNonNull("itemsToAdd");
            foreach (var item in itemsToAdd)
                items.AddIfNew(item);
        }

        #endregion

        #region Replace

        public static bool Replace<TId, TItem>(this INotifyList<TId, TItem> items, TId id, TItem item)
        {
            int index = items.IndexOf(id);
            bool result = index >= 0;
            if (result)
                items[index] = item;
            else
                items.Add(item);
            return result;
        }
        public static bool Replace<TId, TItem>(this INotifyList<TId, TItem> items, IEnumerable<TItem> newItems)
            where TItem : IId<TId>
        {
            bool result = false;
            foreach (var newItem in newItems)
            {
                if (items.Replace(newItem.Id, newItem))
                    result = true;
            }
            return result;
        }

        #endregion

        #region Remove

        public static void Remove<T>(this ICollection<T> items, IEnumerable<T> itemsToBeRemoved)
        {
            foreach (var item in itemsToBeRemoved)
                items.Remove(item);
        }

        #endregion

        /// <summary>
        /// Allows to observe <paramref name="items"/> changes (additions and removals).
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="items">Items.</param>
        /// <returns>Change observable.</returns>
        public static IObservable<Unit> Changed<T>(this INotifyCollectionChange<T> items)
        {
            return items.Added.
                Merge(items.Removed).
                Select(_ => Unit.Default);
        }

        public static IDisposable OnChange<T>(this INotifyCollectionChange<T> items,
            Action<IEnumerable<T>> onAdded,
            Action<IEnumerable<T>> onRemoved)
        {
            return new CompositeDisposable(
                items.Added.Subscribe(onAdded),
                items.Removed.Subscribe(onRemoved)
                );
        }

        public static IDisposable OnChange<T>(this INotifyCollectionChange<T> items,
            Action<T> onAdded,
            Action<T> onRemoved)
        {
            return items.OnChange(
                i =>
                {
                    foreach (var item in i)
                        onAdded(item);
                },
                i =>
                {
                    foreach (var item in i)
                        onRemoved(item);
                }
                );
        }

        #endregion

        #region Sort

        public static void BubbleSort<T>(this IList<T> items, Comparison<T> compare)
        {
            items.ValidateNonNull("items");
            compare.ValidateNonNull("compare");
            int count = items.Count;
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = 0; j < count - i - 1; j++)
                {
                    T item1 = items[j];
                    T item2 = items[j + 1];
                    if (compare(item1, item2) > 1)
                    {
                        T tmp = item2;
                        items[j + 1] = item1;
                        items[j] = tmp;
                    }
                }
            }
        }

        #endregion

        #region KeyValuePair

        /// <summary>
        /// Creates new <see cref="KeyValuePair{Task, Task}"/> from <paramref name="key"/> and <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="K">Key type.</typeparam>
        /// <typeparam name="V">Value type.</typeparam>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public static KeyValuePair<K, V> ToPair<K, V>(this K key, V value)
        {
            return new KeyValuePair<K, V>(key, value);
        }

        #endregion

        private static void ValidateNonNull(IEnumerable items)
        {
            items.ValidateNonNull(nameof(items));
        }
    }
}
