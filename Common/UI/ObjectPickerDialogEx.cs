namespace Gamefreak130.Common.UI
{
    using Sims3.SimIFace;
    using Sims3.UI;
    using System.Collections.Generic;
    using static Sims3.UI.ObjectPicker;

    /// <summary>
    /// An <see cref="ObjectPickerDialog"/> which allows for the okay button to be clicked with no items selected, in which case an empty RowInfo list is returned
    /// </summary>
    public class ObjectPickerDialogEx : ObjectPickerDialog
    {
        public ObjectPickerDialogEx(bool modal, PauseMode pauseMode, string title, string buttonTrue, string buttonFalse, List<TabInfo> listObjs, List<HeaderInfo> headers, int numSelectableRows, Vector2 position, bool viewTypeToggle, List<RowInfo> preSelectedRows, bool showHeadersAndToggle, bool disableCloseButton)
            : base(modal, pauseMode, title, buttonTrue, buttonFalse, listObjs, headers, numSelectableRows, position, viewTypeToggle, preSelectedRows, showHeadersAndToggle, disableCloseButton)
        {
            mOkayButton.Enabled = true;
            mTable.ObjectTable.TableChanged -= OnTableChanged;
            mTable.SelectionChanged -= OnSelectionChanged;
            mTable.SelectionChanged += OnSelectionChangedEx;
            mTable.RowSelected -= OnSelectionChanged;
            mTable.RowSelected += OnSelectionChangedEx;
            mTable.Selected = preSelectedRows;
        }

        new public static List<RowInfo> Show(bool modal, PauseMode pauseType, string title, string buttonTrue, string buttonFalse, List<TabInfo> listObjs, List<HeaderInfo> headers, int numSelectableRows, Vector2 position, bool viewTypeToggle, List<RowInfo> preSelectedRows, bool showHeadersAndToggle, bool disableCloseButton)
        {
            using (ObjectPickerDialogEx objectPickerDialog = new(modal, pauseType, title, buttonTrue, buttonFalse, listObjs, headers, numSelectableRows, position, viewTypeToggle, preSelectedRows, showHeadersAndToggle, disableCloseButton))
            {
                objectPickerDialog.StartModal();
                return objectPickerDialog.Result;
            }
        }

        public override bool OnEnd(uint endID)
        {
            if (endID == OkayID)
            {
                if (!mOkayButton.Enabled)
                {
                    return false;
                }
                mResult = mTable.Selected ?? new();
            }
            else
            {
                mResult = null;
            }
            mTable.Populate(null, null, 0);
            return true;
        }

        private void OnSelectionChangedEx(List<RowInfo> _) => Audio.StartSound("ui_tertiary_button");
    }
}
