using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace CeMaS.Common.Validation
{
    /// <summary>
    /// Helps with argument validations.
    /// </summary>
    public static class Argument
    {
        #region NonNull*

        public static T NonNull<T>(T value, [CallerMemberName] string name = null)
        {
            if (value == null)
                throw new ArgumentNullException(name);
            return value;
        }
        public static V NonNull<T, V>(T value, Func<T, V> result, [CallerMemberName] string name = null)
        {
            NonNull(value, name);
            return result(value);
        }
        public static string NonNullOrEmpty(string value, [CallerMemberName] string name = null)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(name);
            return value;
        }
        public static string NonNullOrWhiteSpace(string value, [CallerMemberName] string name = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(name);
            return value;
        }
        public static IEnumerable<T> NonNullOrEmpty<T>(IEnumerable<T> value, [CallerMemberName] string name = null)
        {
            NonNull((object)value, name);
            if (!value.Any())
                throw new ArgumentException("At least one item is required.", name);
            return value;
        }

        #endregion

        #region Optional<T>

        public static Optional<T> HasValue<T>(Optional<T> value, [CallerMemberName] string name = null)
        {
            if (!value.HasValue)
                throw new ArgumentException("Value is required.", name);
            return value;
        }


        #endregion

        #region IComparable

        public static T NonDefault<T>(T value, [CallerMemberName] string name = null)
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(default(T)) == 0)
                throw new ArgumentOutOfRangeException(name, value, "Must be non-zero.");
            return value;
        }
        public static T Positive<T>(T value, [CallerMemberName] string name = null)
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(default(T)) <= 0)
                throw new ArgumentOutOfRangeException(name, value, "Must be positive.");
            return value;
        }
        public static T GreaterThanOrEqualTo<T>(T value, T referenceValue, [CallerMemberName] string name = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(referenceValue) < 0)
                throw new ArgumentOutOfRangeException(name, value, string.Format("Must be greater than or equal to {0}.", referenceValue));
            return value;
        }
        public static T GreaterThan<T>(T value, T referenceValue, [CallerMemberName] string name = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(referenceValue) <= 0)
                throw new ArgumentOutOfRangeException(name, value, string.Format("Must be greater than {0}.", referenceValue));
            return value;
        }
        public static T LessThanOrEqualTo<T>(T value, T referenceValue, [CallerMemberName] string name = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(referenceValue) > 0)
                throw new ArgumentOutOfRangeException(name, value, string.Format("Must be less than or equal to {0}.", referenceValue));
            return value;
        }
        public static T LessThan<T>(T value, T referenceValue, [CallerMemberName] string name = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(referenceValue) >= 0)
                throw new ArgumentOutOfRangeException(name, value, string.Format("Must be less than {0}.", referenceValue));
            return value;
        }
        public static T In<T>(T value, T from, T to, [CallerMemberName] string name = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(from) < 0 || value.CompareTo(to) > 0)
                throw new ArgumentOutOfRangeException(name, value, string.Format("Must be between {0} and {1}.", from, to));
            return value;
        }

        #endregion

        #region Validate

        public static T Validate<T>(
            T value,
            Func<T, bool> predicate,
            string invalidMessage = null,
            [CallerMemberName] string name = null,
            bool inRange = false
            )
        {
            NonNull(predicate, nameof(predicate));
            if (!predicate(value))
            {
                if (inRange)
                    throw new ArgumentOutOfRangeException(name, value, invalidMessage);
                else
                    throw new ArgumentException(invalidMessage, name);
            }
            return value;
        }

        public static T Validate<T>(
            T value,
            IValidator<T> validator,
            [CallerMemberName] string name = null
            )
        {
            NonNull(validator, nameof(validator));
            validator.Validate(value).Validate(name);
            return value;
        }

        /// <summary>
        /// Validates <paramref name="errors"/>.
        /// </summary>
        /// <param name="errors">Validation errors.</param>
        /// <param name="name">Parameter name.</param>
        /// <exception cref="ValidationException"><paramref name="errors"/> is non empty.</exception>
        public static void Validate(this IEnumerable<ValidationError> errors, [CallerMemberName] string name = null)
        {
            if (!errors.None())
                throw new ValidationException(name, errors);
        }

        #endregion

        public static T In<T>(T value, IEnumerable<T> values, [CallerMemberName] string name = null)
        {
            NonNull(values, nameof(values));
            if (!values.Contains(value))
                throw new ArgumentOutOfRangeException(name, value, "Value is not from valid values.");
            return value;
        }
        public static TKey InKeys<TKey, TValue>(TKey value, IReadOnlyDictionary<TKey, TValue> values, [CallerMemberName] string name = null)
        {
            NonNull(values, nameof(values));
            if (!values.ContainsKey(value))
                throw new ArgumentOutOfRangeException(name, value, "Value is not from valid keys.");
            return value;
        }

        #region Type

        public static object Is<T>(object value, [CallerMemberName] string name = null, bool allowNullIfNullable = false)
        {
            return IsTypeOf(value, typeof(T), name, allowNullIfNullable);
        }
        public static object IsTypeOf(object value, Type requiredType, [CallerMemberName] string name = null, bool allowNullIfNullable = false)
        {
            NonNull(requiredType, nameof(requiredType));
            if (!allowNullIfNullable)
                NonNull(value, name);
            return Validate(
                value,
                i => value.Is(requiredType, allowNullIfNullable),
                value == null ?
                    $"Value must be of type '{requiredType.FullName}'." :
                    $"Value type '{value.GetType().FullName}' must be assignable to '{requiredType.FullName}'.",
                name
                );
        }
        public static object Is<T>(Type value, [CallerMemberName] string name = null)
        {
            return IsTypeOf(value, typeof(T), name);
        }
        public static object IsTypeOf(Type value, Type requiredType, [CallerMemberName] string name = null)
        {
            NonNull(requiredType, nameof(requiredType));
            NonNull(value, name);
            return Validate(
                value,
                i => value.Is(requiredType),
                $"Type '{value.FullName}' must be assignable to '{requiredType.FullName}'.",
                name
                );
        }

        public static T CastTo<T>(object value, [CallerMemberName] string name = null, bool allowNullIfNullable = false)
        {
            Is<T>(value, name, allowNullIfNullable);
            return (T)value;
        }
        public static T CastTo<T>(Expression<Func<object>> property, bool allowNullIfNullable = false)
        {
            return CastTo<T>(property.Compile()(), PropertyName(property), allowNullIfNullable);
        }

        #endregion

        #region PropertyName

        public static string PropertyName(string argumentName, string propertyName)
        {
            return string.Join(ExpressionHelper.PropertyPathSeparator, argumentName, propertyName);
        }
        public static string PropertyName<T>(Expression<Func<T>> property)
        {
            return PropertyName(
                property.MemberExpressionInfo().Name,
                property.PropertyName()
                );
        }

        #endregion

        public static void Count<T>(
           this IReadOnlyCollection<T> value,
           int count,
           [CallerMemberName]
           string name = null
           )
        {
            NonNull(value, name);
            if (value.Count != count)
                throw new ArgumentOutOfRangeException(name, value.Count, $"Expected {count} items.");
        }
    }
}
