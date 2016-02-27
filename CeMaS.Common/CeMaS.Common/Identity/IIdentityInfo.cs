using CeMaS.Common.Properties;

namespace CeMaS.Common.Identity
{
    /// <summary>
    /// Information about <see cref="IIdentity{T}"/>.
    /// </summary>
    public interface IIdentityInfo :
        IHaveMetadata
    {
        /// <summary>
        /// Name.
        /// </summary>
        /// <value>non-empty</value>
        string Name { get; }
        /// <summary>
        /// Optional description.
        /// </summary>
        string Description { get; }
    }
}
