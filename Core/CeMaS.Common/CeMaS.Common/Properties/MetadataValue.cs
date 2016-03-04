using CeMaS.Common.Validation;

namespace CeMaS.Common.Properties
{
    /// <summary>
    /// <see cref="IMetadata"/> typed value access.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public class MetadataValue<T>
    {
        public MetadataValue(
            string name,
            T defaultValue = default(T),
            IValidator<T> validator = null
            )
        {
            name.ValidateNonNullOrEmpty(nameof(name));
            Name = name;
            DefaultValue = defaultValue;
            Validator = validator;
        }

        /// <summary>
        /// Metadata value name.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Metadata default value.
        /// </summary>
        public T DefaultValue { get; }
        /// <summary>
        /// Optional metadata value validator used by <see cref="SetValue"/>.
        /// </summary>
        public IValidator<T> Validator { get; }

        /// <summary>
        /// Gets metadata value.
        /// </summary>
        /// <param name="metadata">Metadata.</param>
        /// <returns>Value if it is <typeparamref name="T"/>, otherwise <see cref="DefaultValue"/>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="metadata"/> is null.</exception>
        public T GetValue(IMetadata metadata)
        {
            Validate(metadata);
            object value = metadata[Name];
            return value is T ?
                (T)value :
                DefaultValue;
        }
        /// <summary>
        /// Sets metadata value.
        /// </summary>
        /// <param name="metadata">Metadata.</param>
        /// <param name="value">Metadata value.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="metadata"/> is null.</exception>
        /// <exception cref="ValidationException"><paramref name="value"/> is invalid.</exception>
        public void SetValue(IMetadataWritable metadata, T value)
        {
            Validate(metadata);
            if (Validator != null)
                Validator.ValidateWithErrors(value).Validate(nameof(value));
            metadata[Name] = value;
        }

        private static void Validate(IMetadata metadata)
        {
            metadata.ValidateNonNull(nameof(metadata));
        }
    }
}
