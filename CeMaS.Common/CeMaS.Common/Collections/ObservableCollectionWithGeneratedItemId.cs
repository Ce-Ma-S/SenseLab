using System.Collections.Generic;

namespace CeMaS.Common.Collections
{
    public abstract class ObservableCollectionWithGeneratedItemId<TId, TItem> :
        ObservableCollectionBase<TId, TItem>
    {
        public ObservableCollectionWithGeneratedItemId(IEnumerable<TItem> items = null)
            : base(items)
        { }

        protected override TId GetItemId(TItem item, bool idStored)
        {
            return idStored ?
                GetStoredItemId(item) :
                GenerateItemId(item);
        }
        protected abstract TId GenerateItemId(TItem item);
    }
}
