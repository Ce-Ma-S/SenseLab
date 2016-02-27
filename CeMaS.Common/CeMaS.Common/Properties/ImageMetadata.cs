namespace CeMaS.Common.Properties
{
    /// <summary>
    /// Image <see cref="IMetadata"/>.
    /// </summary>
    public class ImageMetadata
    {
        /// <summary>
        /// Text identifies an image.
        /// Default value: null.
        /// </summary>
        public static readonly MetadataValue<string> Instance = new MetadataValue<string>("Image");
    }
}
