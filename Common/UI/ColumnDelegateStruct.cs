namespace Gamefreak130.Common.UI
{
    using System;
    using static Sims3.UI.ObjectPicker;

    public struct ColumnDelegateStruct
    {
        public ColumnType mColumnType;

        public Func<ColumnInfo> mInfo;

        public ColumnDelegateStruct(ColumnType colType, Func<ColumnInfo> infoDelegate)
        {
            mColumnType = colType;
            mInfo = infoDelegate;
        }
    }
}
