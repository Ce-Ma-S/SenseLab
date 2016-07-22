using CeMaS.Common.Collections;
using CeMaS.Common.Identity;
using CeMaS.Common.Validation;
using System.Collections.Generic;

namespace SenseLab.Common.Objects
{
    public class ObjectItemInfo :
        Identity<string>,
        IObjectItemInfo
    {
        #region Init

        public ObjectItemInfo(
            ObjectInfo @object,
            string id,
            string name,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(id, name, description, values)
        {
            Argument.NonNull(@object, nameof(@object));
            Object = @object;
        }

        public ObjectItemInfo(IObjectItemInfo value) :
            this(
                new ObjectInfo(value.Object),
                value.Id,
                value.Name,
                value.Description,
                value.Values.ToDictionary()
                )
        { }

        #endregion

        public ObjectInfo Object { get; }
        IObjectInfo IObjectItemInfo.Object
        {
            get { return Object; }
        }
    }
}
