using CeMaS.Common.Collections;
using CeMaS.Common.Identity;
using CeMaS.Common.Validation;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Values
{
    public class ValueInfo :
        Identity<string>,
        IValueInfo
    {
        #region Init

        public ValueInfo(
            string id,
            string name,
            Type type,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(id, name, description, values)
        {
            Argument.NonNull(type, nameof(type));
            Type = type;
        }

        public ValueInfo(
            IValueInfo value
            ) :
            this(
                value.Id,
                value.Name,
                value.Type,
                value.Description,
                value.Values.ToDictionary()
                )
        { }

        #endregion

        public Type Type { get; }
    }
}
