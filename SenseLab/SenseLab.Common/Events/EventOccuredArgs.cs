using CeMaS.Common.Validation;
using System;

namespace SenseLab.Common.Events
{
    public class EventOccuredArgs :
        EventArgs
    {
        public EventOccuredArgs(params object[] arguments)
        {
            arguments.ValidateNonNull(nameof(arguments));
            Arguments = arguments;
        }

        public object[] Arguments { get; private set; }
    }
}
