using CeMaS.Common.Validation;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace CeMaS.Common.Collections
{
    /// <summary>
    /// Cache of <see cref="IObservable{T}"/> items.
    /// </summary>
    /// <typeparam name="T">Item type.</typeparam>
    public class ObservableCache<T> :
        ObservableCollection<T>,
        IDisposable
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="observable"><see cref="Observable"/></param>
        /// <param name="cacheSize"><see cref="CacheSize"/></param>
        /// <exception cref="ArgumentNullException"><paramref name="observable"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="cacheSize"/> is not positive.</exception>
        public ObservableCache(
            IObservable<T> observable,
            int cacheSize = int.MaxValue,
            IScheduler scheduler = null
            )
        {
            observable.ValidateNonNull("observable");
            cacheSize.ValidatePositive("cacheSize");

            Observable = observable;
            CacheSize = cacheSize;

            observableChanges = observable.
                ObserveOn(scheduler ?? Scheduler.CurrentThread).
                Subscribe(OnNext);
        }

        /// <summary>
        /// Observable producing items.
        /// </summary>
        public IObservable<T> Observable { get; private set; }
        /// <summary>
        /// Maximum number of items kept in this cache.
        /// </summary>
        /// <value>Positive.</value>
        public int CacheSize { get; private set; }

        public void Dispose()
        {
            observableChanges.Dispose();
        }

        protected virtual void OnNext(T item)
        {
            EnsureCacheSizeFor(1);
            Add(item);
        }

        private void EnsureCacheSizeFor(int count)
        {
            int removeCount = Count + count - CacheSize;
            if (removeCount > 0)
            {
                for (int i = removeCount - 1; i >= 0; i--)
                    RemoveAt(0);
            }
        }

        private readonly IDisposable observableChanges;
    }
}
