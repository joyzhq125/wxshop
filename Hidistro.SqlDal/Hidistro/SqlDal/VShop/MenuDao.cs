namespace Hidistro.SqlDal.VShop
{
    using Hidistro.Entities;
    using Hidistro.Entities.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class MenuDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool DeleteMenu(int menuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE vshop_Menu WHERE MenuId = @MenuId");
            this.database.AddInParameter(sqlStringCommand, "MenuId", DbType.Int32, menuId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        private int GetAllMenusCount(string wid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(*) from vshop_Menu where wid=@wid");
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            return (1 + Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)));
        }

        public MenuInfo GetMenu(int menuId,string wid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_Menu WHERE MenuId = @MenuId and wid=@wid");
            this.database.AddInParameter(sqlStringCommand, "MenuId", DbType.Int32, menuId);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<MenuInfo>(reader);
            }
        }

        public IList<MenuInfo> GetMenusByParentId(int parentId,string wid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_Menu WHERE ParentMenuId = @ParentMenuId  and wid=@wid ORDER BY DisplaySequence ASC");
            this.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, parentId);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<MenuInfo>(reader);
            }
        }

        public IList<MenuInfo> GetTopMenus(string wid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_Menu WHERE ParentMenuId = 0 and wid =@wid ORDER BY DisplaySequence ASC");
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, wid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<MenuInfo>(reader);
            }
        }

        public bool SaveMenu(MenuInfo menu)
        {
            int allMenusCount = this.GetAllMenusCount(menu.wid);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO vshop_Menu (wid,ParentMenuId, Name, Type, ReplyId, DisplaySequence, Bind, [Content]) VALUES(@wid,@ParentMenuId, @Name, @Type, @ReplyId, @DisplaySequence, @Bind, @Content)");
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, menu.wid);
            this.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, menu.ParentMenuId);
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, menu.Name);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.String, menu.Type);
            this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, menu.ReplyId);
            this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, allMenusCount);
            this.database.AddInParameter(sqlStringCommand, "Bind", DbType.Int32, (int) menu.BindType);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, menu.Content);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public void SwapMenuSequence(int menuId, bool isUp,string wid)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Menu_SwapDisplaySequence");
            this.database.AddInParameter(storedProcCommand, "MenuId", DbType.Int32, menuId);
            this.database.AddInParameter(storedProcCommand, "ZIndex", DbType.Int32, isUp ? 0 : 1);
            this.database.AddInParameter(storedProcCommand, "wid", DbType.String, wid);
            this.database.ExecuteNonQuery(storedProcCommand);
        }

        public bool UpdateMenu(MenuInfo menu)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE vshop_Menu SET ParentMenuId = @ParentMenuId, Name = @Name, Type = @Type, ReplyId = @ReplyId, DisplaySequence = @DisplaySequence, Bind = @Bind, [Content] = @Content WHERE MenuId = @MenuId");
            this.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, menu.ParentMenuId);
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, menu.Name);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.String, menu.Type);
            this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, menu.ReplyId);
            this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, menu.DisplaySequence);
            this.database.AddInParameter(sqlStringCommand, "MenuId", DbType.Int32, menu.MenuId);
            this.database.AddInParameter(sqlStringCommand, "Bind", DbType.Int32, (int) menu.BindType);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, menu.Content);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }
    }
}

