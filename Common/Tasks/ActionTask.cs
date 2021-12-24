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
}
