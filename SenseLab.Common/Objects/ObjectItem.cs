using CeMaS.Common.Validation;

namespace SenseLab.Common.Objects
{
    public class ObjectItem :
        Item<string>,
        IObjectItemWritable
    {
        public ObjectItem(
            IObject @object,
            string id,
            string name,
            string description = null
            ) :
            base(id, name, description)
        {
            @object.ValidateNonNull(nameof(@object));
            Object = @object;
        }

        public IObject Object { get; }
    }
}
