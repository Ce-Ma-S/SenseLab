using System;

namespace SenseLab.Common.Values
{
    public interface IPhysicalValueInfo :
        IValueInfo
    {
        string Unit { get; }
    }
}
