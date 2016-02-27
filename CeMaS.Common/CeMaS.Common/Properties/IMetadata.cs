using CeMaS.Common.Collections;
using System.Collections.Generic;

namespace CeMaS.Common.Properties
{
    /// <summary>
    /// Object`s metadata.
    /// </summary>
    public interface IMetadata :
        IReadOnlyCollection<KeyValuePair<string, object>>,
        IItemLookUp<string, object>
    {
        /// <summary>
        /// Identifiers (names) of available metadata values.
        /// </summary>
        IEnumerable<string> Ids { get; }
        /// <summary>
        /// Gets metadata value.
        /// </summary>
        /// <param name="id">Metadata value identifier.</param>
        /// <returns>Metadata value if found, otherwise null.</returns>
        object this[string id] { get; }
    }
}
