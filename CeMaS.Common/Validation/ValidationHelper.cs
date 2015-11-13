using System;
using System.Runtime.CompilerServices;

namespace CeMaS.Common.Validation
{
    public static class ValidationHelper
    {
        public static void ValidateNonNull(
            this object value,
            [CallerMemberName]
            string name = null
            )
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }
    }
}
