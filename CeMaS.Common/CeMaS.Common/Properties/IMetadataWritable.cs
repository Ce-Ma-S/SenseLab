namespace CeMaS.Common.Properties
{
    /// <summary>
    /// Writable object`s metadata.
    /// </summary>
    public interface IMetadataWritable :
        IMetadata
    {
        /// <summary>
        /// Gets or sets metadata value.
        /// </summary>
        /// <remarks>Setting value to null removes it from available metadata.</remarks>
        /// <param name="id">Metadata value identifier.</param>
        /// <returns>Metadata value.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="id"/> is null or empty.</exception>
        new object this[string id] { get; set; }
    }
}
