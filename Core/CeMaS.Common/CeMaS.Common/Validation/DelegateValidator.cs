using CeMaS.Common.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CeMaS.Common.Validation
{
    public class DelegateValidator<T> :
        Validator<T>
    {
        #region Init

        public DelegateValidator(
            Func<T, IEnumerable<ValidationError>> validate,
            ValidationScope scope
            ) :
            this(
                validate,
                scope == null ?
                    (Func<ValidationScope>)null :
                    () => scope
                )
        { }

        public DelegateValidator(
            Func<T, IEnumerable<ValidationError>> validate,
            Func<ValidationScope> getScope = null
            )
        {
            this.getScope = getScope;
            Argument.NonNull(validate, nameof(validate));
            this.validate = validate;
        }

        #endregion

        protected override ValidationScope GetScope()
        {
            return getScope == null ?
                null :
                getScope();
        }
        protected override IEnumerable<ValidationError> DoValidate(T value)
        {
            return validate(value).
                SetScope(Scope);
        }

        private readonly Func<T, IEnumerable<ValidationError>> validate;
        private readonly Func<ValidationScope> getScope;
    }
}
