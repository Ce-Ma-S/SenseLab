using CeMaS.Common.Validation;
using System;
using System.Threading;

namespace CeMaS.Common.Commands
{
    public class CommandExecutingEventArgs :
        CommandExecuteEventArgs
    {
        public CommandExecutingEventArgs(DateTimeOffset start, CancellationTokenSource cancellation, object parameter = null)
            : base(start, parameter)
        {
            Argument.NonNull(cancellation, nameof(cancellation));
            this.cancellation = cancellation;
        }

        public void Cancel()
        {
            cancellation.Cancel();
        }

        private readonly CancellationTokenSource cancellation;
    }
}
