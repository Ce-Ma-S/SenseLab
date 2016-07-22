using System.Collections.Generic;
using System;

namespace CeMaS.Common.ValueDomains
{
    /// <summary>
    /// <see cref="ICompositeValueDomain{T}"/> implementation of <see cref="IOrderedValueDomain{T}"/>.
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <typeparam name="TDomain">Domain type.</typeparam>
    public class CompositeOrderedValueDomain<TValue, TDomain> :
        CompositeValueDomain<TValue, TDomain>,
        IOrderedValueDomain<TValue>
        where TDomain : IValueDomain<TValue>
        where TValue : IComparable<TValue>
    {
        public CompositeOrderedValueDomain(
            IEnumerable<TDomain> included = null,
            IEnumerable<TDomain> excluded = null
            ) :
            base(included, excluded)
        { }

        /// <summary>
        /// Optional minimum value in this domain if it can be obtained.
        /// </summary>
        public virtual Optional<TValue> Min
        {
            get
            {
                // TODO:
                return Optional<TValue>.None;
            }
        }
        /// <summary>
        /// Optional maximum value in this domain if it can be obtained.
        /// </summary>
        public virtual Optional<TValue> Max
        {
            // TODO:
            get { return Optional<TValue>.None; }
        }
    }
}
