using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace EventinatR.CosmosDB
{
    internal class AsyncLazy<T> : Lazy<Task<T>>
    {
        public AsyncLazy(Func<T> valueFactory)
           : base(() => Task.Factory.StartNew(valueFactory, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Current))
        {
        }

        public AsyncLazy(Func<Task<T>> taskFactory)
            : base(() => Task.Factory.StartNew(() => taskFactory(), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Current).Unwrap())
        {
        }

        public TaskAwaiter<T> GetAwaiter()
            => Value.GetAwaiter();

        public ConfiguredTaskAwaitable<T> ConfigureAwait(bool continueOnCapturedContext = true)
            => Value.ConfigureAwait(continueOnCapturedContext);
    }
}
