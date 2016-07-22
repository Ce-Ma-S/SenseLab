using CeMaS.Common.Units;

namespace SenseLab.Common.Values
{
    public interface IPhysicalValueInfo :
        IValueInfo
    {
        Unit<string> Unit { get; }
    }
}
