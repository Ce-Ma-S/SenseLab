using CeMaS.Common.Validation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CeMaS.Common.Collections
{
    /// <summary>
    /// <see cref="IAsyncEnumerable{T}"/> helper.
    /// </summary>
    public static class AsyncEnumerableHelper
    {
        public static IAsyncEnumerable<T> WithInitialization<T>(this IAsyncEnumerable<T> items, Func<Task> initialization)
        {
            Argument.NonNull(initialization, nameof(initialization));
            return DelegateAsyncEnumerator<IAsyncEnumerator<T>, T>.CreateEnumerable(
                async () =>
                {
                    await initialization();
                    return items.GetEnumerator();
                },
                async (itemsEnumerator, i, c) =>
                {
                    await itemsEnumerator.MoveNext(c);
                    return itemsEnumerator.Current;
                });
        }
    }
}
