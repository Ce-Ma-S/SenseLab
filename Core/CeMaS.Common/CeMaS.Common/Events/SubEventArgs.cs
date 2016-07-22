using CeMaS.Common.Validation;
using System;

namespace CeMaS.Common.Events
{
    public class SubEventArgs :
        EventArgs
    {
        public SubEventArgs(object sender, EventArgs arguments)
        {
            Argument.NonNull(sender, nameof(sender));
            Argument.NonNull(arguments, nameof(arguments));
            Sender = sender;
            Arguments = arguments;
        }

        public object Sender { get; }
        public EventArgs Arguments { get; }
    }
}
