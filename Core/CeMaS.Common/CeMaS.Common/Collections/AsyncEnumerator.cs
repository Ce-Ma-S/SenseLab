using CeMaS.Common.Disposing;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CeMaS.Common.Collections
{
    public abstract class AsyncEnumerator<T> :
        Disposable,
        IAsyncEnumerator<T>
    {
        public T Current { get; private set; }
        public long CurrentIndex { get; private set; } = -1;

        public async Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            CurrentIndex++;
            var next = await GetNext(cancellationToken);
            if (next.HasValue)
            {
                Current = next.Value;
            }
            else
            {
                Current = default(T);
                CurrentIndex = -1;
            }
            return next.HasValue;
        }

        protected abstract Task<Optional<T>> GetNext(CancellationToken cancellationToken);
    }
}
