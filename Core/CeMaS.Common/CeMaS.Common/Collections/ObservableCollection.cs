using CeMaS.Common.Identity;
using CeMaS.Common.Validation;
using CeMaS.Common.ValueDomains;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Subjects;

namespace CeMaS.Common.Collections
{
    public class ObservableCollection<T> :
        System.Collections.ObjectModel.ObservableCollection<T>,
        INotifyList<T>,
        IValueDomain<T>
    {
        public ObservableCollection(IEnumerable<T> items = null)
            : base(items == null ? new T[0] : items)
        { }

        public IObservable<IEnumerable<T>> Added
        {
            get
            {
                return added;
            }
        }
        public IObservable<IEnumerable<T>> Adding
        {
            get
            {
                return adding;
            }
        }
        public IObservable<IEnumerable<T>> Removing
        {
            get
            {
                return removing;
            }
        }
        public IObservable<IEnumerable<T>> Removed
        {
            get
            {
                return removed;
            }
        }

        public void Add(IEnumerable<T> items)
        {
            Insert(Count, items);
        }
        public void Insert(int index, IEnumerable<T> items)
        {
            Validate(items);
            var itemsArray = items.ToArray();
            if (itemsArray.Length > 0)
                OnAdding(itemsArray);
            foreach (var item in itemsArray)
                Items.Insert(index++, item);
            if (itemsArray.Length > 0)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                OnAdded(itemsArray);
            }
        }

        public void ReplaceWith(IEnumerable<T> items)
        {
            Validate(items);
            var oldItems = this.ToArray();
            OnRemoving(oldItems);
            Items.Clear();
            OnRemoved(oldItems);
            Insert(0, items);
        }
        public bool Remove(IEnumerable<T> items)
        {
            Validate(items);
            var itemsList = items.ToList();
            if (itemsList.Count == 0)
                return false;
            OnRemoving(itemsList);
            for (int i = itemsList.Count - 1; i >= 0; i--)
            {
                var item = itemsList[i];
                if (!Items.Remove(item))
                    itemsList.RemoveAt(i);
            }
            if (itemsList.Count > 0)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                OnRemoved(itemsList);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void InsertItem(int index, T item)
        {
            OnAdding(item);
            base.InsertItem(index, item);
            OnAdded(item);
        }
        protected override void SetItem(int index, T item)
        {
            T oldItem = this[index];
            if (EqualityComparer<T>.Default.Equals(oldItem, item))
                return;
            OnRemoving(oldItem);
            OnAdding(item);
            base.SetItem(index, item);
            OnRemoved(oldItem);
            OnAdded(item);
        }
        protected override void RemoveItem(int index)
        {
            T oldItem = this[index];
            OnRemoving(oldItem);
            base.RemoveItem(index);
            OnRemoved(oldItem);
        }
        protected override void ClearItems()
        {
            var oldItems = this.ToArray();
            OnRemoving(oldItems);
            base.ClearItems();
            OnRemoved(oldItems);
        }

        protected virtual void OnAdding(IEnumerable<T> items)
        {
            if (items != null && items.Any())
                adding.OnNext(items);
        }
        protected void OnAdding(T item)
        {
            OnAdding(new[] { item });
        }
        protected virtual void OnAdded(IEnumerable<T> items)
        {
            if (items != null && items.Any())
                added.OnNext(items);
        }
        protected void OnAdded(T item)
        {
            OnAdded(new[] { item });
        }
        protected virtual void OnRemoving(IEnumerable<T> items)
        {
            if (items != null && items.Any())
                removing.OnNext(items);
        }
        protected void OnRemoving(T item)
        {
            OnRemoving(new[] { item });
        }
        protected virtual void OnRemoved(IEnumerable<T> items)
        {
            if (items != null && items.Any())
                removed.OnNext(items);
        }
        protected void OnRemoved(T item)
        {
            OnRemoved(new[] { item });
        }

        private static void Validate(IEnumerable<T> items)
        {
            Argument.NonNull(items, nameof(items));
        }

        private readonly Subject<IEnumerable<T>> adding = new Subject<IEnumerable<T>>();
        private readonly Subject<IEnumerable<T>> added = new Subject<IEnumerable<T>>();
        private readonly Subject<IEnumerable<T>> removing = new Subject<IEnumerable<T>>();
        private readonly Subject<IEnumerable<T>> removed = new Subject<IEnumerable<T>>();
    }


    public class ObservableCollection<TId, TItem> :
        ObservableCollectionBase<TId, TItem>
        where TItem : IId<TId>
    {
        public ObservableCollection(IEnumerable<TItem> items = null)
            : base(items)
        { }

        protected override TId GetItemId(TItem item, bool add)
        {
            return item.Id;
        }
    }
}
