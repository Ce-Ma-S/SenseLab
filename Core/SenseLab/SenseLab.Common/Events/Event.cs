using CeMaS.Common.Events;
using CeMaS.Common.Identity;
using CeMaS.Common.Validation;
using SenseLab.Common.Objects;
using SenseLab.Common.Values;
using System.Collections.Generic;

namespace SenseLab.Common.Events
{
    public class Event :
        ObjectItem,
        IEvent
    {
        public Event(
            Object @object,
            string id,
            IdentityInfo info,
            params ValueInfo[] arguments
            ) :
            base(@object, id, info)
        {
            Arguments = new List<ValueInfo>(arguments);
        }

        public List<ValueInfo> Arguments { get; }
        IReadOnlyList<IValueInfo> IEvent.Arguments
        {
            get { return Arguments; }
        }

        public event System.EventHandler<EventOccuredArgs> Occured;

        public virtual void OnOccured(params object[] arguments)
        {
            arguments.ValidateCount(Arguments.Count, nameof(arguments));
            Occured.RaiseEvent(this, new EventOccuredArgs(arguments));
        }
    }
}
