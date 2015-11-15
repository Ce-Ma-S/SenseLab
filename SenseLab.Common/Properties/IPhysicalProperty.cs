using SenseLab.Common.Values;

namespace SenseLab.Common.Properties
{
    public interface IPhysicalProperty :
        IProperty,
        IPhysicalValueInfo
    {
    }


    public interface IPhysicalProperty<T> :
        IProperty<T>,
        IPhysicalProperty
    {
    }
}
