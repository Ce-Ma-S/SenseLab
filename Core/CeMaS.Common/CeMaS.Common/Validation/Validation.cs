using CeMaS.Common.Conditions;
using CeMaS.Common.Properties;
using System.Collections.Generic;

namespace CeMaS.Common.Validation
{
    public static class Validation
    {
        #region Return

        public static IEnumerable<ValidationError> Return(bool isValid, string error)
        {
            if (!isValid)
                yield return new ValidationError(error);
        }
        public static IEnumerable<ValidationError> ReturnRequired(bool isValid)
        {
            return Return(isValid, Resources.Validation_Required);
        }

        #endregion

        public static IEnumerable<ValidationError> NonNull<T>(T value)
        {
            return ReturnRequired(value != null);
        }
        public static IEnumerable<ValidationError> NonNullOrEmpty(string value)
        {
            return ReturnRequired(!value.IsNullOrEmpty());
        }
    }
}
