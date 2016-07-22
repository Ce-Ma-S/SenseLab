using CeMaS.Common.Events;
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
            string name,
            IEnumerable<ValueInfo> arguments = null,
            string description = null,
            IDictionary<string, object> values = null
            ) :
            base(@object, id, name, description, values)
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
            Argument.Count(arguments, Arguments.Count, nameof(arguments));
            Occured.RaiseEvent(this, new EventOccuredArgs(arguments));
        }
    }
}
