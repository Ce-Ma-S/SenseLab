using CeMaS.Common.Identity;
using System;

namespace CeMaS.Common.Properties
{
    /// <summary>
    /// Information about an object`s property.
    /// </summary>
    public interface IProperty :
        IIdentity<string>
    {
        /// <summary>
        /// Property name excluding <paramref name="Parent"/> information.
        /// </summary>
        /// <remarks>Usable in .net reflection, data bindings etc.</remarks>
        /// <value>non-empty</value>
        string PropertyName { get; }
        /// <summary>
        /// Whether this property value can be read.
        /// </summary>
        bool IsReadable { get; }
        /// <summary>
        /// Whether this property value can be written.
        /// </summary>
        bool IsWritable { get; }
        /// <summary>
        /// Property value type.
        /// </summary>
        /// <value>non-null</value>
        Type ValueType { get; }
    }


    /// <summary>
    /// Information about an object`s property.
    /// </summary>
    /// <typeparam name="T">Property value type.</typeparam>
    public interface IProperty<T> :
        IProperty
    { }


    /// <summary>
    /// Information about an object`s property.
    /// </summary>
    /// <typeparam name="TOwner">Owner type.</typeparam>
    /// <typeparam name="TValue">Property value type.</typeparam>
    public interface IProperty<TOwner, TValue> :
        IProperty<TValue>,
        IPropertyWithOwner<TOwner>
    {
        /// <summary>
        /// Gets property value from <paramref name="owner"/>.
        /// </summary>
        /// <param name="owner">Owner. Can be null for static properties.</param>
        /// <exception cref="ArgumentException"><paramref name="owner"/> is invalid.</exception>
        /// <exception cref="InvalidOperationException"><see cref="IProperty.IsReadable"/> is false.</exception>
        TValue GetValue(TOwner owner);
        /// <summary>
        /// Sets property value to <paramref name="owner"/>.
        /// </summary>
        /// <param name="owner">Owner. Can be null for static properties.</param>
        /// <param name="value">Value.</param>
        /// <exception cref="ArgumentException"><paramref name="owner"/> is invalid.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is invalid.</exception>
        /// <exception cref="InvalidOperationException"><see cref="IProperty.IsWritable"/> is false.</exception>
        void SetValue(TOwner owner, TValue value);
    }
}
