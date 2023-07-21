namespace Gamefreak130.Common.UI
{
    using Sims3.UI;
    using System;

    public static class UIHelper
    {
        public static void ShowElementById(WindowBase containingWindow, uint id, bool recursive = true)
        {
            if (containingWindow.GetChildByID(id, recursive) is WindowBase window)
            {
                window.Visible = true;
            }
        }

        public static void EnableElementById(WindowBase containingWindow, uint id, bool recursive = true)
        {
            if (containingWindow.GetChildByID(id, recursive) is WindowBase window)
            {
                window.Enabled = true;
            }
        }

        public static void HideElementById(WindowBase containingWindow, uint id, bool recursive = true)
        {
            if (containingWindow.GetChildByID(id, recursive) is WindowBase window)
            {
                window.Visible = false;
            }
        }

        public static void DisableElementById(WindowBase containingWindow, uint id, bool recursive = true)
        {
            if (containingWindow.GetChildByID(id, recursive) is WindowBase window)
            {
                window.Enabled = false;
            }
        }

        public static void ShowElementByIndex(WindowBase containingWindow, uint index)
        {
            if (containingWindow.GetChildByIndex(index) is WindowBase window)
            {
                window.Visible = true;
            }
        }

        public static void EnableElementByIndex(WindowBase containingWindow, uint index)
        {
            if (containingWindow.GetChildByIndex(index) is WindowBase window)
            {
                window.Enabled = true;
            }
        }

        public static void HideElementByIndex(WindowBase containingWindow, uint index)
        {
            if (containingWindow.GetChildByIndex(index) is WindowBase window)
            {
                window.Visible = false;
            }
        }

        public static void DisableElementByIndex(WindowBase containingWindow, uint index)
        {
            if (containingWindow.GetChildByIndex(index) is WindowBase window)
            {
                window.Enabled = false;
            }
        }

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

        // The below two methods unsafely allow for multiple references to a single window wrapper
        // Be sure that the wrapper is properly finalized after acquisition, otherwise undefined behavior and/or memory leaks may occur
        public static WindowBase GetWindowWrapper(uint winHandle) 
            => UIManager.mCustomControlInstanceDict.ContainsKey(winHandle)
            ? UIManager.mCustomControlInstanceDict[winHandle]
            : UIManager.mControlInstanceCache.ContainsKey(winHandle) 
            ? UIManager.mControlInstanceCache[winHandle] 
            : null;

        public static T GetWindowWrapperByType<T>() where T : WindowBase
        {
            foreach (WindowBase window in UIManager.mCustomControlInstanceDict.Values)
            {
                if (window is T ret)
                {
                    return ret;
                }
            }
            foreach (WindowBase window2 in UIManager.mControlInstanceCache.Values)
            {
                if (window2 is T ret)
                {
                    return ret;
                }
            }
            return null;
        }
    }
}
