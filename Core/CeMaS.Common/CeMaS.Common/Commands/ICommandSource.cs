using System;
using System.Reactive;

namespace CeMaS.Common.Commands
{
    /// <summary>
    /// Command as parameterless void returning one intended to be used in command bars etc.
    /// </summary>
    /// <typeparam name="TId">Command identifier type.</typeparam>
    public interface ICommandSource<TId> :
        ICommand<Unit, Unit>
    { }


    /// <summary>
    /// Wraps a command as parameterless void returning one intended to be used in command bars etc.
    /// </summary>
    /// <typeparam name="TId">Command identifier type.</typeparam>
    /// <typeparam name="TParameter">Command parameter type. <see cref="Unit"/> means <see cref="void"/> so that parameter is ignored.</typeparam>
    /// <typeparam name="TResult">Command result type. <see cref="Unit"/> means <see cref="void"/>.</typeparam>
    public interface ICommandSource<TId, TParameter, TResult> :
        ICommandSource<TId>
    {
        /// <summary>
        /// UI command.
        /// </summary>
        ICommand<TParameter, TResult> Command { get; }
        /// <summary>
        /// UI command parameter.
        /// </summary>
        TParameter CommandParameter { get; }
        /// <summary>
        /// Fired when <see cref="CommandParameter"/> is changed.
        /// </summary>
        event EventHandler CommandParameterChanged;
    }
}
