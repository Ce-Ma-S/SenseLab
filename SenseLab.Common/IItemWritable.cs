using System;

namespace SenseLab.Common
{
    public interface IItemWritable<T> :
        IItem<T>
        where T : IEquatable<T>
    {
        new string Name { get; set; }
        new string Description { get; set; }
    }
}
