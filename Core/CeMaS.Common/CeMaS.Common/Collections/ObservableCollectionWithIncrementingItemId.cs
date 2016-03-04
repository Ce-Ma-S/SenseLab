using System;
using System.Collections.Generic;
using System.Linq;

namespace CeMaS.Common.Collections
{
    public class ObservableCollectionWithIncrementingItemId<TId, TItem> :
        ObservableCollectionWithGeneratedItemId<TId, TItem>
        where TId : IComparable<TId>
    {
        public ObservableCollectionWithIncrementingItemId(TId firstId, IEnumerable<TItem> items = null)
            : base(items)
        {
            id = firstId;
        }
        public ObservableCollectionWithIncrementingItemId(IEnumerable<KeyValuePair<TId, TItem>> items)
            : this(
                items.
                    Select(i => i.Key).
                    Min()
                )
        {
            Add(items);
        }

        public void Add(TItem item, TId id)
        {
            PresetItemId(item, id);
            Add(item);
            EnsureId(id);
        }
        public void Add(IEnumerable<KeyValuePair<TId, TItem>> items)
        {
            foreach (var item in items)
                PresetItemId(item.Value, item.Key);
            Add(items.
                Select(i => i.Value)
                );
            EnsureId(items.
                Select(i => i.Key).
                Max()
                );
        }

        protected override TId GenerateItemId(TItem item)
        {
            return (TId)id++;
        }

        private void EnsureId(TId id)
        {
            int comparison = id.CompareTo((TId)this.id);
            if (comparison >= 0)
                this.id = id;
            if (comparison == 0)
                this.id++;
        }

        private dynamic id;
    }
}
