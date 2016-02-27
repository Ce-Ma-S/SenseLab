namespace SenseLab.Common.Objects
{
    public class ObjectType :
        Item<string>,
        IObjectType
    {
        public ObjectType(
            string id,
            string name,
            string description = null
            ) :
            base(id, name, description)
        {
        }

        public ObjectType(IObjectType value) :
            this(
                value.Id,
                value.Name,
                value.Description
                )
        {
        }
    }
}
