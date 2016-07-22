using System.Collections.Generic;
using System.Linq;

namespace CeMaS.Common.Validation
{
    public class EnumerableValidator<TValue, TItem> :
        Validator<TValue>
        where TValue : IEnumerable<TItem>
    {
        public EnumerableValidator(
            IValidator<TItem> itemValidator
            )
        {
            Argument.NonNull(itemValidator, nameof(itemValidator));
            ItemValidator = itemValidator;
        }

        public IValidator<TItem> ItemValidator { get; private set; }

        protected override ValidationScope GetScope()
        {
            return ItemValidator.Scope;
        }
        protected override IEnumerable<ValidationError> DoValidate(TValue value)
        {
            return value.SelectMany(i => ItemValidator.Validate(i));
        }
    }
}
