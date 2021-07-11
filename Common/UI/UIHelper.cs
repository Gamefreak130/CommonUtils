namespace Gamefreak130.Common.UI
{
    using Sims3.UI;

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
    }
}
