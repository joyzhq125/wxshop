namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Web.UI.WebControls;

    public class PrizeDeliveryDetail : AdminPage
    {
        private string Did;
        protected Literal txtAddress;
        protected Literal txtCourierNumber;
        protected Literal txtDeliever;
        protected Literal txtDTel;
        protected Literal txtExpressName;
        protected Literal txtGameTitle;
        protected Literal txtGameType;
        protected ListImage txtImage;
        protected Literal txtPlayTime;
        protected Literal txtPrizeGrade;
        protected Literal txtProductName;
        protected Literal txtReceiver;
        protected Literal txtRegionName;
        protected Literal txtStatus;
        protected Literal txtTel;

        protected PrizeDeliveryDetail() : base("m08", "yxp16")
        {
            this.Did = "";
        }

        private string Dbnull2str(object data)
        {
            return ((data == DBNull.Value) ? "" : data.ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Did = base.Request.QueryString["LogId"];
            if (!string.IsNullOrEmpty(this.Did))
            {
                PrizesDeliveQuery query = new PrizesDeliveQuery {
                    Status = -1,
                    SortBy = "LogId",
                    LogId = this.Did,
                    PageIndex = 1,
                    PageSize = 2,
                    PrizeType = -1
                };
                DataTable data = (DataTable) GameHelper.GetPrizesDeliveryList(query, "", "*").Data;
                if ((data != null) && (data.Rows.Count > 0))
                {
                    DataRow row = data.Rows[0];
                    this.txtStatus.Text = GameHelper.GetPrizesDeliveStatus(this.Dbnull2str(row["status"]));
                    this.txtCourierNumber.Text = this.Dbnull2str(row["CourierNumber"]);
                    this.txtExpressName.Text = this.Dbnull2str(row["ExpressName"]);
                    this.txtTel.Text = this.Dbnull2str(row["Tel"]);
                    this.txtDTel.Text = this.Dbnull2str(row["Tel"]);
                    this.txtReceiver.Text = this.Dbnull2str(row["Receiver"]);
                    this.txtDeliever.Text = this.Dbnull2str(row["Receiver"]);
                    if (this.txtTel.Text == "")
                    {
                        this.txtTel.Text = this.Dbnull2str(row["CellPhone"]);
                        this.txtDTel.Text = this.Dbnull2str(row["CellPhone"]);
                    }
                    if (this.txtDeliever.Text == "")
                    {
                        this.txtReceiver.Text = this.Dbnull2str(row["RealName"]);
                        this.txtDeliever.Text = this.Dbnull2str(row["RealName"]);
                    }
                    this.txtAddress.Text = this.Dbnull2str(row["Address"]);
                    this.txtRegionName.Text = this.Dbnull2str(row["ReggionPath"]);
                    if (this.txtRegionName.Text.Trim() != "")
                    {
                        string[] strArray = this.txtRegionName.Text.Trim().Split(new char[] { ',' });
                        this.txtRegionName.Text = RegionHelper.GetFullRegion(int.Parse(strArray[strArray.Length - 1]), ",");
                    }
                    this.txtPlayTime.Text = ((DateTime) row["PlayTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    this.txtPrizeGrade.Text = GameHelper.GetPrizeGradeName(this.Dbnull2str(row["PrizeGrade"]));
                    this.txtGameTitle.Text = row["GameTitle"].ToString();
                    this.txtGameType.Text = GameHelper.GetGameTypeName(row["GameType"].ToString());
                    this.txtProductName.Text = row["ProductName"].ToString();
                    this.txtImage.ImageUrl = (row["ThumbnailUrl100"] == DBNull.Value) ? "/utility/pics/none.gif" : row["ThumbnailUrl100"].ToString();
                }
            }
        }
    }
}

