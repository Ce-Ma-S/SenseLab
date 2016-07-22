using CeMaS.Common.Identity;
using CeMaS.Common.Properties;
using System;

namespace CeMaS.Common.Validation
{
    /// <summary>
    /// Specifies validation scope.
    /// </summary>
    public class ValidationScope :
        IdentityWithInfo<string>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">
        /// <see cref="IdentityBase{T}.Id"/>.
        /// Empty means whole validated value (<see cref="Whole"/>).
        /// </param>
        /// <param name="info"><see cref="IdentityWithInfo{T}.Info"/>. If null, <paramref name="id"/> is used for <see cref="IdentityInfo.Name"/>.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="id"/> is null.</exception>
        public ValidationScope(
            string id,
            IdentityInfo info = null
            ) :
            base(
                id,
                info ?? new IdentityInfo(id)
                )
        {
            Argument.NonNull(id, nameof(id));
        }

        /// <summary>
        /// Represents whole validated value scope.
        /// </summary>
        public static readonly ValidationScope Whole = new ValidationScope(
            string.Empty,
            $"{nameof(ValidationScope)}_{nameof(Whole)}".ToIdentityInfo(Resources.ResourceManager)
            );
    }
}