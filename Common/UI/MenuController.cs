namespace Gamefreak130.Common.UI
{
    using Gamefreak130.Common.Loggers;
    using Sims3.Gameplay.Utilities;
    using Sims3.SimIFace;
    using Sims3.UI;
    using System;
    using System.Collections.Generic;
    using static Sims3.UI.ObjectPicker;

    /// <summary>
    /// Modal dialog utilizing <see cref="MenuContainer"/> to construct NRaas-like settings menus
    /// </summary>
    /// <seealso cref="MenuContainer"/>
    public class MenuController : ModalDialog
    {
        private enum ControlIds : uint
        {
            ItemTable = 99576784u,
            OkayButton,
            CancelButton,
            TitleText,
            TableBackground,
            TableBezel
        }

        private const int kWinExportID = 1;

        private Vector2 mTableOffset;

        private ObjectPicker mTable;

        private readonly Button mOkayButton;

        private readonly Button mCloseButton;

        private readonly TabContainer mTabsContainer;

        public bool Okay { get; private set; }

        public List<RowInfo> Result { get; private set; }

        public Action<List<RowInfo>> EndDelegates { get; private set; }

        public void ShowModal()
        {
            mModalDialogWindow.Moveable = true;
            StartModal();
        }

        public void Stop() => StopModal();

        public MenuController(string title, string buttonTrue, string buttonFalse, List<TabInfo> listObjs, List<HeaderInfo> headers, bool showHeadersAndToggle, Action<List<RowInfo>> endResultDelegates)
            : this(true, PauseMode.PauseSimulator, title, buttonTrue, buttonFalse, listObjs, headers, showHeadersAndToggle, endResultDelegates)
        {
        }

        public MenuController(bool isModal, PauseMode pauseMode, string title, string buttonTrue, string buttonFalse, List<TabInfo> listObjs, List<HeaderInfo> headers, bool showHeadersAndToggle, Action<List<RowInfo>> endResultDelegates)
            : base("UiObjectPicker", kWinExportID, isModal, pauseMode, null)
        {
            if (mModalDialogWindow is not null)
            {
                Text text = mModalDialogWindow.GetChildByID((uint)ControlIds.TitleText, false) as Text;
                text.Caption = title;
                mTable = mModalDialogWindow.GetChildByID((uint)ControlIds.ItemTable, false) as ObjectPicker;
                mTable.SelectionChanged += OnRowClicked;
                mTabsContainer = mTable.mTabs;
                mTable.mTable.mPopulationCompletedCallback += ResizeWindow;
                mOkayButton = mModalDialogWindow.GetChildByID((uint)ControlIds.OkayButton, false) as Button;
                mOkayButton.TooltipText = buttonTrue;
                mOkayButton.Enabled = true;
                mOkayButton.Click += OnOkayButtonClick;
                OkayID = mOkayButton.ID;
                SelectedID = mOkayButton.ID;
                mCloseButton = mModalDialogWindow.GetChildByID((uint)ControlIds.CancelButton, false) as Button;
                mCloseButton.TooltipText = buttonFalse;
                mCloseButton.Click += OnCloseButtonClick;
                CancelID = mCloseButton.ID;
                mTableOffset = mModalDialogWindow.Area.BottomRight - mModalDialogWindow.Area.TopLeft - (mTable.Area.BottomRight - mTable.Area.TopLeft);
                mTable.ShowHeaders = showHeadersAndToggle;
                mTable.ViewTypeToggle = false;
                mTable.ShowToggle = false;
                mTable.Populate(listObjs, headers, 1);
                ResizeWindow();
            }
            EndDelegates = endResultDelegates;
        }

        public void PopulateMenu(List<TabInfo> tabinfo, List<HeaderInfo> headers, int numSelectableRows) => mTable.Populate(tabinfo, headers, numSelectableRows);

        public override void Dispose() => Dispose(true);

        public void AddRow(int Tabnumber, RowInfo info)
        {
            mTable.mItems[Tabnumber].RowInfo.Clear();
            mTable.mItems[Tabnumber].RowInfo.Add(info);
            Repopulate();
        }

        public void SetTableColor(Color color) => mModalDialogWindow.GetChildByID((uint)ControlIds.TableBezel, false).ShadeColor = color;

        public void SetTitleText(string text) => (mModalDialogWindow.GetChildByID((uint)ControlIds.TitleText, false) as Text).Caption = text;

        public void SetTitleText(string text, Color textColor)
        {
            (mModalDialogWindow.GetChildByID((uint)ControlIds.TitleText, false) as Text).Caption = text;
            (mModalDialogWindow.GetChildByID((uint)ControlIds.TitleText, false) as Text).TextColor = textColor;
        }

        public void SetTitleText(string text, Color textColor, uint textStyle)
        {
            (mModalDialogWindow.GetChildByID((uint)ControlIds.TitleText, false) as Text).Caption = text;
            (mModalDialogWindow.GetChildByID((uint)ControlIds.TitleText, false) as Text).TextColor = textColor;
            (mModalDialogWindow.GetChildByID((uint)ControlIds.TitleText, false) as Text).TextStyle = textStyle;
        }

        public void SetTitleText(string text, Color textColor, bool textStyleBold)
        {
            (mModalDialogWindow.GetChildByID((uint)ControlIds.TitleText, false) as Text).Caption = text;
            (mModalDialogWindow.GetChildByID((uint)ControlIds.TitleText, false) as Text).TextColor = textColor;
            if (textStyleBold)
            {
                (mModalDialogWindow.GetChildByID((uint)ControlIds.TitleText, false) as Text).TextStyle = 2u;
            }
        }

        public void SetTitleTextColor(Color textColor) => (mModalDialogWindow.GetChildByID((uint)ControlIds.TitleText, false) as Text).TextColor = textColor;

        public void SetTitleTextStyle(uint textStyle) => (mModalDialogWindow.GetChildByID((uint)ControlIds.TitleText, false) as Text).TextStyle = textStyle;

        private void Repopulate()
        {
            if (mTable.RepopulateTable())
            {
                ResizeWindow();
            }
        }

        private void ResizeWindow()
        {
            Rect area = mModalDialogWindow.Parent.Area;
            float width = area.Width;
            float height = area.Height;
            int num = (int)height - (int)(mTableOffset.y * 2f);
            num /= (int)mTable.mTable.RowHeight;
            if (num > mTable.mTable.NumberRows)
            {
                num = mTable.mTable.NumberRows;
            }
            mTable.mTable.VisibleRows = (uint)num;
            mTable.mTable.GridSizeDirty = true;
            mTable.OnPopulationComplete();
            mModalDialogWindow.Area = new(mModalDialogWindow.Area.TopLeft, mModalDialogWindow.Area.TopLeft + mTable.TableArea.BottomRight + mTableOffset);
            Rect area2 = mModalDialogWindow.Area;
            float width2 = area2.Width;
            float height2 = area2.Height;
            float num2 = (float)Math.Round((width - width2) / 2f);
            float num3 = (float)Math.Round((height - height2) / 2f);
            area2.Set(num2, num3, num2 + width2, num3 + height2);
            mModalDialogWindow.Area = area2;
            Text text = mModalDialogWindow.GetChildByID((uint)ControlIds.TitleText, false) as Text;
            Rect area3 = text.Area;
            area3.Set(area3.TopLeft.x, 20f, area3.BottomRight.x, 50f - area2.Height);
            text.Area = area3;
            mModalDialogWindow.Visible = true;
        }

        private void OnRowClicked(List<RowInfo> _)
        {
            Audio.StartSound("ui_tertiary_button");
            EndDialog(OkayID);
        }

        private void OnCloseButtonClick(WindowBase sender, UIButtonClickEventArgs eventArgs)
        {
            eventArgs.Handled = true;
            EndDialog(CancelID);
        }

        private void OnOkayButtonClick(WindowBase sender, UIButtonClickEventArgs eventArgs)
        {
            eventArgs.Handled = true;
            EndDialog(OkayID);
        }

        public override void EndDialog(uint endID)
        {
            if (OnEnd(endID))
            {
                StopModal();
                Dispose();
            }
            mTable = null;
            mModalDialogWindow = null;
        }

        public override bool OnEnd(uint endID)
        {
            if (endID == OkayID)
            {
                EndDelegates?.Invoke(mTable.Selected);
                Result = mTable.Selected;
                Okay = true;
            }
            else
            {
                Result = null;
                Okay = false;
            }
            mTable.Populate(null, null, 0);
            EndDelegates = null;
            return true;
        }

        /// <summary>Creates and shows a new submenu from the given <see cref="MenuContainer"/>, invoking <see cref="MenuObject.OnActivation()"/> when a <see cref="MenuObject"/> is selected</summary>
        /// <param name="container">The <see cref="MenuContainer"/> used to generate the menu</param>
        /// <param name="showHeaders">Whether or not to show headers at the top of the menu table. Defaults to <see langword="true"/>.</param>
        /// <returns>
        ///     <para><see langword="true"/> to terminate the entire menu tree.</para>
        ///     <para><see langword="false"/> to return control to the invoker of the function.</para>
        /// </returns>
        /// <seealso cref="MenuObject.OnActivation()"/>
        public static bool ShowMenu(MenuContainer container, bool showHeaders = true) => ShowMenu(container, 0, showHeaders);

        /// <summary>Creates and shows a new submenu from the given <see cref="MenuContainer"/>, invoking <see cref="MenuObject.OnActivation()"/> when a <see cref="MenuObject"/> is selected</summary>
        /// <param name="container">The <see cref="MenuContainer"/> used to generate the menu</param>
        /// <param name="tab">The index of the tab that the submenu will open in</param>
        /// <param name="showHeaders">Whether or not to show headers at the top of the menu table. Defaults to <see langword="true"/>.</param>
        /// <returns>
        ///     <para><see langword="true"/> to terminate the entire menu tree.</para>
        ///     <para><see langword="false"/> to return control to the invoker of the function.</para>
        /// </returns>
        /// <seealso cref="MenuObject.OnActivation()"/>
        public static bool ShowMenu(MenuContainer container, int tab, bool showHeaders = true)
        {
            try
            {
                while (true)
                {
                    container.UpdateItems();
                    MenuController controller = Show(container, tab, showHeaders);
                    if (controller.Okay)
                    {
                        if (controller.Result?[0]?.Item is MenuObject menuObject)
                        {
                            if (menuObject.OnActivation())
                            {
                                return true;
                            }
                            continue;
                        }
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.sInstance.Log(ex);
                return true;
            }
        }

        private static MenuController Show(MenuContainer container, int tab, bool showHeaders)
        {
            Sims3.Gameplay.Gameflow.SetGameSpeed(Gameflow.GameSpeed.Pause, Sims3.Gameplay.Gameflow.SetGameSpeedContext.GameStates);
            MenuController menuController = new(container.MenuDisplayName, Localization.LocalizeString("Ui/Caption/Global:Ok"), Localization.LocalizeString("Ui/Caption/Global:Cancel"), container.TabInformation, container.Headers, showHeaders, container.OnEnd);
            menuController.SetTitleTextStyle(2u);
            if (tab >= 0)
            {
                if (tab < menuController.mTabsContainer.mTabs.Count)
                {
                    menuController.mTabsContainer.SelectTab(menuController.mTabsContainer.mTabs[tab]);
                }
                else
                {
                    menuController.mTabsContainer.SelectTab(menuController.mTabsContainer.mTabs[menuController.mTabsContainer.mTabs.Count - 1]);
                }
            }
            menuController.ShowModal();
            return menuController;
        }
    }
}
