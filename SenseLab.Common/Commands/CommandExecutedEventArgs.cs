using System;

namespace SenseLab.Common.Commands
{
    public class CommandExecutedEventArgs :
        CommandExecutionEventArgs
    {
        public CommandExecutedEventArgs(
            DateTime start,
            DateTime end,
            object state,
            bool cancelled,
            Exception error,
            params object[] parameters
            ) :
            base(start, state, parameters)
        {
            End = end;
        }
        
        public DateTime End { get; }
        public bool Cancelled { get; }
        public Exception Error { get; }
    }
}
