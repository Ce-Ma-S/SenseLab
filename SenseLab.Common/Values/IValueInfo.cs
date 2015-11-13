using System;

namespace SenseLab.Common.Values
{
    public interface IValueInfo :
        IItem<string>
    {
        Type Type { get; }
    }
}
