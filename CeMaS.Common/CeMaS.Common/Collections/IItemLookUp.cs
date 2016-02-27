namespace CeMaS.Common.Collections
{
    /// <summary>
    /// Provides access to identifiable items.
    /// </summary>
    /// <typeparam name="TItem">Item type.</typeparam>
    /// <typeparam name="TId">Item identifier type.</typeparam>
    public interface IItemLookUp<TId, TItem>
    {
        /// <summary>
        /// Finds an item with <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <exception cref="KeyNotFoundException">No item with <paramref name="id"/> was found.</exception>
        TItem GetItem(TId id);
        /// <summary>
        /// Finds an item with <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="item">Found item.</param>
        Optional<TItem> TryGetItem(TId id);
        /// <summary>
        /// Whether an item with <paramref name="id"/> is available.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        bool Contains(TId id);
    }
}
