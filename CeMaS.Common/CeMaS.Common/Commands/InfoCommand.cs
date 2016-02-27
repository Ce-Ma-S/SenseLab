using CeMaS.Common.Commands;
using CeMaS.Common.Events;
using CeMaS.Common.Identity;
using CeMaS.Common.Validation;
using Serilog;
using System;

namespace CeMaS.Common.Commands
{
    /// <summary>
    /// <see cref="IInfoCommand{TId,TParameter, TResult}"/> base.
    /// </summary>
    /// <typeparam name="TId">Command identifier type.</typeparam>
    /// <typeparam name="TParameter">Command parameter type. <see cref="Unit"/> means <see cref="void"/> so that parameter is ignored.</typeparam>
    /// <typeparam name="TResult">Command result type. <see cref="Unit"/> means <see cref="void"/>.</typeparam>
    public abstract class InfoCommand<TId, TParameter, TResult> :
        Command<TParameter, TResult>,
        IInfoCommand<TId, TParameter, TResult>
    {
        public InfoCommand(TId id, IdentityInfo info)
        {
            Id = id;
            Info = info;
        }

        public TId Id { get; private set; }

        #region Info

        public IdentityInfo Info
        {
            get { return info; }
            set
            {
                value.ValidateNonNull();
                SetPropertyValue(ref info, value, OnInfoChanged);
            }
        }
        IIdentityInfo IIdentity<TId>.Info
        {
            get { return Info; }
        }
        public event EventHandler InfoChanged;

        protected virtual void OnInfoChanged()
        {
            InfoChanged.RaiseEvent(this);
        }

        private IdentityInfo info;

        #endregion

        public override string ToString()
        {
            return $"{Info.Name} ({Id})";
        }

        protected override ILogger CreateLog()
        {
            return base.CreateLog().
                ForContext("Id", Id);
        }

        protected override void ClearEventHandlers()
        {
            base.ClearEventHandlers();
            InfoChanged = null;
        }
    }
}
