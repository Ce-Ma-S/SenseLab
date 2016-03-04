using CeMaS.Common.Identity;
using System;

namespace SenseLab.Common.Values
{
    public interface IValueInfo :
        IIdentity<string>
    {
        Type Type { get; }
    }
}
