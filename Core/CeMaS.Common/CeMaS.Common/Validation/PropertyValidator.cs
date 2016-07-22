using CeMaS.Common.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CeMaS.Common.Validation
{
    public class PropertyValidator<I, V> :
        Validator<I>
    {
        #region Init

        public PropertyValidator(
            Property<I, V> property,
            Func<I, IValidator<V>> getPropertyValueValidator,
            bool cachePropertyValueValidators = true
            )
        {
            scope = new PropertyValidationScope<I, V>(property);
            Argument.NonNull(getPropertyValueValidator, nameof(getPropertyValueValidator));
            this.getPropertyValueValidator = getPropertyValueValidator;
            if (cachePropertyValueValidators)
                propertyValueValidators = new Dictionary<I, IValidator<V>>();
        }

        public PropertyValidator(
            Property<I, V> property,
            Func<I, Func<V, IEnumerable<ValidationError>>> validation,
            bool cachePropertyValueValidators = true
            ) :
            this(
                property,
                i => new DelegateValidator<V>(validation(i)),
                cachePropertyValueValidators
                )
        {
            Argument.NonNull(validation, nameof(validation));
        }

        public PropertyValidator(
            Property<I, V> property,
            IValidator<V> validator
            ) :
            this(
                property,
                v => validator,
                false
                )
        {
            Argument.NonNull(validator, nameof(validator));
        }

        public PropertyValidator(
            Property<I, V> property,
            Func<V, IEnumerable<ValidationError>> validation
            ) :
            this(
                property,
                v => new DelegateValidator<V>(validation),
                false
                )
        {
            Argument.NonNull(validation, nameof(validation));
        }

        #endregion

        public Property<I, V> Property
        {
            get { return scope.Property; }
        }

        public IValidator<V> GetPropertyValueValidator(I value)
        {
            IValidator<V> propertyValueValidator;
            if (
                propertyValueValidators == null ||
                !propertyValueValidators.TryGetValue(value, out propertyValueValidator)
                )
            {
                propertyValueValidator = getPropertyValueValidator(value);
                Debug.Assert(propertyValueValidator != null);
                if (propertyValueValidators != null)
                    propertyValueValidators.Add(value, propertyValueValidator);
            }
            return propertyValueValidator;
        }

        protected override ValidationScope GetScope()
        {
            return scope;
        }
        protected override IEnumerable<ValidationError> DoValidate(I value)
        {
            V propertyValue = Property.GetValue(value);
            var propertyValueValidator = GetPropertyValueValidator(value);
            return propertyValueValidator.
                Validate(propertyValue).
                SetScope(scope);
        }

        private readonly PropertyValidationScope<I, V> scope;
        private readonly Func<I, IValidator<V>> getPropertyValueValidator;
        private readonly Dictionary<I, IValidator<V>> propertyValueValidators;
    }
}
