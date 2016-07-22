using CeMaS.Common.Identity;
using CeMaS.Common.Validation;
using System;

namespace CeMaS.Common.Properties
{
    /// <summary>
    /// Typed keyed/named value access to a key/value provider.
    /// </summary>
    /// <remarks>
    /// For concrete named value access, create a static class named like <c>Values</c> and add static readonly field of type <see cref="NamedValue{T}"/> and name the field the same as <c><see cref="Id"/>:
    /// <code>
    /// public static class Values
    /// {
    ///     public static readonly NamedValue<T> Name = new NamedValue<T>(nameof(Name));
    /// }
    /// </code>
    /// </remarks>
    /// <typeparam name="T">Value type.</typeparam>
    public class NamedValue<T> :
        IId<string>
    {
        #region Init

        public NamedValue(
            string key,
            T defaultValue = default(T),
            Func<Func<string, object>, T> defaultValueCallback = null,
            IValidator<T> validator = null
            )
        {
            Argument.NonNullOrEmpty(key, nameof(key));
            Id = key;
            DefaultValue = defaultValue;
            DefaultValueCallback = defaultValueCallback;
            Validator = validator;
        }

        #endregion

        /// <summary>
        /// Value key/name.
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Default value.
        /// </summary>
        public T DefaultValue { get; }
        /// <summary>
        /// Optional default value callback.
        /// First parameter is values returning null if a key is not found.
        /// </summary>
        public Func<Func<string, object>, T> DefaultValueCallback { get; }
        /// <summary>
        /// Optional value validator used by <see cref="SetValue"/>.
        /// </summary>
        public IValidator<T> Validator { get; }

        /// <summary>
        /// Gets default value.
        /// </summary>
        /// <param name="getValue">Gets value. Returns null if a key is not found.</param>
        /// <returns><see cref="DefaultValueCallback"/> result if the callback is specified, otherwise <see cref="DefaultValue"/>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="getValue"/> is null.</exception>
        public T GetDefaultValue(Func<string, object> getValue)
        {
            Argument.NonNull(getValue, nameof(getValue));
            return DefaultValueCallback != null ?
                DefaultValueCallback(getValue) :
                DefaultValue;
        }
        /// <summary>
        /// Gets value.
        /// </summary>
        /// <param name="getValue">Gets value. Returns null if a key is not found.</param>
        /// <returns>Value if it is <typeparamref name="T"/>, otherwise <see cref="GetDefaultValue"/>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="getValue"/> is null.</exception>
        public T GetValue(Func<string, object> getValue)
        {
            Argument.NonNull(getValue, nameof(getValue));
            object value = getValue(Id);
            return value is T ?
                (T)value :
                GetDefaultValue(getValue);
        }
        /// <summary>
        /// Sets value.
        /// </summary>
        /// <param name="setValue">Sets value.</param>
        /// <param name="value">Value.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="setValue"/> is null.</exception>
        /// <exception cref="ValidationException"><paramref name="value"/> is invalid.</exception>
        public void SetValue(Action<string, object> setValue, T value)
        {
            Argument.NonNull(setValue, nameof(setValue));
            if (Validator != null)
                Validator.Validate(value).Validate(nameof(value));
            setValue(Id, value);
        }
        /// <summary>
        /// Removes value.
        /// </summary>
        /// <param name="clearValue">Clears value.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="clearValue"/> is null.</exception>
        public void ClearValue(Action<string> clearValue)
        {
            Argument.NonNull(clearValue, nameof(clearValue));
            clearValue(Id);
        }
    }
}
