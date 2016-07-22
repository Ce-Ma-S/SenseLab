using System;

namespace CeMaS.Common.Properties
{
    /// <summary>
    /// Information about an object`s property.
    /// </summary>
    public interface IPropertyWithOwner :
        IProperty
    {
        /// <summary>
        /// Property owner type.
        /// </summary>
        /// <value>non-null</value>
        Type OwnerType { get; }
        /// <summary>
        /// Whether this property is static and <see cref="GetValue"/>/<see cref="SetValue"/> does not require owner instance.
        /// </summary>
        /// <value>non-null</value>
        bool IsStatic { get; }

        /// <summary>
        /// Gets property value from <paramref name="owner"/>.
        /// </summary>
        /// <param name="owner">Owner. Can be null for static properties.</param>
        /// <exception cref="ArgumentException"><paramref name="owner"/> is invalid.</exception>
        /// <exception cref="InvalidOperationException"><see cref="IProperty.IsReadable"/> is false.</exception>
        object GetValue(object owner);
        /// <summary>
        /// Sets property value to <paramref name="owner"/>.
        /// </summary>
        /// <param name="owner">Owner. Can be null for static properties.</param>
        /// <param name="value">Value.</param>
        /// <exception cref="ArgumentException"><paramref name="owner"/> is invalid.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is invalid.</exception>
        /// <exception cref="InvalidOperationException"><see cref="IProperty.IsWritable"/> is false.</exception>
        void SetValue(object owner, object value);
    }


    /// <summary>
    /// Information about an object`s property.
    /// </summary>
    /// <typeparam name="T">Owner type.</typeparam>
    public interface IPropertyWithOwner<T> :
        IPropertyWithOwner
    {
        /// <summary>
        /// Allows to change the owner type if the property is compatible with it.
        /// </summary>
        /// <typeparam name="TNewOwner">New owner type.</typeparam>
        /// <returns>Property with new owner if accessible, otherwise null.</returns>
        IPropertyWithOwner<TNewOwner> ChangeOwner<TNewOwner>();
    }
}