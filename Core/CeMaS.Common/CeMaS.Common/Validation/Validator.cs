using CeMaS.Common.Collections;
using CeMaS.Common.Events;
using System;
using System.Collections.Generic;

namespace CeMaS.Common.Validation
{
    public abstract class Validator<T> :
        NotifyPropertyChange,
        IValidator<T>
    {
        public ValidationScope Scope
        {
            get { return GetScope() ?? ValidationScope.Whole; }
        }

        public IEnumerable<ValidationError> Validate(T value)
        {
            try
            {
                return DoValidate(value) ?? ValidationError.None;
            }
            catch (Exception e)
            {
                return new ValidationError(e).ToEnumerable();
            }
        }

        public override string ToString()
        {
            return $"{typeof(T).FullName}: {Scope}";
        }

        protected abstract ValidationScope GetScope();
        protected abstract IEnumerable<ValidationError> DoValidate(T value);
    }
}
