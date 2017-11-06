namespace Hidistro.UI.ControlPanel.Utility
{
    using Entities.Store;
    using Hidistro.Core;
    using Hidistro.UI.Common.Controls;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;

    public class AdminPage : Page
    {
        public string wid = string.Empty;
        public AdminPage()
        {
            //wid = GetCurWebId();
        }

        protected AdminPage(string moduleId, string pageId) : this(moduleId, pageId, null)
        {
        }
        protected AdminPage(string moduleId, string pageId, string subPageId)
        {
            this.ModuleId = moduleId;
            this.PageId = pageId;
            this.SubPageId = subPageId;
            //wid = GetCurWebId();
        }

        private void CheckPageAccess()
        {
            if (Globals.GetCurrentManagerUserId() == 0)
            {
                this.Page.Response.Redirect(Globals.ApplicationPath + "/admin/Login.aspx", true);
            }
        }

        protected virtual void CloseWindow()
        {
            string str = "var win = art.dialog.open.origin; win.location.reload();";
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>" + str + "</script>");
            }
        }

        protected string CutWords(object obj, int length)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            string str = obj.ToString();
            if (str.Length > length)
            {
                return (str.Substring(0, length) + "......");
            }
            return str;
        }

        private string GenericReloadUrl(NameValueCollection queryStrings)
        {
            if ((queryStrings == null) || (queryStrings.Count == 0))
            {
                return base.Request.Url.AbsolutePath;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(base.Request.Url.AbsolutePath).Append("?");
            foreach (string str2 in queryStrings.Keys)
            {
                string str = queryStrings[str2].Trim().Replace("'", "");
                if (!string.IsNullOrEmpty(str) && (str.Length > 0))
                {
                    builder.Append(str2).Append("=").Append(base.Server.UrlEncode(str)).Append("&");
                }
            }
            queryStrings.Clear();
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        public decimal GetFieldDecimalValue(DataRow drOne, string FieldName)
        {
            if (((drOne == null) || drOne.Table.Columns.Contains(FieldName)) && (((drOne != null) && drOne.Table.Columns.Contains(FieldName)) && !string.IsNullOrEmpty(drOne[FieldName].ToString())))
            {
                return Convert.ToDecimal(drOne[FieldName].ToString());
            }
            return 0M;
        }

        public int GetFieldIntValue(DataRow drOne, string FieldName)
        {
            if (((drOne == null) || drOne.Table.Columns.Contains(FieldName)) && ((drOne != null) && !string.IsNullOrEmpty(drOne[FieldName].ToString())))
            {
                return int.Parse(drOne[FieldName].ToString());
            }
            return 0;
        }

        public string GetFieldValue(DataRow drOne, string FieldName)
        {
            if (((drOne == null) || drOne.Table.Columns.Contains(FieldName)) && ((drOne != null) && (drOne[FieldName] != null)))
            {
                return drOne[FieldName].ToString();
            }
            return "";
        }

        protected int GetFormIntParam(string name)
        {
            string str = base.Request.Form.Get(name);
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            try
            {
                return Convert.ToInt32(str);
            }
            catch (FormatException)
            {
                return 0;
            }
        }

        protected bool GetUrlBoolParam(string name)
        {
            string str = base.Request.QueryString.Get(name);
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            try
            {
                return Convert.ToBoolean(str);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        protected int GetUrlIntParam(string name)
        {
            string str = base.Request.QueryString.Get(name);
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            try
            {
                return Convert.ToInt32(str);
            }
            catch (FormatException)
            {
                return 0;
            }
        }

        protected string GetUrlParam(string name)
        {
            string str = base.Request.QueryString.Get(name);
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return str;
        }

        protected void GotoResourceNotFound()
        {
            base.Response.Redirect(Globals.GetAdminAbsolutePath("ResourceNotFound.aspx"));
        }

        protected override void OnInit(EventArgs e)
        {
            if (ConfigurationManager.AppSettings["Installer"] != null)
            {
                base.Response.Redirect(Globals.ApplicationPath + "/installer/default.aspx", false);
            }
            else
            {
                this.CheckPageAccess();
                base.OnInit(e);
            }
        }

        protected void ReloadPage(NameValueCollection queryStrings)
        {
            base.Response.Redirect(this.GenericReloadUrl(queryStrings));
        }

        protected void ReloadPage(NameValueCollection queryStrings, bool endResponse)
        {
            base.Response.Redirect(this.GenericReloadUrl(queryStrings), endResponse);
        }

        protected virtual void ShowMsg(ValidationResults validateResults)
        {
            StringBuilder builder = new StringBuilder();
            foreach (ValidationResult result in (IEnumerable<ValidationResult>) validateResults)
            {
                builder.Append(Formatter.FormatErrorMessage(result.Message));
            }
            this.ShowMsg(builder.ToString(), false);
        }

        protected virtual void ShowMsg(string msg, bool success)
        {
            string str = string.Format("HiTipsShow(\"{0}\", {1})", msg, success ? "'success'" : "'error'");
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
            }
        }

        protected virtual void ShowMsgAndReUrl(string msg, bool success, string url)
        {
            string str = string.Format("ShowMsgAndReUrl(\"{0}\", {1}, \"{2}\")", msg, success ? "true" : "false", url);
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
            }
        }

        protected virtual void ShowMsgAndReUrl(string msg, bool success, string url, string target)
        {
            string str = string.Format("ShowMsgAndReUrl(\"{0}\", {1}, \"{2}\",\"{3}\")", new object[] { msg, success ? "true" : "false", url, target });
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
            }
        }

        protected virtual void ShowMsgToTarget(string msg, bool success, string targentname)
        {
            string str3;
            string str = string.Empty;
            if (((str3 = targentname.ToLower()) != null) && ((str3 == "parent") || (str3 == "top")))
            {
                str = targentname + ".";
            }
            string str2 = string.Format("{2}HiTipsShow(\"{0}\", {1})", msg, success ? "'success'" : "'error'", str);
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str2 + "},300);</script>");
            }
        }
        #region JS提示============================================
        /// <summary>
        /// 添加编辑删除提示
        /// </summary>
        /// <param name="msgtitle">提示文字</param>
        /// <param name="url">返回地址</param>
        protected void JscriptMsg(string msgtitle, string url)
        {
            string msbox = "parent.jsprint(\"" + msgtitle + "\", \"" + url + "\")";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "JsPrint", msbox, true);
        }
        /// <summary>
        /// 添加编辑删除提示
        /// </summary>
        /// <param name="title">提示文字</param>
        /// <param name="msgtitle">提示文字</param>
        /// <param name="url">返回地址</param>
        protected void JscriptShow(string title, string msgtitle, string url)
        {
            string msbox = "parent.jsdialogshow(\"" + title + "\",\"" + msgtitle + "\", \"" + url + "\")";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "JsShow", msbox, true);
        }
        /// <summary>
        /// 带回传函数的添加编辑删除提示
        /// </summary>
        /// <param name="msgtitle">提示文字</param>
        /// <param name="url">返回地址</param>
        /// <param name="callback">JS回调函数</param>
        //protected void JscriptMsg(string msgtitle, string url, string callback)
        //{
        //    string msbox = "parent.jsprint(\"" + msgtitle + "\", \"" + url + "\", " + callback + ")";
        //    ClientScript.RegisterClientScriptBlock(Page.GetType(), "JsPrint", msbox, true);
        //}
        #endregion


        #region JS提示============================================
        /// <summary>
        /// 添加编辑删除提示
        /// </summary>
        /// <param name="msgtitle">提示文字</param>
        /// <param name="url">返回地址</param>
        /// <param name="msgcss">CSS样式</param>
        protected void JscriptMsg(string msgtitle, string url, string msgcss)
        {
            //string msbox = "parent.jsprint(\"" + msgtitle + "\", \"" + url + "\", \"" + msgcss + "\")";
            //ClientScript.RegisterClientScriptBlock(Page.GetType(), "JsPrint", msbox, true);
            ShowMsgAndReUrl(msgtitle,msgcss.ToLower()=="success"?true:false,url);
        }

        /// <summary>
        /// 带回传函数的添加编辑删除提示
        /// </summary>
        /// <param name="msgtitle">提示文字</param>
        /// <param name="url">返回地址</param>
        /// <param name="msgcss">CSS样式</param>
        /// <param name="callback">JS回调函数</param>
        protected void JscriptMsg(string msgtitle, string url, string msgcss, string callback)
        {
            string msbox = "parent.jsprint(\"" + msgtitle + "\", \"" + url + "\", \"" + msgcss + "\", " + callback + ")";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "JsPrint", msbox, true);
        }
        #endregion
        public string ModuleId { get; private set; }

        public string PageId { get; private set; }

        public string SubPageId { get; private set; }


        #region 管理员============================================
        /// <summary>
        /// 判断管理员是否已经登录(解决Session超时问题)
        /// </summary>
        public bool IsAdminLogin()
        {
            //如果Session为Null
            if (Session[DTKeys.SESSION_ADMIN_INFO] != null)
            {
                return true;
            }
            else
            {
                /*
                //检查Cookies
                string adminname = SFUtils.GetCookie("AdminName", "DTcms");
                string adminpwd = SFUtils.GetCookie("AdminPwd", "DTcms");
                if (adminname != "" && adminpwd != "")
                {
                    BLL.manager bll = new BLL.manager();
                    Model.manager model = bll.GetModel(adminname, adminpwd);
                    if (model != null)
                    {
                        Session[DTKeys.SESSION_ADMIN_INFO] = model;
                        return true;
                    }
                }
                */
            }
            Response.Redirect("/admin/login.aspx");
            return false;
        }

        /// <summary>
        /// 取得管理员信息
        /// </summary>
        public ManagerInfo GetAdminInfo()
        {
            if (IsAdminLogin())
            {
                ManagerInfo model = Session[DTKeys.SESSION_ADMIN_INFO] as ManagerInfo;
                if (model != null)
                {
                    return model;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取当前网站ID
        /// </summary>
        /// <returns></returns>
        public string GetCurWebId()
        {
            if (IsAdminLogin())
            {
                wid = Session[DTKeys.SESSION_WEB_ID] as string;
                if (!string.IsNullOrEmpty(wid))
                {
                    return wid;
                }
            }
            return null;
        }
        /// <summary>
        /// 检查管理员权限
        /// </summary>
        /// <param name="nav_name">菜单名称</param>
        /// <param name="action_type">操作类型</param>
        public void ChkAdminLevel(string nav_name, string action_type)
        {
            /*
            ManagerInfo model = GetAdminInfo();
            BLL.manager_role bll = new BLL.manager_role();
            bool result = bll.Exists(model.role_id, nav_name, action_type);

            if (!result)
            {
                string msgbox = "parent.jsdialog(\"错误提示\", \"您没有管理该页面的权限，请勿非法进入！\", \"back\")";
                Response.Write("<script type=\"text/javascript\">" + msgbox + "</script>");
                Response.End();
            }
            */
        }

        /// <summary>
        /// 写入管理日志
        /// </summary>
        /// <param name="action_type"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool AddAdminLog(string action_type, string remark)
        {
            /*
            if (siteConfig.logstatus > 0)
            {
                Model.manager model = GetAdminInfo();
                int newId = new BLL.manager_log().Add(model.id, model.user_name, action_type, remark);
                if (newId > 0)
                {
                    return true;
                }
            }
            */
            return false;
        }

        #endregion
    }
}

