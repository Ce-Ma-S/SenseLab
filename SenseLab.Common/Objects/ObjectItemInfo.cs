using CeMaS.Common.Validation;

namespace SenseLab.Common.Objects
{
    public class ObjectItemInfo :
        Item<string>,
        IObjectItemInfo
    {
        #region Init

        public ObjectItemInfo(
            ObjectInfo @object,
            string id,
            string name,
            string description = null
            ) :
            base(id, name, description)
        {
            @object.ValidateNonNull(nameof(@object));
            Object = @object;
        }

        public ObjectItemInfo(IObjectItemInfo value) :
            this(
                new ObjectInfo(value.Object),
                value.Id,
                value.Name,
                value.Description
                )
        {
        }

        #endregion

        public ObjectInfo Object { get; }
        IObjectInfo IObjectItemInfo.Object
        {
            get { return Object; }
        }
    }
}
