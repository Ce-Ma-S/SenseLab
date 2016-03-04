using CeMaS.Common.Validation;
using System;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace CeMaS.Common.Commands
{
    /// <summary>
    /// Delegate command.
    /// </summary>
    /// <typeparam name="TParameter">Command parameter type. <see cref="Unit"/> means <see cref="void"/> so that parameter is ignored.</typeparam>
    /// <typeparam name="TResult">Command result type. <see cref="Unit"/> means <see cref="void"/>.</typeparam>
    public class DelegateCommand<TParameter, TResult> :
        Command<TParameter, TResult>
    {
        #region Init

        public DelegateCommand(
            Func<TParameter, CancellationTokenSource, TResult> execute,
            Func<TParameter, bool> canExecute = null,
            bool isSynchronous = true,
            bool allowsParallelExecution = false
            )
        {
            execute.ValidateNonNull(nameof(execute));
            this.isSynchronous = isSynchronous;
            executeTaskFactory = (p, cts) => new Task<TResult>(() => execute(p, cts), cts.Token);
            this.canExecute = canExecute;
            this.allowsParallelExecution = allowsParallelExecution;
        }

        public DelegateCommand(
            Func<TParameter, CancellationTokenSource, Task<TResult>> executeTaskFactory,
            Func<TParameter, bool> canExecute = null,
            bool allowsParallelExecution = false
            )
        {
            executeTaskFactory.ValidateNonNull(nameof(executeTaskFactory));
            this.executeTaskFactory = executeTaskFactory;
            this.canExecute = canExecute;
            this.allowsParallelExecution = allowsParallelExecution;
        }

        #endregion

        public override bool IsSynchronous
        {
            get { return isSynchronous; }
        }

        public override Task<TResult> GetExecuteTask(TParameter parameter, CancellationTokenSource cancellation)
        {
            return executeTaskFactory(parameter, cancellation);
        }

        protected override bool GetAllowsParallelExecution()
        {
            return allowsParallelExecution;
        }
        protected override bool DoCanExecute(TParameter parameter)
        {
            return
                canExecute == null ||
                canExecute(parameter);
        }
        protected sealed override TResult Execute(TParameter parameter, CancellationTokenSource cancellation)
        {
            throw new NotImplementedException();
        }
        protected sealed override Task<TResult> ExecuteAsync(TParameter parameter, CancellationTokenSource cancellation)
        {
            throw new NotImplementedException();
        }

        private readonly bool isSynchronous;
        private readonly bool allowsParallelExecution;
        private readonly Func<TParameter, bool> canExecute;
        private readonly Func<TParameter, CancellationTokenSource, Task<TResult>> executeTaskFactory;
    }

    /// <summary>
    /// Command  without return value.
    /// </summary>
    public class DelegateCommand<TParameter> :
        DelegateCommand<TParameter, Unit>
    {
        public DelegateCommand(
            Action<TParameter, CancellationTokenSource> execute,
            Func<TParameter, bool> canExecute = null,
            bool isSynchronous = true,
            bool allowsParallelExecution = false
            )
            : base(
                (p, cts) =>
                {
                    execute(p, cts);
                    return Unit.Default;
                },
                canExecute,
                isSynchronous,
                allowsParallelExecution
                )
        {
        }
        public DelegateCommand(
            Func<TParameter, CancellationTokenSource, Task> executeTaskFactory,
            Func<TParameter, bool> canExecute = null,
            bool allowsParallelExecution = false
            )
            : base(
                async (p, cts) =>
                {
                    await executeTaskFactory(p, cts);
                    return Unit.Default;
                },
                canExecute,
                allowsParallelExecution
                )
        {
        }
    }


    /// <summary>
    /// Parameterless command without return value.
    /// </summary>
    public class DelegateCommand :
        DelegateCommand<Unit>
    {
        public DelegateCommand(
            Action<CancellationTokenSource> execute,
            Func<bool> canExecute = null,
            bool isSynchronous = true,
            bool allowsParallelExecution = false
            )
            : base(
                (p, cts) => execute(cts),
                canExecute == null ?
                    (Func<Unit, bool>)null :
                    p => canExecute(),
                isSynchronous,
                allowsParallelExecution
                )
        {
        }

        public DelegateCommand(
            Func<CancellationTokenSource, Task> executeTaskFactory,
            Func<bool> canExecute = null,
            bool allowsParallelExecution = false
            )
            : base(
                (p, cts) => executeTaskFactory(cts),
                canExecute == null ?
                    (Func<Unit, bool>)null :
                    p => canExecute(),
                allowsParallelExecution
                )
        {
        }
    }
}
