using CeMaS.Common.Commands;
using SenseLab.Common.Objects;
using SenseLab.Common.Values;
using System.Collections.Generic;

namespace SenseLab.Common.Commands
{
    public interface ICommand :
        IObjectItem
    {
        ICommand<object[], object[]> Value { get; }
        IReadOnlyList<IValueInfo> Parameters { get; }
        IReadOnlyList<IValueInfo> Results { get; }
    }
}
