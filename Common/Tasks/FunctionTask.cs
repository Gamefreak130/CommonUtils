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
}
