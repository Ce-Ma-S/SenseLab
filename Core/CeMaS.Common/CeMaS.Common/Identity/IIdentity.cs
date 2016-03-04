using System;

namespace CeMaS.Common.Identity
{
    /// <summary>
    /// Identifiable object with extended information about itself.
    /// </summary>
    /// <typeparam name="T">Identifier type.</typeparam>
    public interface IIdentity<T> :
        IId<T>
    {
        /// <summary>
        /// Information to be presented in UI.
        /// </summary>
        /// <value>non-null</value>
        IIdentityInfo Info { get; }
        /// <summary>
        /// Raised when <see cref="Info"/> changes.
        /// </summary>
        event EventHandler InfoChanged;
    }
}
