using CeMaS.Common.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CeMaS.Common
{
    /// <summary>
    /// Helps with equality.
    /// </summary>
    public static class EqualityHelper
    {
        public static bool IsNullReference(this object item)
        {
            return item.IsEqualToReference(null);
        }
        public static bool IsEqualToReference(this object item1, object item2)
        {
            return ReferenceEquals(item1, item2);
        }

        public static bool IsEqualTo<T>(this T value1, T value2)
            where T : class, IEquatable<T>
        {
            bool value1IsNull = value1.IsNullReference();
            bool value2IsNull = value2.IsNullReference();
            return
                value1IsNull && value2IsNull ||
                !value1IsNull && value1.Equals(value2);
        }

        public static int CombineHashCodes(params int[] codes)
        {
            return CombineHashCodes((IEnumerable<int>)codes);
        }
        public static int CombineHashCodes(this IEnumerable<int> codes)
        {
            return codes.Aggregate((code1, code2) => code1 ^ code2);
        }
        public static int CombineHashCodes<T>(this IEnumerable<T> items)
        {
            return CombineHashCodes(
                items.
                    Select(i => i.GetHashCode())
                );
        }
        public static int CombineHashCodes<T>(params T[] items)
        {
            return CombineHashCodes((IEnumerable<T>)items);
        }
        public static int CombineReferenceHashCodes(this IEnumerable items)
        {
            return items.
                Cast<object>().
                NonNull().
                CombineHashCodes();
        }
        public static int CombineReferenceHashCodes(params object[] items)
        {
            return CombineReferenceHashCodes((IEnumerable)items);
        }
    }
}
