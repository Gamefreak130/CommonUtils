namespace Gamefreak130.Common.UI
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A <see cref="MenuObject"/> that creates and shows a new submenu from a given <see cref="MenuContainer"/> on activation
    /// </summary>
    public sealed class GenerateMenuObject : MenuObject
    {
        private readonly MenuContainer mToOpen;

        public GenerateMenuObject(string name, Func<bool> test, MenuContainer toOpen) : base(name, test)
            => mToOpen = toOpen;

        public GenerateMenuObject(List<ColumnDelegateStruct> columns, Func<bool> test, MenuContainer toOpen) : base(columns, test)
            => mToOpen = toOpen;

        public override bool OnActivation() => MenuController.ShowMenu(mToOpen);
    }
}
