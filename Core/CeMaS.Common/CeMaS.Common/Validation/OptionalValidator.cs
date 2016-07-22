using System;
using System.Collections.Generic;

namespace CeMaS.Common.Validation
{
    public class OptionalValidator<TValue, TValidator> :
        CompositeValidatorBase<TValue>
        where TValidator : IValidator<TValue>
    {
        public OptionalValidator(
            Func<TValue, bool> validates,
            TValidator validator,
            ValidationScope scope = null
            )
        {
            Argument.NonNull(validates, nameof(validates));
            Argument.NonNull(validator, nameof(validator));
            this.validates = validates;
            Validator = validator;
        }

        public TValidator Validator { get; private set; }

        public bool Validates(TValue value)
        {
            return validates(value);
        }

        public override IEnumerable<IValidator<TValue>> GetChildren(TValue value)
        {
            if (Validates(value))
                yield return Validator;
        }

        protected override ValidationScope GetScope()
        {
            return Validator.Scope;
        }

        private readonly Func<TValue, bool> validates;
    }


    public class OptionalValidator<T> :
        OptionalValidator<T, IValidator<T>>
    {
        public OptionalValidator(Func<T, bool> validates, IValidator<T> validator)
            : base(validates, validator)
        { }
    }
}
