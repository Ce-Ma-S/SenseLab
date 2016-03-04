using CeMaS.Common.Identity;

namespace SenseLab.Common.Objects
{
    public class ObjectType :
        Identity<string>,
        IObjectType
    {
        public ObjectType(
            string id,
            IdentityInfo info
            ) :
            base(id, info)
        { }

        public ObjectType(IObjectType value) :
            this(
                value.Id,
                (IdentityInfo)value.Info
                )
        {
        }
    }
}
