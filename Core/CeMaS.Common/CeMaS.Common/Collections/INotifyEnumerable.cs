using System.Collections.Generic;

namespace CeMaS.Common.Collections
{
    /// <summary>
    /// Enumeration with change notifications.
    /// </summary>
    /// <typeparam name="T">Item type.</typeparam>
    public interface INotifyEnumerable<out T> :
        IEnumerable<T>,
        INotifyCollectionChange<T>
    {
    }


    public interface INotifyEnumerable<TId, TItem> :
        INotifyEnumerable<TItem>,
        IItemLookUp<TId, TItem>
    {
    }
}
