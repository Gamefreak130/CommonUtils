namespace Gamefreak130.Common.Tasks
{
    using Gamefreak130.Common.Helpers;
    using Sims3.SimIFace;
    using System;

    public static class TaskEx
    {
        private class DelayTask : AwaitableTask
        {
            private StopWatch mTimer;

            private readonly uint mDelay;

            private readonly StopWatch.TickStyles mTickStyles;

            public DelayTask(uint delay, StopWatch.TickStyles tickStyles) : base(false)
            {
                mDelay = delay;
                mTickStyles = tickStyles;
                Start();
            }

            protected override void Dispose(bool fromSimulator)
            {
                mTimer?.Dispose();
                mTimer = null;
                base.Dispose(fromSimulator);
            }

            protected override void Perform()
            {
                mTimer = StopWatchEx.StartNew(mTickStyles);
                while (mTimer?.GetElapsedTime() < mDelay)
                {
                    Yield(true);
                }
            }
        }

        private class NopTask : AwaitableTask
        {
            public NopTask(bool complete) : base(true)
                => Status = complete ? AwaitableTaskStatus.RanToCompletion : AwaitableTaskStatus.Canceled;

            public NopTask(Exception exception) : base(true)
            {
                AddException(exception);
                Status = AwaitableTaskStatus.Faulted;
            }

            protected override void Perform() => throw new NotSupportedException();
        }

        private class NopTask<TResult> : AwaitableTask<TResult>
        {
            public NopTask() : base(true)
                => Status = AwaitableTaskStatus.Canceled;

            public NopTask(TResult result) : base(result) 
                => Status = AwaitableTaskStatus.RanToCompletion;

            public NopTask(Exception exception) : base(true)
            {
                AddException(exception);
                Status = AwaitableTaskStatus.Faulted;
            }

            protected override TResult GetResult() => throw new NotSupportedException();
        }

        public static AwaitableTask CompletedTask => new NopTask(true);

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

        public static AwaitableTask Run(Func<AwaitableTask> func) 
            => Run<AwaitableTask>(func).Unwrap();

        public static AwaitableTask Run<TParam>(Func<TParam, AwaitableTask> func, TParam param)
            => Run<TParam, AwaitableTask>(func, param).Unwrap();

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

        public static AwaitableTask<TResult> Run<TResult>(Func<AwaitableTask<TResult>> func)
            => Run<AwaitableTask<TResult>>(func).Unwrap();

        public static AwaitableTask<TResult> Run<TParam, TResult>(Func<TParam, AwaitableTask<TResult>> func, TParam param)
            => Run<TParam, AwaitableTask<TResult>>(func, param).Unwrap();

        public static AwaitableTask<AwaitableTask> WhenAny(params AwaitableTask[] tasks)
            => AwaitableTask.WhenAny(tasks);

        public static AwaitableTask<AwaitableTask<TResult>> WhenAny<TResult>(params AwaitableTask<TResult>[] tasks)
            => AwaitableTask.WhenAny(tasks);

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

        public static AwaitableTask<TResult> ContinueWhenAny<TResult>(AwaitableTask[] tasks, Func<AwaitableTask, TResult> action, bool runSynchronously = false)
            => AwaitableTask.WhenAny(tasks).ContinueWith((antecedent, action) => action(antecedent.Await()), action, runSynchronously);

        public static AwaitableTask<TResult> ContinueWhenAny<TParam, TResult>(AwaitableTask[] tasks, Func<AwaitableTask, TParam, TResult> action, TParam param, bool runSynchronously = false)
            => AwaitableTask.WhenAny(tasks).ContinueWith((antecedent, param) => action(antecedent.Await(), param), param, runSynchronously);

        public static AwaitableTask<TResult> ContinueWhenAny<TAntecedentResult, TResult>(AwaitableTask<TAntecedentResult>[] tasks, Func<AwaitableTask<TAntecedentResult>, TResult> action, bool runSynchronously = false)
            => AwaitableTask.WhenAny(tasks).ContinueWith((antecedent, action) => action(antecedent.Await()), action, runSynchronously);

        public static AwaitableTask<TResult> ContinueWhenAny<TAntecedentResult, TParam, TResult>(AwaitableTask<TAntecedentResult>[] tasks, Func<AwaitableTask<TAntecedentResult>, TParam, TResult> action, TParam param, bool runSynchronously = false)
            => AwaitableTask.WhenAny(tasks).ContinueWith((antecedent, param) => action(antecedent.Await(), param), param, runSynchronously);

        public static AwaitableTask WhenAll(params AwaitableTask[] tasks)
            => AwaitableTask.WhenAll(tasks);

        public static AwaitableTask<TResult[]> WhenAll<TResult>(params AwaitableTask<TResult>[] tasks)
            => AwaitableTask.WhenAll(tasks);

        public static AwaitableTask ContinueWhenAll(AwaitableTask[] tasks, Action<AwaitableTask[]> action, bool runSynchronously = false)
            => AwaitableTask.ContinueWhenAll(tasks, action, runSynchronously);

        public static AwaitableTask ContinueWhenAll<TParam>(AwaitableTask[] tasks, Action<AwaitableTask[], TParam> action, TParam param, bool runSynchronously = false)
            => AwaitableTask.ContinueWhenAll(tasks, action, param, runSynchronously);

        public static AwaitableTask ContinueWhenAll<TAntecedentResult>(AwaitableTask<TAntecedentResult>[] tasks, Action<AwaitableTask<TAntecedentResult>[]> action, bool runSynchronously = false)
            => AwaitableTask.ContinueWhenAll(tasks, action, runSynchronously);

        public static AwaitableTask ContinueWhenAll<TAntecedentResult, TParam>(AwaitableTask<TAntecedentResult>[] tasks, Action<AwaitableTask<TAntecedentResult>[], TParam> action, TParam param, bool runSynchronously = false)
            => AwaitableTask.ContinueWhenAll(tasks, action, param, runSynchronously);

        public static AwaitableTask<TResult> ContinueWhenAll<TResult>(AwaitableTask[] tasks, Func<AwaitableTask[], TResult> action, bool runSynchronously = false)
            => AwaitableTask.ContinueWhenAll(tasks, action, runSynchronously);

        public static AwaitableTask<TResult> ContinueWhenAll<TParam, TResult>(AwaitableTask[] tasks, Func<AwaitableTask[], TParam, TResult> action, TParam param, bool runSynchronously = false)
            => AwaitableTask.ContinueWhenAll(tasks, action, param, runSynchronously);

        public static AwaitableTask<TResult> ContinueWhenAll<TAntecedentResult, TResult>(AwaitableTask<TAntecedentResult>[] tasks, Func<AwaitableTask<TAntecedentResult>[], TResult> action, bool runSynchronously = false)
            => AwaitableTask.ContinueWhenAll(tasks, action, runSynchronously);

        public static AwaitableTask<TResult> ContinueWhenAll<TAntecedentResult, TParam, TResult>(AwaitableTask<TAntecedentResult>[] tasks, Func<AwaitableTask<TAntecedentResult>[], TParam, TResult> action, TParam param, bool runSynchronously = false)
            => AwaitableTask.ContinueWhenAll(tasks, action, param, runSynchronously);

        public static AwaitableTask Delay(uint delay, StopWatch.TickStyles tickStyles = StopWatch.TickStyles.Milliseconds)
        {
            DelayTask task = new(delay, tickStyles);
            return task;
        }

        public static AwaitableTask<TResult> FromResult<TResult>(TResult result)
            => new NopTask<TResult>(result);

        public static AwaitableTask FromCanceled()
            => new NopTask(false);

        public static AwaitableTask<TResult> FromCanceled<TResult>()
            => new NopTask<TResult>();

        public static AwaitableTask FromException(Exception exception)
            => new NopTask(exception);

        public static AwaitableTask<TResult> FromException<TResult>(Exception exception)
            => new NopTask<TResult>(exception);

        public static AwaitableTask Unwrap(this AwaitableTask<AwaitableTask> task)
            => AwaitableTask.Unwrap(task);

        public static AwaitableTask<TResult> Unwrap<TResult>(this AwaitableTask<AwaitableTask<TResult>> task)
            => AwaitableTask.Unwrap(task);

        public static void Yield(bool shouldThrowException = false)
            => Yield(0, shouldThrowException);

        public static void Yield(uint delayTicks, bool shouldThrowException = false)
        {
            if (Simulator.CheckYieldingContext(shouldThrowException))
            {
                Simulator.Sleep(delayTicks);
            }
        }
    }
}
