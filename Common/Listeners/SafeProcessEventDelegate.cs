namespace Gamefreak130.Common.Listeners
{
    using Gamefreak130.Common.Loggers;
    using Sims3.Gameplay.EventSystem;
    using System;

    public class SafeProcessEventDelegate
    {
        private readonly ProcessEventDelegate mOriginalDelegate;

        private readonly ListenerAction mExceptionAction;

        private SafeProcessEventDelegate(ProcessEventDelegate originalDelegate, ListenerAction exceptionAction)
        {
            mOriginalDelegate = originalDelegate;
            mExceptionAction = exceptionAction;
        }

        private ListenerAction ProcessEvent(Event e)
        {
            try
            {
                return mOriginalDelegate(e);
            }
            catch (Exception ex)
            {
                ExceptionLogger.sInstance.Log(ex);

                if (mExceptionAction is ListenerAction.Remove)
                {
                    // Rethrow to the EventTracker so that the listener is removed without setting CompletionEvent
                    throw;
                }
                return ListenerAction.Keep;
            }
        }

        public static ProcessEventDelegate Create(ProcessEventDelegate @delegate, ListenerAction exceptionAction)
            => new SafeProcessEventDelegate(@delegate, exceptionAction).ProcessEvent;
    }
}
