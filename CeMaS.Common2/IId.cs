using System;

namespace CeMaS.Common
{
    public interface IId<T>
        where T : IEquatable<T>
    {
        T Id { get; }
    }
}
