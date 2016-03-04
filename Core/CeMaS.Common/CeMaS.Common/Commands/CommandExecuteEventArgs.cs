using System;

namespace CeMaS.Common.Commands
{
    public class CommandExecuteEventArgs :
        EventArgs
    {
        public CommandExecuteEventArgs(DateTimeOffset start, object parameter = null)
        {
            Start = start;
            Parameter = parameter;
        }

        public DateTimeOffset Start { get; private set; }
        public object Parameter { get; private set; }
    }
}
