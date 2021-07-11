namespace Gamefreak130.Common.UI
{
    using System;
    using System.Collections.Generic;
    using static Sims3.UI.ObjectPicker;

    /// <summary>
    /// Used by <see cref="MenuController"/> to construct menus using <see cref="MenuObject"/>s with arbitrary functionality
    /// </summary>
    /// <seealso cref="MenuController"/>
    public class MenuContainer
    {
        private List<RowInfo> mRowInformation;

        private readonly string[] mTabImage;

        private readonly string[] mTabText;

        private readonly Func<List<RowInfo>> mRowPopulationDelegate;

        private readonly List<RowInfo> mHiddenRows;

        public string MenuDisplayName { get; }

        public List<HeaderInfo> Headers { get; private set; }

        public List<TabInfo> TabInformation { get; private set; }

        public Action<List<RowInfo>> OnEnd { get; }

        public MenuContainer() : this("")
        {
        }

        public MenuContainer(string title) : this(title, "")
        {
        }

        public MenuContainer(string title, string subtitle) : this(title, new[] { "" }, new[] { subtitle }, null)
        {
        }

        public MenuContainer(string title, string[] tabImage, string[] tabName, Action<List<RowInfo>> onEndDelegate) : this(title, tabImage, tabName, onEndDelegate, null)
        {
        }

        public MenuContainer(string title, string[] tabImage, string[] tabName, Action<List<RowInfo>> onEndDelegate, Func<List<RowInfo>> rowPopulationDelegate)
        {
            mHiddenRows = new();
            MenuDisplayName = title;
            mTabImage = tabImage;
            mTabText = tabName;
            OnEnd = onEndDelegate;
            Headers = new();
            mRowInformation = new();
            TabInformation = new();
            mRowPopulationDelegate = rowPopulationDelegate;
            if (mRowPopulationDelegate is not null)
            {
                RefreshMenuObjects(0);
                if (mRowInformation.Count > 0)
                {
                    for (int i = 0; i < mRowInformation[0].ColumnInfo.Count; i++)
                    {
                        Headers.Add(new("Ui/Caption/ObjectPicker:Sim", "", 200));
                    }
                }
            }
        }

        public void RefreshMenuObjects(int tabnumber)
        {
            mRowInformation = mRowPopulationDelegate();
            TabInformation = new()
            {
                new("", mTabText[tabnumber], mRowInformation)
            };
        }

        public void SetHeaders(List<HeaderInfo> headers) => Headers = headers;

        public void SetHeader(int headerNumber, HeaderInfo headerInfos) => Headers[headerNumber] = headerInfos;

        public void ClearMenuObjects() => TabInformation.Clear();

        public void AddMenuObject(MenuObject menuItem)
        {
            if (TabInformation.Count < 1)
            {
                mRowInformation = new()
                {
                    menuItem.RowInformation
                };
                TabInformation.Add(new(mTabImage[0], mTabText[0], mRowInformation));
                Headers.Add(new("Ui/Caption/ObjectPicker:Name", "", 300));
                Headers.Add(new("Ui/Caption/ObjectPicker:Value", "", 100));
                return;
            }
            TabInformation[0].RowInfo.Add(menuItem.RowInformation);
        }

        public void AddMenuObject(List<HeaderInfo> headers, MenuObject menuItem)
        {

            if (TabInformation.Count < 1)
            {
                mRowInformation = new()
                {
                    menuItem.RowInformation
                };
                TabInformation.Add(new(mTabImage[0], mTabText[0], mRowInformation));
                Headers = headers;
                return;
            }
            TabInformation[0].RowInfo.Add(menuItem.RowInformation);
            Headers = headers;
        }

        public void AddMenuObject(List<HeaderInfo> headers, RowInfo item)
        {
            if (TabInformation.Count < 1)
            {
                mRowInformation = new()
                {
                    item
                };
                TabInformation.Add(new(mTabImage[0], mTabText[0], mRowInformation));
                Headers = headers;
                return;
            }
            TabInformation[0].RowInfo.Add(item);
            Headers = headers;
        }

        public void UpdateRows()
        {
            for (int i = mHiddenRows.Count - 1; i >= 0; i--)
            {
                MenuObject item = mHiddenRows[i].Item as MenuObject;
                if (item.Test())
                {
                    mHiddenRows.RemoveAt(i);
                    AddMenuObject(item);
                }
            }
            for (int i = TabInformation[0].RowInfo.Count - 1; i >= 0; i--)
            {
                MenuObject item = TabInformation[0].RowInfo[i].Item as MenuObject;
                if (item.Test is not null && !item.Test())
                {
                    mHiddenRows.Add(TabInformation[0].RowInfo[i]);
                    TabInformation[0].RowInfo.RemoveAt(i);
                }
            }
        }

        public void UpdateItems()
        {
            UpdateRows();
            foreach (TabInfo current in TabInformation)
            {
                foreach (RowInfo current2 in current.RowInfo)
                {
                    (current2.Item as MenuObject)?.UpdateMenuObject();
                }
            }
        }
    }
}
