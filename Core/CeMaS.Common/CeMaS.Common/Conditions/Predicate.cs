using CeMaS.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CeMaS.Common.Conditions
{
    public static class Predicate
    {
        public static bool IsNull<T>(this T value)
        {
            return value == null;
        }
        public static bool IsEqualTo<T>(this T value1, T value2)
        {
            bool value1IsNull = value1 == null;
            bool value2IsNull = value2 == null;
            return
                value1IsNull && value2IsNull ||
                !value1IsNull && EqualityComparer<T>.Default.Equals(value1, value2);
        }
        public static bool IsEqualTo<T, K>(this T value1, T value2, Func<T, K> getKey)
            where T : class
        {
            Argument.NonNull(value1, nameof(value1));
            Argument.NonNull(value2, nameof(value2));
            return
                value1.IsEqualToReference(value2) ||
                getKey(value1).IsEqualTo(getKey(value2));
        }
        public static bool IsEqualToReference(this object value1, object value2)
        {
            return ReferenceEquals(value1, value2);
        }
        public static bool IsEmpty(this string value)
        {
            return value == string.Empty;
        }
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        public static bool IsValid(this INotifyDataErrorInfo info)
        {
            Argument.NonNull(info, nameof(info));
            return !info.HasErrors;
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> value)
        {
            return value == null || !value.Any();
        }
        public static bool IsIn<T>(this T value, IEnumerable<T> values)
        {
            Argument.NonNull(values, nameof(values));
            return values.Contains(value);
        }

        #region IComparable

        public static bool IsDefault<T>(this T value)
            where T : struct, IComparable<T>
        {
            return value.CompareTo(default(T)) == 0;
        }
        public static bool IsPositive<T>(this T value)
            where T : struct, IComparable<T>
        {
            return value.CompareTo(default(T)) > 0;
        }
        public static bool IsGreaterThanOrEqualTo<T>(this T value, T referenceValue)
            where T : IComparable<T>
        {
            return value.CompareTo(referenceValue) >= 0;
        }
        public static bool IsGreaterThan<T>(this T value, T referenceValue)
            where T : IComparable<T>
        {
            return value.CompareTo(referenceValue) > 0;
        }
        public static bool IsLessThanOrEqualTo<T>(this T value, T referenceValue)
            where T : IComparable<T>
        {
            return value.CompareTo(referenceValue) <= 0;
        }
        public static bool IsLessThan<T>(this T value, T referenceValue)
            where T : IComparable<T>
        {
            return value.CompareTo(referenceValue) < 0;
        }
        public static bool IsIn<T>(this T value, T from, T to)
            where T : IComparable<T>
        {
            return
                value.CompareTo(from) >= 0 && value.CompareTo(to) <= 0;
        }

        #endregion

        #region Transformations

        public static Func<T, bool> And<T>(this Func<T, bool> predicate1, Func<T, bool> predicate2)
        {
            Validate(predicate1, predicate2);
            return v => predicate1(v) && predicate2(v);
        }
        public static Func<T, bool> Or<T>(this Func<T, bool> predicate1, Func<T, bool> predicate2)
        {
            Validate(predicate1, predicate2);
            return v => predicate1(v) || predicate2(v);
        }
        public static Func<T, bool> Not<T>(this Func<T, bool> predicate)
        {
            Validate(predicate);
            return v => !predicate(v);
        }

        private static void Validate<T>(Func<T, bool> predicate, string name = nameof(Predicate))
        {
            Argument.NonNull(predicate, name);
        }
        private static void Validate<T>(Func<T, bool> predicate1, Func<T, bool> predicate2)
        {
            Validate(predicate1, nameof(predicate1));
            Validate(predicate2, nameof(predicate2));
        }

        #endregion
    }
}
