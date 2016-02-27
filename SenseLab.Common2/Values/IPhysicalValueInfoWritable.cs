using CeMaS.Common.Units;
using System;

namespace SenseLab.Common.Values
{
    public interface IPhysicalValueInfoWritable :
        IValueInfoWritable,
        IPhysicalValueInfo
    {
        new Unit Unit { get; set; }
    }
}
