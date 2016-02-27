using CeMaS.Common.Identity;
using CeMaS.Common.Validation;

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
            IdentityInfo info
            ) :
            base(id, info)
        {
            @object.ValidateNonNull(nameof(@object));
            Object = @object;
        }

        public ObjectItemInfo(IObjectItemInfo value) :
            this(
                new ObjectInfo(value.Object),
                value.Id,
                (IdentityInfo)value.Info
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
