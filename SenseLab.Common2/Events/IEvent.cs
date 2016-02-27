using SenseLab.Common.Objects;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Events
{
    public interface IEvent :
        IObjectItem
    {
        IReadOnlyList<IEventArgumentInfo> Arguments { get; }
        event EventHandler<EventOccuredArgs> Occured;
    }
}
