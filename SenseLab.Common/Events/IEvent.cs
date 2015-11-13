using SenseLab.Common.Objects;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Events
{
    public interface IEvent :
        IObjectItem
    {
        IEnumerable<IEventArgumentInfo> Arguments { get; }
        event EventHandler<EventOccuredArgs> Occured;
    }
}
