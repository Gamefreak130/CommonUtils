namespace Gamefreak130.Common.UI
{
    using Sims3.SimIFace;

    public struct RowTextFormat
    {
        public Color mTextColor;

        public bool mBoldTextStyle;

        public string mTooltip;

        public RowTextFormat(Color textColor, bool boldText, string tooltipText)
        {
            mTextColor = textColor;
            mBoldTextStyle = boldText;
            mTooltip = tooltipText;
        }
    }
}
