using CeMaS.Common.Validation;
using System;

namespace SenseLab.Common.Commands
{
    public class CommandExecuteEventArgs :
        EventArgs
    {
        public CommandExecuteEventArgs(params object[] parameters)
        {
            parameters.ValidateNonNull(nameof(parameters));
            Parameters = parameters;
        }

        public object[] Parameters { get; }
    }
}
