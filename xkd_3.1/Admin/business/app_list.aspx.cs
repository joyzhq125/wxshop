using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hidistro.Entities;
using Hidistro.Core.Urls;
using Hidistro.Core;
using Hidistro.UI.ControlPanel.Utility;

namespace XKD.Web.Admin
{
    public partial class app_list : AdminPage /*System.Web.UI.Page*/
    {
        protected string action = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.action = DTRequest.GetQueryString("action");
            if (this.action == "setting")
            {
                int id = DTRequest.GetQueryInt("id");

                SF.BLL.sf_website bll = new SF.BLL.sf_website();
                SF.Model.sf_website model = bll.GetModel(id);

                Session[DTKeys.SESSION_WEB_ID] = model.wid;
                //Session[DTKeys.SESSION_TEMPLATES_INFO] = model.templatesNum;
                //Response.Write("<script>parent.location.href='/admin/business/index.aspx';</script>");
                
                Response.Write("<script>parent.location.href='/admin/default.aspx';</script>");
                //Response.Redirect("/admin/business/index.aspx",true);
                Response.End();
                return;
            }

            RptBind("", "id desc");
        }

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {

            SF.BLL.sf_website bll = new SF.BLL.sf_website();
            System.Data.DataSet dsData = bll.GetList(" mid =  '" + GetAdminInfo().UserId + "'");
            dsData.Tables[0].Columns.Add("index");
            dsData.Tables[0].Columns.Add("index1");
            for (int i = 0; i < dsData.Tables[0].Rows.Count; i++)
            {
                dsData.Tables[0].Rows[i]["index"] = i;
                dsData.Tables[0].Rows[i]["index1"] = (i + 1);
            }

            this.rptList.DataSource = dsData;
            this.rptList.DataBind();

            //一个账号最多一个网站
            if (dsData.Tables[0].Rows.Count >=1)
            {
                divAdd.Visible = false;
            }
        }
        #endregion
    }
}