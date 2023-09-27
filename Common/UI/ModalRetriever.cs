namespace Gamefreak130.Common.UI
{
    using Gamefreak130.Common.Loggers;
    using Sims3.SimIFace;
    using Sims3.UI;
    using System;
    using System.Collections.Generic;

    public static class ModalRetriever
    {
        public static bool TryGetModalDialog(out ModalDialog modal) => TryGetModalDialog(UIManager.GetModalWindow() as Dialog, out modal);

        public static bool TryGetModalDialog(Dialog window, out ModalDialog modal)
        {
            modal = null;

            // ModalDialog instances always add trigger hooks to their associated dialog windows when constructed
            // By inspecting the callback delegates mapped to the TriggerDown event of a dialog window, we can retrieve the ModalDialog logic that constructed it
            if (window is not null && UIManager.mEventRegistry.ContainsKey(window.WinHandle) && UIManager.mEventRegistry[window.WinHandle].EventTypesAndCallbacks.ContainsKey((uint)WindowBase.WindowBaseEvents.kEventWindowBaseTriggerDown)
                && UIManager.mEventRegistry[window.WinHandle].EventTypesAndCallbacks[(uint)WindowBase.WindowBaseEvents.kEventWindowBaseTriggerDown].mEventHandlers.Find(d => d.Method.DeclaringType == typeof(ModalDialog)) is Delegate @delegate)
            {
                modal = (ModalDialog)@delegate.Target;
                return true;
            }
            return false;
        }
    }

    public class ModalRetriever<T> : IDisposable where T : ModalDialog
    {
        private bool mBootstrapped;

        private readonly HashSet<uint> mActiveModals = new();

        private WindowBase mFocusedWindow;

        private Action<T> mModalPushed;

        public event Action<T> ModalPushed
        {
            add
            {
                if (!mBootstrapped)
                {
                    OnFocusChange(null, null);
                    mBootstrapped = true;
                }
                mModalPushed += value;
            }

            remove => mModalPushed -= value;
        }

        private void SetFocusedWindow(WindowBase window)
        { 
            if (mFocusedWindow is not null)
            {
                mFocusedWindow.FocusLost -= OnFocusChange;
                mFocusedWindow.Detach -= OnFocusChange;
            }
            mFocusedWindow = window;
            if (mFocusedWindow is not null)
            {
                // Modals are not registered as such by the UIManager until after the focus is set
                // So we run the modal check as a task to delay its execution
                Simulator.AddObject(new OneShotFunctionWithParams(RetrieveModal, mFocusedWindow));
                mFocusedWindow.FocusLost += OnFocusChange;
                mFocusedWindow.Detach += OnFocusChange;
            }
        }

        public void Dispose() => SetFocusedWindow(null);

        private void OnFocusChange(WindowBase _, UIEventArgs __) 
            => SetFocusedWindow(UIManager.GetFocus(InputContext.kICKeyboard));

        private void RetrieveModal(object window)
        {
            try
            {
                if (window is Dialog dialog && ModalRetriever.TryGetModalDialog(dialog, out ModalDialog modal) && modal is T && !mActiveModals.Contains(dialog.WinHandle))
                {
                    mActiveModals.Add(dialog.WinHandle);
                    mModalPushed?.Invoke(modal as T);
                    dialog.Detach += (_, _) => mActiveModals.Remove(dialog.WinHandle);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.sInstance.Log(ex);
            }
        }
    }
}
