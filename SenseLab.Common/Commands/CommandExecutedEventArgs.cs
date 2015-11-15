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
            bool isCancelled,
            Exception error,
            params object[] parameters
            ) :
            base(start, state, parameters)
        {
            End = end;
            IsCancelled = isCancelled;
            Error = error;
        }
        
        public DateTime End { get; }
        public bool IsCancelled { get; }
        public Exception Error { get; }
    }
}
