using CeMaS.Common.Events;
using CeMaS.Common.Validation;
using SenseLab.Common.Objects;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Events
{
    public class Event :
        ObjectItem,
        IEvent
    {
        public Event(
            IObject @object,
            string id,
            string name,
            string description = null,
            params IEventArgumentInfoWritable[] arguments
            ) :
            base(@object, id, name, description)
        {
            Arguments = new List<IEventArgumentInfoWritable>(arguments);
        }

        public List<IEventArgumentInfoWritable> Arguments { get; }
        IReadOnlyList<IEventArgumentInfo> IEvent.Arguments
        {
            get { return Arguments; }
        }

        public event EventHandler<EventOccuredArgs> Occured;

        protected virtual void OnOccured(params object[] arguments)
        {
            arguments.ValidateCount(Arguments.Count, nameof(arguments));
            Occured.RaiseEvent(this, () => new EventOccuredArgs(arguments));
        }
    }
}
