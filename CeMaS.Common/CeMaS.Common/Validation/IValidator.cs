using System.Collections.Generic;

namespace CeMaS.Common.Validation
{
    /// <summary>
    /// Validates a value.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public interface IValidator<T>
    {
        /// <summary>
        /// Validates <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value to be validated.</param>
        /// <returns>Errors if <paramref name="value"/> is invalid, otherwise empty.</returns>
        IEnumerable<ValidationError> ValidateWithErrors(T value);
        /// <summary>
        /// Validates <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value to be validated.</param>
        /// <returns>Whether <paramref name="value"/> is valid.</returns>
        bool Validate(T value);
    }
}
