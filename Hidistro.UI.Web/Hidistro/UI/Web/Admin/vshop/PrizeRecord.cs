namespace Hidistro.UI.Web.Admin.vshop
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class PrizeRecord : AdminPage
    {
        protected int activeid;
        protected Literal Litdesc;
        protected Literal LitTitle;
        protected Repeater rpMaterial;

        protected PrizeRecord() : base("", "")
        {
        }

        protected void BindPrizeList()
        {
            PrizeQuery page = new PrizeQuery {
                ActivityId = this.activeid
            };
            List<PrizeRecordInfo> prizeList = VShopHelper.GetPrizeList(page);
            if ((prizeList != null) && (prizeList.Count > 0))
            {
                this.LitTitle.Text = prizeList[0].ActivityName;
            }
            this.rpMaterial.DataSource = prizeList;
            this.rpMaterial.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (int.TryParse(base.Request.QueryString["id"], out this.activeid))
            {
                if (!this.Page.IsPostBack)
                {
                    this.BindPrizeList();
                }
            }
            else
            {
                this.ShowMsg("参数错误", false);
            }
        }
    }
}

