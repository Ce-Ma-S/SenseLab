using System.Collections.Generic;

namespace SenseLab.Common.Objects
{
    public abstract class ObjectItem :
        ObjectItemInfo,
        IObjectItem
    {
        public ObjectItem(
            Object @object,
            string id,
            string name,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(@object, id, name, description, values)
        { }

        public new Object Object
        {
            get { return (Object)base.Object; }
        }
        IObject IObjectItem.Object
        {
            get { return Object; }
        }
    }
}
