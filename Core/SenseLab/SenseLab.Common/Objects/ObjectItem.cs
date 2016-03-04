using CeMaS.Common.Identity;

namespace SenseLab.Common.Objects
{
    public abstract class ObjectItem :
        ObjectItemInfo,
        IObjectItem
    {
        public ObjectItem(
            Object @object,
            string id,
            IdentityInfo info
            ) :
            base(@object, id, info)
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
