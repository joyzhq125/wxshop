namespace Hidistro.UI.Web.Admin
{
    using System;
    using System.Collections.Generic;

    internal class NavModule
    {
        internal string Class;
        internal string ID;
        internal bool IsDivide;
        internal Dictionary<string, NavItem> ItemList = new Dictionary<string, NavItem>();
        internal string Link;
        internal string Title;
    }
}

