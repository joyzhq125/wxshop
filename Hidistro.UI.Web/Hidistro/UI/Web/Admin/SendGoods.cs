namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.Messages;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Plugins;
    using Hishop.Weixin.Pay;
    using Hishop.Weixin.Pay.Domain;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.OrderSendGoods)]
    public class SendGoods : AdminPage
    {
        protected Button btnSendGoods;
        protected ExpressRadioButtonList expressRadioButtonList;
        private string goUrl;
        protected Order_ItemsList itemsList;
        protected Label lblOrderId;
        protected FormatedTimeLabel lblOrderTime;
        protected Literal litReceivingInfo;
        protected Label litRemark;
        protected Literal litShippingModeName;
        protected Label litShipToDate;
        private string orderId;
        protected ShippingModeRadioButtonList radioShippingMode;
        protected TextBox txtShipOrderNumber;
        protected HtmlGenericControl txtShipOrderNumberTip;
        protected string wid;
        protected SendGoods()
            : base("", "")
        {
            this.goUrl = string.Empty;
        }

        private void BindExpressCompany(int modeId)
        {
            this.expressRadioButtonList.ExpressCompanies = SalesHelper.GetExpressCompanysByMode(modeId);
            this.expressRadioButtonList.DataBind();
        }

        private void BindOrderItems(OrderInfo order)
        {
            this.lblOrderId.Text = order.OrderId;
            this.lblOrderTime.Time = order.OrderDate;
            this.itemsList.Order = order;
        }

        private void BindShippingAddress(OrderInfo order)
        {
            string shippingRegion = string.Empty;
            if (!string.IsNullOrEmpty(order.ShippingRegion))
            {
                shippingRegion = order.ShippingRegion;
            }
            if (!string.IsNullOrEmpty(order.Address))
            {
                shippingRegion = shippingRegion + order.Address;
            }
            if (!string.IsNullOrEmpty(order.ShipTo))
            {
                shippingRegion = shippingRegion + "  " + order.ShipTo;
            }
            if (!string.IsNullOrEmpty(order.ZipCode))
            {
                shippingRegion = shippingRegion + "  " + order.ZipCode;
            }
            if (!string.IsNullOrEmpty(order.TelPhone))
            {
                shippingRegion = shippingRegion + "  " + order.TelPhone;
            }
            if (!string.IsNullOrEmpty(order.CellPhone))
            {
                shippingRegion = shippingRegion + "  " + order.CellPhone;
            }
            this.litReceivingInfo.Text = shippingRegion;
        }

        private void btnSendGoods_Click(object sender, EventArgs e)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
            if (orderInfo != null)
            {
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
                if (currentManager != null)
                {
                    if ((orderInfo.GroupBuyId > 0) && (orderInfo.GroupBuyStatus != GroupBuyStatus.Success))
                    {
                        this.ShowMsg("当前订单为团购订单，团购活动还未成功结束，所以不能发货", false);
                    }
                    else if (!orderInfo.CheckAction(OrderActions.SELLER_SEND_GOODS))
                    {
                        this.ShowMsg("当前订单状态没有付款或不是等待发货的订单，所以不能发货", false);
                    }
                    else if (!this.radioShippingMode.SelectedValue.HasValue)
                    {
                        this.ShowMsg("请选择配送方式", false);
                    }
                    else if (string.IsNullOrEmpty(this.txtShipOrderNumber.Text.Trim()) || (this.txtShipOrderNumber.Text.Trim().Length > 20))
                    {
                        this.ShowMsg("运单号码不能为空，在1至20个字符之间", false);
                    }
                    else if (string.IsNullOrEmpty(this.expressRadioButtonList.SelectedValue))
                    {
                        this.ShowMsg("请选择物流公司", false);
                    }
                    else
                    {
                        bool flag = false;
                        foreach (LineItemInfo info3 in orderInfo.LineItems.Values)
                        {
                            if (info3.OrderItemsStatus == OrderStatus.ApplyForRefund)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                        {
                            this.ShowMsg("订单中商品正在处理退款！", false);
                        }
                        else
                        {
                            ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(this.radioShippingMode.SelectedValue.Value, true);
                            orderInfo.RealShippingModeId = this.radioShippingMode.SelectedValue.Value;
                            orderInfo.RealModeName = shippingMode.Name;
                            ExpressCompanyInfo info5 = ExpressHelper.FindNode(this.expressRadioButtonList.SelectedValue);
                            if (info5 != null)
                            {
                                orderInfo.ExpressCompanyAbb = info5.Kuaidi100Code;
                                orderInfo.ExpressCompanyName = info5.Name;
                            }
                            orderInfo.ShipOrderNumber = this.txtShipOrderNumber.Text;
                            if (OrderHelper.SendGoods(orderInfo))
                            {
                                SendNoteInfo info6 = new SendNoteInfo();
                                info6.NoteId = Globals.GetGenerateId();
                                info6.OrderId = this.orderId;
                                info6.Operator = currentManager.UserName;
                                info6.Remark = "后台" + info6.Operator + "发货成功";
                                OrderHelper.SaveSendNote(info6);
                                MemberInfo member = MemberHelper.GetMember(orderInfo.UserId);
                                Messenger.OrderShipping(orderInfo, member,this.wid);
                                if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && (orderInfo.GatewayOrderId.Trim().Length > 0))
                                {
                                    if (orderInfo.Gateway == "hishop.plugins.payment.ws_wappay.wswappayrequest")
                                    {
                                        PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.PaymentTypeId);
                                        if (paymentMode != null)
                                        {
                                            PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[] { paymentMode.Gateway })), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[] { paymentMode.Gateway })), "").SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
                                        }
                                    }
                                    if (orderInfo.Gateway == "hishop.plugins.payment.weixinrequest")
                                    {
                                        SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
                                        PayClient client = new PayClient(wid,masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey);
                                        DeliverInfo deliver = new DeliverInfo
                                        {
                                            TransId = orderInfo.GatewayOrderId,
                                            OutTradeNo = orderInfo.OrderId,
                                            OpenId = MemberHelper.GetMember(orderInfo.UserId).OpenId
                                        };
                                        client.DeliverNotify(deliver);
                                    }
                                }
                                orderInfo.OnDeliver();
                                this.ShowMsgAndReUrl("发货成功", true, this.goUrl, "frammain");
                            }
                            else
                            {
                                this.ShowMsg("发货失败", false);
                            }
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.goUrl = this.Page.Request.QueryString["reurl"];
            if (string.IsNullOrEmpty(this.goUrl))
            {
                this.goUrl = "manageorder.aspx";
            }
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.orderId = this.Page.Request.QueryString["OrderId"];
                OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
                this.BindOrderItems(orderInfo);
                this.btnSendGoods.Click += new EventHandler(this.btnSendGoods_Click);
                this.radioShippingMode.SelectedIndexChanged += new EventHandler(this.radioShippingMode_SelectedIndexChanged);
                if (!this.Page.IsPostBack)
                {
                    if (orderInfo == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.radioShippingMode.DataBind();
                        this.radioShippingMode.SelectedValue = new int?(orderInfo.ShippingModeId);
                        this.BindExpressCompany(orderInfo.ShippingModeId);
                        this.expressRadioButtonList.SelectedValue = orderInfo.ExpressCompanyAbb;
                        this.BindShippingAddress(orderInfo);
                        this.litShippingModeName.Text = orderInfo.ModeName;
                        this.litShipToDate.Text = orderInfo.ShipToDate;
                        this.litRemark.Text = orderInfo.Remark;
                        this.txtShipOrderNumber.Text = orderInfo.ShipOrderNumber;
                    }
                }
            }
        }

        private void radioShippingMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioShippingMode.SelectedValue.HasValue)
            {
                this.BindExpressCompany(this.radioShippingMode.SelectedValue.Value);
            }
        }
    }
}

