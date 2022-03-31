namespace Gamefreak130.Common.Tasks
{
    using System;

    public class FunctionTask<TResult> : AwaitableTask<TResult>
    {
        protected Func<TResult> mFunc;

        public FunctionTask(Func<TResult> func) : base(false)
            => mFunc = func;

        protected FunctionTask(bool runSynchronously) : base(runSynchronously)
        {
        }

        protected override TResult GetResult() => mFunc();
    }

    public class FunctionTask<TParam, TResult> : FunctionTask<TResult>
    {
        public FunctionTask(Func<TParam, TResult> func, TParam param) : base(false)
            => mFunc = () => func(param);
    }

    public abstract partial class AwaitableTask : IDisposable
    {
        private class ContinuationFuncTask<TTask, TResult> : FunctionTask<TResult>, IContinuationTask where TTask : AwaitableTask
        {
            protected AwaitableTask mAntecedent;

            public ContinuationFuncTask(Func<TTask, TResult> action, bool runSynchronously) : this(runSynchronously)
                => mFunc = delegate {
                    TTask antecedent = mAntecedent as TTask;
                    mAntecedent = null;
                    return action(antecedent);
                };

            protected ContinuationFuncTask(bool runSynchronously) : base(runSynchronously)
                => Status = AwaitableTaskStatus.WaitingForContinuation;

            public void StartAsContinuation(AwaitableTask antecedent)
            {
                mAntecedent = antecedent;
                StartInternal();
            }
        }

        private class ContinuationFuncTask<TTask, TParam, TResult> : ContinuationFuncTask<TTask, TResult> where TTask : AwaitableTask
        {
            public ContinuationFuncTask(Func<TTask, TParam, TResult> action, TParam param, bool runSynchronously) : base(runSynchronously)
                => mFunc = delegate {
                    TTask antecedent = mAntecedent as TTask;
                    mAntecedent = null;
                    return action(antecedent, param);
                };
        }

        public AwaitableTask<TResult> ContinueWith<TResult>(Func<AwaitableTask, TResult> func, bool runSynchronously = false)
            => ContinueWithInternal(this, func, runSynchronously);

        public AwaitableTask<TResult> ContinueWith<TParam, TResult>(Func<AwaitableTask, TParam, TResult> func, TParam param, bool runSynchronously = false)
            => ContinueWithInternal(this, func, param, runSynchronously);

        private protected static AwaitableTask<TResult> ContinueWithInternal<TTask, TResult>(TTask antecedent, Func<TTask, TResult> func, bool runSynchronously = false)
            where TTask : AwaitableTask
        {
            ContinuationFuncTask<TTask, TResult> task = new(func, runSynchronously);
            antecedent.AddContinuation(task);
            return task;
        }

        private protected static AwaitableTask<TResult> ContinueWithInternal<TTask, TParam, TResult>(TTask antecedent, Func<TTask, TParam, TResult> func, TParam param, bool runSynchronously = false)
            where TTask : AwaitableTask
        {
            ContinuationFuncTask<TTask, TParam, TResult> task = new(func, param, runSynchronously);
            antecedent.AddContinuation(task);
            return task;
        }

        internal static AwaitableTask<TResult> ContinueWhenAll<TResult>(AwaitableTask[] tasks, Func<AwaitableTask[], TResult> action, bool runSynchronously = false)
            => ContinueWhenAllInternal(tasks).ContinueWith((antecedent, action) => action(antecedent.Await()), action, runSynchronously);

        internal static AwaitableTask<TResult> ContinueWhenAll<TParam, TResult>(AwaitableTask[] tasks, Func<AwaitableTask[], TParam, TResult> action, TParam param, bool runSynchronously = false)
            => ContinueWhenAllInternal(tasks).ContinueWith((antecedent, param) => action(antecedent.Await(), param), param, runSynchronously);

        internal static AwaitableTask<TResult> ContinueWhenAll<TAntecedentResult, TResult>(AwaitableTask<TAntecedentResult>[] tasks, Func<AwaitableTask<TAntecedentResult>[], TResult> action, bool runSynchronously = false)
            => ContinueWhenAllInternal(tasks).ContinueWith((antecedent, action) => action(antecedent.Await()), action, runSynchronously);

        internal static AwaitableTask<TResult> ContinueWhenAll<TAntecedentResult, TParam, TResult>(AwaitableTask<TAntecedentResult>[] tasks, Func<AwaitableTask<TAntecedentResult>[], TParam, TResult> action, TParam param, bool runSynchronously = false)
            => ContinueWhenAllInternal(tasks).ContinueWith((antecedent, param) => action(antecedent.Await(), param), param, runSynchronously);
    }

    public abstract partial class AwaitableTask<TResult> : AwaitableTask
    {
        public AwaitableTask<TFinalResult> ContinueWith<TFinalResult>(Func<AwaitableTask<TResult>, TFinalResult> func, bool runSynchronously = false)
            => ContinueWithInternal(this, func, runSynchronously);

        public AwaitableTask<TFinalResult> ContinueWith<TParam, TFinalResult>(Func<AwaitableTask<TResult>, TParam, TFinalResult> func, TParam param, bool runSynchronously = false)
            => ContinueWithInternal(this, func, param, runSynchronously);
    }

    public static partial class TaskEx
    {
        public static AwaitableTask Run(Func<AwaitableTask> func)
            => Run<AwaitableTask>(func).Unwrap();

        public static AwaitableTask Run<TParam>(Func<TParam, AwaitableTask> func, TParam param)
            => Run<TParam, AwaitableTask>(func, param).Unwrap();

        public static AwaitableTask<TResult> Run<TResult>(Func<AwaitableTask<TResult>> func)
            => Run<AwaitableTask<TResult>>(func).Unwrap();

        public static AwaitableTask<TResult> Run<TParam, TResult>(Func<TParam, AwaitableTask<TResult>> func, TParam param)
            => Run<TParam, AwaitableTask<TResult>>(func, param).Unwrap();

        public static AwaitableTask<TResult> Run<TResult>(Func<TResult> func)
        {
            FunctionTask<TResult> task = new(func);
            task.Start();
            return task;
        }

        public static AwaitableTask<TResult> Run<TParam, TResult>(Func<TParam, TResult> func, TParam param)
        {
            FunctionTask<TParam, TResult> task = new(func, param);
            task.Start();
            return task;
        }

        public static AwaitableTask<TResult> ContinueWhenAny<TResult>(AwaitableTask[] tasks, Func<AwaitableTask, TResult> action, bool runSynchronously = false)
            => AwaitableTask.WhenAny(tasks).ContinueWith((antecedent, action) => action(antecedent.Await()), action, runSynchronously);

        public static AwaitableTask<TResult> ContinueWhenAny<TParam, TResult>(AwaitableTask[] tasks, Func<AwaitableTask, TParam, TResult> action, TParam param, bool runSynchronously = false)
            => AwaitableTask.WhenAny(tasks).ContinueWith((antecedent, param) => action(antecedent.Await(), param), param, runSynchronously);

        public static AwaitableTask<TResult> ContinueWhenAny<TAntecedentResult, TResult>(AwaitableTask<TAntecedentResult>[] tasks, Func<AwaitableTask<TAntecedentResult>, TResult> action, bool runSynchronously = false)
            => AwaitableTask.WhenAny(tasks).ContinueWith((antecedent, action) => action(antecedent.Await()), action, runSynchronously);

        public static AwaitableTask<TResult> ContinueWhenAny<TAntecedentResult, TParam, TResult>(AwaitableTask<TAntecedentResult>[] tasks, Func<AwaitableTask<TAntecedentResult>, TParam, TResult> action, TParam param, bool runSynchronously = false)
            => AwaitableTask.WhenAny(tasks).ContinueWith((antecedent, param) => action(antecedent.Await(), param), param, runSynchronously);

        public static AwaitableTask<TResult> ContinueWhenAll<TResult>(AwaitableTask[] tasks, Func<AwaitableTask[], TResult> action, bool runSynchronously = false)
            => AwaitableTask.ContinueWhenAll(tasks, action, runSynchronously);

        public static AwaitableTask<TResult> ContinueWhenAll<TParam, TResult>(AwaitableTask[] tasks, Func<AwaitableTask[], TParam, TResult> action, TParam param, bool runSynchronously = false)
            => AwaitableTask.ContinueWhenAll(tasks, action, param, runSynchronously);

        public static AwaitableTask<TResult> ContinueWhenAll<TAntecedentResult, TResult>(AwaitableTask<TAntecedentResult>[] tasks, Func<AwaitableTask<TAntecedentResult>[], TResult> action, bool runSynchronously = false)
            => AwaitableTask.ContinueWhenAll(tasks, action, runSynchronously);

        public static AwaitableTask<TResult> ContinueWhenAll<TAntecedentResult, TParam, TResult>(AwaitableTask<TAntecedentResult>[] tasks, Func<AwaitableTask<TAntecedentResult>[], TParam, TResult> action, TParam param, bool runSynchronously = false)
            => AwaitableTask.ContinueWhenAll(tasks, action, param, runSynchronously);
    }
}
