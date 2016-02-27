using CeMaS.Common.Validation;
using System.Reactive;
using System.Threading.Tasks;

namespace CeMaS.Common.Commands
{
    public static class CommandHelper
    {
        public static async Task<Optional<TResult>> ExecuteIfPossible<TParameter, TResult>(
            this ICommand<TParameter, TResult> command,
            TParameter parameter
            )
        {
            command.ValidateNonNull(nameof(command));
            var result = Optional<TResult>.None;
            if (command.CanExecute(parameter))
                result = new Optional<TResult>(await command.Execute(parameter));
            return result;
        }

        public static Task<Optional<TResult>> ExecuteIfPossible<TResult>(
            this ICommand<Unit, TResult> command
            )
        {
            return command.ExecuteIfPossible(Unit.Default);
        }
    }
}
