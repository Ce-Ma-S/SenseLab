using CeMaS.Common.Validation;
using System;
using System.Collections.Generic;

namespace CeMaS.Common.Collections
{
    public class DelegateAsyncEnumerable<TItem, TEnumerator> :
        AsyncEnumerable<TItem>
        where TEnumerator : IAsyncEnumerator<TItem>
    {
        public DelegateAsyncEnumerable(
            Func<TEnumerator> createEnumerator
            )
        {
            Argument.NonNull(createEnumerator, nameof(createEnumerator));
            this.createEnumerator = createEnumerator;
        }

        protected override IAsyncEnumerator<TItem> CreateEnumerator()
        {
            return createEnumerator();
        }

        private readonly Func<TEnumerator> createEnumerator;
    }
}
