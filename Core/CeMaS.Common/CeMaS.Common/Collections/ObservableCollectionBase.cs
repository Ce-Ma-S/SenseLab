using System.Collections.Generic;
using System.Linq;

namespace CeMaS.Common.Collections
{
    public abstract class ObservableCollectionBase<TId, TItem> :
        ObservableCollection<TItem>,
        INotifyList<TId, TItem>
    {
        public ObservableCollectionBase(IEnumerable<TItem> items = null)
            : base(items)
        {
            idToItem = new Dictionary<TId, TItem>();
            presetItemId = new Dictionary<TItem, TId>();
            if (items != null)
                OnAdded(items);
        }

        public IEnumerable<TId> Ids
        {
            get { return idToItem.Keys; }
        }

        public TItem GetItem(TId id)
        {
            return idToItem[id];
        }
        public Optional<TItem> TryGetItem(TId id)
        {
            TItem item;
            return idToItem.TryGetValue(id, out item) ?
                item :
                Optional<TItem>.None;
        }
        public bool Contains(TId id)
        {
            return idToItem.ContainsKey(id);
        }

        public bool Remove(TId id)
        {
            var item = TryGetItem(id);
            if (!item.HasValue)
                return false;
            return Remove(item.Value);
        }
        public bool Remove(IEnumerable<TId> ids)
        {
            bool result = false;
            foreach (var id in ids)
            {
                if (Remove(id))
                    result = true;
            }
            return result;
        }

        protected abstract TId GetItemId(TItem item, bool idStored);
        protected TId GetStoredItemId(TItem item)
        {
            return idToItem.
                Single(i => EqualityComparer<TItem>.Default.Equals(i.Value, item)).
                Key;
        }
        protected void PresetItemId(TItem item, TId id)
        {
            presetItemId[item] = id;
        }

        protected override void OnAdded(IEnumerable<TItem> items)
        {
            foreach (var item in items)
            {
                TId id;
                bool presetId = presetItemId.TryGetValue(item, out id);
                if (!presetId)
                    id = GetItemId(item, false);
                idToItem.Add(id, item);
                if (presetId)
                    presetItemId.Remove(item);
            }
            base.OnAdded(items);
        }
        protected override void OnRemoved(IEnumerable<TItem> items)
        {
            foreach (var item in items)
            {
                TId id = GetItemId(item, true);
                idToItem.Remove(id);
            }
            base.OnRemoved(items);
        }

        private readonly Dictionary<TId, TItem> idToItem;
        private readonly Dictionary<TItem, TId> presetItemId;
    }
}
