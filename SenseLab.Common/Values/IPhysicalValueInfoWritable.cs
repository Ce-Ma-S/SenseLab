using System;

namespace SenseLab.Common.Values
{
    public interface IPhysicalValueInfoWritable :
        IValueInfoWritable,
        IPhysicalValueInfo
    {
        new string Unit { get; set; }
    }
}
