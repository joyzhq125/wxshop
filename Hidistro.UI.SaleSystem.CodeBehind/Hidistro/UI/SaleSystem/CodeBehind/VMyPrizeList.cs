namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMyPrizeList : VMemberTemplatedWebControl
    {
        private VshopTemplatedRepeater rptList;
        private HtmlSelect txtRevAddress;
        private HtmlInputHidden txtShowTabNum;
        private HtmlInputHidden txtTotal;

        protected override void AttachChildControls()
        {
            int num;
            int num2;
            PageTitle.AddSiteNameTitle("我的奖品");
            this.rptList = (VshopTemplatedRepeater) this.FindControl("rptList");
            this.txtTotal = (HtmlInputHidden) this.FindControl("txtTotal");
            this.txtShowTabNum = (HtmlInputHidden) this.FindControl("txtShowTabNum");
            HtmlSelect select = (HtmlSelect) this.FindControl("txtRevAddress");
            IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses();
            select.Items.Clear();
            foreach (ShippingAddressInfo info in shippingAddresses)
            {
                string str = "";
                string text = "";
                str = string.Concat(new object[] { info.ShipTo, "|", info.CellPhone, "|", info.RegionId, "|", info.Address });
                text = info.ShipTo + " " + info.CellPhone + " " + RegionHelper.GetFullRegion(info.RegionId, ",") + " " + info.Address;
                select.Items.Add(new ListItem(text, str));
            }
            select.Items.Add(new ListItem("--新增收货地址--", "addNew"));
            string str3 = Globals.RequestQueryStr("ShowTab");
            if (string.IsNullOrEmpty(str3))
            {
                str3 = "0";
            }
            PrizesDeliveQuery entity = new PrizesDeliveQuery();
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 20;
            }
            string extendLimits = "";
            if (str3 == "0")
            {
                extendLimits = " and status in(0,1,2) ";
            }
            else
            {
                extendLimits = " and status=3 ";
            }
            this.txtShowTabNum.Value = str3;
            entity.Status = -1;
            entity.SortBy = "LogId";
            entity.SortOrder = SortAction.Desc;
            entity.PageIndex = num;
            entity.PrizeType = -1;
            entity.PageSize = num2;
            entity.UserId = Globals.GetCurrentMemberUserId();
            Globals.EntityCoding(entity, true);
            DbQueryResult result = GameHelper.GetPrizesDeliveryList(entity, extendLimits, "*");
            this.rptList.DataSource = result.Data;
            this.rptList.DataBind();
            this.txtTotal.SetWhenIsNotNull(result.TotalRecords.ToString());
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vMyPrizeList.html";
            }
            base.OnInit(e);
        }
    }
}

