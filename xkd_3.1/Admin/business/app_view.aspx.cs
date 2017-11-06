using Hidistro.Core.Urls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XKD.Web.Admin
{
    public partial class app_view : AdminPage
    {
        private int id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            id = DTRequest.GetQueryInt("id");

            if (!Page.IsPostBack)
            {
                ShowInfo(this.id);
            }
        }


        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            SF.BLL.sf_website bll = new SF.BLL.sf_website();
            SF.Model.sf_website model = bll.GetModel(_id);

            txtappid_name.Text = model.appid_name;
            txtappid_origin_id.Text = model.appid_origin_id;
            txtweixin_account.Text = model.weixin_account;
            txtavatar.Text = model.avatar;
            txtinterface_url.Text = model.interface_url;
            txttoken_value.Text = model.token_value;
            txtencodingaeskey.Text = model.encodingaeskey;
            txtappid.Text = model.appid;
            txtappsecret.Text = model.appsecret;

            txtpayment_name.Text = model.payment_name;
            cbstate.Checked = model.state == 1 ? true : false;
            txtweixin_pay_account.Text = model.weixin_pay_account;
            txtaccount_pay_key.Text = model.account_pay_key;
            rblsend_type.SelectedValue = model.send_type.ToString();
            txtlogo.Text = model.logo;
            txtdescription.Text = model.description;
        }
        #endregion
    }
}