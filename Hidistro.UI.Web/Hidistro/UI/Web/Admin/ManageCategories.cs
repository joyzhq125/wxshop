namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class ManageCategories : AdminPage
    {
        protected LinkButton btnOrder;
        protected Button btnSetCommissions;
        protected Grid grdTopCategries;
        protected HtmlInputText txtcategoryId;
        protected HtmlInputText txtfirst;
        protected HtmlInputText txtsecond;
        protected HtmlInputText txtsecondtoone;
        protected HtmlInputText txtsecondtothree;
        protected HtmlInputText txtthird;
        protected HtmlInputText txtthridtoone;
        protected string wid;
        protected ManageCategories() : base("", "")
        {
        }

        private void BindData()
        {
            this.grdTopCategries.DataSource = CatalogHelper.GetSequenceCategories(this.wid);
            this.grdTopCategries.DataBind();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in this.grdTopCategries.Rows)
            {
                int result = 0;
                TextBox box = (TextBox) row.FindControl("txtSequence");
                if (int.TryParse(box.Text.Trim(), out result))
                {
                    int categoryId = (int) this.grdTopCategries.DataKeys[row.RowIndex].Value;
                    if (CatalogHelper.GetCategory(categoryId).DisplaySequence != result)
                    {
                        CatalogHelper.SwapCategorySequence(categoryId, result);
                    }
                }
            }
            HiCache.Remove("DataCache-Categories");
            this.BindData();
        }

        private void btnSetCommissions_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtcategoryId.Value) || (Convert.ToInt32(this.txtcategoryId.Value) <= 0))
            {
                this.ShowMsg("请选择要编辑的佣金分类", false);
            }
            else
            {
                CategoryInfo categorys = this.GetCategorys();
                if (categorys != null)
                {
                    if (CatalogHelper.UpdateCategory(categorys) == CategoryActionStatus.Success)
                    {
                        this.ShowMsg("编缉商品佣金成功", true);
                        this.BindData();
                    }
                    else
                    {
                        this.ShowMsg("编缉商品分类错误,未知", false);
                    }
                }
            }
        }

        private CategoryInfo GetCategorys()
        {
            CategoryInfo category = CatalogHelper.GetCategory(Convert.ToInt32(this.txtcategoryId.Value));
            if (category == null)
            {
                this.ShowMsg("无法获取当前分类", false);
                return null;
            }
            category.FirstCommission = this.txtfirst.Value;
            category.SecondCommission = this.txtsecond.Value;
            category.ThirdCommission = this.txtthird.Value;
            bool flag = false;
            try
            {
                if ((Convert.ToDecimal(category.FirstCommission) < 1M) || (Convert.ToDecimal(category.FirstCommission) > 100M))
                {
                    this.ShowMsg("输入的佣金格式不正确！", false);
                    flag = true;
                }
                if ((Convert.ToDecimal(category.SecondCommission) < 1M) || (Convert.ToDecimal(category.SecondCommission) > 100M))
                {
                    this.ShowMsg("输入的佣金格式不正确！", false);
                    flag = true;
                }
                if ((Convert.ToDecimal(category.ThirdCommission) < 1M) || (Convert.ToDecimal(category.ThirdCommission) > 100M))
                {
                    this.ShowMsg("输入的佣金格式不正确！", false);
                    flag = true;
                }
                if (flag)
                {
                    return null;
                }
            }
            catch (Exception)
            {
                this.ShowMsg("输入的佣金格式不正确！", false);
                if (true)
                {
                    return null;
                }
            }
            return category;
        }

        private void grdTopCategries_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            int categoryId = (int) this.grdTopCategries.DataKeys[rowIndex].Value;
            if (e.CommandName == "DeleteCagetory")
            {
                if (!CatalogHelper.IsExitProduct(categoryId.ToString()))
                {
                    if (CatalogHelper.DeleteCategory(categoryId,this.wid))
                    {
                        this.ShowMsg("成功删除了指定的分类", true);
                    }
                    else
                    {
                        this.ShowMsg("分类删除失败，未知错误", false);
                    }
                }
                else
                {
                    this.ShowMsg("分类下有产品，请先删除产品再到商品回收站彻底删除。", false);
                }
            }
            this.BindData();
        }

        private void grdTopCategries_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int num = (int) DataBinder.Eval(e.Row.DataItem, "Depth");
                string str = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                if (num == 1)
                {
                    str = "<b>" + str + "</b>";
                }
                else
                {
                    HtmlGenericControl control = e.Row.FindControl("spShowImage") as HtmlGenericControl;
                    control.Visible = false;
                }
                for (int i = 1; i < num; i++)
                {
                    str = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + str;
                }
                Literal literal = e.Row.FindControl("lblCategoryName") as Literal;
                literal.Text = str;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.grdTopCategries.RowCommand += new GridViewCommandEventHandler(this.grdTopCategries_RowCommand);
            this.grdTopCategries.RowDataBound += new GridViewRowEventHandler(this.grdTopCategries_RowDataBound);
            this.btnSetCommissions.Click += new EventHandler(this.btnSetCommissions_Click);
            this.btnOrder.Click += new EventHandler(this.btnOrder_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }
    }
}

