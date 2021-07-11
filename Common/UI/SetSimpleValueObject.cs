namespace Gamefreak130.Common.UI
{
    using Sims3.UI;
    using System;
    using System.Collections.Generic;
    using static Sims3.UI.ObjectPicker;

    /// <summary>
    /// <para>A <see cref="MenuObject"/> that prompts the user to enter a new string value for a given <typeparamref name="T"/> (or toggles a boolean value).</para> 
    /// <para>Control is returned to the containing <see cref="MenuController"/>, regardless of the result of toggling or converting to <typeparamref name="T"/></para>
    /// </summary>
    /// <typeparam name="T">The type of the value to set</typeparam>
    public abstract class SetSimpleValueObject<T> : MenuObject where T : IConvertible
    {
        protected readonly string mMenuTitle;

        protected readonly string mDialogPrompt;

        protected Func<T> mGetValue;

        protected Action<T> mSetValue;

        public SetSimpleValueObject(string menuTitle, string dialogPrompt, Func<bool> test) : this(menuTitle, dialogPrompt, new(), test)
        {
        }

        public SetSimpleValueObject(string menuTitle, string dialogPrompt, List<ColumnDelegateStruct> columns, Func<bool> test) : base(columns, test)
        {
            mMenuTitle = menuTitle;
            mDialogPrompt = dialogPrompt;
        }

        protected void ConstructDefaultColumnInfo()
        {
            mColumnActions = new()
            {
                new(ColumnType.kText, () => new TextColumn(mMenuTitle)),
                new(ColumnType.kText, () => new TextColumn(mGetValue().ToString()))
            };
            PopulateColumnInfo();
            Fillin();
        }

        public override bool OnActivation()
        {
            try
            {
                Type t = typeof(T);
                T val = default;
                if (t == typeof(bool))
                {
                    // Holy boxing Batman
                    val = (T)(object)!(bool)(object)mGetValue();
                }
                else
                {
                    string str = StringInputDialog.Show(mMenuTitle, mDialogPrompt, mGetValue().ToString());
                    if (str is not null)
                    {
                        val = t.IsEnum ? (T)Enum.Parse(t, str) : (T)Convert.ChangeType(str, t);
                    }
                }

                if (val is not null)
                {
                    mSetValue(val);
                }
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }
            catch (ArgumentException)
            {
            }
            return false;
        }
    }

}
