using CeMaS.Common.Validation;
using System;
using System.Collections.Generic;

namespace CeMaS.Common
{
    /// <summary>
    /// Optional value.
    /// </summary>
    /// <remarks><typeparamref name="T"/>? alternative for both reference and value types.</remarks>
    /// <typeparam name="T">Value type.</typeparam>
    public struct Optional<T> :
        IEquatable<Optional<T>>,
        IEquatable<T>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value"><see cref="Value"/></param>
        public Optional(T value)
            : this()
        {
            HasValue = true;
            this.value = value;
        }

        /// <summary>
        /// No value.
        /// </summary>
        public static readonly Optional<T> None = new Optional<T>();

        /// <summary>
        /// Whether <see cref="Value"/> is specified.
        /// </summary>
        public bool HasValue { get; private set; }
        /// <summary>
        /// Value if specified.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="HasValue"/> is false.</exception>
        public T Value
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException(noValueText);
                return value;
            }
        }
        /// <summary>
        /// <see cref="Value"/> if specified, otherwise default(T).
        /// </summary>
        public T ValueOrDefault
        {
            get { return GetValueOrDefault(default(T)); }
        }

        /// <summary>
        /// Gets result with <see cref="Optional{T}.Value"/> set to <paramref name="value"/> if it is not default(T), otherwise <see cref="Optional{T}.None"/>.
        /// </summary>
        /// <param name="value">Value.</param>
        public static Optional<T> ValueIfNonDefault(T value)
        {
            return EqualityComparer<T>.Default.Equals(value, default(T)) ?
                None :
                value;
        }

        #region Equality

        public static bool operator ==(Optional<T> value1, Optional<T> value2)
        {
            return
                value1.HasValue == value2.HasValue &&
                EqualityComparer<T>.Default.Equals(value1.Value, value2.Value);
        }
        public static bool operator !=(Optional<T> value1, Optional<T> value2)
        {
            return !(value1 == value2);
        }
        public static bool operator ==(Optional<T> value1, T value2)
        {
            return
                value1.HasValue &&
                EqualityComparer<T>.Default.Equals(value1.Value, value2);
        }
        public static bool operator !=(Optional<T> value1, T value2)
        {
            return !(value1 == value2);
        }
        public static bool operator ==(T value1, Optional<T> value2)
        {
            return
                value2.HasValue &&
                EqualityComparer<T>.Default.Equals(value1, value2.Value);
        }
        public static bool operator !=(T value1, Optional<T> value2)
        {
            return !(value1 == value2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Optional<T>)
            {
                return this == (Optional<T>)obj;
            }
            else if (obj is T)
            {
                return this == (T)obj;
            }
            return false;
        }
        public bool Equals(Optional<T> other)
        {
            return this == other;
        }
        public bool Equals(T other)
        {
            return this == other;
        }
        public override int GetHashCode()
        {
            return HasValue ?
                int.MinValue :
                Value.GetHashCode();
        }

        #endregion

        #region Casting

        public static implicit operator Optional<T>(T value)
        {
            return new Optional<T>(value);
        }
        public static explicit operator T(Optional<T> value)
        {
            if (!value.HasValue)
                throw new InvalidCastException(noValueText);
            return value.Value;
        }

        #endregion

        public T GetValueOrDefault(T defaultValue)
        {
            return HasValue ?
                Value :
                defaultValue;
        }
        public T GetValueOrDefault(Func<T> defaultValue)
        {
            return HasValue ?
                Value :
                Argument.NonNull(defaultValue, nameof(defaultValue))();
        }

        public override string ToString()
        {
            return HasValue ?
                $"{Value}" :
                "?";
        }

        private const string noValueText = "No value exists.";

        private readonly T value;
    }


    /// <summary>
    /// <see cref="Optional{T}"/> helper.
    /// </summary>
    public static class Optional
    {
        public static Optional<T> NoneIfDefault<T>(this T value)
        {
            return EqualityComparer<T>.Default.Equals(value, default(T)) ?
                Optional<T>.None :
                value;
        }
        public static Optional<string> NoneIfNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) ?
                Optional<string>.None :
                value;
        }
    }
}
