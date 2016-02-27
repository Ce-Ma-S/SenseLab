using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CeMaS.Common.Validation
{
    /// <summary>
    /// Helps with validations.
    /// </summary>
    public static class ValidationHelper
    {
        public static void ValidateNonNull(this object value, [CallerMemberName] string name = null)
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }
        public static void ValidateNonNullOrEmpty(this string value, [CallerMemberName] string name = null)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(name);
        }
        public static void ValidateNonNullOrWhiteSpace(this string value, [CallerMemberName] string name = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(name);
        }
        public static void ValidateNonNullOrEmpty<T>(this IEnumerable<T> value, [CallerMemberName] string name = null)
        {
            ValidateNonNull(value, name);
            if (!value.Any())
                throw new ArgumentException("At least one item is required.", name);
        }

        #region IComparable

        public static void ValidateNonDefault<T>(this T value, [CallerMemberName] string name = null)
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(default(T)) == 0)
                throw new ArgumentOutOfRangeException(name, value, "Must be non-zero.");
        }
        public static void ValidatePositive<T>(this T value, [CallerMemberName] string name = null)
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(default(T)) <= 0)
                throw new ArgumentOutOfRangeException(name, value, "Must be positive.");
        }
        public static void ValidateGreaterThanOrEqualTo<T>(this T value, T referenceValue, [CallerMemberName] string name = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(referenceValue) < 0)
                throw new ArgumentOutOfRangeException(name, value, $"Must be greater than or equal to {referenceValue}.");
        }
        public static void ValidateGreaterThan<T>(this T value, T referenceValue, [CallerMemberName] string name = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(referenceValue) <= 0)
                throw new ArgumentOutOfRangeException(name, value, $"Must be greater than {referenceValue}.");
        }
        public static void ValidateLessThanOrEqualTo<T>(this T value, T referenceValue, [CallerMemberName] string name = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(referenceValue) > 0)
                throw new ArgumentOutOfRangeException(name, value, $"Must be less than or equal to {referenceValue}.");
        }
        public static void ValidateLessThan<T>(this T value, T referenceValue, [CallerMemberName] string name = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(referenceValue) >= 0)
                throw new ArgumentOutOfRangeException(name, value, $"Must be less than {referenceValue}.");
        }
        public static void ValidateBetween<T>(this T value, T from, T to, [CallerMemberName] string name = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(from) < 0 || value.CompareTo(to) > 0)
                throw new ArgumentOutOfRangeException(name, value, $"Must be between {from} and {to}.");
        }

        #endregion

        public static void ValidateContainment<T>(this T value, IEnumerable<T> values, [CallerMemberName] string name = null)
        {
            values.ValidateNonNull(nameof(values));
            if (!values.Contains(value))
                throw new ArgumentOutOfRangeException(name, value, "Value is not valid.");
        }

        public static void ValidateCount<T>(
           this IReadOnlyCollection<T> value,
           int count,
           [CallerMemberName]
           string name = null
           )
        {
            value.ValidateNonNull(name);
            if (value.Count != count)
                throw new ArgumentOutOfRangeException(name, value.Count, $"Expected {count} items.");
        }
    }
}
