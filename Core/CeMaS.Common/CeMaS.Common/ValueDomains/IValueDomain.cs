namespace CeMaS.Common.ValueDomains
{
    /// <summary>
    /// Defines a domain of values.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public interface IValueDomain<T>
    {
        /// <summary>
        /// Whether <paramref name="value"/> is in this domain.
        /// </summary>
        /// <param name="value">Value.</param>
        bool Contains(T value);
    }
}
