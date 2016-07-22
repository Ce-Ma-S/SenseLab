using System.Collections.Generic;
using System.Linq;

namespace CeMaS.Common.Validation
{
    public abstract class CompositeValidatorBase<T> :
        Validator<T>,
        ICompositeValidator<T>
    {
        public abstract IEnumerable<IValidator<T>> GetChildren(T value);

        protected override IEnumerable<ValidationError> DoValidate(T value)
        {
            return GetChildren(value).Validate(value);
        }
    }
}
