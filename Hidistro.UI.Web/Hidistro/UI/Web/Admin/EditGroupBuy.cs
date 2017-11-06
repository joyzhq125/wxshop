namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.GroupBuy)]
    public class EditGroupBuy : AdminPage
    {
        protected HyperLink addProduct;
        protected Button btnFail;
        protected Button btnFinish;
        protected Button btnSuccess;
        protected Button btnUpdateGroupBuy;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        protected HourDropDownList drophours;
        private int groupBuyId;
        protected HourDropDownList HourDropDownList1;
        protected Label lblPrice;
        protected TextBox ProductId;
        protected Label productName;
        protected TextBox txtContent;
        protected TextBox txtCount;
        protected TextBox txtMaxCount;
        protected TextBox txtNeedPrice;
        protected TextBox txtPrice;

        protected EditGroupBuy() : base("", "")
        {
        }

        private void btnFail_Click(object sender, EventArgs e)
        {
            if (GroupBuyHelper.SetGroupBuyStatus(this.groupBuyId, GroupBuyStatus.Failed))
            {
                this.btnFail.Visible = false;
                this.btnSuccess.Visible = false;
                this.ShowMsg("成功设置团购活动为失败状态", true);
            }
            else
            {
                this.ShowMsg("设置团购活动状态失败", true);
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            if (GroupBuyHelper.SetGroupBuyEndUntreated(this.groupBuyId))
            {
                this.btnFail.Visible = true;
                this.btnSuccess.Visible = true;
                this.btnFinish.Visible = false;
                this.ShowMsg("成功设置团购活动为结束状态", true);
            }
            else
            {
                this.ShowMsg("设置团购活动状态失败", true);
            }
        }

        private void btnSuccess_Click(object sender, EventArgs e)
        {
            if (GroupBuyHelper.SetGroupBuyStatus(this.groupBuyId, GroupBuyStatus.Success))
            {
                this.btnFail.Visible = false;
                this.btnSuccess.Visible = false;
                this.ShowMsg("成功设置团购活动为成功状态", true);
            }
            else
            {
                this.ShowMsg("设置团购活动状态失败", true);
            }
        }

        private void btnUpdateGroupBuy_Click(object sender, EventArgs e)
        {
            GroupBuyInfo groupBuy = new GroupBuyInfo {
                GroupBuyId = this.groupBuyId
            };
            string str = string.Empty;
            if (!string.IsNullOrWhiteSpace(this.ProductId.Text))
            {
                int num;
                if (int.TryParse(this.ProductId.Text.Trim(), out num))
                {
                    groupBuy.ProductId = num;
                }
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("未选择商品");
            }
            if ((this.ViewState["oridProductId"].ToString() != this.ProductId.Text) && GroupBuyHelper.ProductGroupBuyExist(groupBuy.ProductId))
            {
                this.ShowMsg("已经存在此商品的团购活动，并且活动正在进行中", false);
            }
            else
            {
                int num3;
                int num4;
                decimal num5;
                if (!this.calendarStartDate.SelectedDate.HasValue)
                {
                    str = str + Formatter.FormatErrorMessage("请选择开始日期");
                }
                if (!this.calendarEndDate.SelectedDate.HasValue)
                {
                    str = str + Formatter.FormatErrorMessage("请选择结束日期");
                }
                else
                {
                    groupBuy.EndDate = this.calendarEndDate.SelectedDate.Value.AddHours((double) this.HourDropDownList1.SelectedValue.Value);
                    if ((DateTime.Compare(groupBuy.EndDate, DateTime.Now) <= 0) && (groupBuy.Status == GroupBuyStatus.UnderWay))
                    {
                        str = str + Formatter.FormatErrorMessage("结束日期必须要晚于今天日期");
                    }
                    else if (DateTime.Compare(this.calendarStartDate.SelectedDate.Value.AddHours((double) this.drophours.SelectedValue.Value), groupBuy.EndDate) >= 0)
                    {
                        str = str + Formatter.FormatErrorMessage("开始日期必须要早于结束日期");
                    }
                    else
                    {
                        groupBuy.StartDate = this.calendarStartDate.SelectedDate.Value.AddHours((double) this.drophours.SelectedValue.Value);
                    }
                }
                if (!string.IsNullOrEmpty(this.txtNeedPrice.Text))
                {
                    decimal num2;
                    if (decimal.TryParse(this.txtNeedPrice.Text.Trim(), out num2))
                    {
                        groupBuy.NeedPrice = num2;
                    }
                    else
                    {
                        str = str + Formatter.FormatErrorMessage("违约金填写格式不正确");
                    }
                }
                if (int.TryParse(this.txtMaxCount.Text.Trim(), out num3))
                {
                    groupBuy.MaxCount = num3;
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("限购数量不能为空，只能为整数");
                }
                groupBuy.Content = this.txtContent.Text;
                if (int.TryParse(this.txtCount.Text.Trim(), out num4))
                {
                    groupBuy.Count = num4;
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("团购满足数量不能为空，只能为整数");
                }
                if (decimal.TryParse(this.txtPrice.Text.Trim(), out num5))
                {
                    groupBuy.Price = num5;
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("团购价格不能为空，只能为数值类型");
                }
                if (groupBuy.MaxCount < groupBuy.Count)
                {
                    str = str + Formatter.FormatErrorMessage("限购数量必须大于等于满足数量 ");
                }
                if (!string.IsNullOrEmpty(str))
                {
                    this.ShowMsg(str, false);
                }
                else if (GroupBuyHelper.UpdateGroupBuy(groupBuy))
                {
                    this.ShowMsg("编辑团购活动成功", true);
                }
                else
                {
                    this.ShowMsg("编辑团购活动失败", true);
                }
            }
        }

        private void LoadGroupBuy(GroupBuyInfo groupBuy, decimal salePrice)
        {
            this.txtCount.Text = groupBuy.Count.ToString();
            this.txtPrice.Text = groupBuy.Price.ToString("F");
            this.txtContent.Text = Globals.HtmlDecode(groupBuy.Content);
            this.txtMaxCount.Text = groupBuy.MaxCount.ToString();
            this.txtNeedPrice.Text = groupBuy.NeedPrice.ToString("F");
            this.calendarEndDate.SelectedDate = new DateTime?(groupBuy.EndDate.Date);
            this.calendarStartDate.SelectedDate = new DateTime?(groupBuy.StartDate.Date);
            this.drophours.SelectedValue = new int?(groupBuy.StartDate.Hour);
            this.HourDropDownList1.SelectedValue = new int?(groupBuy.EndDate.Hour);
            ProductInfo productBaseInfo = ProductHelper.GetProductBaseInfo(groupBuy.ProductId);
            this.productName.Text = productBaseInfo.ProductName;
            this.ProductId.Text = groupBuy.ProductId.ToString();
            this.ViewState["oridProductId"] = groupBuy.ProductId;
            this.lblPrice.Text = salePrice.ToString("F2");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request["isCallback"]) && (base.Request["isCallback"] == "true"))
            {
                int num;
                if (int.TryParse(base.Request["productId"], out num))
                {
                    string priceByProductId = GroupBuyHelper.GetPriceByProductId(num);
                    if (priceByProductId.Length > 0)
                    {
                        base.Response.Clear();
                        base.Response.ContentType = "application/json";
                        base.Response.Write("{ ");
                        base.Response.Write("\"Status\":\"OK\",");
                        base.Response.Write(string.Format("\"Price\":\"{0}\"", decimal.Parse(priceByProductId).ToString("F2")));
                        base.Response.Write("}");
                        base.Response.End();
                    }
                }
            }
            else if (!int.TryParse(base.Request.QueryString["groupBuyId"], out this.groupBuyId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnUpdateGroupBuy.Click += new EventHandler(this.btnUpdateGroupBuy_Click);
                this.btnFail.Click += new EventHandler(this.btnFail_Click);
                this.btnSuccess.Click += new EventHandler(this.btnSuccess_Click);
                this.btnFinish.Click += new EventHandler(this.btnFinish_Click);
                if (!base.IsPostBack)
                {
                    this.HourDropDownList1.DataBind();
                    this.drophours.DataBind();
                    GroupBuyInfo groupBuy = GroupBuyHelper.GetGroupBuy(this.groupBuyId);
                    decimal productSalePrice = ProductHelper.GetProductSalePrice(groupBuy.ProductId);
                    if (GroupBuyHelper.GetOrderCount(this.groupBuyId) > 0)
                    {
                        this.addProduct.Visible = false;
                    }
                    if (groupBuy == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        if (groupBuy.Status == GroupBuyStatus.EndUntreated)
                        {
                            this.btnFail.Visible = true;
                            this.btnSuccess.Visible = true;
                        }
                        if (groupBuy.Status == GroupBuyStatus.UnderWay)
                        {
                            this.btnFinish.Visible = true;
                        }
                        this.LoadGroupBuy(groupBuy, productSalePrice);
                    }
                }
            }
        }
    }
}

