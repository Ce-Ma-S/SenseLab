using CeMaS.Common.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CeMaS.Common
{
    /// <summary>
    /// Helps with tasks.
    /// </summary>
    public static partial class TaskHelper
    {
        #region Init

        static TaskHelper()
        {
            InitSynchronizationScheduler();
        }
        /// <summary>
        /// Initializes <see cref="SynchronizationScheduler"/> with scheduler from <see cref="SynchronizationContext.Current"/> if available, otherwise <see cref="TaskScheduler.Current"/>.
        /// Called from static constructor.
        /// </summary>
        /// <returns></returns>
        public static bool InitSynchronizationScheduler()
        {
            if (SynchronizationScheduler == null)
            {
                SynchronizationScheduler = SynchronizationContext.Current == null ?
                    TaskScheduler.Current :
                    TaskScheduler.FromCurrentSynchronizationContext();
                return true;
            }
            return false;
        }

        #endregion

        /// <summary>
        /// Scheduler initialized with <see cref="InitSynchronizationScheduler"/>.
        /// </summary>
        public static TaskScheduler SynchronizationScheduler { get; private set; }

        public static Task RunOnSynchronizationScheduler(
            this Action action,
            CancellationToken? cancellationToken = null,
            TaskCreationOptions? creationOptions = null
            )
        {
            action.ValidateNonNull(nameof(action));
            return Task.Factory.StartNew(
                action,
                cancellationToken ?? CancellationToken.None,
                creationOptions ?? TaskCreationOptions.None,
                SynchronizationScheduler
                );
        }

        public static Task<T> RunOnSynchronizationScheduler<T>(
            this Func<T> function,
            CancellationToken? cancellationToken = null,
            TaskCreationOptions? creationOptions = null
            )
        {
            function.ValidateNonNull(nameof(function));
            return Task.Factory.StartNew(
                function,
                cancellationToken ?? CancellationToken.None,
                creationOptions ?? TaskCreationOptions.None,
                SynchronizationScheduler
                );
        }

        public static Task RunLong(
            this Action action,
            CancellationToken? cancellationToken = null
            )
        {
            action.ValidateNonNull(nameof(action));
            return Task.Factory.StartNew(
                action,
                cancellationToken ?? CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default
                );
        }

        public static Task<T> RunLong<T>(
            this Func<T> function,
            CancellationToken? cancellationToken = null
            )
        {
            function.ValidateNonNull(nameof(function));
            return Task.Factory.StartNew(
                function,
                cancellationToken ?? CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default
                );
        }
    }
}
