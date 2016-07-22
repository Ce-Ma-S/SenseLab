using CeMaS.Common.Collections;
using CeMaS.Common.Commands;
using CeMaS.Common.Conditions;
using CeMaS.Common.Identity;
using CeMaS.Common.Validation;
using Serilog;
using System.Collections.Generic;

namespace CeMaS.Mvvm.Commands
{
    /// <summary>
    /// <see cref="IIdentityCommand{TId,TParameter, TResult}"/> base.
    /// </summary>
    /// <typeparam name="TId">Command identifier type.</typeparam>
    /// <typeparam name="TParameter">Command parameter type. <see cref="Unit"/> means <see cref="void"/> so that parameter is ignored.</typeparam>
    /// <typeparam name="TResult">Command result type. <see cref="Unit"/> means <see cref="void"/>.</typeparam>
    public abstract class IdentityCommand<TId, TParameter, TResult> :
        Command<TParameter, TResult>,
        IIdentityCommand<TId, TParameter, TResult>
    {
        public IdentityCommand(TId id, IdentityInfo info)
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
                Argument.NonNull(value);
                SetPropertyValue(ref info, value, OnInfoChanged);
            }
        }
        public string Name
        {
            get { return Info.Name; }
        }
        public string Description
        {
            get { return Info.Description; }
        }
        public IReadOnlyDictionary<string, object> Values
        {
            get { return Info.Values.ReadOnly(); }
        }

        protected virtual void OnInfoChanged()
        {
            OnNameChanged();
            OnDescriptionChanged();
            OnValuesChanged();
        }
        protected virtual void OnNameChanged()
        {
            OnPropertyChanged(nameof(Name));
        }
        protected virtual void OnDescriptionChanged()
        {
            OnPropertyChanged(nameof(Description));
        }
        protected virtual void OnValuesChanged()
        {
            OnPropertyChanged(nameof(Values));
        }

        private IdentityInfo info;

        #endregion

        #region Equals

        public static bool operator ==(IdentityCommand<TId, TParameter, TResult> value1, IIdentity<TId> value2)
        {
            return value1.IsEqualTo(value2);
        }
        public static bool operator !=(IdentityCommand<TId, TParameter, TResult> value1, IIdentity<TId> value2)
        {
            return !(value1 == value2);
        }

        public bool Equals(IIdentity<TId> other)
        {
            return
                other != null &&
                EqualityComparer<TId>.Default.Equals(Id, other.Id);
        }
        public sealed override bool Equals(object obj)
        {
            return Equals(obj as IIdentity<TId>);
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

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
    }
}
