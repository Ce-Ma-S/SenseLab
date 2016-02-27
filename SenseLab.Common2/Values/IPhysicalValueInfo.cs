using CeMaS.Common.Units;
using System;

namespace SenseLab.Common.Values
{
    public interface IPhysicalValueInfo :
        IValueInfo
    {
        Unit Unit { get; }
    }
}
