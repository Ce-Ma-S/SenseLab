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
        #region SynchronizationScheduler

        #region Init

        static TaskHelper()
        {
            InitSynchronizationScheduler();
        }
        /// <summary>
        /// Initializes <see cref="SynchronizationScheduler"/> to a scheduler from <see cref="SynchronizationContext.Current"/> if available, otherwise <see cref="TaskScheduler.Default"/>.
        /// Called from static constructor.
        /// </summary>
        public static void InitSynchronizationScheduler()
        {
            SynchronizationSchedulerContext = SynchronizationContext.Current;
            SynchronizationScheduler = SynchronizationSchedulerContext == null ?
                TaskScheduler.Default :
                TaskScheduler.FromCurrentSynchronizationContext();
        }

        #endregion

        /// <summary>
        /// Scheduler initialized with <see cref="InitSynchronizationScheduler"/>.
        /// </summary>
        public static TaskScheduler SynchronizationScheduler { get; private set; }
        /// <summary>
        /// Context of <see cref="SynchronizationScheduler"/>.
        /// </summary>
        public static SynchronizationContext SynchronizationSchedulerContext { get; private set; }
        /// <summary>
        /// Whether <see cref="SynchronizationSchedulerContext"/> is not null and is <see cref="SynchronizationContext.Current"/> or <see cref="SynchronizationScheduler"/> is <see cref="TaskScheduler.Current"/>.
        /// </summary>
        public static bool SynchronizationSchedulerIsCurrent
        {
            get
            {
                return
                    SynchronizationSchedulerContext != null &&
                    SynchronizationSchedulerContext == SynchronizationContext.Current ||
                    TaskScheduler.Current.Id == SynchronizationScheduler.Id;
            }
        }

        public static async Task RunOnSynchronizationScheduler(
            this Action action,
            CancellationToken cancellationToken = default(CancellationToken),
            TaskCreationOptions creationOptions = default(TaskCreationOptions)
            )
        {
            Argument.NonNull(action, nameof(action));
            if (SynchronizationSchedulerIsCurrent)
            {
                action();
            }
            else
            {
                await Task.Factory.StartNew(
                    action,
                    cancellationToken,
                    creationOptions,
                    SynchronizationScheduler
                    );
            }
        }

        public static async Task<T> RunOnSynchronizationScheduler<T>(
            this Func<T> function,
            CancellationToken cancellationToken = default(CancellationToken),
            TaskCreationOptions creationOptions = default(TaskCreationOptions)
            )
        {
            Argument.NonNull(function, nameof(function));
            if (SynchronizationSchedulerIsCurrent)
            {
                return function();
            }
            else
            {
                return await Task.Factory.StartNew(
                    function,
                    cancellationToken,
                    creationOptions,
                    SynchronizationScheduler
                    );
            }
        }

        public static async Task<T> ScheduleOnSynchronizationScheduler<T>(
            this Func<Task<T>> function,
            CancellationToken cancellationToken = default(CancellationToken),
            TaskCreationOptions creationOptions = default(TaskCreationOptions)
            )
        {
            Argument.NonNull(function, nameof(function));
            var task = await Task.Factory.StartNew(
                () => function(),
                cancellationToken,
                creationOptions,
                SynchronizationScheduler
                );
            return await task;
        }

        #endregion

        #region RunLong

        public static Task RunLong(
            this Action action,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            Argument.NonNull(action, nameof(action));
            return Task.Factory.StartNew(
                action,
                cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default
                );
        }

        public static Task<T> RunLong<T>(
            this Func<T> function,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            Argument.NonNull(function, nameof(function));
            return Task.Factory.StartNew(
                function,
                cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default
                );
        }

        #endregion

        #region RunSynchronously

        /// <summary>
        /// Represents infinite timeout with -1 ms.
        /// </summary>
        public static readonly TimeSpan InfiniteTimeout = TimeSpan.FromMilliseconds(-1);

        /// <summary>
        /// Runs <paramref name="action"/> on thread pool and waits for the completion.
        /// </summary>
        /// <param name="action">Action.</param>
        /// <param name="timeout">Wait timeout. null means <see cref="InfiniteTimeout"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        /// <exception cref="TimeoutException">Waiting for <paramref name="action"/> has timed out.</exception>
        public static void RunSynchronously(
            this Func<Task> action,
            TimeSpan? timeout = null
            )
        {
            Argument.NonNull(action, nameof(action));
            Task.Run(() => action().Wait(timeout ?? InfiniteTimeout)).Wait();
        }

        /// <summary>
        /// Runs <paramref name="function"/> on thread pool and waits for its result.
        /// </summary>
        /// <typeparam name="T">Result type.</typeparam>
        /// <param name="function">Function.</param>
        /// <param name="timeout">Wait timeout. null means <see cref="InfiniteTimeout"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null.</exception>
        /// <exception cref="TimeoutException">Waiting for <paramref name="function"/> has timed out.</exception>
        public static T RunSynchronously<T>(
            this Func<Task<T>> function,
            TimeSpan? timeout = null
            )
        {
            Argument.NonNull(function, nameof(function));
            return Task.Run(() =>
                {
                    var run = function();
                    run.Wait(timeout ?? InfiniteTimeout);
                    return run.Result;
                }).
                Result;
        }

        #endregion
    }
}
