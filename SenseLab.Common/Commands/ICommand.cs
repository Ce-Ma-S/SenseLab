using SenseLab.Common.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SenseLab.Common.Commands
{
    public interface ICommand :
        IObjectItem
    {
        IReadOnlyList<ICommandParameterInfo> Parameters { get; }
        bool IsCancellable { get; }

        event EventHandler<CommandExecuteEventArgs> CanExecuteChanged;

        bool CanExecute(params object[] parameters);
        Task Execute(object state, params object[] parameters);

        event EventHandler<CommandExecutingEventArgs> Executing;
        event EventHandler<CommandExecutedEventArgs> Executed;
    }
}
