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
            string description = null
            ) :
            base(@object, id, name, description)
        {
        }

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
