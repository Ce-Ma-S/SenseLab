using CeMaS.Common.Collections;
using CeMaS.Common.Identity;
using System.Collections.Generic;

namespace SenseLab.Common.Objects
{
    public class ObjectType :
        Identity<string>,
        IObjectType
    {
        public ObjectType(
            string id,
            string name,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(id, name, description, values)
        { }

        public ObjectType(IObjectType value) :
            this(
                value.Id,
                value.Name,
                value.Description,
                value.Values.ToDictionary()
                )
        {
        }
    }
}
