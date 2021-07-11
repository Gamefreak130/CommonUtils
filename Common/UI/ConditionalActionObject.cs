namespace Gamefreak130.Common.UI
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// <para>A <see cref="MenuObject"/> that performs a predicate on activation.</para> 
    /// <para>If the predicate returns <see langword="true"/>, then the entire menu tree terminates; if it returns <see langword="false"/>, then control returns to the containing <see cref="MenuController"/></para>
    /// </summary>
    public class ConditionalActionObject : MenuObject
    {
        private readonly Func<bool> mPredicate;

        public ConditionalActionObject(string name, Func<bool> test, Func<bool> action) : base(name, test)
            => mPredicate = action;

        public ConditionalActionObject(string name, Func<string> getValue, Func<bool> test, Func<bool> action) : base(name, getValue, test)
            => mPredicate = action;

        public ConditionalActionObject(List<ColumnDelegateStruct> columns, Func<bool> test, Func<bool> action) : base(columns, test)
            => mPredicate = action;

        public override bool OnActivation() => mPredicate();
    }
}
