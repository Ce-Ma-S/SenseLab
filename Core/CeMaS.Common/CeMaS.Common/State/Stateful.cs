using CeMaS.Common.Collections;
using CeMaS.Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace CeMaS.Common.State
{
    /// <summary>
    /// <see cref="IStateful"/>/<see cref="IStatefulWritable"/> helper.
    /// </summary>
    public static class Stateful
    {
        /// <summary>
        /// Default stateful properties filter.
        /// Filters properties with <see cref="DataMemberAttribute"/>.
        /// </summary>
        public static readonly Func<PropertyInfo, bool> PropertyFilter =
            i => GetStatefulAttribute(i) != null;

        /// <summary>
        /// Fills <paramref name="state"/> with values of <paramref name="stateful"/>`s properties filtered by <paramref name="propertyFilter"/>.
        /// </summary>
        /// <remarks>It is intended for <see cref="IStateful.GetState"/> implementations.</remarks>
        /// <param name="stateful">Stateful.</param>
        /// <param name="state">State.</param>
        /// <param name="getPropertyValue">
        /// Optional property value getter.
        /// If null or <see cref="Optional{object}.None"/> is returned, <see cref="PropertyInfo.GetValue(object)"/> is used.
        /// </param>
        /// <param name="propertyFilter">
        /// Optional property filter for <paramref name="stateful"/> public properties.
        /// If null, <see cref="PropertyFilter"/> is used.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="stateful"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="stateful"/> property value cannot be got.</exception>
        public static void FillState(
            this IStateful stateful,
            IDictionary<string, object> state,
            Func<PropertyInfo, Optional<object>> getPropertyValue = null,
            Func<PropertyInfo, bool> propertyFilter = null
            )
        {
            Argument.NonNull(state, nameof(state));
            ProcessProperties(
                stateful,
                propertyFilter,
                (id, property, required) =>
                {
                    try
                    {
                        state[id] = GetPropertyValue(stateful, getPropertyValue, property);
                    }
                    catch (Exception e)
                    {
                        throw new ArgumentException($"Cannot get state value '{id}' from property '{property.Name}'.", nameof(stateful), e);
                    }
                });
        }

        /// <summary>
        /// Applies <paramref name="state"/> to values of <paramref name="stateful"/>`s properties filtered by <paramref name="propertyFilter"/>.
        /// </summary>
        /// <remarks>It is intended for <see cref="IStatefulWritable.SetState"/> implementations.</remarks>
        /// <param name="stateful">Stateful.</param>
        /// <param name="state">State.</param>
        /// <param name="setPropertyValue">
        /// Optional property value setter.
        /// If <c>null</c> or <c>false</c> is returned, <see cref="PropertyInfo.SetValue(object)"/> is used.
        /// </param>
        /// <param name="propertyFilter">
        /// Optional property filter for <paramref name="stateful"/> public properties.
        /// If null, <see cref="PropertyFilter"/> is used.
        /// </param>
        /// <returns>Whether any property value was set.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stateful"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is null.</exception>
        /// <exception cref="ArgumentException">Required <paramref name="state"/>`s value does not exist.</exception>
        /// <exception cref="ArgumentException"><paramref name="stateful"/> property value cannot be set.</exception>
        public static bool ApplyState(
            this IStatefulWritable stateful,
            IReadOnlyDictionary<string, object> state,
            Func<PropertyInfo, object, bool> setPropertyValue = null,
            Func<PropertyInfo, bool> propertyFilter = null
            )
        {
            Argument.NonNull(state, nameof(state));
            bool changed = false;
            ProcessProperties(
                stateful,
                propertyFilter,
                (id, property, required) =>
                {
                    var value = state.Value(id);
                    if (!value.HasValue)
                    {
                        if (required)
                            throw new ArgumentException($"Required state value '{id}' does not exist.", nameof(state));
                        else
                            return;
                    }
                    try
                    {
                        SetPropertyValue(stateful, setPropertyValue, property, value);
                    }
                    catch (Exception e)
                    {
                        throw new ArgumentException($"Cannot set state value '{id}' to property '{property.Name}'.", nameof(stateful), e);
                    }
                    // TODO: check value change if required
                    changed = true;
                });
            return changed;
        }


        private static object GetPropertyValue(
            IStateful stateful,
            Func<PropertyInfo, Optional<object>> getPropertyValue,
            PropertyInfo property
            )
        {
            var value = getPropertyValue == null ?
                Optional<object>.None :
                getPropertyValue(property);
            if (!value.HasValue)
                value = property.GetValue(stateful);
            return value.Value;
        }

        private static void SetPropertyValue(
            IStatefulWritable stateful,
            Func<PropertyInfo, object, bool> setPropertyValue,
            PropertyInfo property,
            object value
            )
        {
            if (
                setPropertyValue == null ||
                !setPropertyValue(property, value)
                )
            {
                property.SetValue(stateful, value);
            }
        }

        private static void ProcessProperties(
            this IStateful stateful,
            Func<PropertyInfo, bool> propertyFilter,
            Action<string, PropertyInfo, bool> processProperty
            )
        {
            Argument.NonNull(stateful, nameof(stateful));
            var properties = stateful.GetType().GetProperties().
                Where(propertyFilter ?? PropertyFilter).
                Select(i => new { Property = i, Attribute = GetStatefulAttribute(i) ?? new DataMemberAttribute() }).
                OrderBy(i => i.Attribute.Order);
            foreach (var property in properties)
            {
                var id = property.Attribute.IsNameSetExplicitly ?
                    property.Attribute.Name :
                    property.Property.Name;
                processProperty(id, property.Property, property.Attribute.IsRequired);
            }
        }

        private static DataMemberAttribute GetStatefulAttribute(PropertyInfo property)
        {
            return property.GetCustomAttribute<DataMemberAttribute>();
        }
    }
}
