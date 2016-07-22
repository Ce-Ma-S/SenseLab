using System;

namespace CeMaS.Common.ValueDomains
{
    /// <summary>
    /// Defines a domain of ordered values.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public interface IOrderedValueDomain<T> :
        IValueDomain<T>
        where T : IComparable<T>
    {
        /// <summary>
        /// Optional minimum value in this domain if it can be obtained.
        /// </summary>
        Optional<T> Min { get; }
        /// <summary>
        /// Optional maximum value in this domain if it can be obtained.
        /// </summary>
        Optional<T> Max { get; }
    }
}
