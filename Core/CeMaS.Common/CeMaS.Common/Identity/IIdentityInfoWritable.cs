using CeMaS.Common.Properties;

namespace CeMaS.Common.Identity
{
    /// <summary>
    /// Writable <see cref="IIdentityInfo"/>.
    /// </summary>
    public interface IIdentityInfoWritable :
        IIdentityInfo,
        IHaveNamedValuesWritable
    {
        /// <summary>
        /// Name.
        /// </summary>
        /// <value>non-empty</value>
        new string Name { get; set; }
        /// <summary>
        /// Optional description.
        /// </summary>
        new string Description { get; set; }
    }
}
