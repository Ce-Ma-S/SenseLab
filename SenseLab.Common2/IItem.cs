using CeMaS.Common;
using System;

namespace SenseLab.Common
{
    public interface IItem<T> :
        IId<T>
        where T : IEquatable<T>
    {
        string Name { get; }
        string Description { get; }
    }
}
