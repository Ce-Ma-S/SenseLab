using System.Collections.Generic;

namespace CeMaS.Common.ValueDomains
{
    /// <summary>
    /// <see cref="ICompositeValueDomain{T}"/> implementation.
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <typeparam name="TDomain">Domain type.</typeparam>
    public interface ICompositeValueDomain<T> :
        IValueDomain<T>
    {
        /// <summary>
        /// Included domains.
        /// </summary>
        /// <value>non-null</value>
        IEnumerable<IValueDomain<T>> Included { get; }
        /// <summary>
        /// Excluded domains.
        /// </summary>
        /// <value>non-null</value>
        IEnumerable<IValueDomain<T>> Excluded { get; }
    }
}
