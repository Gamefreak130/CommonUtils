namespace Gamefreak130.Common.Tasks
{
    using System;

    public class ActionTask : AwaitableTask
    {
        protected Action mAction;

        public ActionTask(Action action) : base(false)
            => mAction = action;

        protected ActionTask(bool runSynchronously) : base(runSynchronously)
        {
        }

        protected override void Perform() => mAction();
    }

    public class ActionTask<TParam> : ActionTask
    {
        public ActionTask(Action<TParam> action, TParam param) : base(false)
            => mAction = () => action(param);
    }

    public abstract partial class AwaitableTask : IDisposable
    {
        private class ContinuationActionTask<TTask> : ActionTask, IContinuationTask where TTask : AwaitableTask
        {
            protected AwaitableTask mAntecedent;

            public ContinuationActionTask(Action<TTask> action, bool runSynchronously) : this(runSynchronously)
                => mAction = delegate {
                    TTask antecedent = mAntecedent as TTask;
                    mAntecedent = null;
                    action(antecedent);
                };

            protected ContinuationActionTask(bool runSynchronously) : base(runSynchronously)
                => Status = AwaitableTaskStatus.WaitingForContinuation;

            public void StartAsContinuation(AwaitableTask antecedent)
            {
                mAntecedent = antecedent;
                StartInternal();
            }
        }

        private class ContinuationActionTask<TTask, TParam> : ContinuationActionTask<TTask> where TTask : AwaitableTask
        {
            public ContinuationActionTask(Action<TTask, TParam> action, TParam param, bool runSynchronously) : base(runSynchronously)
                => mAction = delegate {
                    TTask antecedent = mAntecedent as TTask;
                    mAntecedent = null;
                    action(antecedent, param);
                };
        }

        public AwaitableTask ContinueWith(Action<AwaitableTask> action, bool runSynchronously = false)
            => ContinueWithInternal(this, action, runSynchronously);

        public AwaitableTask ContinueWith<TParam>(Action<AwaitableTask, TParam> action, TParam param, bool runSynchronously = false)
            => ContinueWithInternal(this, action, param, runSynchronously);

        private protected static AwaitableTask ContinueWithInternal<TTask>(TTask antecedent, Action<TTask> action, bool runSynchronously = false)
            where TTask : AwaitableTask
        {
            ContinuationActionTask<TTask> task = new(action, runSynchronously);
            antecedent.AddContinuation(task);
            return task;
        }

        private protected static AwaitableTask ContinueWithInternal<TTask, TParam>(TTask antecedent, Action<TTask, TParam> action, TParam param, bool runSynchronously = false)
            where TTask : AwaitableTask
        {
            ContinuationActionTask<TTask, TParam> task = new(action, param, runSynchronously);
            antecedent.AddContinuation(task);
            return task;
        }

        internal static AwaitableTask ContinueWhenAll(AwaitableTask[] tasks, Action<AwaitableTask[]> action, bool runSynchronously = false)
            => ContinueWhenAllInternal(tasks).ContinueWith((antecedent, action) => action(antecedent.Await()), action, runSynchronously);

        internal static AwaitableTask ContinueWhenAll<TParam>(AwaitableTask[] tasks, Action<AwaitableTask[], TParam> action, TParam param, bool runSynchronously = false)
            => ContinueWhenAllInternal(tasks).ContinueWith((antecedent, param) => action(antecedent.Await(), param), param, runSynchronously);

        internal static AwaitableTask ContinueWhenAll<TAntecedentResult>(AwaitableTask<TAntecedentResult>[] tasks, Action<AwaitableTask<TAntecedentResult>[]> action, bool runSynchronously = false)
            => ContinueWhenAllInternal(tasks).ContinueWith((antecedent, action) => action(antecedent.Await()), action, runSynchronously);

        internal static AwaitableTask ContinueWhenAll<TAntecedentResult, TParam>(AwaitableTask<TAntecedentResult>[] tasks, Action<AwaitableTask<TAntecedentResult>[], TParam> action, TParam param, bool runSynchronously = false)
            => ContinueWhenAllInternal(tasks).ContinueWith((antecedent, param) => action(antecedent.Await(), param), param, runSynchronously);
    }

    public abstract partial class AwaitableTask<TResult> : AwaitableTask
    {
        public AwaitableTask ContinueWith(Action<AwaitableTask<TResult>> action, bool runSynchronously = false)
            => ContinueWithInternal(this, action, runSynchronously);

        public AwaitableTask ContinueWith<TParam>(Action<AwaitableTask<TResult>, TParam> action, TParam param, bool runSynchronously = false)
            => ContinueWithInternal(this, action, param, runSynchronously);
    }

    public static partial class TaskEx
    {
        public static AwaitableTask Run(Action action)
        {
            ActionTask task = new(action);
            task.Start();
            return task;
        }

        public static AwaitableTask Run<TParam>(Action<TParam> action, TParam param)
        {
            ActionTask<TParam> task = new(action, param);
            task.Start();
            return task;
        }

        public static AwaitableTask ContinueWhenAny(AwaitableTask[] tasks, Action<AwaitableTask> action, bool runSynchronously = false)
            // Ordinarily we'd use Unwrap() after the WhenAny() to forward cancellations and exceptions
            // But WhenAny should never fault, and since there is no way to directly reference it here it can't be canceled
            // So we can just directly await the result of the WhenAny() on completion and not have to construct another continuation task
            => AwaitableTask.WhenAny(tasks).ContinueWith((antecedent, action) => action(antecedent.Await()), action, runSynchronously);

        public static AwaitableTask ContinueWhenAny<TParam>(AwaitableTask[] tasks, Action<AwaitableTask, TParam> action, TParam param, bool runSynchronously = false)
            => AwaitableTask.WhenAny(tasks).ContinueWith((antecedent, param) => action(antecedent.Await(), param), param, runSynchronously);

        public static AwaitableTask ContinueWhenAny<TAntecedentResult>(AwaitableTask<TAntecedentResult>[] tasks, Action<AwaitableTask<TAntecedentResult>> action, bool runSynchronously = false)
            => AwaitableTask.WhenAny(tasks).ContinueWith((antecedent, action) => action(antecedent.Await()), action, runSynchronously);

        public static AwaitableTask ContinueWhenAny<TAntecedentResult, TParam>(AwaitableTask<TAntecedentResult>[] tasks, Action<AwaitableTask<TAntecedentResult>, TParam> action, TParam param, bool runSynchronously = false)
            => AwaitableTask.WhenAny(tasks).ContinueWith((antecedent, param) => action(antecedent.Await(), param), param, runSynchronously);

        public static AwaitableTask ContinueWhenAll(AwaitableTask[] tasks, Action<AwaitableTask[]> action, bool runSynchronously = false)
            => AwaitableTask.ContinueWhenAll(tasks, action, runSynchronously);

        public static AwaitableTask ContinueWhenAll<TParam>(AwaitableTask[] tasks, Action<AwaitableTask[], TParam> action, TParam param, bool runSynchronously = false)
            => AwaitableTask.ContinueWhenAll(tasks, action, param, runSynchronously);

        public static AwaitableTask ContinueWhenAll<TAntecedentResult>(AwaitableTask<TAntecedentResult>[] tasks, Action<AwaitableTask<TAntecedentResult>[]> action, bool runSynchronously = false)
            => AwaitableTask.ContinueWhenAll(tasks, action, runSynchronously);

        public static AwaitableTask ContinueWhenAll<TAntecedentResult, TParam>(AwaitableTask<TAntecedentResult>[] tasks, Action<AwaitableTask<TAntecedentResult>[], TParam> action, TParam param, bool runSynchronously = false)
            => AwaitableTask.ContinueWhenAll(tasks, action, param, runSynchronously);
    }
}
