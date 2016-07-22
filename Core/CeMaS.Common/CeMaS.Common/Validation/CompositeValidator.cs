using System.Collections.Generic;
using System.Linq;

namespace CeMaS.Common.Validation
{
    public class CompositeValidator<T> :
        CompositeValidatorBase<T>
    {
        #region Init

        public CompositeValidator(IEnumerable<IValidator<T>> validators)
        {
            Argument.NonNull(validators, nameof(validators));
            Validators = new List<IValidator<T>>(validators);
        }

        public CompositeValidator(
            params IValidator<T>[] validators
            )
            : this((IEnumerable<IValidator<T>>)validators)
        { }

        #endregion

        public IList<IValidator<T>> Validators { get; private set; }

        public override IEnumerable<IValidator<T>> GetChildren(T value)
        {
            return Validators;
        }

        protected override ValidationScope GetScope()
        {
            return CompositeValidationScope.From(Validators.Select(i => i.Scope));
        }
    }
}
