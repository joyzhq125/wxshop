using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.ControlPanel.Store;
using Hidistro.UI.ControlPanel.Utility;

namespace XKD.Web.Admin
{
    public partial class business_edit : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ManagerInfo model = (ManagerInfo)Session[DTKeys.SESSION_ADMIN_INFO];
                if (model != null)
                {
                    ShowBusinessInfo(model);
                }
            }
        }

        private void ShowBusinessInfo(ManagerInfo bInfo)
        {
            txtBusinessName.Text = bInfo.realname;
            txtTelephone.Text = bInfo.telephone;
            txtEMail.Text = bInfo.Email;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            ManagerInfo model = (ManagerInfo)Session[DTKeys.SESSION_ADMIN_INFO];
            if (model != null)
            {
                //SF.BLL.manager bll = new SF.BLL.manager();
                //Model.manager temp_model = bll.GetModel(model.id);
                ManagerInfo temp_model = ManagerHelper.GetManager(model.UserId);
                temp_model.realname = txtBusinessName.Text;
                temp_model.telephone = txtTelephone.Text;
                temp_model.Email = txtEMail.Text;
                
                if (ManagerHelper.Update(temp_model) == true)
                {
                    Session[DTKeys.SESSION_ADMIN_INFO] = temp_model;
                    JscriptMsg("商户信息保存成功！", string.Empty);
                }
                else
                {
                    JscriptMsg("商户信息保存失败，请重新尝试！", string.Empty);
                }
            }
        }
    }
}