namespace Gamefreak130.Common.UI
{
    using Sims3.SimIFace;
    using System;
    using System.Collections.Generic;
    using static Sims3.UI.ObjectPicker;

    /// <summary>
    /// Represents a single item within a <see cref="MenuController"/> dialog with arbitrary behavior upon selection
    /// </summary>
    public abstract class MenuObject : IDisposable
    {
        private List<ColumnInfo> mColumnInfoList;

        protected List<ColumnDelegateStruct> mColumnActions;

        private RowTextFormat mTextFormat;

        public Func<bool> Test { get; protected set; }

        public RowInfo RowInformation { get; private set; }

        public MenuObject() : this(new List<ColumnDelegateStruct>(), null)
        {
        }

        public MenuObject(List<ColumnDelegateStruct> columns, Func<bool> test)
        {
            mColumnInfoList = new();
            mColumnActions = columns;
            Test = test;
            PopulateColumnInfo();
            Fillin();
        }

        public MenuObject(string name, Func<bool> test) : this(name, () => "", test)
        {
        }

        public MenuObject(string name, Func<string> getValue, Func<bool> test)
        {
            mColumnInfoList = new();
            Test = test;
            mColumnActions = new()
            {
                new(ColumnType.kText, () => new TextColumn(name)),
                new(ColumnType.kText, () => new TextColumn(getValue()))
            };
            PopulateColumnInfo();
            Fillin();
        }

        public void Fillin() => RowInformation = new(this, mColumnInfoList);

        public void Fillin(Color textColor)
        {
            mTextFormat.mTextColor = textColor;
            Fillin();
        }

        public void Fillin(Color textColor, bool boldTextStyle)
        {
            mTextFormat.mTextColor = textColor;
            Fillin(boldTextStyle);
        }

        public void Fillin(bool boldTextStyle)
        {
            mTextFormat.mBoldTextStyle = boldTextStyle;
            Fillin();
        }

        public void Fillin(string tooltipText)
        {
            mTextFormat.mTooltip = tooltipText;
            Fillin();
        }

        public void Dispose()
        {
            RowInformation = null;
            mColumnInfoList.Clear();
            mColumnInfoList = null;
        }

        public virtual void PopulateColumnInfo()
        {
            foreach (ColumnDelegateStruct column in mColumnActions)
            {
                mColumnInfoList.Add(column.mInfo());
            }
        }

        public virtual void AdaptToMenu(TabInfo tabInfo)
        {
        }

        /// <summary>Callback method raised by <see cref="MenuController"/> when a <see cref="MenuObject"/> is selected</summary>
        /// <returns><see langword="true"/> if entire menu tree should be termined; otherwise, <see langword="false"/> to return control to the containing <see cref="MenuController"/></returns>
        /// <seealso cref="MenuController.ShowMenu(MenuContainer)"/>
        public virtual bool OnActivation() => true;

        public void UpdateMenuObject()
        {
            for (int i = 0; i < mColumnInfoList.Count; i++)
            {
                mColumnInfoList[i] = mColumnActions[i].mInfo();
            }
            Fillin();
        }
    }
}
