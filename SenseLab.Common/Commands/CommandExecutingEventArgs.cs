using System;
using System.Threading;

namespace SenseLab.Common.Commands
{
    public class CommandExecutingEventArgs :
        CommandExecutionEventArgs
    {
        public CommandExecutingEventArgs(
            DateTime start,
            object state,
            CancellationTokenSource cancellation,
            params object[] parameters
            ) :
            base(start, state, parameters)
        {
            Cancellation = cancellation;
        }
        
        public CancellationTokenSource Cancellation { get; }
    }
}
