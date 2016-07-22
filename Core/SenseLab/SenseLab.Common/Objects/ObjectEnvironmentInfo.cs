using CeMaS.Common.Collections;
using CeMaS.Common.Identity;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Objects
{
    public class ObjectEnvironmentInfo :
        Identity<Guid>,
        IObjectEnvironmentInfo
    {
        public ObjectEnvironmentInfo(
            Guid id,
            string name,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(id, name, description, values)
        { }
        public ObjectEnvironmentInfo(IObjectEnvironmentInfo value) :
            this(
                value.Id,
                value.Name,
                value.Description,
                value.Values.ToDictionary()
                )
        { }
    }
}
