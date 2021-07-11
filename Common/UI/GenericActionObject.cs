namespace Gamefreak130.Common.UI
{
    using Sims3.UI;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A <see cref="MenuObject"/> that performs a one-shot function before returning control to the containing <see cref="MenuController"/>
    /// </summary>
    public class GenericActionObject : MenuObject
    {
        protected readonly Function mCallback;

        public GenericActionObject(string name, Func<bool> test, Function action) : base(name, test)
            => mCallback = action;

        public GenericActionObject(string name, Func<string> getValue, Func<bool> test, Function action) : base(name, getValue, test)
            => mCallback = action;

        public GenericActionObject(List<ColumnDelegateStruct> columns, Func<bool> test, Function action) : base(columns, test)
            => mCallback = action;

        public override bool OnActivation()
        {
            mCallback();
            return false;
        }
    }
}
