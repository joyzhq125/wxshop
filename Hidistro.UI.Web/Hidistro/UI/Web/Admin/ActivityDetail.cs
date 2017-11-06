namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class ActivityDetail : AdminPage
    {
        protected ActivityInfo _act;
        protected Repeater rpt;

        protected ActivityDetail() : base("", "")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                int urlIntParam = base.GetUrlIntParam("id");
                ActivityInfo activity = VShopHelper.GetActivity(urlIntParam);
                if (activity != null)
                {
                    this._act = activity;
                    this.rpt.DataSource = VShopHelper.GetActivitySignUpById(urlIntParam);
                    this.rpt.DataBind();
                }
            }
        }
    }
}

