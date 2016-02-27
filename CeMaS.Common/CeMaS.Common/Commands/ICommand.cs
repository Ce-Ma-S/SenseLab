using CeMaS.Common.Events;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace CeMaS.Common.Commands
{
    /// <summary>
    /// Executable command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Whether command execution is done synchronously, otherwise asynchronously.
        /// </summary>
        bool IsSynchronous { get; }
        /// <summary>
        /// Command execution parameter type.
        /// </summary>
        /// <value><see cref="Unit"/> type means no parameter is used (it is ignored).</value>
        Type ParameterType { get; }
        /// <summary>
        /// Command execution result type.
        /// </summary>
        /// <value><see cref="Unit"/> type means no result is returned by <see cref="Execute(object)"/>.</value>
        Type ResultType { get; }

        /// <summary>
        /// Whether this command can be executed.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        bool CanExecute(object parameter);
        /// <summary>
        /// Raised when <see cref="CanExecute"/> result changes.
        /// </summary>
        /// <remarks>
        /// <see cref="EventArgs{T}.Data"/> specifes for which parameters <see cref="CanExecute"/> changes.
        /// Empty means any parameter.
        /// </remarks>
        event EventHandler<EventArgs<IEnumerable>> CanExecuteChanged;
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>Command result.</returns>
        Task<object> Execute(object parameter);

        /// <summary>
        /// Fired when command execution starts.
        /// </summary>
        /// <remarks><see cref="CommandExecutingEventArgs.Cancel"/> allows command execution cancelation.</remarks>
        event EventHandler<CommandExecutingEventArgs> Executing;
        /// <summary>
        /// Fired when command execution ends.
        /// </summary>
        event EventHandler<CommandExecutedEventArgs> Executed;
    }


    /// <summary>
    /// Executable typed command.
    /// </summary>
    /// <typeparam name="TParameter">Command parameter type. <see cref="Unit"/> means <see cref="void"/> so that parameter is ignored.</typeparam>
    /// <typeparam name="TResult">Command result type. <see cref="Unit"/> means <see cref="void"/>.</typeparam>
    public interface ICommand<TParameter, TResult> :
        ICommand
    {
        /// <summary>
        /// Whether this command can be executed.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        bool CanExecute(TParameter parameter);
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>Command result.</returns>
        Task<TResult> Execute(TParameter parameter);
    }
}
