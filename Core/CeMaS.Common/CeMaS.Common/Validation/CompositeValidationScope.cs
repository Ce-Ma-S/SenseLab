using CeMaS.Common.Collections;
using CeMaS.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CeMaS.Common.Validation
{
    /// <summary>
    /// Specifies composite validation scope.
    /// </summary>
    public class CompositeValidationScope :
        ValidationScope
    {
        #region Init

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="scopes"><see cref="Scopes"/></param>
        /// <param name="info">Information. If null, it is composed.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="scopes"/> is null.</exception>
        public CompositeValidationScope(IEnumerable<ValidationScope> scopes, IdentityInfo info = null) :
            this(info, scopes.ToArray())
        { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="scopes"><see cref="PropertyName"/></param>
        /// <exception cref="System.ArgumentNullException"><paramref name="scopes"/> is null.</exception>
        public CompositeValidationScope(params ValidationScope[] scopes) :
            this(null, scopes)
        { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="scopes"><see cref="PropertyName"/></param>
        /// <param name="info">Information. If null, it is composed.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="scopes"/> is null.</exception>
        public CompositeValidationScope(IdentityInfo info, params ValidationScope[] scopes) :
            base(GetId(scopes), GetInfo(scopes, info))
        {
            Scopes = scopes.ToArray();
        }

        /// <summary>
        /// Creates <see cref="CompositeValidationScope"/> from <paramref name="scopes"/>, but if the composite would be equal to <see cref="ValidationScope.Whole"/>, that is returned instead of the composite.
        /// </summary>
        /// <param name="scopes"><see cref="Scopes"/></param>
        /// <exception cref="System.ArgumentNullException"><paramref name="scopes"/> is null.</exception>
        public static ValidationScope From(IEnumerable<ValidationScope> scopes)
        {
            return GetId(scopes) == Whole.Id ?
                Whole :
                new CompositeValidationScope(scopes);
        }

        #endregion

        /// <summary>
        /// Scopes of this scope.
        /// </summary>
        /// <value>non-null.</value>
        public IEnumerable<ValidationScope> Scopes { get; }

        private static string GetId(IEnumerable<ValidationScope> scopes)
        {
            return GetValue(scopes, i => i.Id, IdSeparator, Whole.Id);
        }
        private static IdentityInfo GetInfo(IEnumerable<ValidationScope> scopes, IdentityInfo info)
        {
            return info ??
                new IdentityInfo(
                    GetValue(scopes, i => i.Info.Name, NameSeparator, Whole.Info.Name)
                    );
        }
        private static string GetValue(
            IEnumerable<ValidationScope> scopes,
            Func<ValidationScope, string> value,
            string[] separator,
            string whole
            )
        {
            Argument.NonNull(scopes, nameof(scopes));
            scopes = scopes.Distinct();
            var values = scopes.
                SelectMany(i => value(i).Split(separator, StringSplitOptions.RemoveEmptyEntries)).
                Distinct().
                Order();
            return values.IsEmpty() || values.Contains(whole) ?
                whole :
                string.Join(separator[0], values);
        }

        private static readonly string[] IdSeparator = { "+" };
        private static readonly string[] NameSeparator = { Environment.NewLine };
    }
}