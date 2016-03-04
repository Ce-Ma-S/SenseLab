using System;

namespace CeMaS.Common.Commands
{
    public class CommandExecutedEventArgs :
        CommandExecuteEventArgs
    {
        public CommandExecutedEventArgs(
            DateTimeOffset start,
            DateTimeOffset end,
            object parameter = null,
            object result = null,
            bool canceled = false,
            Exception error = null
            )
            : base(start, parameter)
        {
            End = end;
            Result = result;
            Canceled = canceled;
            Error = error;
        }

        public DateTimeOffset End { get; private set; }
        public object Result { get; private set; }
        public bool Canceled { get; private set; }
        public Exception Error { get; private set; }
    }
}
