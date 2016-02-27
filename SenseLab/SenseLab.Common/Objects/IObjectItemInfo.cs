using CeMaS.Common.Identity;

namespace SenseLab.Common.Objects
{
    public interface IObjectItemInfo :
        IIdentity<string>
    {
        IObjectInfo Object { get; }
    }
}
