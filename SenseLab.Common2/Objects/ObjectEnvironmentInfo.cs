using System;

namespace SenseLab.Common.Objects
{
    public class ObjectEnvironmentInfo :
        Item<Guid>,
        IObjectEnvironmentInfo
    {
        public ObjectEnvironmentInfo(
            Guid id,
            string name,
            string description = null
            ) :
            base(id, name, description)
        {
        }
        public ObjectEnvironmentInfo(IObjectEnvironmentInfo value) :
            this(
                value.Id,
                value.Name,
                value.Description
                )
        {
        }
    }
}
