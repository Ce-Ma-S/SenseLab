using System;

namespace CeMaS.Common.ValueDomains
{
    /// <summary>
    /// <see cref="IOrderedValueDomain{T}"/> base.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public abstract class OrderedValueDomain<T> :
        ValueDomain<T>,
        IOrderedValueDomain<T>
        where T : IComparable<T>
    {
        /// <summary>
        /// Optional minimum value in this domain if it can be obtained.
        /// </summary>
        public virtual Optional<T> Min
        {
            get { return Optional<T>.None; }
        }
        /// <summary>
        /// Optional maximum value in this domain if it can be obtained.
        /// </summary>
        public virtual Optional<T> Max
        {
            get { return Optional<T>.None; }
        }
    }
}
