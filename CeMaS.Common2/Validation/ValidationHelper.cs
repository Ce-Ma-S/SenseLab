using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CeMaS.Common.Validation
{
    public static class ValidationHelper
    {
        public static void ValidateNonNull(
            this object value,
            [CallerMemberName]
            string name = null
            )
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }

        public static void ValidateNonEmpty(
            this string value,
            [CallerMemberName]
            string name = null
            )
        {
            value.ValidateNonNull(name);
            if (value.Length == 0)
                throw new ArgumentNullException(name);
        }

        public static void ValidateIn<T>(
            this T value,
            T min,
            T max,
            [CallerMemberName]
            string name = null
            )
            where T : IComparable<T>
        {
            value.ValidateGreaterThanOrEqualTo(min, name);
            value.ValidateLessThanOrEqualTo(max, name);
        }

        public static void ValidateGreaterThanOrEqualTo<T>(
            this T value,
            T min,
            [CallerMemberName]
            string name = null
            )
            where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
                throw new ArgumentOutOfRangeException(name);
        }

        public static void ValidateLessThanOrEqualTo<T>(
            this T value,
            T max,
            [CallerMemberName]
            string name = null
            )
            where T : IComparable<T>
        {
            if (value.CompareTo(max) > 0)
                throw new ArgumentOutOfRangeException(name);
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
                throw new ArgumentOutOfRangeException(name);
        }
    }
}
