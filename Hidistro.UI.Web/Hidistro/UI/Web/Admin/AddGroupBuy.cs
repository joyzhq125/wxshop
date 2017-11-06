namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Text;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.GroupBuy)]
    public class AddGroupBuy : AdminPage
    {
        protected Button btnAddGroupBuy;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        protected HourDropDownList drophours;
        protected HourDropDownList HourDropDownList1;
        protected Label lblPrice;
        protected TextBox ProductId;
        protected TextBox txtContent;
        protected TextBox txtCount;
        protected TextBox txtMaxCount;
        protected TextBox txtNeedPrice;
        protected TextBox txtPrice;

        protected AddGroupBuy() : base("", "")
        {
        }

        private void btnAddGroupBuy_Click(object sender, EventArgs e)
        {
            int num2;
            int num3;
            decimal num4;
            GroupBuyInfo groupBuy = new GroupBuyInfo();
            string str = string.Empty;
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
                if (DateTime.Compare(groupBuy.EndDate, DateTime.Now) < 0)
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
            if (!string.IsNullOrWhiteSpace(this.txtNeedPrice.Text))
            {
                decimal num;
                if (decimal.TryParse(this.txtNeedPrice.Text.Trim(), out num))
                {
                    groupBuy.NeedPrice = num;
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("违约金填写格式不正确");
                }
            }
            if (int.TryParse(this.txtMaxCount.Text.Trim(), out num2))
            {
                groupBuy.MaxCount = num2;
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("限购数量不能为空，只能为整数");
            }
            if (int.TryParse(this.txtCount.Text.Trim(), out num3))
            {
                groupBuy.Count = num3;
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("团购满足数量不能为空，只能为整数");
            }
            if (decimal.TryParse(this.txtPrice.Text.Trim(), out num4))
            {
                groupBuy.Price = num4;
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("团购价格不能为空，只能为数值类型");
            }
            if (groupBuy.MaxCount < groupBuy.Count)
            {
                str = str + Formatter.FormatErrorMessage("限购数量必须大于等于满足数量 ");
            }
            if (!string.IsNullOrWhiteSpace(this.ProductId.Text))
            {
                int num5;
                if (int.TryParse(this.ProductId.Text.Trim(), out num5))
                {
                    groupBuy.ProductId = num5;
                }
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("未选择商品");
            }
            if (GroupBuyHelper.ProductGroupBuyExist(groupBuy.ProductId))
            {
                this.ShowMsg("已经存在此商品的团购活动，并且活动正在进行中", false);
            }
            else if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
            }
            else
            {
                groupBuy.Content = this.txtContent.Text;
                if (GroupBuyHelper.AddGroupBuy(groupBuy))
                {
                    this.ShowMsg("添加团购活动成功", true);
                }
                else
                {
                    this.ShowMsg("添加团购活动失败", true);
                }
            }
        }

        private void DoCallback()
        {
            base.Response.Clear();
            base.Response.ContentType = "application/json";
            string str = base.Request.QueryString["action"];
            if (str.Equals("getGroupBuyProducts"))
            {
                int num;
                ProductQuery query = new ProductQuery();
                int.TryParse(base.Request.QueryString["categoryId"], out num);
                string str2 = base.Request.QueryString["sku"];
                string str3 = base.Request.QueryString["productName"];
                query.Keywords = str3;
                query.ProductCode = str2;
                query.SaleStatus = ProductSaleStatus.OnSale;
                if (num > 0)
                {
                    query.CategoryId = new int?(num);
                    query.MaiCategoryPath = CatalogHelper.GetCategory(num).Path;
                }
                DataTable groupBuyProducts = ProductHelper.GetGroupBuyProducts(query);
                if ((groupBuyProducts == null) || (groupBuyProducts.Rows.Count == 0))
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("{\"Status\":\"OK\",");
                    builder.AppendFormat("\"Product\":[{0}]", this.GenerateBrandString(groupBuyProducts));
                    builder.Append("}");
                    base.Response.Write(builder.ToString());
                }
            }
            base.Response.End();
        }

        private string GenerateBrandString(DataTable tb)
        {
            StringBuilder builder = new StringBuilder();
            foreach (DataRow row in tb.Rows)
            {
                builder.Append("{");
                builder.AppendFormat("\"ProductId\":\"{0}\",\"ProductName\":\"{1}\"", row["ProductId"], row["ProductName"]);
                builder.Append("},");
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && (base.Request.QueryString["isCallback"] == "true"))
            {
                this.DoCallback();
            }
            else
            {
                this.btnAddGroupBuy.Click += new EventHandler(this.btnAddGroupBuy_Click);
                if (!this.Page.IsPostBack)
                {
                    this.HourDropDownList1.DataBind();
                    this.drophours.DataBind();
                }
            }
        }
    }
}

