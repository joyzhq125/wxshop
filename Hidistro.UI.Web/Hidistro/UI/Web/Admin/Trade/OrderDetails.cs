namespace Hidistro.UI.Web.Admin.Trade
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.StatisticsReport;
    using Hidistro.Entities.Store;
    using Hidistro.Messages;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Orders)]
    public class OrderDetails : AdminPage
    {
        protected Button btnAgreeConfirm;
        protected HtmlInputButton btnClocsOrder;
        protected Button btnCloseOrder;
        protected HtmlInputButton btnConfirmOrder;
        protected Button btnConfirmPay;
        protected HtmlInputButton btnModifyAddr;
        protected HtmlInputButton btnModifyPrice;
        protected Button btnMondifyPay;
        protected Button btnMondifyShip;
        protected Button btnRefuseConfirm;
        protected Button btnRemark;
        protected HtmlInputButton btnSendGoods;
        protected HtmlInputButton btnViewLogistic;
        protected CloseTranReasonDropDownList ddlCloseReason;
        protected PaymentDropDownList ddlpayment;
        protected ShippingModeDropDownList ddlshippingMode;
        protected HtmlGenericControl divOrderProcess;
        protected HtmlGenericControl divRemarkShow;
        protected HiddenField hdfOrderID;
        protected HiddenField hdProductID;
        protected Label lbCloseReason;
        protected FormatedTimeLabel lblorderDateForRemark;
        protected OrderStatusLabel lblOrderStatus;
        protected FormatedMoneyLabel lblorderTotalForRemark;
        protected Literal lblOriAddress;
        protected Label lbReason;
        protected Literal litActivityShow;
        protected Literal litAddress;
        protected Literal litCommissionInfo;
        protected Literal litCompanyName;
        protected Literal litFinishDate;
        protected Literal litFreight;
        protected Literal litManagerRemark;
        protected Literal litModeName;
        protected Literal litOrderDate;
        protected Literal litOrderId;
        protected Literal litPayDate;
        protected Literal litPayType;
        protected Literal litRealName;
        protected Literal litRemark;
        protected Literal litShipOrderNumber;
        protected Literal litShippingDate;
        protected Literal litShippingRegion;
        protected Literal litShipToDate;
        protected Literal litSiteName;
        protected Literal litUserName;
        protected Literal litUserTel;
        protected Literal litWeiXinNickName;
        private UpdateStatistics myEvent;
        private StatisticNotifier myNotifier;
        private OrderInfo order;
        protected string orderId;
        protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;
        protected OrderRemarkImage OrderRemarkImageLink;
        protected decimal otherDiscountPrice;
        protected HtmlGenericControl pLoginsticInfo;
        protected HtmlGenericControl pNewAddress;
        protected HtmlAnchor power;
        protected string ProcessClass2;
        protected string ProcessClass3;
        protected string ProcessClass4;
        private string reurl;
        protected Repeater rptItemList;
        protected Repeater rptRefundList;
        protected Literal spanOrderId;
        protected TextBox txtAdminMemo;
        protected HtmlInputText txtcategoryId;
        protected TextBox txtMemo;
        protected TextBox txtMoney;
        protected TextBox txtRemark;

        protected OrderDetails()
            : base("m03", "0000")
        {
            this.myNotifier = new StatisticNotifier();
            this.myEvent = new UpdateStatistics();
            this.orderId = Globals.RequestQueryStr("OrderId");
            this.reurl = string.Empty;
            this.ProcessClass2 = string.Empty;
            this.ProcessClass3 = string.Empty;
            this.ProcessClass4 = string.Empty;
        }

        private void BindRemark(OrderInfo order)
        {
            this.spanOrderId.Text = order.OrderId;
            this.lblorderDateForRemark.Time = order.OrderDate;
            this.lblorderTotalForRemark.Money = order.GetTotal();
            this.txtRemark.Text = Globals.HtmlDecode(order.ManagerRemark);
            this.orderRemarkImageForRemark.SelectedValue = order.ManagerMark;
        }

        protected void btnAgreeConfirm_Click(object sender, EventArgs e)
        {
            decimal result = 0M;
            decimal.TryParse(this.txtMoney.Text.Trim(), out result);
            int num2 = 0;
            int productid = Globals.ToNum(this.hdProductID.Value);
            if (productid <= 0)
            {
                this.ShowMsg("服务器错误，请刷新页面重试！", false);
            }
            else
            {
                RefundInfo byOrderIdAndProductID = RefundHelper.GetByOrderIdAndProductID(this.hdfOrderID.Value, productid);
                if (byOrderIdAndProductID != null)
                {
                    byOrderIdAndProductID.ProductId = productid;
                    byOrderIdAndProductID.AdminRemark = this.txtMemo.Text.Trim();
                    byOrderIdAndProductID.HandleTime = DateTime.Now;
                    byOrderIdAndProductID.RefundTime = DateTime.Now.ToString();
                    byOrderIdAndProductID.HandleStatus = RefundInfo.Handlestatus.Refunded;
                    byOrderIdAndProductID.Operator = Globals.GetCurrentManagerUserId().ToString();
                    if (result <= 0M)
                    {
                        this.ShowMsg("输入的金额格式不正确", false);
                    }
                    else
                    {
                        byOrderIdAndProductID.RefundMoney = result;
                        byOrderIdAndProductID.RefundId = byOrderIdAndProductID.ReturnsId;
                        if (!RefundHelper.UpdateByReturnsId(byOrderIdAndProductID))
                        {
                            this.ShowMsg("退款失败，请重试。", false);
                        }
                        else
                        {
                            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hdfOrderID.Value);
                            string skuid = null;
                            string stock = null;
                            foreach (LineItemInfo info3 in orderInfo.LineItems.Values)
                            {
                                if (info3.ProductId == byOrderIdAndProductID.ProductId)
                                {
                                    skuid = info3.SkuId;
                                    stock = info3.Quantity.ToString();
                                    break;
                                }
                            }
                            if (RefundHelper.UpdateOrderGoodStatu(this.hdfOrderID.Value, skuid, 9))
                            {
                                RefundHelper.UpdateRefundOrderStock(stock, skuid);
                                OrderInfo info4 = OrderHelper.GetOrderInfo(this.hdfOrderID.Value);
                                foreach (LineItemInfo info5 in info4.LineItems.Values)
                                {
                                    if (info5.OrderItemsStatus.ToString() == OrderStatus.Refunded.ToString())
                                    {
                                        num2++;
                                    }
                                }
                                OrderHelper.UpdateOrderAmount(OrderHelper.GetOrderInfo(this.hdfOrderID.Value));
                                if (orderInfo.LineItems.Values.Count == num2)
                                {
                                    this.CloseOrder(this.hdfOrderID.Value);
                                }
                                OrderHelper.UpdateCalculadtionCommission(this.hdfOrderID.Value);
                                this.ShowMsgAndReUrl("同意退款成功!", true, "OrderDetails.aspx?OrderId=" + this.hdfOrderID.Value + "&t=" + DateTime.Now.ToString("HHmmss"));
                                this.myNotifier.updateAction = UpdateAction.OrderUpdate;
                                this.myNotifier.actionDesc = "同意退款成功";
                                this.myNotifier.RecDateUpdate = info4.PayDate.HasValue ? info4.PayDate.Value : DateTime.Today;
                                this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                                this.myNotifier.UpdateDB();
                            }
                        }
                    }
                }
            }
        }

        private void btnCloseOrder_Click(object sender, EventArgs e)
        {
            this.order.CloseReason = this.ddlCloseReason.SelectedValue;
            if ("请选择关闭的理由" == this.order.CloseReason)
            {
                this.ShowMsg("请选择关闭的理由", false);
            }
            else if (OrderHelper.CloseTransaction(this.order))
            {
                this.order.OnClosed();
                this.ShowMsgAndReUrl("关闭订单成功", true, "OrderDetails.aspx?OrderId=" + this.orderId + "&t=" + DateTime.Now.ToString("HHmmss"));
            }
            else
            {
                this.ShowMsg("关闭订单失败", false);
            }
        }

        protected void btnConfirmPay_Click(object sender, EventArgs e)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
            if ((orderInfo != null) && orderInfo.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
            {
                if (OrderHelper.ConfirmPay(orderInfo))
                {
                    DebitNoteInfo info2 = new DebitNoteInfo();
                    info2.NoteId = Globals.GetGenerateId();
                    info2.OrderId = this.orderId;
                    info2.Operator = ManagerHelper.GetCurrentManager().UserName;
                    info2.Remark = "后台" + info2.Operator + "收款成功";
                    OrderHelper.SaveDebitNote(info2);
                    orderInfo.OnPayment();
                    this.ShowMsgAndReUrl("成功的确认了订单收款", true, "OrderDetails.aspx?OrderId=" + this.orderId + "&t=" + DateTime.Now.ToString("HHmmss"));
                }
                else
                {
                    this.ShowMsg("确认订单收款失败", false);
                }
            }
        }

        private void btnMondifyPay_Click(object sender, EventArgs e)
        {
            this.order = OrderHelper.GetOrderInfo(this.orderId);
            if (this.ddlpayment.SelectedValue.HasValue && (this.ddlpayment.SelectedValue == -1))
            {
                this.order.PaymentTypeId = 0;
                this.order.PaymentType = "货到付款";
                this.order.Gateway = "hishop.plugins.payment.podrequest";
            }
            else if (this.ddlpayment.SelectedValue.HasValue && (this.ddlpayment.SelectedValue == 0x63))
            {
                this.order.PaymentTypeId = 0x63;
                this.order.PaymentType = "线下付款";
                this.order.Gateway = "hishop.plugins.payment.offlinerequest";
            }
            else if (this.ddlpayment.SelectedValue.HasValue && (this.ddlpayment.SelectedValue == 0x58))
            {
                this.order.PaymentTypeId = 0x58;
                this.order.PaymentType = "微信支付";
                this.order.Gateway = "hishop.plugins.payment.weixinrequest";
            }
            else
            {
                PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(this.ddlpayment.SelectedValue.Value);
                this.order.PaymentTypeId = paymentMode.ModeId;
                this.order.PaymentType = paymentMode.Name;
                this.order.Gateway = paymentMode.Gateway;
            }
            if (OrderHelper.UpdateOrderPaymentType(this.order))
            {
                this.ShowMsgAndReUrl("修改支付方式成功", true, "OrderDetails.aspx?OrderId=" + this.orderId + "&t=" + DateTime.Now.ToString("HHmmss"));
            }
            else
            {
                this.ShowMsg("修改支付方式失败", false);
            }
        }

        private void btnMondifyShip_Click(object sender, EventArgs e)
        {
            this.order = OrderHelper.GetOrderInfo(this.orderId);
            ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(this.ddlshippingMode.SelectedValue.Value, false);
            this.order.ShippingModeId = shippingMode.ModeId;
            this.order.ModeName = shippingMode.Name;
            if (OrderHelper.UpdateOrderShippingMode(this.order))
            {
                this.ShowMsgAndReUrl("修改配送方式成功", true, "OrderDetails.aspx?OrderId=" + this.orderId + "&t=" + DateTime.Now.ToString("HHmmss"));
            }
            else
            {
                this.ShowMsg("修改配送方式失败", false);
            }
        }

        protected void btnRefuseConfirm_Click(object sender, EventArgs e)
        {
            int productid = Globals.ToNum(this.hdProductID.Value);
            if (productid <= 0)
            {
                this.ShowMsg("服务器错误，请刷新页面重试！", false);
            }
            else
            {
                RefundInfo byOrderIdAndProductID = RefundHelper.GetByOrderIdAndProductID(this.hdfOrderID.Value, productid);
                byOrderIdAndProductID.RefundId = byOrderIdAndProductID.ReturnsId;
                byOrderIdAndProductID.AdminRemark = this.txtAdminMemo.Text.Trim();
                byOrderIdAndProductID.HandleTime = DateTime.Now;
                byOrderIdAndProductID.HandleStatus = RefundInfo.Handlestatus.RefuseRefunded;
                byOrderIdAndProductID.Operator = Globals.GetCurrentManagerUserId().ToString();
                if (!RefundHelper.UpdateByReturnsId(byOrderIdAndProductID))
                {
                    this.ShowMsg("操作失败，请重试。", false);
                }
                else
                {
                    OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hdfOrderID.Value);
                    string skuid = null;
                    foreach (LineItemInfo info3 in orderInfo.LineItems.Values)
                    {
                        if (info3.ProductId == productid)
                        {
                            skuid = info3.SkuId;
                            OrderStatus orderItemsStatus = info3.OrderItemsStatus;
                            break;
                        }
                    }
                    if (RefundHelper.UpdateOrderGoodStatu(this.hdfOrderID.Value, skuid, 3))
                    {
                        this.ShowMsgAndReUrl("拒绝退款成功!", true, "OrderDetails.aspx?OrderId=" + this.hdfOrderID.Value + "&t=" + DateTime.Now.ToString("HHmmss"));
                    }
                }
            }
        }

        private void btnRemark_Click(object sender, EventArgs e)
        {
            if (this.txtRemark.Text.Length > 300)
            {
                this.ShowMsg("备注长度限制在300个字符以内", false);
            }
            else
            {
                this.order.OrderId = this.orderId;
                if (this.orderRemarkImageForRemark.SelectedItem != null)
                {
                    this.order.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
                }
                this.order.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
                if (OrderHelper.SaveRemark(this.order))
                {
                    this.BindRemark(this.order);
                    this.ShowMsgAndReUrl("保存备注成功", true, "OrderDetails.aspx?OrderId=" + this.orderId + "&t=" + DateTime.Now.ToString("HHmmss"));
                }
                else
                {
                    this.ShowMsg("保存失败", false);
                }
            }
        }

        private void CloseOrder(string orderid)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderid);
            orderInfo.CloseReason = "客户要求退货(款)！";
            if (RefundHelper.CloseTransaction(orderInfo))
            {
                orderInfo.OnClosed();
                Messenger.OrderClosed(MemberHelper.GetMember(orderInfo.UserId), orderInfo, orderInfo.CloseReason,wid);
            }
        }

        private void LoadUserControl(OrderInfo order)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;

            this.reurl = "OrderDetails.aspx?OrderId=" + this.orderId + "&t=" + DateTime.Now.ToString("HHmmss");
            this.btnMondifyPay.Click += new EventHandler(this.btnMondifyPay_Click);
            this.btnMondifyShip.Click += new EventHandler(this.btnMondifyShip_Click);
            this.btnCloseOrder.Click += new EventHandler(this.btnCloseOrder_Click);
            this.btnRemark.Click += new EventHandler(this.btnRemark_Click);
            this.order = OrderHelper.GetOrderInfo(this.orderId);
            if (!base.IsPostBack)
            {
                if (string.IsNullOrEmpty(this.orderId))
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    this.hdfOrderID.Value = this.orderId;
                    this.litOrderDate.Text = this.order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss");
                    if (this.order.PayDate.HasValue)
                    {
                        this.litPayDate.Text = DateTime.Parse(this.order.PayDate.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (this.order.ShippingDate.HasValue)
                    {
                        this.litShippingDate.Text = DateTime.Parse(this.order.ShippingDate.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (this.order.FinishDate.HasValue)
                    {
                        this.litFinishDate.Text = DateTime.Parse(this.order.FinishDate.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    this.lblOrderStatus.OrderStatusCode = this.order.OrderStatus;
                    switch (this.order.OrderStatus)
                    {
                        case OrderStatus.WaitBuyerPay:
                            this.ProcessClass2 = "active";
                            if (this.order.Gateway != "hishop.plugins.payment.podrequest")
                            {
                                this.btnConfirmPay.Visible = true;
                            }
                            this.btnModifyAddr.Attributes.Add("onclick", "DialogFrame('../trade/ShipAddress.aspx?action=update&OrderId=" + this.orderId + "','修改收货地址',620,410)");
                            this.btnModifyAddr.Visible = true;
                            break;

                        case OrderStatus.BuyerAlreadyPaid:
                            this.ProcessClass2 = "ok";
                            this.ProcessClass3 = "active";
                            this.btnModifyAddr.Attributes.Add("onclick", "DialogFrame('../trade/ShipAddress.aspx?action=update&OrderId=" + this.orderId + "','修改收货地址',620,410)");
                            this.btnModifyAddr.Visible = true;
                            break;

                        case OrderStatus.SellerAlreadySent:
                            this.ProcessClass2 = "ok";
                            this.ProcessClass3 = "ok";
                            this.ProcessClass4 = "active";
                            break;

                        case OrderStatus.Finished:
                            this.ProcessClass2 = "ok";
                            this.ProcessClass3 = "ok";
                            this.ProcessClass4 = "ok";
                            break;
                    }
                    if (this.order.ManagerMark.HasValue)
                    {
                        this.OrderRemarkImageLink.ManagerMarkValue = (int)this.order.ManagerMark.Value;
                        this.litManagerRemark.Text = this.order.ManagerRemark;
                    }
                    else
                    {
                        this.divRemarkShow.Visible = false;
                    }
                    this.litRemark.Text = this.order.Remark;
                    this.litOrderId.Text = this.order.OrderId;
                    this.litUserName.Text = this.order.Username;
                    this.litPayType.Text = this.order.PaymentType;
                    this.litShipToDate.Text = this.order.ShipToDate;
                    this.litRealName.Text = this.order.ShipTo;
                    this.litUserTel.Text = string.IsNullOrEmpty(this.order.CellPhone) ? this.order.TelPhone : this.order.CellPhone;
                    this.litShippingRegion.Text = this.order.ShippingRegion;
                    this.litFreight.Text = Globals.FormatMoney(this.order.AdjustedFreight);
                    if (this.order.ReferralUserId == 0)
                    {
                        this.litSiteName.Text = "主站";
                    }
                    else
                    {
                        DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(this.order.ReferralUserId);
                        if (distributorInfo != null)
                        {
                            this.litSiteName.Text = distributorInfo.StoreName;
                        }
                    }
                    StringBuilder builder = new StringBuilder();
                    if (!string.IsNullOrEmpty(this.order.ActivitiesName))
                    {
                        this.otherDiscountPrice += this.order.DiscountAmount;
                        builder.Append("<p>" + this.order.ActivitiesName + ":￥" + this.order.DiscountAmount.ToString("F2") + "</p>");
                    }
                    if (!string.IsNullOrEmpty(this.order.ReducedPromotionName))
                    {
                        this.otherDiscountPrice += this.order.ReducedPromotionAmount;
                        builder.Append("<p>" + this.order.ReducedPromotionName + ":￥" + this.order.ReducedPromotionAmount.ToString("F2") + "</p>");
                    }
                    if (!string.IsNullOrEmpty(this.order.CouponName))
                    {
                        this.otherDiscountPrice += this.order.CouponAmount;
                        builder.Append("<p>" + this.order.CouponName + ":￥" + this.order.CouponAmount.ToString("F2") + "</p>");
                    }
                    if (!string.IsNullOrEmpty(this.order.RedPagerActivityName))
                    {
                        this.otherDiscountPrice += this.order.RedPagerAmount;
                        builder.Append("<p>" + this.order.RedPagerActivityName + ":￥" + this.order.RedPagerAmount.ToString("F2") + "</p>");
                    }
                    if (this.order.PointToCash > 0M)
                    {
                        this.otherDiscountPrice += this.order.PointToCash;
                        builder.Append("<p>积分抵现:￥" + this.order.PointToCash.ToString("F2") + "</p>");
                    }
                    decimal adjustCommssion = this.order.GetAdjustCommssion();
                    if (adjustCommssion > 0M)
                    {
                        this.otherDiscountPrice -= adjustCommssion;
                        builder.Append("<p>管理员调价优惠:￥" + adjustCommssion.ToString("F2") + "</p>");
                    }
                    else if (adjustCommssion < 0M)
                    {
                        this.otherDiscountPrice -= adjustCommssion;
                        builder.Append("<p>管理员调价增加:￥" + adjustCommssion.ToString("F2").Trim(new char[] { '-' }) + "</p>");
                    }
                    this.litActivityShow.Text = builder.ToString();
                    if (((int)this.lblOrderStatus.OrderStatusCode) != 4)
                    {
                        this.lbCloseReason.Visible = false;
                    }
                    else
                    {
                        this.divOrderProcess.Visible = false;
                        this.lbReason.Text = this.order.CloseReason;
                    }
                    if ((this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid) || ((this.order.OrderStatus == OrderStatus.WaitBuyerPay) && (this.order.Gateway == "hishop.plugins.payment.podrequest")))
                    {
                        this.btnSendGoods.Visible = true;
                    }
                    else
                    {
                        this.btnSendGoods.Visible = false;
                    }
                    if (((this.order.OrderStatus == OrderStatus.SellerAlreadySent) || (this.order.OrderStatus == OrderStatus.Finished)) && !string.IsNullOrEmpty(this.order.ExpressCompanyAbb))
                    {
                        this.pLoginsticInfo.Visible = true;
                        this.btnViewLogistic.Visible = true;
                        if ((Express.GetExpressType() == "kuaidi100") && (this.power != null))
                        {
                            this.power.Visible = true;
                        }
                    }
                    if (this.order.OrderStatus == OrderStatus.WaitBuyerPay)
                    {
                        this.btnClocsOrder.Visible = true;
                        this.btnModifyPrice.Visible = true;
                    }
                    else
                    {
                        this.btnClocsOrder.Visible = false;
                        this.btnModifyPrice.Visible = false;
                    }
                    this.btnModifyPrice.Attributes.Add("onclick", "DialogFrame('../trade/EditOrder.aspx?OrderId=" + this.orderId + "&reurl=" + base.Server.UrlEncode(this.reurl) + "','修改订单价格',900,450)");
                    this.BindRemark(this.order);
                    this.ddlshippingMode.DataBind();
                    this.ddlshippingMode.SelectedValue = new int?(this.order.ShippingModeId);
                    this.ddlpayment.DataBind();
                    this.ddlpayment.SelectedValue = new int?(this.order.PaymentTypeId);
                    this.rptItemList.DataSource = this.order.LineItems.Values;
                    this.rptItemList.DataBind();
                    string oldAddress = this.order.OldAddress;
                    string str2 = string.Empty;
                    if (!string.IsNullOrEmpty(this.order.ShippingRegion))
                    {
                        str2 = this.order.ShippingRegion.Replace('，', ' ');
                    }
                    if (!string.IsNullOrEmpty(this.order.Address))
                    {
                        str2 = str2 + this.order.Address;
                    }
                    if (!string.IsNullOrEmpty(this.order.ShipTo))
                    {
                        str2 = str2 + "   " + this.order.ShipTo;
                    }
                    if (!string.IsNullOrEmpty(this.order.ZipCode))
                    {
                        str2 = str2 + "   " + this.order.ZipCode;
                    }
                    if (!string.IsNullOrEmpty(this.order.TelPhone))
                    {
                        str2 = str2 + "   " + this.order.TelPhone;
                    }
                    if (!string.IsNullOrEmpty(this.order.CellPhone))
                    {
                        str2 = str2 + "   " + this.order.CellPhone;
                    }
                    if (string.IsNullOrEmpty(oldAddress))
                    {
                        this.lblOriAddress.Text = str2;
                        this.pNewAddress.Visible = false;
                    }
                    else
                    {
                        this.lblOriAddress.Text = oldAddress;
                        this.litAddress.Text = str2;
                    }
                    if ((this.order.OrderStatus == OrderStatus.Finished) || (this.order.OrderStatus == OrderStatus.SellerAlreadySent))
                    {
                        string realModeName = this.order.RealModeName;
                        if (string.IsNullOrEmpty(realModeName))
                        {
                            realModeName = this.order.ModeName;
                        }
                        this.litModeName.Text = realModeName;
                        this.litShipOrderNumber.Text = this.order.ShipOrderNumber;
                    }
                    else
                    {
                        this.litModeName.Text = this.order.ModeName;
                    }
                    if (!string.IsNullOrEmpty(this.order.ExpressCompanyName))
                    {
                        this.litCompanyName.Text = this.order.ExpressCompanyName;
                    }
                    MemberInfo member = MemberProcessor.GetMember(this.order.UserId, true);
                    if ((member != null) && !string.IsNullOrEmpty(member.OpenId))
                    {
                        this.litWeiXinNickName.Text = member.UserName;
                    }
                    if (this.order.ReferralUserId > 0)
                    {
                        builder = new StringBuilder();
                        builder.Append("<div class=\"commissionInfo mb20\"><h3>佣金信息</h3><div class=\"commissionInfoInner\">");
                        decimal num2 = 0M;
                        decimal totalCommssion = 0M;
                        decimal secondTotalCommssion = 0M;
                        decimal thirdTotalCommssion = 0M;
                        if (this.order.OrderStatus != OrderStatus.Closed)
                        {
                            totalCommssion = this.order.GetTotalCommssion();
                            secondTotalCommssion = this.order.GetSecondTotalCommssion();
                            thirdTotalCommssion = this.order.GetThirdTotalCommssion();
                        }
                        num2 += totalCommssion;
                        string storeName = string.Empty;
                        DistributorsInfo info3 = DistributorsBrower.GetDistributorInfo(this.order.ReferralUserId);
                        if (info3 != null)
                        {
                            storeName = info3.StoreName;
                            if ((info3.ReferralPath != null) && (info3.ReferralPath.Length > 0))
                            {
                                string[] strArray = info3.ReferralPath.Trim().Split(new char[] { '|' });
                                int distributorid = 0;
                                if (strArray.Length > 1)
                                {
                                    distributorid = Globals.ToNum(strArray[0]);
                                    if (distributorid > 0)
                                    {
                                        info3 = DistributorsBrower.GetDistributorInfo(distributorid);
                                        if (info3 != null)
                                        {
                                            num2 += thirdTotalCommssion;
                                            builder.Append("<p class=\"mb5\"><span>上二级分销商：</span> " + info3.StoreName + "<i>￥" + thirdTotalCommssion.ToString("F2") + "</i></p>");
                                        }
                                    }
                                    distributorid = Globals.ToNum(strArray[1]);
                                    if (distributorid > 0)
                                    {
                                        info3 = DistributorsBrower.GetDistributorInfo(distributorid);
                                        if (info3 != null)
                                        {
                                            num2 += secondTotalCommssion;
                                            builder.Append("<p class=\"mb5\"><span>上一级分销商：</span> " + info3.StoreName + "<i>￥" + secondTotalCommssion.ToString("F2") + "</i></p>");
                                        }
                                    }
                                }
                                else if (strArray.Length == 1)
                                {
                                    distributorid = Globals.ToNum(strArray[0]);
                                    if (distributorid > 0)
                                    {
                                        info3 = DistributorsBrower.GetDistributorInfo(distributorid);
                                        if (info3 != null)
                                        {
                                            builder.Append("<p class=\"mb5\"><span>上二级分销商：</span>-</p>");
                                            num2 += secondTotalCommssion;
                                            builder.Append("<p class=\"mb5\"><span>上一级分销商：</span>" + info3.StoreName + " <i>￥" + secondTotalCommssion.ToString("F2") + "</i></p>");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                builder.Append("<p class=\"mb5\"><span>上二级分销商：</span>-</p>");
                                builder.Append("<p class=\"mb5\"><span>上一级分销商：</span>-</p>");
                            }
                        }
                        builder.Append("<div class=\"clearfix\">");
                        builder.Append("<p><span>成交店铺：</span>" + storeName + " <i>￥" + totalCommssion.ToString("F2") + "</i></p>");
                        builder.Append("<p><span>佣金总额：</span><i>￥" + num2.ToString("F2") + "</i></p>");
                        builder.Append("</div></div></div>");
                        this.litCommissionInfo.Text = builder.ToString();
                    }
                    DataTable orderItemsReFundByOrderID = RefundHelper.GetOrderItemsReFundByOrderID(this.orderId);
                    if (orderItemsReFundByOrderID.Rows.Count > 0)
                    {
                        this.rptRefundList.DataSource = orderItemsReFundByOrderID;
                        this.rptRefundList.DataBind();
                    }
                }
                //Page.DataBind();
            }
        }

        protected void rptRefundList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                HtmlInputButton button = (HtmlInputButton)e.Item.FindControl("btnAgree");
                HtmlInputButton button2 = (HtmlInputButton)e.Item.FindControl("btnRefuce");
                Label label = (Label)e.Item.FindControl("lblIsAgree");
                switch (((RefundInfo.Handlestatus)DataBinder.Eval(e.Item.DataItem, "HandleStatus")))
                {
                    case RefundInfo.Handlestatus.Applied:
                        button.Visible = false;
                        button2.Visible = false;
                        label.Visible = true;
                        label.Text = "已申请";
                        return;

                    case RefundInfo.Handlestatus.Refunded:
                        button.Visible = false;
                        button2.Visible = false;
                        label.Visible = true;
                        label.Text = "已退款";
                        return;

                    case RefundInfo.Handlestatus.Refused:
                        button.Visible = false;
                        button2.Visible = false;
                        label.Visible = true;
                        label.Text = "拒绝申请";
                        return;

                    case RefundInfo.Handlestatus.NoneAudit:
                    case RefundInfo.Handlestatus.HasTheAudit:
                    case RefundInfo.Handlestatus.NoRefund:
                        return;

                    case RefundInfo.Handlestatus.AuditNotThrough:
                        button.Visible = false;
                        button2.Visible = false;
                        label.Visible = true;
                        label.Text = "审核不通过";
                        return;

                    case RefundInfo.Handlestatus.RefuseRefunded:
                        button.Visible = false;
                        button2.Visible = false;
                        label.Visible = true;
                        label.Text = "拒绝退款";
                        return;
                }
            }
        }
    }
}

