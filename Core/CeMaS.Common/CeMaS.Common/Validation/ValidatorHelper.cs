using CeMaS.Common.Collections;
using System.Collections.Generic;

namespace CeMaS.Common.Validation
{
    public static class ValidatorHelper
    {
        /// <summary>
        /// Validates <paramref name="errors"/>.
        /// </summary>
        /// <param name="errors">Validation errors.</param>
        /// <param name="name">Parameter name.</param>
        /// <exception cref="ValidationException"><paramref name="errors"/> is non empty.</exception>
        public static void Validate(this IEnumerable<ValidationError> errors, string name)
        {
            if (!errors.IsNullOrEmpty())
                throw new ValidationException(name, errors);
        }
        /// <summary>
        /// Converts <paramref name="errors"/> to a message.
        /// </summary>
        /// <param name="errors">Validation errors.</param>
        /// <param name="separator">Separates errors.</param>
        public static string Message(this IEnumerable<ValidationError> errors, string separator = "\n")
        {
            return errors.IsNullOrEmpty() ?
                null :
                string.Join(separator, errors);
        }
    }
}
