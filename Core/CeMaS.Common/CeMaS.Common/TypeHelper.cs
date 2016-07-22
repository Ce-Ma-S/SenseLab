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
        public static bool Is<T>(
            this object item,
            bool allowNullIfNullable = false
            )
        {
            return item.Is(typeof(T), allowNullIfNullable);
        }
        public static bool Is(
            this object item,
            Type requiredType,
            bool allowNullIfNullable = false
            )
        {
            return
                allowNullIfNullable &&
                item == null &&
                requiredType.IsNullable() ||
                item != null &&
                item.GetType().Is(requiredType);
        }
        public static bool IsTypeOf(
            this object item,
            object itemOfRequiredType,
            bool allowNullIfNullable = false
            )
        {
            return
                itemOfRequiredType != null &&
                item.Is(itemOfRequiredType.GetType(), allowNullIfNullable);
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
        public static bool IsNullable(this Type type)
        {
            Validate(type);
            return
                !type.IsValueType() ||
                type.IsNullableValueType();
        }

        #region Member

        public static T Attribute<T>(this MemberInfo memberInfo)
            where T : Attribute
        {
            return (T)memberInfo.GetCustomAttribute(typeof(T), true);
        }

        #endregion

        #region Property

        public static bool IsReadable(this PropertyInfo propertyInfo, bool @public = true)
        {
            Argument.NonNull(propertyInfo, nameof(propertyInfo));
            return
                propertyInfo.CanRead &&
                propertyInfo.GetGetMethod(!@public) != null;
        }
        public static bool IsWritable(this PropertyInfo propertyInfo, bool @public = true)
        {
            Argument.NonNull(propertyInfo, nameof(propertyInfo));
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
            Validate(type);
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
            Argument.NonNull(type, nameof(type));
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

        public static T Attribute<T>(this Type type)
            where T : Attribute
        {
            return type.GetTypeInfo().Attribute<T>();
        }


        private static void Validate(Type type)
        {
            Argument.NonNull(type, nameof(type));
        }
    }
}
