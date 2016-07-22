using CeMaS.Common.Identity;

namespace CeMaS.Common.Commands
{
    /// <summary>
    /// Command with informations usable in user interfaces (UI).
    /// </summary>
    /// <typeparam name="TId">Command identifier type.</typeparam>
    public interface IIdentityCommand<TId> :
        ICommand,
        IIdentity<TId>
    { }


    /// <summary>
    /// Typed command with informations usable in user interfaces (UI).
    /// </summary>
    /// <typeparam name="TId">Command identifier type.</typeparam>
    /// <typeparam name="TParameter">Command parameter type. <see cref="Unit"/> means <see cref="void"/> so that parameter is ignored.</typeparam>
    /// <typeparam name="TResult">Command result type. <see cref="Unit"/> means <see cref="void"/>.</typeparam>
    public interface IIdentityCommand<TId, TParameter, TResult> :
        IIdentityCommand<TId>,
        ICommand<TParameter, TResult>
    { }
}
