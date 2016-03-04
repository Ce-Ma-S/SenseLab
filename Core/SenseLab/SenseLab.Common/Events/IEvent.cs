using SenseLab.Common.Objects;
using SenseLab.Common.Values;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Events
{
    public interface IEvent :
        IObjectItem
    {
        IReadOnlyList<IValueInfo> Arguments { get; }
        event EventHandler<EventOccuredArgs> Occured;
    }
}
