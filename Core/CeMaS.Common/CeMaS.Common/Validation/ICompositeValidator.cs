using System.Collections.Generic;

namespace CeMaS.Common.Validation
{
    /// <summary>
    /// Validates a value using its inner validators only.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public interface ICompositeValidator<in T> :
        IValidator<T>
    {
        /// <summary>
        /// Gets inner validators for <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value to be validated.</param>
        /// <returns>non-null</returns>
        IEnumerable<IValidator<T>> GetChildren(T value);
    }
}
