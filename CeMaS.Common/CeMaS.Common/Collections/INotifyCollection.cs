using System.Collections.Generic;

namespace CeMaS.Common.Collections
{
    /// <summary>
    /// Collection with change notifications.
    /// </summary>
    /// <typeparam name="T">Item type.</typeparam>
    public interface INotifyCollection<T> :
        ICollection<T>,
        INotifyEnumerable<T>
    {
        void Add(IEnumerable<T> items);
        void ReplaceWith(IEnumerable<T> items);
        bool Remove(IEnumerable<T> items);
    }


    public interface INotifyCollection<TId, TItem> :
        INotifyCollection<TItem>,
        INotifyEnumerable<TId, TItem>
    {
        bool Remove(TId id);
        bool Remove(IEnumerable<TId> ids);
    }
}
