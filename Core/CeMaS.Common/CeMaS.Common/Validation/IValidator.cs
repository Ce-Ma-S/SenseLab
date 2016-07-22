using System.Collections.Generic;

namespace CeMaS.Common.Validation
{
    /// <summary>
    /// Validates a value.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public interface IValidator<in T>
    {
        /// <summary>
        /// Validation scope this validator operates at.
        /// </summary>
        /// <value>non-null</value>
        ValidationScope Scope { get; }

        /// <summary>
        /// Validates <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value to be validated.</param>
        /// <returns>Errors if <paramref name="value"/> is invalid, otherwise empty.</returns>
        IEnumerable<ValidationError> Validate(T value);
    }
}
