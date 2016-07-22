using CeMaS.Common.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CeMaS.Common.Validation
{
    /// <summary>
    /// <see cref="IValidator{T}"/> helper.
    /// </summary>
    public static class ValidatorHelper
    {
        /// <summary>
        /// Validates <paramref name="value"/>.
        /// </summary>
        /// <param name="validator">Validator.</param>
        /// <param name="value">Value to be validated.</param>
        /// <returns>Whether <paramref name="value"/> is valid.</returns>
        public static bool IsValid<T>(this IValidator<T> validator, T value)
        {
            Argument.NonNull(validator, nameof(validator));
            return validator.Validate(value).None();
        }

        #region Conversion

        /// <summary>
        /// Whether <paramref name="errors"/> represent valid case with no errors.
        /// </summary>
        /// <param name="errors">Validation errors.</param>
        /// <returns><paramref name="errors"/> is null or empty.</returns>
        public static bool None(this IEnumerable<ValidationError> errors)
        {
            return errors.IsNullOrEmpty();
        }

        /// <summary>
        /// Converts <paramref name="errors"/> to a message.
        /// </summary>
        /// <param name="errors">Validation errors.</param>
        /// <param name="separator">Separates errors.</param>
        public static string Message(this IEnumerable<ValidationError> errors, string separator = "\n")
        {
            return errors.None() ?
                null :
                string.Join(separator, errors);
        }

        /// <summary>
        /// Sets <see cref="ValidationError.Scope"/> of <paramref name="errors"/> to <paramref name="scope"/>.
        /// </summary>
        /// <param name="errors">Validation errors.</param>
        /// <param name="scope">Scope. null means <see cref="ValidationScope.Whole"/>.</param>
        public static IEnumerable<ValidationError> SetScope(
            this IEnumerable<ValidationError> errors,
            ValidationScope scope = null
            )
        {
            Argument.NonNull(errors, nameof(errors));
            return errors.Select(i => i.SetScope(scope));
        }

        #endregion
    }
}
