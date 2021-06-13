using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace QuickBulletLibrary.Extentions
{
    public static class EnumerableExtension
    {
        public static Task AsyncParallelForEach<T>(this IEnumerable<T> source, Func<T, Task> body, int maxDegreeOfParallelism)
        {
            var options = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            };

            var block = new ActionBlock<T>(body, options);

            foreach (var item in source)
            {
                block.Post(item);
            }

            block.Complete();

            return block.Completion;
        }
    }
}
