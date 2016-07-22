using System.Collections.Generic;

namespace CeMaS.Common.Properties
{
    /// <summary>
    /// Object`s writable values access.
    /// </summary>
    public interface IHaveNamedValuesWritable :
        IHaveNamedValues
    {
        /// <summary>
        /// Object`s named values.
        /// </summary>
        /// <value>non-null</value>
        new IDictionary<string, object> Values { get; }
    }
}
