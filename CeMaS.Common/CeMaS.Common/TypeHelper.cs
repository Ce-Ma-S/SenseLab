using CeMaS.Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CeMaS.Common
{
    /// <summary>
    /// Helps with types.
    /// </summary>
    public static class TypeHelper
    {
        public static bool Is(this Type type, Type requiredType)
        {
            return
                type != null &&
                requiredType != null &&
                requiredType.IsAssignableFrom(type);
        }
        public static bool Is<T>(this Type type)
        {
            return type.Is(typeof(T));
        }
        public static bool Is(this object item, Type requiredType)
        {
            return
                item != null &&
                item.GetType().Is(requiredType);
        }
        public static bool IsValidFor(this object item, Type type)
        {
            return
                item == null &&
                type.AllowsNull() ||
                item.Is(type);
        }
        public static bool FirstIsSecond<T1, T2>()
        {
            return typeof(T1).Is(typeof(T2));
        }
        public static bool IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }
        public static bool IsNullableValueType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }
        public static bool AllowsNull(this Type type)
        {
            type.ValidateNonNull(nameof(type));
            return
                !type.IsValueType() ||
                type.IsNullableValueType();
        }

        public static bool IsNull<T>(this T item)
        {
            return
                typeof(T).AllowsNull() &&
                (object)item == null;
        }

        #region Member

        public static T Attribute<T>(this MemberInfo memberInfo)
            where T : Attribute
        {
            return (T)memberInfo.GetCustomAttributes(typeof(T), true).FirstOrDefault();
        }

        #endregion

        #region Property

        public static bool IsReadable(this PropertyInfo propertyInfo, bool @public = true)
        {
            propertyInfo.ValidateNonNull("propertyInfo");
            return
                propertyInfo.CanRead &&
                propertyInfo.GetGetMethod(!@public) != null;
        }
        public static bool IsWritable(this PropertyInfo propertyInfo, bool @public = true)
        {
            propertyInfo.ValidateNonNull("propertyInfo");
            return
                propertyInfo.CanWrite &&
                propertyInfo.GetSetMethod(!@public) != null;
        }

        public static PropertyInfo PropertyOf<T>(string propertyName)
        {
            return typeof(T).
                GetProperty(propertyName);
        }

        #endregion

        #region Enum

        public static IEnumerable<T> EnumFlags<T>(this T value, bool skipDefault = true)
            where T : struct
        {
            var flags = Enum.GetValues(typeof(T)).
                Cast<T>().
                Where(flag => ((Enum)(object)value).HasFlag((Enum)(object)flag));
            if (skipDefault)
                flags = flags.Where(flag => !EqualityComparer<T>.Default.Equals(flag, default(T)));
            return flags;
        }
        public static IEnumerable<string> EnumFlagNames<T>(this T value, bool skipDefault = true)
            where T : struct
        {
            return value.EnumFlags(skipDefault).
                Select(flag => Enum.GetName(typeof(T), flag));
        }
        public static string EnumName<T>(this T value)
            where T : struct
        {
            return Enum.GetName(typeof(T), value);
        }

        #endregion

        public static string Name(this Type type, bool excludeGenericPart)
        {
            type.ValidateNonNull("type");
            string name = type.Name;
            if (excludeGenericPart)
            {
                int index = name.IndexOf('`');
                if (index > 0)
                    name = name.Substring(0, index);
            }
            return name;
        }

        public static IEnumerable<Type> Types(
            this Type type,
            bool includeBases = true,
            bool includeInterfaces = true,
            bool includeThis = true,
            bool includeObjectBase = true
            )
        {
            type.ValidateNonNull("type");
            if (includeThis)
                yield return type;
            var baseType = type;
            while ((baseType = baseType.GetTypeInfo().BaseType) != null)
            {
                if (includeObjectBase || baseType != typeof(object))
                    yield return baseType;
            }
            if (includeInterfaces)
            {
                foreach (var interfaceType in type.GetInterfaces())
                    yield return interfaceType;
            }
        }
    }
}
