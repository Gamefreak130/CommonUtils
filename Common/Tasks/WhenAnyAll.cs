namespace Gamefreak130.Common.Tasks
{
    using Sims3.SimIFace;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public abstract partial class AwaitableTask : IDisposable
    {
        private class WhenAnyPromise<TTask> : AwaitableTask<TTask>, IContinuationTask where TTask : AwaitableTask
        {
            protected AwaitableTask mAntecedent;

            public WhenAnyPromise() : base(true)
                => Status = AwaitableTaskStatus.WaitingForContinuation;

            public void StartAsContinuation(AwaitableTask antecedent)
            {
                mAntecedent = antecedent;
                StartInternal();
            }

            protected override TTask GetResult()
            {
                TTask antecedent = mAntecedent as TTask;
                mAntecedent = null;
                return antecedent;
            }
        }

        private class WhenAllPromise<TResult> : AwaitableTask<TResult[]>, IContinuationTask
        {
            private int mNumRemaining;

            private AwaitableTask[] mAntecedentTasks;

            public WhenAllPromise(AwaitableTask[] tasks) : base(true)
            {
                Status = AwaitableTaskStatus.WaitingForContinuation;
                mAntecedentTasks = tasks;
                mNumRemaining = mAntecedentTasks.Length;
                foreach (AwaitableTask task in mAntecedentTasks)
                {
                    task.AddContinuation(this);
                }
            }

            protected override void Dispose(bool fromSimulator)
            {
                mAntecedentTasks = null;
                base.Dispose(fromSimulator);
            }

            public void StartAsContinuation(AwaitableTask antecedent)
            {
                if (--mNumRemaining < 0)
                {
                    throw new Exception("Remaining task count should never go below 0.");
                }
                if (mNumRemaining == 0)
                {
                    StartInternal();
                }
            }

            protected override TResult[] GetResult()
            {
                TResult[] results = new TResult[mAntecedentTasks.Length];
                List<Exception> observedExceptions = null;
                bool shouldFault = false;
                bool shouldCancel = false;
                for (int i = 0; i < mAntecedentTasks.Length; i++)
                {
                    AwaitableTask task = mAntecedentTasks[i];
                    if (task.IsFaulted)
                    {
                        observedExceptions ??= new();
                        observedExceptions.AddRange(task.Exception.InnerExceptions);
                        shouldFault = true;
                    }
                    else if (task.IsCanceled)
                    {
                        ReadOnlyCollection<Exception> innerExceptions = task.Exception.InnerExceptions;
                        if (innerExceptions.Count > 1)
                        {
                            observedExceptions ??= new();
                            for (int j = 0; j < innerExceptions.Count - 1; i++)
                            {
                                observedExceptions.Add(innerExceptions[j]);
                            }
                        }
                        shouldCancel = true;
                    }
                    else
                    {
                        results[i] = task is AwaitableTask<TResult> taskWithResult ? taskWithResult.Await() : default;
                    }
                    mAntecedentTasks[i] = null;
                }

                if (observedExceptions?.Count > 0)
                {
                    mInnerExceptions = observedExceptions;
                }

                return shouldCancel && !shouldFault ? throw new ResetException() : results;
            }
        }

        private class ContinueWhenAllPromise<TTask> : AwaitableTask<TTask[]>, IContinuationTask where TTask : AwaitableTask
        {
            private int mNumRemaining;

            private TTask[] mAntecedentTasks;

            public ContinueWhenAllPromise(TTask[] tasks) : base(true)
            {
                Status = AwaitableTaskStatus.WaitingForContinuation;
                mAntecedentTasks = tasks;
                mNumRemaining = mAntecedentTasks.Length;
                foreach (AwaitableTask task in mAntecedentTasks)
                {
                    task.AddContinuation(this);
                }
            }

            protected override void Dispose(bool fromSimulator)
            {
                mAntecedentTasks = null;
                base.Dispose(fromSimulator);
            }

            public void StartAsContinuation(AwaitableTask antecedent)
            {
                if (--mNumRemaining < 0)
                {
                    throw new Exception("Remaining task count should never go below 0.");
                }
                if (mNumRemaining == 0)
                {
                    StartInternal();
                }
            }

            protected override TTask[] GetResult() => mAntecedentTasks;
        }

        internal static AwaitableTask<AwaitableTask> WhenAny(params AwaitableTask[] tasks)
            => WhenAnyInternal(tasks);

        internal static AwaitableTask<AwaitableTask<TResult>> WhenAny<TResult>(params AwaitableTask<TResult>[] tasks)
            => WhenAnyInternal(tasks);

        private static AwaitableTask<TTask> WhenAnyInternal<TTask>(TTask[] tasks) where TTask : AwaitableTask
        {
            if (tasks.Length == 0)
            {
                throw new ArgumentException(kNoTasksMessage, nameof(tasks));
            }

            // We would just like to do this:
            //    return (Task<Task<TResult>>) WhenAny( (Task[]) tasks);
            // but classes are not covariant to enable casting Task<TResult> to Task<Task<TResult>>.

            // Check the first two tasks for completion so that we can avoid constructing an unnecessary task
            // In what is apparently the most common use case, according to Microsoft
            if (tasks[0].IsCompleted)
            {
                return TaskEx.FromResult(tasks[0]);
            }
            if (tasks.Length >= 2 && tasks[1].IsCompleted)
            {
                return TaskEx.FromResult(tasks[1]);
            }

            WhenAnyPromise<TTask> completionTask = new();
            foreach (AwaitableTask task in tasks)
            {
                task.AddContinuation(completionTask);
            }
            return completionTask;
        }

        internal static AwaitableTask WhenAll(params AwaitableTask[] tasks)
            => WhenAllInternal<object>(tasks);

        internal static AwaitableTask<TResult[]> WhenAll<TResult>(params AwaitableTask<TResult>[] tasks)
            => WhenAllInternal<TResult>(tasks);

        private static AwaitableTask<TResult[]> WhenAllInternal<TResult>(AwaitableTask[] tasks)
        {
            if (tasks.Length == 0)
            {
                throw new ArgumentException(kNoTasksMessage, nameof(tasks));
            }

            AwaitableTask[] tasksCopy = new AwaitableTask[tasks.Length];
            tasks.CopyTo(tasksCopy, 0);
            return new WhenAllPromise<TResult>(tasksCopy);
        }

        private static AwaitableTask<TTask[]> ContinueWhenAllInternal<TTask>(TTask[] tasks) where TTask : AwaitableTask
        {
            if (tasks.Length == 0)
            {
                throw new ArgumentException(kNoTasksMessage, nameof(tasks));
            }

            TTask[] tasksCopy = new TTask[tasks.Length];
            tasks.CopyTo(tasksCopy, 0);
            return new ContinueWhenAllPromise<TTask>(tasks);
        }
    }

    public static partial class TaskEx
    {
        public static AwaitableTask<AwaitableTask> WhenAny(params AwaitableTask[] tasks)
            => AwaitableTask.WhenAny(tasks);

        public static AwaitableTask<AwaitableTask<TResult>> WhenAny<TResult>(params AwaitableTask<TResult>[] tasks)
            => AwaitableTask.WhenAny(tasks);

        public static AwaitableTask WhenAll(params AwaitableTask[] tasks)
            => AwaitableTask.WhenAll(tasks);

        public static AwaitableTask<TResult[]> WhenAll<TResult>(params AwaitableTask<TResult>[] tasks)
            => AwaitableTask.WhenAll(tasks);
    }
}
