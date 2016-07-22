using System;
using System.Collections.Generic;

namespace CeMaS.Common.Collections
{
    /// <summary>
    /// Notifies about a collection changes to be done.
    /// </summary>
    /// <typeparam name="T">Item type.</typeparam>
    public interface INotifyCollectionChanging<out T>
    {
        /// <summary>
        /// Notifies about to be added new items.
        /// </summary>
        IObservable<IEnumerable<T>> Adding { get; }
        /// <summary>
        /// Notifies about to be removed items.
        /// </summary>
        IObservable<IEnumerable<T>> Removing { get; }
    }
}
