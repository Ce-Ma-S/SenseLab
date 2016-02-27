using CeMaS.Common.Identity;
using CeMaS.Common.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Subjects;

namespace CeMaS.Common.Collections
{
    public class ObservableCollection<T> :
        System.Collections.ObjectModel.ObservableCollection<T>,
        INotifyList<T>
    {
        public ObservableCollection(IEnumerable<T> items = null)
            : base(items == null ? new T[0] : items)
        {
        }

        public IObservable<IEnumerable<T>> Added
        {
            get
            {
                return added;
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
            items.ValidateNonNull("items");
            var added = items.ToArray();
            foreach (var item in added)
                Items.Insert(index++, item);
            if (added.Length > 0)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                OnAdded(added);
            }
        }
        public void ReplaceWith(IEnumerable<T> items)
        {
            items.ValidateNonNull("items");
            var oldItems = this.ToArray();
            Items.Clear();
            OnRemoved(oldItems);
            Insert(0, items);
        }
        public bool Remove(IEnumerable<T> items)
        {
            items.ValidateNonNull("items");
            var removed = new List<T>();
            foreach (var item in items)
            {
                if (Items.Remove(item))
                    removed.Add(item);
            }
            if (removed.Count > 0)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                OnRemoved(removed);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            OnAdded(item);
        }
        protected override void SetItem(int index, T item)
        {
            T oldItem = this[index];
            if (EqualityComparer<T>.Default.Equals(oldItem, item))
                return;
            base.SetItem(index, item);
            OnRemoved(oldItem);
            OnAdded(item);
        }
        protected override void RemoveItem(int index)
        {
            T oldItem = this[index];
            base.RemoveItem(index);
            OnRemoved(oldItem);
        }
        protected override void ClearItems()
        {
            var oldItems = this.ToArray();
            base.ClearItems();
            OnRemoved(oldItems);
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
        protected virtual void OnRemoved(IEnumerable<T> items)
        {
            if (items != null && items.Any())
                removed.OnNext(items);
        }
        protected void OnRemoved(T item)
        {
            OnRemoved(new[] { item });
        }

        private readonly Subject<IEnumerable<T>> added = new Subject<IEnumerable<T>>();
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
