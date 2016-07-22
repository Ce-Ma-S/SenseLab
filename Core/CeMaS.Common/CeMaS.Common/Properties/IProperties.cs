using System.Collections.Generic;

namespace CeMaS.Common.Properties
{
    /// <summary>
    /// Information about an object`s properties.
    /// </summary>
    public interface IProperties
    {
        /// <summary>
        /// Properties.
        /// </summary>
        /// <value>non-null</value>
        IEnumerable<IProperty> Properties { get; }
    }
}
