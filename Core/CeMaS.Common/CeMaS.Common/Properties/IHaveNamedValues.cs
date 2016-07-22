using System.Collections.Generic;

namespace CeMaS.Common.Properties
{
    /// <summary>
    /// Object`s values access.
    /// </summary>
    public interface IHaveNamedValues
    {
        /// <summary>
        /// Object`s named values.
        /// </summary>
        /// <value>non-null</value>
        IReadOnlyDictionary<string, object> Values { get; }
    }
}
