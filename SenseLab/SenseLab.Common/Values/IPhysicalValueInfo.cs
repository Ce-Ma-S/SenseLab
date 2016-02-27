using CeMaS.Common.Units;

namespace SenseLab.Common.Values
{
    public interface IPhysicalValueInfo :
        IValueInfo
    {
        Unit Unit { get; }
    }
}
