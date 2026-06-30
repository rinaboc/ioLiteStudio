using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Core.Services
{
    public interface IThreadPoolService : IDisposable
    {
        void Post(Func<Task> task);
        void Post(Action action);
    }

    public class ThreadPoolService : IThreadPoolService
    {
        private readonly ActionBlock<Func<Task>> _taskActionBlock;
        private readonly ActionBlock<Action> _actionBlock;

        public ThreadPoolService()
        {
            _taskActionBlock = new(async task =>
            {
                await task?.Invoke();
            }, new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = 1
            });

            _actionBlock = new(action => action?.Invoke(),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1
                });
        }

        public void Dispose()
        {
            _taskActionBlock.Complete();
            _actionBlock.Complete();
        }

        public void Post(Func<Task> task)
        {
            _taskActionBlock.Post(task);
        }

        public void Post(Action action)
        {
            _actionBlock.Post(action);
        }
    }
}
