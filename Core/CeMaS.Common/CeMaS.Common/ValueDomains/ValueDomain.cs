using CeMaS.Common.Events;

namespace CeMaS.Common.ValueDomains
{
    /// <summary>
    /// <see cref="IValueDomain{T}"/> base.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public abstract class ValueDomain<T> :
        NotifyPropertyChange,
        IValueDomain<T>
    {
        /// <summary>
        /// Whether <paramref name="value"/> is in this domain.
        /// </summary>
        /// <param name="value">Value.</param>
        public abstract bool Contains(T value);
    }
}
