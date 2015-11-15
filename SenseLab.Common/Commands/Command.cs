﻿using CeMaS.Common;
using CeMaS.Common.Events;
using SenseLab.Common.Objects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SenseLab.Common.Commands
{
    public abstract class Command :
        ObjectItem,
        ICommand
    {
        public Command(
            IObject @object,
            string id,
            string name,
            bool isCancellable,
            string description = null,
            params ICommandParameterInfoWritable[] parameters
            ) :
            base(@object, id, name, description)
        {
            Parameters = new List<ICommandParameterInfoWritable>(parameters);
            IsCancellable = isCancellable;
        }

        public List<ICommandParameterInfoWritable> Parameters { get; }
        IReadOnlyList<ICommandParameterInfo> ICommand.Parameters
        {
            get { return Parameters; }
        }
        public bool IsCancellable { get; }

        public event EventHandler<CommandExecuteEventArgs> CanExecuteChanged;
        public event EventHandler<CommandExecutingEventArgs> Executing;
        public event EventHandler<CommandExecutedEventArgs> Executed;

        public bool CanExecute(params object[] parameters)
        {
            return
                ParametersAreValid(parameters) &&
                DoCanExecute(parameters);
        }

        public async Task Execute(object state, params object[] parameters)
        {
            CancellationTokenSource cancellation = IsCancellable ?
                new CancellationTokenSource() :
                null;
            var start = DateTime.Now;
            try
            {
                OnExecuting(
                    start,
                    state,
                    cancellation,
                    parameters
                    );
                await DoExecute(
                    cancellation?.Token,
                    parameters
                    );
                OnExecuted(
                    start,
                    DateTime.Now,
                    false,
                    null,
                    parameters
                    );
            }
            catch (Exception error)
            {
                bool isCancelled = error is OperationCanceledException;
                OnExecuted(
                    start,
                    DateTime.Now,
                    isCancelled,
                    isCancelled ?
                        null :
                        error,
                    parameters
                    );
            }
        }

        public virtual void OnCanExecuteChanged(params object[] parameters)
        {
            CanExecuteChanged.RaiseEvent(this, () => new CommandExecuteEventArgs(parameters));
        }

        protected abstract bool DoCanExecute(object[] parameters);
        protected abstract Task DoExecute(CancellationToken? cancellation, object[] parameters);

        protected virtual void OnExecuting(
            DateTime start,
            object state,
            CancellationTokenSource cancellation,
            params object[] parameters
            )
        {
            Executing.RaiseEvent(this, () => new CommandExecutingEventArgs(
                start,
                state,
                cancellation,
                parameters
                ));
        }

        protected virtual void OnExecuted(
            DateTime start,
            object state,
            bool isCancelled,
            Exception error,
            params object[] parameters
            )
        {
            Executed.RaiseEvent(this, () => new CommandExecutedEventArgs(
                start,
                DateTime.Now,
                state,
                isCancelled,
                error,
                parameters
                ));
        }

        private bool ParametersAreValid(object[] parameters)
        {
            return parameters == null &&
                Parameters.Count == 0 ||
                parameters != null &&
                Parameters.Count == parameters.Length &&
                TypesAreValid(Parameters, parameters);
        }

        private bool TypesAreValid(
            List<ICommandParameterInfoWritable> parameterInfos,
            object[] parameters
            )
        {
            for (int i = 0; i < parameterInfos.Count; i++)
            {
                var parameterInfo = parameterInfos[i];
                var parameter = parameters[i];
                if (!parameter.IsValidFor(parameterInfo.Type))
                    return false;
            }
            return true;
        }
    }
}