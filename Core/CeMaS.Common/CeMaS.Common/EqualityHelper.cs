﻿using CeMaS.Common.Collections;
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
