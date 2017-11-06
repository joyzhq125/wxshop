namespace Hidistro.ControlPanel.Settings
{
    using Hidistro.Entities.Settings;
    using Hidistro.SqlDal.Settings;
    using System;
    using System.Collections.Generic;

    public static class MenuHelper
    {
        public static bool CanAddMenu(string wid,int parentId)
        {
            IList<MenuInfo> menusByParentId = new MenuDao().GetMenusByParentId(wid,parentId);
            if ((menusByParentId == null) || (menusByParentId.Count == 0))
            {
                return true;
            }
            if (parentId == 0)
            {
                return (menusByParentId.Count < 4);
            }
            return (menusByParentId.Count < 5);
        }

        public static bool DeleteMenu(int menuId)
        {
            return new MenuDao().DeleteMenu(menuId);
        }

        public static MenuInfo GetMenu(int menuId)
        {
            return new MenuDao().GetMenu(menuId);
        }

        public static IList<MenuInfo> GetMenus(string wid)
        {
            IList<MenuInfo> list = new List<MenuInfo>();
            MenuDao dao = new MenuDao();
            IList<MenuInfo> topMenus = dao.GetTopMenus(wid);
            if (topMenus != null)
            {
                foreach (MenuInfo info in topMenus)
                {
                    IList<MenuInfo> menusByParentId = dao.GetMenusByParentId(wid,info.MenuId);
                    info.SubMenus = menusByParentId;
                    list.Add(info);
                }
            }
            return list;
        }

        public static IList<MenuInfo> GetMenusByParentId(string wid,int parentId)
        {
            return new MenuDao().GetMenusByParentId(wid,parentId);
        }

        public static IList<MenuInfo> GetTopMenus(string wid)
        {
            return new MenuDao().GetTopMenus(wid);
        }

        public static bool SaveMenu(MenuInfo menu)
        {
            return new MenuDao().SaveMenu(menu);
        }

        public static bool UpdateMenu(MenuInfo menu)
        {
            return new MenuDao().UpdateMenu(menu);
        }

        public static bool UpdateMenuName(MenuInfo menu)
        {
            return new MenuDao().UpdateMenuName(menu);
        }
    }
}

