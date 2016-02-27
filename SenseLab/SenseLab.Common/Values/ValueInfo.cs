using CeMaS.Common.Identity;
using CeMaS.Common.Validation;
using System;

namespace SenseLab.Common.Values
{
    public class ValueInfo :
        Identity<string>,
        IValueInfo
    {
        #region Init

        public ValueInfo(
            string id,
            IdentityInfo info,
            Type type
            ) :
            base(id, info)
        {
            type.ValidateNonNull(nameof(type));
            Type = type;
        }

        public ValueInfo(
            IValueInfo value
            ) :
            this(
                value.Id,
                (IdentityInfo)value.Info,
                value.Type
                )
        { }

        #endregion

        public Type Type { get; }
    }
}
