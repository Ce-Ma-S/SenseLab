using System;

namespace SenseLab.Common.Commands
{
    public class CommandExecutionEventArgs :
        CommandExecuteEventArgs
    {
        public CommandExecutionEventArgs(
            DateTime start,
            object state,
            params object[] parameters
            ) :
            base(parameters)
        {
            Start = start;
            State = state;
        }

        public DateTime Start { get; }
        public object State { get; }
    }
}
