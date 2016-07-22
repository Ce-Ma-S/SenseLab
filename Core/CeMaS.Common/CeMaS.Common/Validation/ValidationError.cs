using System;
using System.Collections.Generic;

namespace CeMaS.Common.Validation
{
    /// <summary>
    /// Validation error.
    /// </summary>
    public class ValidationError
    {
        #region Init

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message"><see cref="Message"/></param>
        /// <param name="scope"><see cref="Scope"/></param>
        /// <param name="error"><see cref="Error"/></param>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is null or empty.</exception>
        public ValidationError(
            string message,
            ValidationScope scope = null,
            Exception error = null
            )
        {
            Argument.NonNullOrEmpty(message, nameof(message));
            Message = message;
            SetScope(scope);
            Error = error;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="error"><see cref="Error"/></param>
        /// <param name="scope"><see cref="Scope"/></param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is null.</exception>
        public ValidationError(
            Exception error,
            ValidationScope scope = null
            ) :
            this(error.Message(), scope, error)
        { }

        #endregion

        /// <summary>
        /// Represents <see cref="IValidator{T}.Validate(T)"/> result for valid values.
        /// </summary>
        public static IEnumerable<ValidationError> None = Array.Empty<ValidationError>();

        /// <summary>
        /// Display message.
        /// </summary>
        /// <value>non-empty</value>
        public string Message { get; }
        /// <summary>
        /// Validation scope.
        /// </summary>
        /// <value>non-null</value>
        public ValidationScope Scope
        {
            get { return scope ?? ValidationScope.Whole; }
        }
        /// <summary>
        /// Optional error.
        /// </summary>
        public Exception Error { get; }

        public override string ToString()
        {
            return Scope == ValidationScope.Whole ?
                Message :
                $"{Scope.Info.Name}:{Environment.NewLine}{Message}";
        }
        public ValidationError SetScope(ValidationScope scope)
        {
            this.scope = scope;
            return this;
        }

        private ValidationScope scope;
    }
}