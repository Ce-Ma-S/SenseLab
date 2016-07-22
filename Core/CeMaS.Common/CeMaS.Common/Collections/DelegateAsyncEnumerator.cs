using CeMaS.Common.Validation;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CeMaS.Common.Collections
{
    public class DelegateAsyncEnumerator<T> :
        AsyncEnumerator<T>
    {
        public DelegateAsyncEnumerator(
            Func<long, CancellationToken, Task<Optional<T>>> getNext,
            Action<bool> dispose
            )
        {
            Argument.NonNull(getNext, nameof(getNext));
            Argument.NonNull(dispose, nameof(dispose));
            this.getNext = getNext;
            this.dispose = dispose;
        }

        public static DelegateAsyncEnumerable<T, DelegateAsyncEnumerator<T>> CreateEnumerable(
            Func<long, CancellationToken, Task<Optional<T>>> getNext,
            Action<bool> dispose
            )
        {
            return new DelegateAsyncEnumerable<T, DelegateAsyncEnumerator<T>>(
                () => new DelegateAsyncEnumerator<T>(getNext, dispose)
                );
        }

        protected override Task<Optional<T>> GetNext(CancellationToken cancellationToken)
        {
            return getNext(CurrentIndex, cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            dispose(disposing);
        }

        private readonly Func<long, CancellationToken, Task<Optional<T>>> getNext;
        private readonly Action<bool> dispose;
    }


    public class DelegateAsyncEnumerator<TResource, TItem> :
        AsyncEnumerator<TItem>
        where TResource : IDisposable
    {
        public DelegateAsyncEnumerator(
            Func<Task<TResource>> initialize,
            Func<TResource, long, CancellationToken, Task<Optional<TItem>>> getNext
            )
        {
            Argument.NonNull(initialize, nameof(initialize));
            Argument.NonNull(getNext, nameof(getNext));
            this.initialize = initialize;
            this.getNext = getNext;
        }

        public static DelegateAsyncEnumerable<TItem, DelegateAsyncEnumerator<TResource, TItem>> CreateEnumerable(
            Func<Task<TResource>> initialize,
            Func<TResource, long, CancellationToken, Task<Optional<TItem>>> getNext
            )
        {
            return new DelegateAsyncEnumerable<TItem, DelegateAsyncEnumerator<TResource, TItem>>(
                () => new DelegateAsyncEnumerator<TResource, TItem>(initialize, getNext)
                );
        }

        protected override async Task<Optional<TItem>> GetNext(CancellationToken cancellationToken)
        {
            if (CurrentIndex == 0)
            {
                resource = await initialize();
                Debug.Assert(resource != null);
            }
            return await getNext(resource, CurrentIndex, cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
                resource.Dispose();
        }

        private readonly Func<Task<TResource>> initialize;
        private TResource resource;
        private readonly Func<TResource, long, CancellationToken, Task<Optional<TItem>>> getNext;
    }
}
