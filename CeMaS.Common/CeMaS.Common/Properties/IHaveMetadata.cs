namespace CeMaS.Common.Properties
{
    /// <summary>
    /// Object`s metadata access.
    /// </summary>
    public interface IHaveMetadata
    {
        /// <summary>
        /// Object`s metadata.
        /// </summary>
        /// <value>non-null</value>
        IMetadata Metadata { get; }
    }
}
