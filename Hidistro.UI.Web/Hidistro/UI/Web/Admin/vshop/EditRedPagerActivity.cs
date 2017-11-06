namespace Hidistro.UI.Web.Admin.vshop
{
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Web.UI.WebControls;

    public class EditRedPagerActivity : AdminPage
    {
        protected Button btnSubmit;
        protected DropDownList ddlCategoryId;
        protected string htmlEditName;
        private int redpaperactivityid;
        protected string ReUrl;
        protected TextBox txtExpiryDays;
        protected TextBox txtItemAmountLimit;
        protected TextBox txtMaxGetTimes;
        protected TextBox txtMinOrderAmount;
        protected TextBox txtName;
        protected TextBox txtOrderAmountCanUse;
        protected string wid;
        protected EditRedPagerActivity() : base("", "")
        {
            this.ReUrl = "redpageractivitylist.aspx";
            this.htmlEditName = "修改";
        }

        private void Bind()
        {
            RedPagerActivityInfo redPagerActivityInfo = RedPagerActivityBrower.GetRedPagerActivityInfo(this.redpaperactivityid);
            if (redPagerActivityInfo == null)
            {
                this.ShowMsg("代金券活动不存在！", false);
            }
            else
            {
                this.txtName.Text = redPagerActivityInfo.Name;
                for (int i = 0; i < this.ddlCategoryId.Items.Count; i++)
                {
                    if (this.ddlCategoryId.Items[i].Value == redPagerActivityInfo.CategoryId.ToString())
                    {
                        this.ddlCategoryId.Items[i].Selected = true;
                        break;
                    }
                }
                this.txtMinOrderAmount.Text = redPagerActivityInfo.MinOrderAmount.ToString("F2");
                this.txtMaxGetTimes.Text = redPagerActivityInfo.MaxGetTimes.ToString();
                this.txtItemAmountLimit.Text = redPagerActivityInfo.ItemAmountLimit.ToString("F2");
                this.txtOrderAmountCanUse.Text = redPagerActivityInfo.OrderAmountCanUse.ToString("F2");
                this.txtExpiryDays.Text = redPagerActivityInfo.ExpiryDays.ToString();
            }
        }

        private void BindCategoryID()
        {
            DataTable allCategories = CategoryBrowser.GetAllCategories(this.wid);
            this.ddlCategoryId.DataTextField = "Name";
            this.ddlCategoryId.DataValueField = "CategoryId";
            this.ddlCategoryId.DataSource = allCategories;
            this.ddlCategoryId.DataBind();
            ListItem item = new ListItem("--全部--", "0");
            this.ddlCategoryId.Items.Insert(0, item);
        }

        private void btn_Submint(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtName.Text.Trim()))
            {
                this.ShowMsg("名称不能为空", false);
            }
            else
            {
                decimal result = 0M;
                int num2 = 0;
                decimal num3 = 0M;
                int num4 = 0;
                decimal num5 = 0M;
                RedPagerActivityInfo redpaperactivity = new RedPagerActivityInfo();
                if (this.redpaperactivityid > 0)
                {
                    redpaperactivity = RedPagerActivityBrower.GetRedPagerActivityInfo(this.redpaperactivityid);
                }
                if (redpaperactivity != null)
                {
                    redpaperactivity.Name = this.txtName.Text.Trim();
                    redpaperactivity.CategoryId = int.Parse(this.ddlCategoryId.SelectedValue);
                    decimal.TryParse(this.txtMinOrderAmount.Text.Trim(), out result);
                    int.TryParse(this.txtMaxGetTimes.Text.Trim(), out num2);
                    decimal.TryParse(this.txtItemAmountLimit.Text.Trim(), out num3);
                    int.TryParse(this.txtExpiryDays.Text.Trim(), out num4);
                    decimal.TryParse(this.txtOrderAmountCanUse.Text.Trim(), out num5);
                    redpaperactivity.MinOrderAmount = result;
                    redpaperactivity.MaxGetTimes = num2;
                    redpaperactivity.ItemAmountLimit = num3;
                    redpaperactivity.ExpiryDays = num4;
                    redpaperactivity.OrderAmountCanUse = num5;
                    if (RedPagerActivityBrower.IsExistsMinOrderAmount(redpaperactivity.RedPagerActivityId, redpaperactivity.MinOrderAmount))
                    {
                        this.ShowMsg("已存在相同金额的代金券活动", false);
                    }
                    else if (this.redpaperactivityid > 0)
                    {
                        if (RedPagerActivityBrower.UpdateRedPagerActivity(redpaperactivity))
                        {
                            if (base.Request.QueryString["reurl"] != null)
                            {
                                this.ReUrl = base.Request.QueryString["reurl"].ToString();
                            }
                            this.ShowMsgAndReUrl("代金券活动修改成功", true, this.ReUrl);
                        }
                        else
                        {
                            this.ShowMsg("代金券活动修改失败", false);
                        }
                    }
                    else if (RedPagerActivityBrower.CreateRedPagerActivity(redpaperactivity))
                    {
                        this.ShowMsgAndReUrl("代金券活动新增成功", true, this.ReUrl);
                    }
                    else
                    {
                        this.ShowMsg("代金券活动新增失败", false);
                    }
                }
                else
                {
                    this.ShowMsg("代金券活动不存在！", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.btnSubmit.Click += new EventHandler(this.btn_Submint);
            bool flag = int.TryParse(this.Page.Request.QueryString["redpaperactivityid"], out this.redpaperactivityid);
            if (!base.IsPostBack)
            {
                this.BindCategoryID();
                if (flag)
                {
                    this.Bind();
                }
                else
                {
                    this.htmlEditName = "新增";
                    this.btnSubmit.Text = "新 增";
                }
            }
        }
    }
}

