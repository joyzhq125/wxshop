namespace Hidistro.UI.Web.Admin
{
    using System;
    using System.Collections.Generic;

    internal class NavItem
    {
        internal string Class;
        internal string ID;
        internal Dictionary<string, NavPageLink> PageLinks = new Dictionary<string, NavPageLink>();
        internal string SpanName;
    }
}

