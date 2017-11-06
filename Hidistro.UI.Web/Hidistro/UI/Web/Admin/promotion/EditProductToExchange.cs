namespace Hidistro.UI.Web.Admin.promotion
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditProductToExchange : AdminPage
    {
        protected Button btnQuery;
        protected int exchangeId;
        protected Repeater grdProducts;
        protected PageSize hrefPageSize;
        protected Label lbsaleNumber;
        protected Label lbSelectNumber;
        protected Label lbwareNumber;
        protected Pager pager;
        protected ProductSaleStatus status;
        protected HtmlForm thisForm;
        protected TextBox txt_maxPrice;
        protected TextBox txt_minPrice;
        protected TextBox txt_name;
        protected string wid;
        protected EditProductToExchange() : base("m08", "yxp02")
        {
            this.status = ProductSaleStatus.All;
        }

        private bool bDecimal(string val, ref decimal i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            return decimal.TryParse(val, out i);
        }

        private void BindProducts(int eId)
        {
            if (this.exchangeId != 0)
            {
                string text = this.txt_name.Text;
                string val = this.txt_minPrice.Text;
                string str3 = this.txt_maxPrice.Text;
                decimal? nullable = null;
                decimal? nullable2 = null;
                decimal i = 0M;
                if (!this.bDecimal(val, ref i))
                {
                    nullable = null;
                }
                else
                {
                    nullable = new decimal?(i);
                }
                if (!this.bDecimal(str3, ref i))
                {
                    nullable2 = null;
                }
                else
                {
                    nullable2 = new decimal?(i);
                }
                ProductQuery entity = new ProductQuery {
                    wid=this.wid,
                    Keywords = text,
                    ProductCode = "",
                    CategoryId = null,
                    PageSize = this.pager.PageSize,
                    PageIndex = this.pager.PageIndex,
                    SortOrder = SortAction.Desc,
                    SortBy = "DisplaySequence",
                    StartDate = null,
                    BrandId = null,
                    EndDate = null,
                    TypeId = null,
                    SaleStatus = this.status,
                    minPrice = nullable,
                    maxPrice = nullable2,
                    selectQuery = string.Format(" ProductId  in (  select ProductId from Hishop_PointExChange_Products where exChangeId={0})", this.exchangeId)
                };
                Globals.EntityCoding(entity, true);
                DbQueryResult products = ProductHelper.GetProducts(entity);
                DataTable data = (DataTable) products.Data;
                data.Columns.Add("ProductNumber");
                data.Columns.Add("PointNumber");
                data.Columns.Add("eachMaxNumber");
                data.Columns.Add("status");
                data.Columns.Add("seledStatus");
                data.Columns.Add("canSelStatus");
                data.Columns.Add("canChkStatus");
                DataTable table2 = PointExChangeHelper.GetProducts(this.exchangeId);
                if ((data != null) && (data.Rows.Count > 0))
                {
                    for (int j = data.Rows.Count - 1; j >= 0; j--)
                    {
                        DataRow[] rowArray = table2.Select(" productId='" + data.Rows[j]["ProductId"].ToString() + "'");
                        if (rowArray.Length > 0)
                        {
                            data.Rows[j]["ProductNumber"] = rowArray[0]["ProductNumber"];
                            data.Rows[j]["PointNumber"] = rowArray[0]["PointNumber"];
                            data.Rows[j]["eachMaxNumber"] = rowArray[0]["eachMaxNumber"];
                            data.Rows[j]["status"] = rowArray[0]["status"];
                        }
                        else
                        {
                            data.Rows.RemoveAt(j);
                        }
                    }
                }
                if (table2 != null)
                {
                    if ((data.Rows.Count > 0) && (table2.Rows.Count > 0))
                    {
                        for (int k = 0; k < data.Rows.Count; k++)
                        {
                            int num4 = int.Parse(data.Rows[k]["ProductId"].ToString());
                            if (table2.Select(string.Format("ProductId={0}", num4)).Length > 0)
                            {
                                data.Rows[k]["seledStatus"] = "''";
                                data.Rows[k]["canSelStatus"] = "none";
                                data.Rows[k]["canChkStatus"] = "disabled";
                            }
                            else
                            {
                                data.Rows[k]["seledStatus"] = "none";
                                data.Rows[k]["canSelStatus"] = "''";
                                data.Rows[k]["canChkStatus"] = string.Empty;
                            }
                        }
                    }
                    else if (data.Rows.Count > 0)
                    {
                        for (int m = 0; m < data.Rows.Count; m++)
                        {
                            data.Rows[m]["seledStatus"] = "none";
                            data.Rows[m]["canSelStatus"] = "''";
                            data.Rows[m]["canChkStatus"] = string.Empty;
                        }
                    }
                }
                this.grdProducts.DataSource = products.Data;
                this.grdProducts.DataBind();
                this.pager.TotalRecords = products.TotalRecords;
                this.lbSelectNumber.Text = (table2 != null) ? table2.Rows.Count.ToString() : "0";
                this.setInSaleAndStock();
            }
        }

        private bool bInt(string val, ref int i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            if (val.Contains(".") || val.Contains("-"))
            {
                return false;
            }
            return int.TryParse(val, out i);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            this.BindProducts(this.exchangeId);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            if (base.Request.Params.AllKeys.Contains<string>("id") && !this.bInt(base.Request["id"].ToString(), ref this.exchangeId))
            {
                this.exchangeId = 0;
            }
            if (!base.IsPostBack)
            {
                this.btnQuery.Click += new EventHandler(this.btnQuery_Click);
                this.BindProducts(this.exchangeId);
            }
        }

        private void setInSaleAndStock()
        {
            DataTable productNum = ProductHelper.GetProductNum(this.wid);
            this.lbsaleNumber.Text = productNum.Rows[0]["OnSale"].ToString();
            this.lbwareNumber.Text = productNum.Rows[0]["OnStock"].ToString();
        }
    }
}

