namespace Hidistro.UI.Web.Admin.member
{
    using Ajax;
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.StatisticsReport;
    using Hidistro.Entities.Store;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Plugins;
    using Ionic.Zlib;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    [PrivilegeCheck(Privilege.Members)]
    public class ManageMembers : AdminPage
    {
        private bool? approved;
        protected Button btnExport;
        protected Button btnSearchButton;
        protected Button btnSendMessage;
        public string clientType;
        protected DropDownList DDL_ReferralUser;
        protected MemberGradeDropDownList DDL_User;
        protected ExportFieldsCheckBoxList exportFieldsCheckBoxList;
        protected ExportFormatRadioButtonList exportFormatRadioButtonList;
        protected Button GradeCheck;
        protected MemberGradeDropDownList GradeCheckList;
        protected Grid grdMemberList;
        protected PageSize hrefPageSize;
        protected Literal ListActive;
        protected Literal Listfrozen;
        protected Literal Literal1;
        protected Literal Literal2;
        protected ImageLinkButton lkbDelectCheck1;
        private UpdateStatistics myEvent;
        private StatisticNotifier myNotifier;
        protected Pager pager;
        protected Pager pager1;
        private int? rankId;
        protected MemberGradeDropDownList rankList;
        private string realName;
        protected Script Script4;
        protected Script Script5;
        protected Script Script6;
        private string searchKey;
        protected HtmlForm thisForm;
        protected TextBox txtContent;
        protected HtmlTextArea txtmsgcontent;
        protected TextBox txtPhone;
        protected TextBox txtRealName;
        protected TextBox txtSearchText;
        public string ValidSmsNum;
        private int? vipCard;
        //protected string wid;

        public ManageMembers() : base("m04", "hyp02")
        {
            this.ValidSmsNum = "0";
            this.myNotifier = new StatisticNotifier();
            this.myEvent = new UpdateStatistics();
        }

        protected void BindData()
        {
            MemberQuery query = new MemberQuery {
                Username = this.searchKey,
                UserBindName = this.realName,
                GradeId = this.rankId,
                PageIndex = this.pager.PageIndex,
                IsApproved = this.approved,
                SortBy = this.grdMemberList.SortOrderBy,
                PageSize = this.pager.PageSize,
                Stutas = UserStatus.Normal,
                EndTime = new DateTime?(DateTime.Now),
                StartTime = new DateTime?(DateTime.Now.AddDays((double) -this.GetSiteSetting().ActiveDay)),
                CellPhone = (base.Request.QueryString["phone"] != null) ? base.Request.QueryString["phone"] : "",
                ClientType = (base.Request.QueryString["clientType"] != null) ? base.Request.QueryString["clientType"] : "",
                wid = this.wid
            };
            if (this.grdMemberList.SortOrder.ToLower() == "desc")
            {
                query.SortOrder = SortAction.Desc;
            }
            if (this.vipCard.HasValue && (this.vipCard.Value != 0))
            {
                query.HasVipCard = new bool?(this.vipCard.Value == 1);
            }
            DbQueryResult members = MemberHelper.GetMembers(query);
            this.grdMemberList.DataSource = members.Data;
            this.grdMemberList.DataBind();
            this.pager1.TotalRecords = this.pager.TotalRecords = members.TotalRecords;
        }

        public void BindDDL()
        {
            this.rankList.wid = this.wid;
            this.rankList.DataBind();
            this.rankList.SelectedValue = this.rankId;

            this.GradeCheckList.wid = this.wid;
            this.GradeCheckList.DataBind();
            this.GradeCheckList.SelectedValue = this.rankId;

            this.DDL_User.wid = this.wid;
            this.DDL_User.DataBind();
            this.DDL_User.SelectedValue = this.rankId;
            this.BindDDLDistributors();
        }

        public void BindDDLDistributors()
        {
            DistributorsQuery query = new DistributorsQuery();
            this.DDL_ReferralUser.DataSource = DistributorsBrower.SelectDistributors(query);
            this.DDL_ReferralUser.DataTextField = "StoreName";
            this.DDL_ReferralUser.DataValueField = "UserId";
            this.DDL_ReferralUser.DataBind();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.exportFieldsCheckBoxList.SelectedItem == null)
            {
                this.ShowMsg("请选择需要导出的会员信息", false);
            }
            else
            {
                IList<string> fields = new List<string>();
                IList<string> list2 = new List<string>();
                foreach (ListItem item in this.exportFieldsCheckBoxList.Items)
                {
                    if (item.Selected)
                    {
                        fields.Add(item.Value);
                        list2.Add(item.Text);
                    }
                }
                MemberQuery query = new MemberQuery {
                    Username = this.searchKey,
                    Realname = this.realName,
                    GradeId = this.rankId
                };
                if (this.vipCard.HasValue && (this.vipCard.Value != 0))
                {
                    query.HasVipCard = new bool?(this.vipCard.Value == 1);
                }
                DataTable membersNopage = MemberHelper.GetMembersNopage(query, fields);
                StringBuilder builder = new StringBuilder();
                foreach (string str in list2)
                {
                    builder.Append(str + ",");
                    if (str == list2[list2.Count - 1])
                    {
                        builder = builder.Remove(builder.Length - 1, 1);
                        builder.Append("\r\n");
                    }
                }
                foreach (DataRow row in membersNopage.Rows)
                {
                    foreach (string str2 in fields)
                    {
                        builder.Append(row[str2]).Append(",");
                        if (str2 == fields[list2.Count - 1])
                        {
                            builder = builder.Remove(builder.Length - 1, 1);
                            builder.Append("\r\n");
                        }
                    }
                }
                this.Page.Response.Clear();
                this.Page.Response.Buffer = false;
                this.Page.Response.Charset = "GB2312";
                if (this.exportFormatRadioButtonList.SelectedValue == "csv")
                {
                    this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberInfo.csv");
                    this.Page.Response.ContentType = "application/octet-stream";
                }
                else
                {
                    this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberInfo.txt");
                    this.Page.Response.ContentType = "application/vnd.ms-word";
                }
                this.Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
                this.Page.EnableViewState = false;
                this.Page.Response.Write(builder.ToString());
                this.Page.Response.End();
            }
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            SiteSettings siteSetting = this.GetSiteSetting();
            string sMSSender = siteSetting.SMSSender;
            if (string.IsNullOrEmpty(sMSSender))
            {
                this.ShowMsg("请先选择发送方式", false);
            }
            else
            {
                ConfigData data = null;
                if (siteSetting.SMSEnabled)
                {
                    data = new ConfigData(HiCryptographer.Decrypt(siteSetting.SMSSettings));
                }
                if (data == null)
                {
                    this.ShowMsg("请先选择发送方式并填写配置信息", false);
                }
                else if (!data.IsValid)
                {
                    string msg = "";
                    foreach (string str3 in data.ErrorMsgs)
                    {
                        msg = msg + Formatter.FormatErrorMessage(str3);
                    }
                    this.ShowMsg(msg, false);
                }
                else
                {
                    string str4 = this.txtmsgcontent.Value.Trim();
                    if (string.IsNullOrEmpty(str4))
                    {
                        this.ShowMsg("请先填写发送的内容信息", false);
                    }
                    else
                    {
                        int smsValidCount = this.GetSmsValidCount();
                        string str5 = null;
                        foreach (GridViewRow row in this.grdMemberList.Rows)
                        {
                            CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                            if (box.Checked)
                            {
                                string str6 = ((DataBoundLiteralControl) row.Controls[4].Controls[0]).Text.Trim().Replace("<div></div>", "").Replace("&nbsp;", "");
                                HiddenField field = (HiddenField) row.FindControl("hidCellPhone");
                                str6 = field.Value;
                                if (!string.IsNullOrEmpty(str6) && Regex.IsMatch(str6, @"^(13|14|15|18)\d{9}$"))
                                {
                                    str5 = str5 + str6 + ",";
                                }
                            }
                        }
                        if (str5 == null)
                        {
                            this.ShowMsg("请先选择要发送的会员或检测所选手机号格式是否正确", false);
                        }
                        else
                        {
                            str5 = str5.Substring(0, str5.Length - 1);
                            string[] phoneNumbers = null;
                            if (str5.Contains(","))
                            {
                                phoneNumbers = str5.Split(new char[] { ',' });
                            }
                            else
                            {
                                phoneNumbers = new string[] { str5 };
                            }
                            if (smsValidCount < phoneNumbers.Length)
                            {
                                this.ShowMsg("发送失败，您的剩余短信条数不足。可用条数：" + smsValidCount.ToString(), false);
                            }
                            else
                            {
                                string str7;
                                bool success = SMSSender.CreateInstance(sMSSender, data.SettingsXml).Send(phoneNumbers, str4, out str7);
                                this.ShowMsg(str7, success);
                                this.ValidSmsNum = this.GetSmsValidCount().ToString();
                            }
                        }
                    }
                }
            }
        }

        private void ddlApproved_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ReBind(false);
        }

        [AjaxMethod]
        public string DelUser(int userID)
        {
            if (VShopHelper.IsExistUsers(userID.ToString()) > 0)
            {
                return "isExistVShop";
            }
            if (MemberHelper.Delete2(userID))
            {
                return "success";
            }
            return "fail";
        }

        protected int GetAmount(SiteSettings settings)
        {
            int num = 0;
            if (!string.IsNullOrEmpty(settings.SMSSettings))
            {
                int num2;
                string xml = HiCryptographer.Decrypt(settings.SMSSettings);
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                string innerText = document.SelectSingleNode("xml/Appkey").InnerText;
                string postData = "method=getAmount&Appkey=" + innerText;
                string s = this.PostData("http://sms.kuaidiantong.cn/getAmount.aspx", postData);
                if (int.TryParse(s, out num2))
                {
                    num = Convert.ToInt32(s);
                }
            }
            return num;
        }

        private SiteSettings GetSiteSetting()
        {
            return SettingsManager.GetMasterSettings(false,wid);
        }

        private int GetSmsValidCount()
        {
            SiteSettings siteSetting = this.GetSiteSetting();
            if (siteSetting.SMSEnabled)
            {
                return int.Parse(this.GetAmount(siteSetting).ToString());
            }
            return 0;
        }

        private void GradeCheck_Click(object sender, EventArgs e)
        {
            string userId = "";
            ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            foreach (GridViewRow row in this.grdMemberList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    userId = userId + this.grdMemberList.DataKeys[row.RowIndex].Value.ToString() + ",";
                }
            }
            userId = userId.TrimEnd(new char[] { ',' });
            if (userId.Length <= 0)
            {
                this.ShowMsg("请先选择要修改等级的用户", false);
            }
            else
            {
                int gradeId = Convert.ToInt32(this.GradeCheckList.SelectedValue);
                int num2 = MemberHelper.SetUsersGradeId(userId, gradeId);
                this.ShowMsg(string.Format("成功修改了{0}个用户的等级", num2), true);
                this.BindData();
            }
        }

        [AjaxMethod]
        public string GradeCheckUser(string userID, int gradeID)
        {
            if (MemberHelper.SetUsersGradeId(userID, gradeID) > 0)
            {
                return "success";
            }
            return "fail";
        }

        private void grdMemberList_ReBindData(object sender)
        {
            this.ReBind(false);
        }

        private void grdMemberList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            int userId = (int) this.grdMemberList.DataKeys[e.RowIndex].Value;
            if (!MemberHelper.Delete(userId))
            {
                this.ShowMsg("未知错误", false);
            }
            else
            {
                try
                {
                    this.myNotifier.updateAction = UpdateAction.MemberUpdate;
                    this.myNotifier.actionDesc = "删除会员";
                    this.myNotifier.RecDateUpdate = DateTime.Today;
                    this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                    this.myNotifier.UpdateDB();
                }
                catch (Exception)
                {
                }
                this.BindData();
                this.ShowMsg("成功删除了选择的会员", true);
            }
        }

        private void lkbDelectCheck_Click(object sender, EventArgs e)
        {
            string str = "";
            ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            foreach (GridViewRow row in this.grdMemberList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    str = str + this.grdMemberList.DataKeys[row.RowIndex].Value.ToString() + ",";
                }
            }
            str = str.TrimEnd(new char[] { ',' });
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要删除的会员账号！", false);
            }
            else if (VShopHelper.IsExistUsers(str) > 0)
            {
                this.ShowMsg("选中会员中有分销商，请取消分销商资质后再删除！", false);
            }
            else if (MemberHelper.Deletes(str))
            {
                this.ShowMsg("成功删除了选择的会员！", true);
                this.BindData();
            }
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["rankId"], out result))
                {
                    this.rankId = new int?(result);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Username"]))
                {
                    this.searchKey = base.Server.UrlDecode(this.Page.Request.QueryString["Username"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["realName"]))
                {
                    this.realName = base.Server.UrlDecode(this.Page.Request.QueryString["realName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Approved"]))
                {
                    this.approved = new bool?(Convert.ToBoolean(this.Page.Request.QueryString["Approved"]));
                }
                this.rankList.SelectedValue = this.rankId;
                this.txtSearchText.Text = this.searchKey;
                this.txtRealName.Text = this.realName;
            }
            else
            {
                this.rankId = this.rankList.SelectedValue;
                this.searchKey = this.txtSearchText.Text;
                this.realName = this.txtRealName.Text.Trim();
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.grdMemberList.RowDeleting += new GridViewDeleteEventHandler(this.grdMemberList_RowDeleting);
            this.grdMemberList.ReBindData += new Grid.ReBindDataEventHandler(this.grdMemberList_ReBindData);
            this.lkbDelectCheck1.Click += new EventHandler(this.lkbDelectCheck_Click);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.btnExport.Click += new EventHandler(this.btnExport_Click);
            this.GradeCheck.Click += new EventHandler(this.GradeCheck_Click);
            this.btnSendMessage.Click += new EventHandler(this.btnSendMessage_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;

            Utility.RegisterTypeForAjax(typeof(ManageMembers));
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.ViewState["ClientType"] = (base.Request.QueryString["clientType"] != null) ? base.Request.QueryString["clientType"] : null;
                this.BindDDL();
                this.BindData();
                this.ValidSmsNum = this.GetSmsValidCount().ToString();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        public string PostData(string url, string postData)
        {
            string str = string.Empty;
            try
            {
                Uri requestUri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    using (Stream stream2 = response.GetResponseStream())
                    {
                        Encoding encoding = Encoding.UTF8;
                        Stream stream3 = stream2;
                        if (response.ContentEncoding.ToLower() == "gzip")
                        {
                            stream3 = new GZipStream(stream2, CompressionMode.Decompress);
                        }
                        else if (response.ContentEncoding.ToLower() == "deflate")
                        {
                            stream3 = new DeflateStream(stream2, CompressionMode.Decompress);
                        }
                        using (StreamReader reader = new StreamReader(stream3, encoding))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                str = string.Format("获取信息错误：{0}", exception.Message);
            }
            return str;
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (this.rankList.SelectedValue.HasValue)
            {
                queryStrings.Add("rankId", this.rankList.SelectedValue.Value.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("Username", this.txtSearchText.Text);
            queryStrings.Add("realName", this.txtRealName.Text);
            queryStrings.Add("clientType", (this.ViewState["ClientType"] != null) ? this.ViewState["ClientType"].ToString() : "");
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            queryStrings.Add("phone", this.txtPhone.Text);
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }

        [AjaxMethod]
        public string SetDistributors(string ids, int rid)
        {
            ids = ids.TrimEnd(new char[] { ',' });
            if (string.IsNullOrEmpty(ids))
            {
                return "请先选择要删除的会员账号！";
            }
            if (VShopHelper.IsExistUsers(ids) > 0)
            {
                return "选中会员中有分销商，请取消分销商资质后再设置！";
            }
            int num = MemberHelper.SetRegions(ids, rid);
            if (num > 0)
            {
                return string.Format("success", num);
            }
            return "设置失败";
        }
    }
}

