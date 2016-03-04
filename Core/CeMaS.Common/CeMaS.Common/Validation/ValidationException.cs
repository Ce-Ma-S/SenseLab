using System;
using System.Collections.Generic;

namespace CeMaS.Common.Validation
{
    /// <summary>
    /// Thrown on a parameter validation error.
    /// </summary>
    public class ValidationException :
        ArgumentException
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="validationError"><see cref="ValidationError"/></param>
        public ValidationException(string paramName, IEnumerable<ValidationError> errors)
            : base(errors.Message(), paramName)
        {
            Errors = errors;
        }

        /// <summary>
        /// Parameter validation error.
        /// </summary>
        public IEnumerable<ValidationError> Errors { get; private set; }
    }
}
