using System.Collections.Generic;

namespace CeMaS.Common.Collections
{
    public abstract class AsyncEnumerable<T> :
        IAsyncEnumerable<T>
    {
        public IAsyncEnumerator<T> GetEnumerator()
        {
            return CreateEnumerator();
        }

        protected abstract IAsyncEnumerator<T> CreateEnumerator();
    }
}
