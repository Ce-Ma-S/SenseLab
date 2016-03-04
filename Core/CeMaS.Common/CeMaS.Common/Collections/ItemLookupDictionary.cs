using System;
using System.Collections.Generic;
using System.Linq;

namespace CeMaS.Common.Collections
{
    public class ItemLookupDictionary<TId, TItem> :
        Dictionary<TId, TItem>,
        IItemLookUp<TId, TItem>
    {
        #region Init

        public ItemLookupDictionary(
            IDictionary<TId, TItem> dictionary = null,
            IEqualityComparer<TId> comparer = null)
            : base(
                dictionary ?? new Dictionary<TId, TItem>(),
                comparer ?? EqualityComparer<TId>.Default
                )
        {
        }

        #endregion

        public TItem GetItem(TId id)
        {
            return this[id];
        }
        public Optional<TItem> TryGetItem(TId id)
        {
            TItem item;
            return TryGetValue(id, out item) ?
                item :
                Optional<TItem>.None;
        }
        public bool Contains(TId id)
        {
            return ContainsKey(id);
        }
    }
}
