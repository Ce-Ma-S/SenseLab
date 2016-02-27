using CeMaS.Common.Properties;
using CeMaS.Common.Validation;
using System;
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
        /// <param name="propertyFilter">
        /// Optional property filter for <paramref name="stateful"/> public properties.
        /// If null, <see cref="PropertyFilter"/> is used.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="stateful"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="state"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="stateful"/> property value cannot be got.</exception>
        public static void FillState(
            this IStateful stateful,
            IMetadataWritable state,
            Func<PropertyInfo, bool> propertyFilter = null
            )
        {
            state.ValidateNonNull(nameof(state));
            ProcessProperties(
                stateful,
                propertyFilter,
                (id, property, required) =>
                {
                    try
                    {
                        state[id] = property.GetValue(stateful);
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
            IMetadata state,
            Func<PropertyInfo, bool> propertyFilter = null
            )
        {
            state.ValidateNonNull(nameof(state));
            bool changed = false;
            ProcessProperties(
                stateful,
                propertyFilter,
                (id, property, required) =>
                {
                    object value = state[id];
                    if (value == null)
                    {
                        if (required)
                            throw new ArgumentException($"Required state value '{id}' does not exist.", nameof(state));
                        else
                            return;
                    }
                    try
                    {
                        property.SetValue(stateful, value);
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

        private static void ProcessProperties(
            this IStateful stateful,
            Func<PropertyInfo, bool> propertyFilter,
            Action<string, PropertyInfo, bool> processProperty
            )
        {
            stateful.ValidateNonNull(nameof(stateful));
            var properties = stateful.GetType().GetTypeInfo().DeclaredProperties.
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
