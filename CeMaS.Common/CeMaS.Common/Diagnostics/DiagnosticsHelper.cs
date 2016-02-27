using CeMaS.Common.Disposing;
using CeMaS.Common.Validation;
using System;
using System.Diagnostics;
using System.Linq;

namespace CeMaS.Common.Diagnostics
{
    /// <summary>
    /// Helps with diagnostics.
    /// </summary>
    public static class DiagnosticsHelper
    {
        /// <summary>
        /// Measures time elapsed from this call until the returned object is disposed.
        /// </summary>
        /// <param name="onStart">Optional start action returning custom state.</param>
        /// <param name="onStop">Required stop action consuming <paramref name="onStart"/> state or null and elapsed time.</param>
        /// <returns>Object which stops measurement when disposed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="onStop"/> is null.</exception>
        public static IDisposable MeasureTime(
            Func<object> onStart,
            Action<object, TimeSpan> onStop
            )
        {
            onStop.ValidateNonNull(nameof(onStop));
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            object state = onStart == null ?
                null :
                onStart();
            return new DelegateDisposable(
                disposing =>
                {
                    if (disposing)
                    {
                        stopwatch.Stop();
                        onStop(state, stopwatch.Elapsed);
                    }
                });
        }

        /// <summary>
        /// Runs <paramref name="action"/> and measures its execution duration.
        /// </summary>
        /// <param name="action">Action.</param>
        /// <returns>Execution duration.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public static TimeSpan RunAndMeasureTime(this Action action)
        {
            action.ValidateNonNull(nameof(action));
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            action();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        /// <summary>
        /// Runs <paramref name="action"/> multiple times and measures execution durations.
        /// </summary>
        /// <param name="action">Action.</param>
        /// <param name="repeatCount">Number of <paramref name="action"/> executions.</param>
        /// <param name="parallel">Whether to do multiple executions in parallel.</param>
        /// <returns>Execution durations.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="repeatCount"/> is not positive.</exception>
        public static TimeSpan[] RunAndMeasureTime(
            this Action action,
            int repeatCount,
            bool parallel
            )
        {
            repeatCount.ValidatePositive(nameof(repeatCount));
            if (repeatCount == 1)
                return new[] { action.RunAndMeasureTime() };
            if (parallel)
            {
                action.ValidateNonNull(nameof(action));
                return Enumerable.Range(1, repeatCount).
                    AsParallel().
                    Select(i => action.RunAndMeasureTime()).
                    ToArray();
            }
            return Enumerable.Range(1, repeatCount).
                Select(i => action.RunAndMeasureTime()).
                ToArray();
        }
    }
}
