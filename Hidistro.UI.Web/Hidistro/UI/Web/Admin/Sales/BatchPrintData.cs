namespace Hidistro.UI.Web.Admin.Sales
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class BatchPrintData : AdminPage
    {
        protected string Address;
        protected Button btnPrint;
        protected Button btnUpdateAddrdss;
        protected string CellPhone;
        protected string City;
        protected ShippersDropDownList ddlShoperTag;
        protected DropDownList ddlTemplates;
        protected string Departure;
        protected string Destination;
        protected string District;
        protected RegionSelector dropRegions;
        protected string height;
        protected Literal litNumber;
        protected string mailNo;
        protected string OrderId;
        protected static string orderIds = string.Empty;
        protected string OrderTotal;
        protected Panel pnlEmptySender;
        protected Panel pnlEmptyTemplates;
        protected Panel pnlShipper;
        protected Panel pnlTask;
        protected Panel pnlTaskEmpty;
        protected Panel pnlTemplates;
        protected int pringrows;
        protected string Province;
        protected string Remark;
        protected string ShipAddress;
        protected string ShipCellPhone;
        protected string ShipCity;
        protected string ShipDistrict;
        protected string ShipitemInfos;
        protected string Shipitemweith;
        protected string ShipperName;
        protected string ShipProvince;
        protected string ShipSizeAddress;
        protected string ShipSizeCity;
        protected string ShipSizeDistrict;
        protected string ShipSizeProvnce;
        protected string ShipTelPhone;
        protected string ShipTo;
        protected string ShipToDate;
        protected string ShipZipCode;
        protected string SiteName;
        private SiteSettings siteSettings;
        protected string SizeAddress;
        protected string SizeCellPhone;
        protected string SizeCity;
        protected string SizeDeparture;
        protected string SizeDestination;
        protected string SizeDistrict;
        protected string SizeitemInfos;
        protected string SizeOrderId;
        protected string SizeOrderTotal;
        protected string SizeProvnce;
        protected string SizeRemark;
        protected string SizeShipCellPhone;
        protected string SizeShipitemweith;
        protected string SizeShipperName;
        protected string SizeShipTelPhone;
        protected string SizeShipTo;
        protected string SizeShipToDate;
        protected string SizeShipZipCode;
        protected string SizeSiteName;
        protected string SizeTelPhone;
        protected string SizeZipcode;
        protected string TelPhone;
        protected string templateName;
        protected TextBox txtAddress;
        protected TextBox txtCellphone;
        protected TextBox txtShipTo;
        protected TextBox txtStartCode;
        protected TextBox txtTelphone;
        protected TextBox txtZipcode;
        protected string UpdateOrderIds;
        protected string width;
        protected string Zipcode;

        protected BatchPrintData() : base("", "")
        {
            this.mailNo = "";
            this.templateName = "";
            this.width = "";
            this.height = "";
            this.UpdateOrderIds = string.Empty;
            this.ShipperName = string.Empty;
            this.SizeShipperName = string.Empty;
            this.CellPhone = string.Empty;
            this.SizeCellPhone = string.Empty;
            this.TelPhone = string.Empty;
            this.SizeTelPhone = string.Empty;
            this.Address = string.Empty;
            this.SizeAddress = string.Empty;
            this.Zipcode = string.Empty;
            this.SizeZipcode = string.Empty;
            this.Province = string.Empty;
            this.SizeProvnce = string.Empty;
            this.City = string.Empty;
            this.SizeCity = string.Empty;
            this.District = string.Empty;
            this.SizeDistrict = string.Empty;
            this.ShipToDate = string.Empty;
            this.SizeShipToDate = string.Empty;
            this.OrderId = string.Empty;
            this.SizeOrderId = string.Empty;
            this.OrderTotal = string.Empty;
            this.SizeOrderTotal = string.Empty;
            this.Shipitemweith = string.Empty;
            this.SizeShipitemweith = string.Empty;
            this.Remark = string.Empty;
            this.SizeRemark = string.Empty;
            this.ShipitemInfos = string.Empty;
            this.SizeitemInfos = string.Empty;
            this.SiteName = string.Empty;
            this.SizeSiteName = string.Empty;
            this.ShipTo = string.Empty;
            this.SizeShipTo = string.Empty;
            this.ShipTelPhone = string.Empty;
            this.SizeShipTelPhone = string.Empty;
            this.ShipCellPhone = string.Empty;
            this.SizeShipCellPhone = string.Empty;
            this.ShipZipCode = string.Empty;
            this.SizeShipZipCode = string.Empty;
            this.ShipAddress = string.Empty;
            this.ShipSizeAddress = string.Empty;
            this.ShipProvince = string.Empty;
            this.ShipSizeProvnce = string.Empty;
            this.ShipCity = string.Empty;
            this.ShipSizeCity = string.Empty;
            this.ShipDistrict = string.Empty;
            this.ShipSizeDistrict = string.Empty;
            this.Departure = string.Empty;
            this.SizeDeparture = string.Empty;
            this.Destination = string.Empty;
            this.SizeDestination = string.Empty;
            this.siteSettings = SettingsManager.GetMasterSettings(false,wid);
        }

        private void btnbtnPrint_Click(object sender, EventArgs e)
        {
            this.printdata();
            string[] orderIds = this.UpdateOrderIds.Split(new char[] { ',' });
            List<string> list = new List<string>();
            foreach (string str in orderIds)
            {
                list.Add("'" + str + "'");
            }
            if (!string.IsNullOrEmpty(this.UpdateOrderIds))
            {
                OrderHelper.SetOrderExpressComputerpe(string.Join(",", list.ToArray()), this.templateName, this.templateName);
                OrderHelper.SetOrderShipNumber(orderIds, this.mailNo, this.templateName);
                OrderHelper.SetOrderPrinted(orderIds);
            }
            else
            {
                this.ShowMsg("订单当前状态不能打印！", false);
            }
        }

        private void btnUpdateAddrdss_Click(object sender, EventArgs e)
        {
            if (!this.dropRegions.GetSelectedRegionId().HasValue)
            {
                this.ShowMsg("请选择发货地区！", false);
            }
            else if (this.UpdateAddress())
            {
                this.ShowMsg("修改成功", true);
            }
            else
            {
                this.ShowMsg("修改失败，请确认信息填写正确或订单还未发货", false);
            }
        }

        private void ddlShoperTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadShipper();
        }

        private DataSet GetPrintData(string orderIds)
        {
            orderIds = "'" + orderIds.Replace(",", "','") + "'";
            return OrderHelper.GetOrdersAndLines(orderIds);
        }

        private void LoadShipper()
        {
            ShippersInfo shipper = SalesHelper.GetShipper(this.ddlShoperTag.SelectedValue);
            if (shipper != null)
            {
                this.txtAddress.Text = shipper.Address;
                this.txtCellphone.Text = shipper.CellPhone;
                this.txtShipTo.Text = shipper.ShipperName;
                this.txtTelphone.Text = shipper.TelPhone;
                this.txtZipcode.Text = shipper.Zipcode;
                this.dropRegions.SetSelectedRegionId(new int?(shipper.RegionId));
                this.pnlEmptySender.Visible = false;
                this.pnlShipper.Visible = true;
            }
            else
            {
                this.pnlShipper.Visible = false;
                this.pnlEmptySender.Visible = true;
            }
        }

        private void LoadTemplates()
        {
            DataTable isUserExpressTemplates = SalesHelper.GetIsUserExpressTemplates();
            if ((isUserExpressTemplates != null) && (isUserExpressTemplates.Rows.Count > 0))
            {
                this.ddlTemplates.Items.Add(new ListItem("-请选择-", ""));
                foreach (DataRow row in isUserExpressTemplates.Rows)
                {
                    this.ddlTemplates.Items.Add(new ListItem(row["ExpressName"].ToString(), row["XmlFile"].ToString()));
                }
                this.pnlEmptyTemplates.Visible = false;
                this.pnlTemplates.Visible = true;
            }
            else
            {
                this.pnlEmptyTemplates.Visible = true;
                this.pnlTemplates.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            if (!string.IsNullOrEmpty(base.Request["OrderIds"]))
            {
                orderIds = base.Request["OrderIds"];
                this.litNumber.Text = orderIds.Trim(new char[] { ',' }).Split(new char[] { ',' }).Length.ToString();
            }
            this.btnPrint.Click += new EventHandler(this.btnbtnPrint_Click);
            this.ddlShoperTag.SelectedIndexChanged += new EventHandler(this.ddlShoperTag_SelectedIndexChanged);
            this.btnUpdateAddrdss.Click += new EventHandler(this.btnUpdateAddrdss_Click);
            if (!this.Page.IsPostBack)
            {
                this.ddlShoperTag.DataBind();
                foreach (ShippersInfo info in SalesHelper.GetShippers(false,wid))
                {
                    if (info.IsDefault)
                    {
                        this.ddlShoperTag.SelectedValue = info.ShipperId;
                    }
                }
                this.LoadShipper();
                this.LoadTemplates();
            }
        }

        private void printdata()
        {
            this.mailNo = this.txtStartCode.Text.Trim();
            ShippersInfo shipper = SalesHelper.GetShipper(int.Parse(this.ddlShoperTag.SelectedValue.ToString()));
            if (shipper == null)
            {
                this.ShowMsg("请选择一个发货人", false);
            }
            else
            {
                string path = HttpContext.Current.Request.MapPath(string.Format("../../Storage/master/flex/{0}", this.ddlTemplates.SelectedValue));
                if (File.Exists(path))
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(path);
                    XmlNode node = document.DocumentElement.SelectSingleNode("//printer");
                    this.templateName = node.SelectSingleNode("kind").InnerText;
                    string innerText = node.SelectSingleNode("pic").InnerText;
                    string str2 = node.SelectSingleNode("size").InnerText;
                    this.width = str2.Split(new char[] { ':' })[0];
                    this.height = str2.Split(new char[] { ':' })[1];
                    DataSet printData = this.GetPrintData(orderIds);
                    this.pringrows = printData.Tables[0].Rows.Count;
                    foreach (DataRow row in printData.Tables[0].Rows)
                    {
                        this.UpdateOrderIds = this.UpdateOrderIds + row["orderid"] + ",";
                        DataTable table = printData.Tables[1];
                        string[] strArray = row["shippingRegion"].ToString().Split(new char[] { '，' });
                        foreach (XmlNode node2 in node.SelectNodes("item"))
                        {
                            string str3 = string.Empty;
                            string str4 = node2.SelectSingleNode("name").InnerText;
                            string str5 = node2.SelectSingleNode("position").InnerText;
                            string str6 = str5.Split(new char[] { ':' })[0];
                            string str7 = str5.Split(new char[] { ':' })[1];
                            string str8 = str5.Split(new char[] { ':' })[2];
                            string str9 = str5.Split(new char[] { ':' })[3];
                            string str10 = str9 + "," + str8 + "," + str6 + "," + str7;
                            string[] strArray2 = new string[] { "", "", "" };
                            if (shipper != null)
                            {
                                strArray2 = RegionHelper.GetFullRegion(shipper.RegionId, "-").Split(new char[] { '-' });
                            }
                            string str11 = string.Empty;
                            if (str4.Split(new char[] { '_' })[0] == "收货人-姓名")
                            {
                                this.ShipTo = this.ShipTo + "'" + this.ReplaceString(row["ShipTo"].ToString()) + "',";
                                if (!string.IsNullOrEmpty(row["ShipTo"].ToString().Trim()))
                                {
                                    this.SizeShipTo = this.SizeShipTo + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeShipTo = this.SizeShipTo + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "收货人-电话")
                            {
                                this.ShipTelPhone = this.ShipTelPhone + "'" + row["TelPhone"].ToString() + "',";
                                if (!string.IsNullOrEmpty(row["TelPhone"].ToString().Trim()))
                                {
                                    this.SizeShipTelPhone = this.SizeShipTelPhone + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeShipTelPhone = this.SizeShipTelPhone + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "收货人-手机")
                            {
                                this.ShipCellPhone = this.ShipCellPhone + "'" + row["CellPhone"].ToString() + "',";
                                if (!string.IsNullOrEmpty(row["CellPhone"].ToString().Trim()))
                                {
                                    this.SizeShipCellPhone = this.SizeShipCellPhone + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeShipCellPhone = this.SizeShipCellPhone + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "收货人-邮编")
                            {
                                this.ShipZipCode = this.ShipZipCode + "'" + row["ZipCode"].ToString() + "',";
                                if (!string.IsNullOrEmpty(row["ZipCode"].ToString().Trim()))
                                {
                                    this.SizeShipZipCode = this.SizeShipZipCode + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeShipZipCode = this.SizeShipZipCode + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "收货人-地址")
                            {
                                this.ShipAddress = this.ShipAddress + "'" + this.ReplaceString(row["Address"].ToString()) + "',";
                                if (!string.IsNullOrEmpty(row["Address"].ToString().Trim()))
                                {
                                    this.ShipSizeAddress = this.ShipSizeAddress + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.ShipSizeAddress = this.ShipSizeAddress + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "收货人-地区1级")
                            {
                                if (strArray.Length > 0)
                                {
                                    str3 = strArray[0];
                                }
                                this.ShipProvince = this.ShipProvince + "'" + str3 + "',";
                                if (!string.IsNullOrEmpty(str3.Trim()))
                                {
                                    this.ShipSizeProvnce = this.ShipSizeProvnce + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.ShipSizeProvnce = this.ShipSizeProvnce + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "收货人-地区2级")
                            {
                                str3 = string.Empty;
                                if (strArray.Length > 1)
                                {
                                    str3 = strArray[1];
                                }
                                this.ShipCity = this.ShipCity + "'" + str3 + "',";
                                if (!string.IsNullOrEmpty(str3.Trim()))
                                {
                                    this.ShipSizeCity = this.ShipSizeCity + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.ShipSizeCity = this.ShipSizeCity + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "目的地-地区")
                            {
                                str3 = string.Empty;
                                if (strArray.Length > 1)
                                {
                                    str3 = strArray[1];
                                }
                                this.Destination = this.Destination + "'" + str3 + "',";
                                if (!string.IsNullOrEmpty(str3.Trim()))
                                {
                                    this.SizeDestination = this.SizeDestination + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeDestination = this.SizeDestination + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "收货人-地区3级")
                            {
                                str3 = string.Empty;
                                if (strArray.Length > 2)
                                {
                                    str3 = strArray[2];
                                }
                                this.ShipDistrict = this.ShipDistrict + "'" + str3 + "',";
                                if (!string.IsNullOrEmpty(str3.Trim()))
                                {
                                    this.ShipSizeDistrict = this.ShipSizeDistrict + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.ShipSizeDistrict = this.ShipSizeDistrict + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "送货-上门时间")
                            {
                                this.ShipToDate = this.ShipToDate + "'" + row["ShipToDate"].ToString() + "',";
                                if (!string.IsNullOrEmpty(row["ShipToDate"].ToString().Trim()))
                                {
                                    this.SizeShipToDate = this.SizeShipToDate + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeShipToDate = this.SizeShipToDate + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "订单-订单号")
                            {
                                this.OrderId = this.OrderId + "'订单号：" + row["OrderId"].ToString() + "',";
                                if (!string.IsNullOrEmpty(row["OrderId"].ToString().Trim()))
                                {
                                    this.SizeOrderId = this.SizeOrderId + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeOrderId = this.SizeOrderId + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "订单-总金额")
                            {
                                if (!string.IsNullOrEmpty(row["OrderTotal"].ToString().Trim()))
                                {
                                    this.OrderTotal = this.OrderTotal + "'" + decimal.Parse(row["OrderTotal"].ToString()).ToString("F2") + "',";
                                }
                                if (!string.IsNullOrEmpty(row["OrderTotal"].ToString().Trim()))
                                {
                                    this.SizeOrderTotal = this.SizeOrderTotal + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeOrderTotal = this.SizeOrderTotal + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "订单-详情")
                            {
                                DataRow[] rowArray = table.Select(" OrderId='" + row["OrderId"] + "'");
                                string str = string.Empty;
                                if (rowArray.Length > 0)
                                {
                                    foreach (DataRow row2 in rowArray)
                                    {
                                        str = string.Concat(new object[] { str, "规格", row2["SKUContent"], " 数量", row2["ShipmentQuantity"], "货号 :", row2["SKU"] });
                                    }
                                    str = str.Replace(";", "");
                                }
                                if (!string.IsNullOrEmpty(str.Trim()))
                                {
                                    this.SizeitemInfos = this.SizeitemInfos + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeitemInfos = this.SizeitemInfos + "'0,0,0,0',";
                                }
                                this.ShipitemInfos = this.ShipitemInfos + "'" + this.ReplaceString(str) + "',";
                            }
                            if (str4.Split(new char[] { '_' })[0] == "订单-物品总重量")
                            {
                                decimal result = 0M;
                                decimal.TryParse(row["Weight"].ToString(), out result);
                                this.Shipitemweith = this.Shipitemweith + "'" + result.ToString("F2") + "',";
                                if (!string.IsNullOrEmpty(result.ToString().Trim()))
                                {
                                    this.SizeShipitemweith = this.SizeShipitemweith + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeShipitemweith = this.SizeShipitemweith + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "订单-备注")
                            {
                                this.Remark = this.Remark + "'" + this.ReplaceString(row["Remark"].ToString()) + "',";
                                if (!string.IsNullOrEmpty(row["Remark"].ToString().Trim()))
                                {
                                    this.SizeRemark = this.SizeRemark + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeRemark = this.SizeRemark + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "发货人-姓名")
                            {
                                this.ShipperName = this.ShipperName + "'" + this.ReplaceString(shipper.ShipperName) + "',";
                                if (!string.IsNullOrEmpty(shipper.ShipperName.Trim()))
                                {
                                    this.SizeShipperName = this.SizeShipperName + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeShipperName = this.SizeShipperName + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "发货人-电话")
                            {
                                this.TelPhone = this.TelPhone + "'" + shipper.TelPhone + "',";
                                if (!string.IsNullOrEmpty(shipper.TelPhone.Trim()))
                                {
                                    this.SizeTelPhone = this.SizeTelPhone + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeTelPhone = this.SizeTelPhone + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "发货人-手机")
                            {
                                this.CellPhone = this.CellPhone + "'" + shipper.CellPhone + "',";
                                if (!string.IsNullOrEmpty(shipper.CellPhone.Trim()))
                                {
                                    this.SizeCellPhone = this.SizeCellPhone + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeCellPhone = this.SizeCellPhone + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "发货人-邮编")
                            {
                                this.Zipcode = this.Zipcode + "'" + shipper.Zipcode + "',";
                                if (!string.IsNullOrEmpty(shipper.Zipcode.Trim()))
                                {
                                    this.SizeZipcode = this.SizeZipcode + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeZipcode = this.SizeZipcode + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "发货人-地址")
                            {
                                this.Address = this.Address + "'" + this.ReplaceString(shipper.Address) + "',";
                                if (!string.IsNullOrEmpty(shipper.Address.Trim()))
                                {
                                    this.SizeAddress = this.SizeAddress + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeAddress = this.SizeAddress + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "发货人-地区1级")
                            {
                                if (strArray2.Length > 0)
                                {
                                    str11 = strArray2[0];
                                }
                                this.Province = this.Province + "'" + str11 + "',";
                                if (!string.IsNullOrEmpty(str11.Trim()))
                                {
                                    this.SizeProvnce = this.SizeProvnce + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeProvnce = this.SizeProvnce + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "发货人-地区2级")
                            {
                                str11 = str11 + string.Empty;
                                if (strArray2.Length > 1)
                                {
                                    str11 = strArray2[1];
                                }
                                this.City = this.City + "'" + str11 + "',";
                                if (!string.IsNullOrEmpty(str11.Trim()))
                                {
                                    this.SizeCity = this.SizeCity + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeCity = this.SizeCity + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "始发地-地区")
                            {
                                str11 = str11 + string.Empty;
                                if (strArray2.Length > 1)
                                {
                                    str11 = strArray2[1];
                                }
                                this.Departure = this.Departure + "'" + str11 + "',";
                                if (!string.IsNullOrEmpty(str11.Trim()))
                                {
                                    this.SizeDeparture = this.SizeDeparture + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeDeparture = this.SizeDeparture + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "发货人-地区3级")
                            {
                                str11 = str11 + string.Empty;
                                if (strArray2.Length > 2)
                                {
                                    str11 = strArray2[2];
                                }
                                this.District = this.District + "'" + str11 + "',";
                                if (!string.IsNullOrEmpty(str11.Trim()))
                                {
                                    this.SizeDistrict = this.SizeDistrict + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeDistrict = this.SizeDistrict + "'0,0,0,0',";
                                }
                            }
                            if (str4.Split(new char[] { '_' })[0] == "网店名称")
                            {
                                this.SiteName = this.SiteName + "'" + this.ReplaceString(this.siteSettings.SiteName) + "',";
                                if (!string.IsNullOrEmpty(this.siteSettings.SiteName.Trim()))
                                {
                                    this.SizeSiteName = this.SizeSiteName + "'" + str10 + "',";
                                }
                                else
                                {
                                    this.SizeSiteName = this.SizeSiteName + "'0,0,0,0',";
                                }
                            }
                        }
                    }
                    this.UpdateOrderIds = this.UpdateOrderIds.TrimEnd(new char[] { ',' });
                    this.PrintPage(this.width, this.height);
                }
            }
        }

        private void PrintPage(string pagewidth, string pageheght)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<script language='javascript'>");
            builder.Append("function clicks(){");
            if (!string.IsNullOrEmpty(this.SizeShipperName.Trim()))
            {
                builder.Append(" var ShipperName=[" + this.ShipperName.Substring(0, this.ShipperName.Length - 1) + "];var SizeShipperName=[" + this.SizeShipperName.Substring(0, this.SizeShipperName.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeCellPhone.Trim()))
            {
                builder.Append(" var CellPhone=[" + this.CellPhone.Substring(0, this.CellPhone.Length - 1) + "];var SizeCellPhone=[" + this.SizeCellPhone.Substring(0, this.SizeCellPhone.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeTelPhone.Trim()))
            {
                builder.Append(" var TelPhone=[" + this.TelPhone.Substring(0, this.TelPhone.Length - 1) + "];var SizeTelPhone=[" + this.SizeTelPhone.Substring(0, this.SizeTelPhone.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeAddress.Trim()))
            {
                builder.Append(" var Address=[" + this.Address.Substring(0, this.Address.Length - 1) + "];var SizeAddress=[" + this.SizeAddress.Substring(0, this.SizeAddress.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeZipcode.Trim()))
            {
                builder.Append(" var Zipcode=[" + this.Zipcode.Substring(0, this.Zipcode.Length - 1) + "];var SizeZipcode=[" + this.SizeZipcode.Substring(0, this.SizeZipcode.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeProvnce.Trim()))
            {
                builder.Append(" var Province=[" + this.Province.Substring(0, this.Province.Length - 1) + "];var SizeProvnce=[" + this.SizeProvnce.Substring(0, this.SizeProvnce.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeCity.Trim()))
            {
                builder.Append(" var City=[" + this.City.Substring(0, this.City.Length - 1) + "];var SizeCity=[" + this.SizeCity.Substring(0, this.SizeCity.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeDistrict.Trim()))
            {
                builder.Append(" var District=[" + this.District.Substring(0, this.District.Length - 1) + "];var SizeDistrict=[" + this.SizeDistrict.Substring(0, this.SizeDistrict.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeShipToDate.Trim()))
            {
                builder.Append(" var ShipToDate=[" + this.ShipToDate.Substring(0, this.ShipToDate.Length - 1) + "];var SizeShipToDate=[" + this.SizeShipToDate.Substring(0, this.SizeShipToDate.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeOrderId.Trim()))
            {
                builder.Append(" var OrderId=[" + this.OrderId.Substring(0, this.OrderId.Length - 1) + "];var SizeOrderId=[" + this.SizeOrderId.Substring(0, this.SizeOrderId.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeOrderTotal.Trim()))
            {
                builder.Append(" var OrderTotal=[" + this.OrderTotal.Substring(0, this.OrderTotal.Length - 1) + "];var SizeOrderTotal=[" + this.SizeOrderTotal.Substring(0, this.SizeOrderTotal.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeShipitemweith.Trim()))
            {
                builder.Append(" var Shipitemweith=[" + this.Shipitemweith.Substring(0, this.Shipitemweith.Length - 1) + "];var SizeShipitemweith=[" + this.SizeShipitemweith.Substring(0, this.SizeShipitemweith.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeRemark.Trim()))
            {
                builder.Append(" var Remark=[" + this.Remark.Substring(0, this.Remark.Length - 1) + "];var SizeRemark=[" + this.SizeRemark.Substring(0, this.SizeRemark.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeitemInfos.Trim()))
            {
                builder.Append(" var ShipitemInfos=[" + this.ShipitemInfos.Substring(0, this.ShipitemInfos.Length - 1) + "];var SizeitemInfos=[" + this.SizeitemInfos.Substring(0, this.SizeitemInfos.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeSiteName.Trim()))
            {
                builder.Append(" var SiteName=[" + this.SiteName.Substring(0, this.SiteName.Length - 1) + "];var SizeSiteName=[" + this.SizeSiteName.Substring(0, this.SizeSiteName.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeShipTo.Trim()))
            {
                builder.Append(" var ShipTo=[" + this.ShipTo.Substring(0, this.ShipTo.Length - 1) + "];var SizeShipTo=[" + this.SizeShipTo.Substring(0, this.SizeShipTo.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeShipTelPhone.Trim()))
            {
                builder.Append(" var ShipTelPhone=[" + this.ShipTelPhone.Substring(0, this.ShipTelPhone.Length - 1) + "];var SizeShipTelPhone=[" + this.SizeShipTelPhone.Substring(0, this.SizeShipTelPhone.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeShipCellPhone.Trim()))
            {
                builder.Append(" var ShipCellPhone=[" + this.ShipCellPhone.Substring(0, this.ShipCellPhone.Length - 1) + "];var SizeShipCellPhone=[" + this.SizeShipCellPhone.Substring(0, this.SizeShipCellPhone.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeShipZipCode.Trim()))
            {
                builder.Append(" var ShipZipCode=[" + this.ShipZipCode.Substring(0, this.ShipZipCode.Length - 1) + "];var SizeShipZipCode=[" + this.SizeShipZipCode.Substring(0, this.SizeShipZipCode.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.ShipSizeAddress.Trim()))
            {
                builder.Append(" var ShipAddress=[" + this.ShipAddress.Substring(0, this.ShipAddress.Length - 1) + "];var ShipSizeAddress=[" + this.ShipSizeAddress.Substring(0, this.ShipSizeAddress.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.ShipSizeProvnce.Trim()))
            {
                builder.Append(" var ShipProvince=[" + this.ShipProvince.Substring(0, this.ShipProvince.Length - 1) + "];var ShipSizeProvnce=[" + this.ShipSizeProvnce.Substring(0, this.ShipSizeProvnce.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.ShipSizeCity.Trim()))
            {
                builder.Append(" var ShipCity=[" + this.ShipCity.Substring(0, this.ShipCity.Length - 1) + "];var ShipSizeCity=[" + this.ShipSizeCity.Substring(0, this.ShipSizeCity.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.ShipSizeDistrict.Trim()))
            {
                builder.Append(" var ShipDistrict=[" + this.ShipDistrict.Substring(0, this.ShipDistrict.Length - 1) + "];var ShipSizeDistrict=[" + this.ShipSizeDistrict.Substring(0, this.ShipSizeDistrict.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeDeparture.Trim()))
            {
                builder.Append(" var Departure=[" + this.Departure.Substring(0, this.Departure.Length - 1) + "];var SizeDeparture=[" + this.SizeDeparture.Substring(0, this.SizeDeparture.Length - 1) + "];");
            }
            if (!string.IsNullOrEmpty(this.SizeDestination.Trim()))
            {
                builder.Append(" var Destination=[" + this.Destination.Substring(0, this.Destination.Length - 1) + "];var SizeDestination=[" + this.SizeDestination.Substring(0, this.SizeDestination.Length - 1) + "];");
            }
            builder.Append(" var LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));");
            builder.Append(" try{ ");
            builder.Append("  for(var i=0;i<" + this.pringrows + ";++i){ ");
            builder.Append("showdiv();");
            builder.Append(string.Concat(new object[] { " LODOP.SET_PRINT_PAGESIZE (1,", decimal.Parse(pagewidth) * 10M, ",", decimal.Parse(pageheght) * 10M, ",\"\");" }));
            builder.Append(" LODOP.SET_PRINT_STYLE(\"FontSize\",12);");
            builder.Append(" LODOP.SET_PRINT_STYLE(\"Bold\",1);");
            if (!string.IsNullOrEmpty(this.SizeShipperName.Trim()))
            {
                builder.Append("LODOP.ADD_PRINT_TEXT(SizeShipperName[i].split(',')[0],SizeShipperName[i].split(',')[1],SizeShipperName[i].split(',')[2],SizeShipperName[i].split(',')[3],ShipperName[0]);");
            }
            if (!string.IsNullOrEmpty(this.SizeCellPhone.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeCellPhone[i].split(',')[0],SizeCellPhone[i].split(',')[1],SizeCellPhone[i].split(',')[2],SizeCellPhone[i].split(',')[3],CellPhone[0]);");
            }
            if (!string.IsNullOrEmpty(this.SizeTelPhone.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeTelPhone[i].split(',')[0],SizeTelPhone[i].split(',')[1],SizeTelPhone[i].split(',')[2],SizeTelPhone[i].split(',')[3],TelPhone[0]);");
            }
            if (!string.IsNullOrEmpty(this.SizeAddress.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeAddress[i].split(',')[0],SizeAddress[i].split(',')[1],SizeAddress[i].split(',')[2],SizeAddress[i].split(',')[3],Address[0]);");
            }
            if (!string.IsNullOrEmpty(this.SizeZipcode.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeZipcode[i].split(',')[0],Zipcode[0]);");
            }
            if (!string.IsNullOrEmpty(this.SizeProvnce.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeProvnce[i].split(',')[0],SizeProvnce[i].split(',')[1],SizeProvnce[i].split(',')[2],SizeProvnce[i].split(',')[3],Province[0]);");
            }
            if (!string.IsNullOrEmpty(this.SizeCity.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeCity[i].split(',')[0],SizeCity[i].split(',')[1],SizeCity[i].split(',')[2],SizeCity[i].split(',')[3],City[0]);");
            }
            if (!string.IsNullOrEmpty(this.SizeDistrict.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeDistrict[i].split(',')[0],SizeDistrict[i].split(',')[1],SizeDistrict[i].split(',')[2],SizeDistrict[i].split(',')[3],District[0]);");
            }
            if (!string.IsNullOrEmpty(this.SizeShipToDate.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipToDate[i].split(',')[0],SizeShipToDate[i].split(',')[1],SizeShipToDate[i].split(',')[2],SizeShipToDate[i].split(',')[3],ShipToDate[i]);");
            }
            if (!string.IsNullOrEmpty(this.SizeOrderId.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeOrderId[i].split(',')[0],SizeOrderId[i].split(',')[1],SizeOrderId[i].split(',')[2],SizeOrderId[i].split(',')[3],OrderId[i]);");
            }
            if (!string.IsNullOrEmpty(this.SizeOrderTotal.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeOrderTotal[i].split(',')[0],SizeOrderTotal[i].split(',')[1],SizeOrderTotal[i].split(',')[2],SizeOrderTotal[i].split(',')[3],OrderTotal[i]);");
            }
            if (!string.IsNullOrEmpty(this.SizeShipitemweith.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipitemweith[i].split(',')[0],SizeShipitemweith[i].split(',')[1],SizeShipitemweith[i].split(',')[2],SizeShipitemweith[i].split(',')[3],Shipitemweith[i]);");
            }
            if (!string.IsNullOrEmpty(this.SizeRemark.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeRemark[i].split(',')[0],SizeRemark[i].split(',')[1],SizeRemark[i].split(',')[2],SizeRemark[i].split(',')[3],Remark[i]);");
            }
            if (!string.IsNullOrEmpty(this.SizeitemInfos.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeitemInfos[i].split(',')[0],SizeitemInfos[i].split(',')[1],SizeitemInfos[i].split(',')[2],SizeitemInfos[i].split(',')[3],ShipitemInfos[i]);");
            }
            if (!string.IsNullOrEmpty(this.SizeSiteName.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeSiteName[i].split(',')[0],SizeSiteName[i].split(',')[1],SizeSiteName[i].split(',')[2],SizeSiteName[i].split(',')[3],SiteName[i]);");
            }
            if (!string.IsNullOrEmpty(this.SizeShipTo.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipTo[i].split(',')[0],SizeShipTo[i].split(',')[1],SizeShipTo[i].split(',')[2],SizeShipTo[i].split(',')[3],ShipTo[i]);");
            }
            if (!string.IsNullOrEmpty(this.SizeShipTelPhone.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipTelPhone[i].split(',')[0],SizeShipTelPhone[i].split(',')[1],SizeShipTelPhone[i].split(',')[2],SizeShipTelPhone[i].split(',')[3],ShipTelPhone[i]);");
            }
            if (!string.IsNullOrEmpty(this.SizeShipCellPhone.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipCellPhone[i].split(',')[0],SizeShipCellPhone[i].split(',')[1],SizeShipCellPhone[i].split(',')[2],SizeShipCellPhone[i].split(',')[3],ShipCellPhone[i]);");
            }
            if (!string.IsNullOrEmpty(this.SizeShipZipCode.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipZipCode[i].split(',')[0],SizeShipZipCode[i].split(',')[1],SizeShipZipCode[i].split(',')[2],SizeShipZipCode[i].split(',')[3],ShipZipCode[i]);");
            }
            if (!string.IsNullOrEmpty(this.ShipSizeAddress.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(ShipSizeAddress[i].split(',')[0],ShipSizeAddress[i].split(',')[1],ShipSizeAddress[i].split(',')[2],ShipSizeAddress[i].split(',')[3],ShipAddress[i]);");
            }
            if (!string.IsNullOrEmpty(this.ShipSizeProvnce.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(ShipSizeProvnce[i].split(',')[0],ShipSizeProvnce[i].split(',')[1],ShipSizeProvnce[i].split(',')[2],ShipSizeProvnce[i].split(',')[3],ShipProvince[i]);");
            }
            if (!string.IsNullOrEmpty(this.ShipSizeCity.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(ShipSizeCity[i].split(',')[0],ShipSizeCity[i].split(',')[1],ShipSizeCity[i].split(',')[2],ShipSizeCity[i].split(',')[3],ShipCity[i]);");
            }
            if (!string.IsNullOrEmpty(this.ShipSizeDistrict.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(ShipSizeDistrict[i].split(',')[0],ShipSizeDistrict[i].split(',')[1],ShipSizeDistrict[i].split(',')[2],ShipSizeDistrict[i].split(',')[3],ShipDistrict[i]);");
            }
            if (!string.IsNullOrEmpty(this.SizeDeparture.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeDeparture[i].split(',')[0],SizeDeparture[i].split(',')[1],SizeDeparture[i].split(',')[2],SizeDeparture[i].split(',')[3],Departure[0]);");
            }
            if (!string.IsNullOrEmpty(this.SizeDestination.Trim()))
            {
                builder.Append(" LODOP.ADD_PRINT_TEXT(SizeDestination[i].split(',')[0],SizeDestination[i].split(',')[1],SizeDestination[i].split(',')[2],SizeDestination[i].split(',')[3],Destination[i]);");
            }
            builder.Append(" LODOP.PRINT();");
            builder.Append("   }");
            builder.Append(" setTimeout(\"hidediv()\",3000);");
            builder.Append("  }catch(e){ alert(\"请先安装打印控件！\");hidediv();return false;}");
            builder.Append("}");
            builder.Append(" setTimeout(\"clicks()\",1000); ");
            builder.Append("</script>");
            base.ClientScript.RegisterStartupScript(base.GetType(), "myscript", builder.ToString());
        }

        private string ReplaceString(string str)
        {
            return str.Replace("'", "＇");
        }

        private bool UpdateAddress()
        {
            ShippersInfo shipper = SalesHelper.GetShipper(this.ddlShoperTag.SelectedValue);
            if (shipper != null)
            {
                shipper.Address = this.txtAddress.Text;
                shipper.CellPhone = this.txtCellphone.Text;
                shipper.RegionId = this.dropRegions.GetSelectedRegionId().Value;
                shipper.ShipperName = this.txtShipTo.Text;
                shipper.TelPhone = this.txtTelphone.Text;
                shipper.Zipcode = this.txtZipcode.Text;
                return SalesHelper.UpdateShipper(shipper);
            }
            return false;
        }
    }
}

