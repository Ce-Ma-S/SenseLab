using CeMaS.Common.Collections;
using CeMaS.Common.Validation;
using System.Collections;
using System.Collections.Generic;

namespace CeMaS.Common.Properties
{
    /// <summary>
    /// <see cref="IReadOnlyDictionary{string, object}"/>/<see cref="IDictionary{string, object}"/> helper.
    /// </summary>
    public static class NamedValueHelper
    {
        #region Value

        public static T Value<T>(
            this IReadOnlyDictionary<string, object> values,
            NamedValue<T> accessor
            )
        {
            Validate(values);
            Validate(accessor);
            return accessor.GetValue(key => values.Value(key, null));
        }

        public static T Value<T>(
            this IDictionary<string, object> values,
            NamedValue<T> accessor
            )
        {
            Validate(values);
            Validate(accessor);
            return accessor.GetValue(key => values.Value(key, null));
        }

        public static TValues SetValue<TValues, TValue>(
            this TValues values,
            NamedValue<TValue> accessor,
            TValue value
            )
            where TValues : IDictionary<string, object>
        {
            Validate(values);
            Validate(accessor);
            accessor.SetValue((k, v) => values[k] = v, value);
            return values;
        }

        public static TValues SetValueIfMissing<TValues, TValue>(
            this TValues values,
            NamedValue<TValue> accessor,
            TValue value
            )
            where TValues : IDictionary<string, object>
        {
            Validate(values);
            if (!values.ContainsKey(accessor.Id))
                return values.SetValue(accessor, value);
            return values;
        }

        public static TValues ClearValue<TValues, TValue>(
            this TValues values,
            NamedValue<TValue> accessor
            )
            where TValues : IDictionary<string, object>
        {
            Validate(values);
            Validate(accessor);
            accessor.ClearValue(key => values.Remove(key));
            return values;
        }

        private static void Validate(IEnumerable values)
        {
            Argument.NonNull(values, nameof(values));
        }
        private static void Validate<TValue>(NamedValue<TValue> accessor)
        {
            Argument.NonNull(accessor, nameof(accessor));
        }

        #endregion
    }
}
