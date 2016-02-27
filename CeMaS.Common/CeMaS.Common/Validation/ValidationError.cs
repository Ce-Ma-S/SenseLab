using System;

namespace CeMaS.Common.Validation
{
    /// <summary>
    /// Validation error.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message"><see cref="Message"/></param>
        /// <param name="scope"><see cref="Scope"/></param>
        /// <param name="error"><see cref="Error"/></param>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is null or empty.</exception>
        public ValidationError(
            string message,
            IValidationScope scope = null,
            Exception error = null
            )
        {
            message.ValidateNonNullOrEmpty(nameof(message));
            Message = message;
            Scope = scope;
            Error = error;
        }

        /// <summary>
        /// Display message.
        /// </summary>
        /// <value>non-empty</value>
        public string Message { get; }
        /// <summary>
        /// Optional validation scope.
        /// </summary>
        /// <value>null means whole validated value.</value>
        public IValidationScope Scope { get; }
        /// <summary>
        /// Optional error.
        /// </summary>
        public Exception Error { get; }

        public override string ToString()
        {
            return Scope == null ?
                Message :
                $"{Scope}: {Message}";
        }
    }
}