namespace Hidistro.UI.Web.Admin.Sales
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.Messages;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Returnsapply : AdminPage
    {
        protected Button btnAcceptRefund;
        protected Button btnAuditAcceptRefund;
        protected Button btnAuditRefuseRefund;
        protected Button btnRefuseRefund;
        protected Button btnSearchButton;
        protected DropDownList ddlHandleStatus;
        protected DataList dlstRefund;
        protected HtmlInputHidden hidAdminRemark;
        protected HtmlInputHidden hidAuditM;
        protected HtmlInputHidden hidOrderId;
        protected HtmlInputHidden hidOrderTotal;
        protected HtmlInputHidden hidProductId;
        protected HtmlInputHidden hidRefundM;
        protected HtmlInputHidden hidRefundMoney;
        protected HtmlInputHidden hidRefundType;
        protected HtmlInputHidden hidReturnsId;
        protected HtmlInputHidden hidStatus;
        protected PageSize hrefPageSize;
        protected TextBox lblAuditOrderTotal;
        protected Label lblAuditRefundRemark;
        protected Label lblAuditReturnsId;
        protected TextBox lblOrderTotal;
        protected Label lblRefundRemark;
        protected Label lblReturnsId;
        protected Label lblStatus;
        protected ImageLinkButton lkbtnDeleteCheck;
        protected Pager pager;
        protected Pager pager1;
        protected TextBox txtAdminRemark;
        protected TextBox txtOrderId;

        protected Returnsapply() : base("", "")
        {
        }

        private void BindRefund()
        {
            ReturnsApplyQuery refundQuery = this.GetRefundQuery();
            DbQueryResult returnOrderAll = RefundHelper.GetReturnOrderAll(refundQuery);
            this.dlstRefund.DataSource = returnOrderAll.Data;
            this.dlstRefund.DataBind();
            this.pager.TotalRecords = returnOrderAll.TotalRecords;
            this.pager1.TotalRecords = returnOrderAll.TotalRecords;
            this.txtOrderId.Text = refundQuery.OrderId;
            this.ddlHandleStatus.SelectedIndex = 0;
            this.ddlHandleStatus.SelectedValue = refundQuery.HandleStatus.Value.ToString();
        }

        protected void btnAcceptRefund_Click(object sender, EventArgs e)
        {
            decimal result = 0M;
            int num2 = 0;
            RefundInfo refundInfo = new RefundInfo {
                RefundId = int.Parse(this.hidReturnsId.Value),
                AdminRemark = this.hidAdminRemark.Value.Trim(),
                HandleTime = DateTime.Now,
                RefundTime = DateTime.Now.ToString(),
                HandleStatus = RefundInfo.Handlestatus.Refunded,
                Operator = Globals.GetCurrentManagerUserId().ToString()
            };
            if (!decimal.TryParse(this.hidRefundM.Value, out result))
            {
                this.ShowMsg("输入的金额格式不正确", false);
            }
            else
            {
                refundInfo.RefundMoney = result;
                if (result < 0M)
                {
                    this.ShowMsg("不能为负数！", false);
                }
                else if (RefundHelper.UpdateByReturnsId(refundInfo))
                {
                    OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
                    string skuid = null;
                    foreach (LineItemInfo info3 in orderInfo.LineItems.Values)
                    {
                        if (info3.ProductId == int.Parse(this.hidProductId.Value))
                        {
                            skuid = info3.SkuId;
                        }
                    }
                    if (RefundHelper.UpdateOrderGoodStatu(this.hidOrderId.Value, skuid, 9))
                    {
                        foreach (LineItemInfo info5 in OrderHelper.GetOrderInfo(this.hidOrderId.Value).LineItems.Values)
                        {
                            if (info5.OrderItemsStatus.ToString() == OrderStatus.Refunded.ToString())
                            {
                                num2++;
                            }
                        }
                        if (orderInfo.LineItems.Values.Count == num2)
                        {
                            this.CloseOrder(this.hidOrderId.Value);
                        }
                        this.ShowMsg("成功退款", true);
                        this.BindRefund();
                    }
                }
                else
                {
                    this.ShowMsg("退款失败，请重试。", false);
                }
            }
        }

        private void btnAuditAcceptRefund_Click(object sender, EventArgs e)
        {
            decimal result = 0M;
            RefundInfo refundInfo = new RefundInfo {
                RefundId = int.Parse(this.hidReturnsId.Value),
                AdminRemark = this.txtAdminRemark.Text.Trim(),
                HandleTime = DateTime.Now,
                AuditTime = DateTime.Now.ToString(),
                HandleStatus = RefundInfo.Handlestatus.NoRefund
            };
            if (!decimal.TryParse(this.hidAuditM.Value, out result))
            {
                this.ShowMsg("输入的金额格式不正确", false);
            }
            else
            {
                refundInfo.RefundMoney = result;
                if (result < 0M)
                {
                    this.ShowMsg("不能为负数！", false);
                }
                else if (RefundHelper.UpdateByAuditReturnsId(refundInfo))
                {
                    this.ShowMsg("审核成功", true);
                    this.BindRefund();
                }
                else
                {
                    this.ShowMsg("审核失败，请重试。", false);
                }
            }
        }

        private void btnAuditRefuseRefund_Click(object sender, EventArgs e)
        {
            decimal result = 0M;
            RefundInfo refundInfo = new RefundInfo {
                RefundId = int.Parse(this.hidReturnsId.Value),
                AdminRemark = this.txtAdminRemark.Text.Trim(),
                HandleTime = DateTime.Now,
                HandleStatus = RefundInfo.Handlestatus.AuditNotThrough,
                Operator = Globals.GetCurrentManagerUserId().ToString()
            };
            if (!decimal.TryParse(this.hidAuditM.Value, out result))
            {
                this.ShowMsg("输入的金额格式不正确", false);
            }
            else
            {
                refundInfo.RefundMoney = result;
                if (result < 0M)
                {
                    this.ShowMsg("不能为负数！", false);
                }
                else if (RefundHelper.UpdateByAuditReturnsId(refundInfo))
                {
                    OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
                    string skuid = null;
                    foreach (LineItemInfo info3 in orderInfo.LineItems.Values)
                    {
                        if (info3.ProductId == int.Parse(this.hidProductId.Value))
                        {
                            skuid = info3.SkuId;
                        }
                    }
                    if (RefundHelper.UpdateOrderGoodStatu(this.hidOrderId.Value, skuid, 3))
                    {
                        this.ShowMsg("操作成功", true);
                        this.BindRefund();
                    }
                }
                else
                {
                    this.ShowMsg("操作失败，请重试。", false);
                }
            }
        }

        private void btnRefuseRefund_Click(object sender, EventArgs e)
        {
            decimal result = 0M;
            RefundInfo refundInfo = new RefundInfo {
                RefundId = int.Parse(this.hidReturnsId.Value),
                AdminRemark = this.hidAdminRemark.Value.Trim(),
                HandleTime = DateTime.Now,
                HandleStatus = RefundInfo.Handlestatus.RefuseRefunded,
                Operator = Globals.GetCurrentManagerUserId().ToString()
            };
            if (!decimal.TryParse(this.hidRefundM.Value, out result))
            {
                this.ShowMsg("输入的金额格式不正确", false);
            }
            else if (result < 0M)
            {
                this.ShowMsg("不能为负数！", false);
            }
            else
            {
                refundInfo.RefundMoney = result;
                if (RefundHelper.UpdateByReturnsId(refundInfo))
                {
                    OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
                    string skuid = null;
                    foreach (LineItemInfo info3 in orderInfo.LineItems.Values)
                    {
                        if (info3.ProductId == int.Parse(this.hidProductId.Value))
                        {
                            skuid = info3.SkuId;
                        }
                    }
                    if (RefundHelper.UpdateOrderGoodStatu(this.hidOrderId.Value, skuid, (int.Parse(this.hidStatus.Value) == 6) ? 2 : 3))
                    {
                        this.ShowMsg("操作成功", true);
                        this.BindRefund();
                    }
                }
                else
                {
                    this.ShowMsg("操作失败，请重试。", false);
                }
            }
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReloadRefunds(true);
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

        private void dlstRefund_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                HtmlAnchor anchor1 = (HtmlAnchor) e.Item.FindControl("lkbtnCheckRefund");
                Label label = (Label) e.Item.FindControl("lblHandleStatus");
                if (label.Text == "4")
                {
                    label.Text = "未审核";
                }
                else if (label.Text == "5")
                {
                    label.Text = "已审核";
                }
                else if (label.Text == "6")
                {
                    label.Text = "未退款";
                }
                else if (label.Text == "2")
                {
                    label.Text = "已退款";
                }
                else if (label.Text == "8")
                {
                    label.Text = "拒绝退款";
                }
                else
                {
                    label.Text = "审核不通过";
                }
            }
        }

        private ReturnsApplyQuery GetRefundQuery()
        {
            ReturnsApplyQuery query = new ReturnsApplyQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                query.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
            {
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out result))
                {
                    query.HandleStatus = new int?(result);
                }
            }
            else
            {
                query.HandleStatus = -2;
            }
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortBy = "ApplyForTime";
            query.SortOrder = SortAction.Desc;
            return query;
        }

        private void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要删除的退款申请单", false);
            }
            else
            {
                int num;
                string format = "成功删除了{0}个退款申请单";
                if (RefundHelper.DelRefundApply(str.Split(new char[] { ',' }), out num))
                {
                    format = string.Format(format, num);
                }
                else
                {
                    format = string.Format(format, num) + ",待处理的申请不能删除";
                }
                this.BindRefund();
                this.ShowMsg(format, true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;

            this.dlstRefund.ItemDataBound += new DataListItemEventHandler(this.dlstRefund_ItemDataBound);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
            this.btnAcceptRefund.Click += new EventHandler(this.btnAcceptRefund_Click);
            this.btnRefuseRefund.Click += new EventHandler(this.btnRefuseRefund_Click);
            this.btnAuditAcceptRefund.Click += new EventHandler(this.btnAuditAcceptRefund_Click);
            this.btnAuditRefuseRefund.Click += new EventHandler(this.btnAuditRefuseRefund_Click);
            if (!base.IsPostBack)
            {
                this.BindRefund();
            }
        }

        private void ReloadRefunds(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("OrderId", this.txtOrderId.Text);
            queryStrings.Add("PageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            if (!string.IsNullOrEmpty(this.ddlHandleStatus.SelectedValue))
            {
                queryStrings.Add("HandleStatus", this.ddlHandleStatus.SelectedValue);
            }
            base.ReloadPage(queryStrings);
        }
    }
}

