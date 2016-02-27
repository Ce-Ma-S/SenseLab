using CeMaS.Common.Events;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace CeMaS.Common.Commands
{
    /// <summary>
    /// <see cref="ICommand{TParameter, TResult}"/> base.
    /// </summary>
    /// <typeparam name="TParameter">Command parameter type. <see cref="Unit"/> means <see cref="void"/> so that parameter is ignored.</typeparam>
    /// <typeparam name="TResult">Command result type. <see cref="Unit"/> means <see cref="void"/>.</typeparam>
    [DataContract]
    public abstract class Command<TParameter, TResult> :
        NotifyPropertyChange,
        ICommand<TParameter, TResult>
    {
        public Command()
        {
            Log = CreateLog();
        }

        public abstract bool IsSynchronous { get; }
        public bool AllowsParallelExecution
        {
            get
            {
                return
                    !IsSynchronous &&
                    GetAllowsParallelExecution();
            }
        }
        public bool IsExecuting
        {
            get { return executionCount > 0; }
        }
        public virtual Type ParameterType
        {
            get { return typeof(TParameter); }
        }
        public virtual Type ResultType
        {
            get { return typeof(TResult); }
        }
        public event EventHandler<CommandExecutingEventArgs> Executing;
        public event EventHandler<CommandExecutedEventArgs> Executed;
        public event EventHandler<EventArgs<IEnumerable>> CanExecuteChanged;

        public bool CanExecute(TParameter parameter)
        {
            return
                (
                    AllowsParallelExecution ||
                    !IsExecuting
                ) &&
                DoCanExecute(parameter);
        }
        bool ICommand.CanExecute(object parameter)
        {
            return
                (
                    parameter == null &&
                    parameterAllowsNull ||
                    parameter is TParameter
                ) &&
                CanExecute((TParameter)parameter);
        }
        public Task NotifyCanExecuteChanged(params TParameter[] parameters)
        {
            return new Action(() => OnCanExecuteChanged(parameters)).
                RunOnSynchronizationScheduler();
        }
        public Task<TResult> Execute(TParameter parameter)
        {
            var cancellation = new CancellationTokenSource();
            var task = GetExecuteTask(parameter, cancellation);
            var startData = OnExecuteStarted(parameter, task, cancellation);
            // synchronous
            if (IsSynchronous)
            {
                task.RunSynchronously();
                OnExecuteEnded(parameter, task, startData);
            }
            // asynchronous
            else
            {
                // start task if not running already
                if (task.Status == TaskStatus.Created)
                    task.Start();
                task.ContinueWith(
                    t => OnExecuteEnded(parameter, t, startData),
                    TaskHelper.SynchronizationScheduler
                    );
            }
            return task;
        }
        public virtual Task<TResult> GetExecuteTask(TParameter parameter, CancellationTokenSource cancellation)
        {
            return IsSynchronous ?
                new Task<TResult>(() => Execute(parameter, cancellation)) :
                ExecuteAsync(parameter, cancellation);
        }

        async Task<object> ICommand.Execute(object parameter)
        {
            return await Execute((TParameter)parameter);
        }

        protected abstract bool GetAllowsParallelExecution();
        protected abstract bool DoCanExecute(TParameter parameter);
        protected virtual TResult Execute(TParameter parameter, CancellationTokenSource cancellation)
        {
            throw new NotImplementedException();
        }
        protected virtual Task<TResult> ExecuteAsync(TParameter parameter, CancellationTokenSource cancellation)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnCanExecuteChanged(IEnumerable<TParameter> parameters)
        {
            CanExecuteChanged.RaiseEvent(this, new EventArgs<IEnumerable>(parameters ?? Enumerable.Empty<TParameter>()));
        }
        protected virtual object OnExecuteStarted(TParameter parameter, Task<TResult> task, CancellationTokenSource cancellation)
        {
            LogInformation(parameter, "Executing");
            Interlocked.Increment(ref executionCount);
            var start = DateTimeOffset.Now;
            try
            {
                OnExecuting(parameter, start, cancellation);
            }
            catch
            {
                Interlocked.Decrement(ref executionCount);
                throw;
            }
            if (!AllowsParallelExecution)
                NotifyCanExecuteChanged();
            return start;
        }
        protected virtual void OnExecuting(TParameter parameter, DateTimeOffset start, CancellationTokenSource cancellation)
        {
            Executing.RaiseEvent(
                this,
                new CommandExecutingEventArgs(
                    start,
                    cancellation,
                    parameter
                    ));
        }
        protected virtual void OnExecuteEnded(TParameter parameter, Task<TResult> task, object startData)
        {
            Interlocked.Decrement(ref executionCount);
            if (task.IsCanceled)
                LogInformation(parameter, "Canceled");
            else if (task.Exception != null)
                LogError(parameter, "Failed", task.Exception);
            else
                LogInformation(parameter, "Executed");
            OnExecuted(
                parameter,
                (DateTimeOffset)startData,
                DateTimeOffset.Now,
                task.Result,
                task.IsCanceled,
                task.Exception
                );
            if (!AllowsParallelExecution)
                NotifyCanExecuteChanged();
        }
        protected virtual void OnExecuted(
            TParameter parameter,
            DateTimeOffset start,
            DateTimeOffset end,
            TResult result,
            bool canceled,
            Exception error
            )
        {
            Executed.RaiseEvent(
                this,
                new CommandExecutedEventArgs(
                    start,
                    end,
                    parameter,
                    result,
                    canceled,
                    error
                    ));
        }
        protected override void ClearEventHandlers()
        {
            base.ClearEventHandlers();
            CanExecuteChanged = null;
            Executing = null;
            Executed = null;
        }

        #region Log

        protected virtual ILogger CreateLog()
        {
            return Serilog.Log.Logger.
                ForContext(GetType());
        }
        protected virtual ILogger GetLog(TParameter parameter)
        {
            return Log.
                ForContext("Parameter", parameter);
        }
        protected void LogInformation(TParameter parameter, string state)
        {
            GetLog(parameter).Information(stateFormat, state);
        }
        protected void LogError(TParameter parameter, string state, Exception error)
        {
            GetLog(parameter).Error(error, stateFormat, state);
        }

        private const string stateFormat = "{State}";
        protected readonly ILogger Log;

        #endregion

        private static readonly bool parameterAllowsNull = typeof(TParameter).AllowsNull();

        private int executionCount;
    }
}
