using CeMaS.Common.Identity;
using System;

namespace SenseLab.Common.Objects
{
    public class ObjectEnvironmentInfo :
        Identity<Guid>,
        IObjectEnvironmentInfo
    {
        public ObjectEnvironmentInfo(Guid id, IdentityInfo info) :
            base(id, info)
        {
        }
        public ObjectEnvironmentInfo(IObjectEnvironmentInfo value) :
            this(
                value.Id,
                (IdentityInfo)value.Info
                )
        { }
    }
}
