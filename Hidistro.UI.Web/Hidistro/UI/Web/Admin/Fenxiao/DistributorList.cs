namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.StatisticsReport;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class DistributorList : AdminPage
    {
        protected HyperLink btnDownTaobao;
        protected Button btnSearchButton;
        protected ImageLinkButton CancleCheck;
        private string CellPhone;
        protected DistributorGradeDropDownList DrGrade;
        protected Button EditSave;
        protected TextBox EdittxtCellPhone;
        protected TextBox EdittxtPassword;
        protected TextBox EdittxtQQNum;
        protected TextBox EdittxtRealname;
        protected HiddenField EditUserID;
        protected ImageLinkButton FrozenCheck;
        private string Grade;
        protected Button GradeCheck;
        protected DistributorGradeDropDownList GradeCheckList;
        protected PageSize hrefPageSize;
        protected Literal ListActive;
        protected Literal Listfrozen;
        private string MicroSignal;
        private UpdateStatistics myEvent;
        private StatisticNotifier myNotifier;
        protected Pager pager;
        protected Button PassCheck;
        private string RealName;
        protected Repeater reDistributor;
        private string Status;
        private string StoreName;
        protected TextBox txtCellPhone;
        protected TextBox txtConformPassword;
        protected TextBox txtMicroSignal;
        protected TextBox txtPassword;
        protected TextBox txtRealName;
        protected TextBox txtStoreName;

        protected DistributorList() : base("m05", "fxp03")
        {
            this.StoreName = "";
            this.Grade = "0";
            this.Status = "0";
            this.RealName = "";
            this.CellPhone = "";
            this.MicroSignal = "";
            this.myNotifier = new StatisticNotifier();
            this.myEvent = new UpdateStatistics();
        }

        private void BindData()
        {
            DistributorsQuery entity = new DistributorsQuery {
                GradeId = int.Parse(this.Grade),
                StoreName = this.StoreName,
                CellPhone = this.CellPhone,
                RealName = this.RealName,
                MicroSignal = this.MicroSignal,
                ReferralStatus = int.Parse(this.Status),
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "userid"
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult result = VShopHelper.GetDistributors(entity, null, null);
            this.reDistributor.DataSource = result.Data;
            this.reDistributor.DataBind();
            this.pager.TotalRecords = result.TotalRecords;
            DataTable distributorsNum = VShopHelper.GetDistributorsNum();
            this.ListActive.Text = "分销商列表(" + distributorsNum.Rows[0]["active"].ToString() + ")";
            this.Listfrozen.Text = "已冻结(" + distributorsNum.Rows[0]["frozen"].ToString() + ")";
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void CancleCheck_Click(object sender, EventArgs e)
        {
            string userids = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                userids = base.Request["CheckBoxGroup"];
            }
            if (userids.Length <= 0)
            {
                this.ShowMsg("请先选择要取消资质的分销商", false);
            }
            else
            {
                int num = DistributorsBrower.FrozenCommisionChecks(userids, "9");
                try
                {
                    this.myNotifier.updateAction = UpdateAction.MemberUpdate;
                    this.myNotifier.actionDesc = "批量取消分销商资质";
                    this.myNotifier.RecDateUpdate = DateTime.Today;
                    this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                    this.myNotifier.UpdateDB();
                }
                catch (Exception)
                {
                }
                this.ShowMsg(string.Format("成功取消了{0}个分销商的资质", num), true);
                this.ReBind(true);
            }
        }

        private void EditSave_Click(object sender, EventArgs e)
        {
            string userid = this.EditUserID.Value.Trim();
            if (userid.Length <= 0)
            {
                this.ShowMsg("用户ID为空，参数异常！", false);
            }
            else
            {
                string sourceData = this.EdittxtPassword.Text.Trim();
                if ((sourceData.Length > 0) && ((sourceData.Length > 20) || (sourceData.Length < 6)))
                {
                    this.ShowMsg("用户密码长度在6-20位之间！", false);
                }
                else if (DistributorsBrower.EditDisbutosInfos(userid, this.EdittxtQQNum.Text, this.EdittxtCellPhone.Text, this.EdittxtRealname.Text, HiCryptographer.Md5Encrypt(sourceData)))
                {
                    this.ReBind(true);
                }
                else
                {
                    this.ShowMsg("成功用户信息失败", false);
                }
            }
        }

        private void FrozenCheck_Click(object sender, EventArgs e)
        {
            string userids = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                userids = base.Request["CheckBoxGroup"];
            }
            if (userids.Length <= 0)
            {
                this.ShowMsg("请先选择要冻结的分销商", false);
            }
            else
            {
                int num = DistributorsBrower.FrozenCommisionChecks(userids, "1");
                this.ShowMsg(string.Format("成功冻结了{0}分销商", num), true);
                this.ReBind(true);
            }
        }

        private void GradeCheck_Click(object sender, EventArgs e)
        {
            string userids = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                userids = base.Request["CheckBoxGroup"];
            }
            string grade = this.GradeCheckList.SelectedValue.ToString();
            if (userids.Length <= 0)
            {
                this.ShowMsg("请先选择要修改等级的分销商", false);
            }
            else
            {
                int num = DistributorsBrower.EditCommisionsGrade(userids, grade);
                this.ShowMsg(string.Format("成功修改了{0}个分销商的等级", num), true);
                this.ReBind(true);
            }
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
                {
                    this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Grade"]))
                {
                    this.Grade = base.Server.UrlDecode(this.Page.Request.QueryString["Grade"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Status"]))
                {
                    this.Status = base.Server.UrlDecode(this.Page.Request.QueryString["Status"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RealName"]))
                {
                    this.RealName = base.Server.UrlDecode(this.Page.Request.QueryString["RealName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CellPhone"]))
                {
                    this.CellPhone = base.Server.UrlDecode(this.Page.Request.QueryString["CellPhone"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["MicroSignal"]))
                {
                    this.MicroSignal = base.Server.UrlDecode(this.Page.Request.QueryString["MicroSignal"]);
                }
                this.txtStoreName.Text = this.StoreName;
                this.DrGrade.SelectedValue = new int?(int.Parse(this.Grade));
                this.txtCellPhone.Text = this.CellPhone;
                this.txtMicroSignal.Text = this.MicroSignal;
                this.txtRealName.Text = this.RealName;
            }
            else
            {
                this.StoreName = this.txtStoreName.Text;
                this.Grade = this.DrGrade.SelectedValue.ToString();
                this.CellPhone = this.txtCellPhone.Text;
                this.RealName = this.txtRealName.Text;
                this.MicroSignal = this.txtMicroSignal.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.QueryString["task"];
            if (!string.IsNullOrEmpty(str))
            {
                string s = "{error:1,msg:'未定义操作！'}";
                if (str == "readinfo")
                {
                    int num;
                    if (int.TryParse(this.Page.Request.QueryString["UserId"], out num))
                    {
                        DistributorsQuery entity = new DistributorsQuery {
                            UserId = int.Parse(this.Page.Request.QueryString["UserId"]),
                            ReferralStatus = -1,
                            PageIndex = 1,
                            PageSize = 1,
                            SortOrder = SortAction.Desc,
                            SortBy = "userid"
                        };
                        Globals.EntityCoding(entity, true);
                        DbQueryResult result = VShopHelper.GetDistributors(entity, null, null);
                        if (result.Data != null)
                        {
                            DataTable data = new DataTable();
                            data = (DataTable) result.Data;
                            s = s = "{error:0,Data:" + JsonConvert.SerializeObject(data) + "}";
                        }
                        else
                        {
                            s = "{error:1,msg:'分销商信息不存在'}";
                        }
                    }
                    else
                    {
                        s = "{error:1,msg:'userid错误'}";
                    }
                }
                base.Response.Write(s);
                base.Response.End();
            }
            this.reDistributor.ItemCommand += new RepeaterCommandEventHandler(this.reDistributor_ItemCommand);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.FrozenCheck.Click += new EventHandler(this.FrozenCheck_Click);
            this.CancleCheck.Click += new EventHandler(this.CancleCheck_Click);
            this.PassCheck.Click += new EventHandler(this.PassCheck_Click);
            this.GradeCheck.Click += new EventHandler(this.GradeCheck_Click);
            this.EditSave.Click += new EventHandler(this.EditSave_Click);
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindData();
                this.GradeCheckList.DataBind();
                this.DrGrade.DataBind();
                this.DrGrade.SelectedValue = new int?(int.Parse(this.Grade));
            }
        }

        private void PassCheck_Click(object sender, EventArgs e)
        {
            string userids = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                userids = base.Request["CheckBoxGroup"];
            }
            if (userids.Length <= 0)
            {
                this.ShowMsg("请先选择要修改密码的分销商", false);
            }
            else if ((this.txtPassword.Text.Trim().Length < 6) || (this.txtPassword.Text.Trim().Length > 20))
            {
                this.ShowMsg("密码长度在6-20位之间！", false);
            }
            else if (this.txtPassword.Text != this.txtConformPassword.Text)
            {
                this.ShowMsg("两次输入密码不一致！", false);
            }
            else
            {
                int num = MemberProcessor.SetMultiplePwd(userids, HiCryptographer.Md5Encrypt(this.txtPassword.Text.Trim()));
                this.ShowMsg(string.Format("成功修改了{0}个分销商的密码", num), true);
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Grade", this.DrGrade.Text);
            queryStrings.Add("StoreName", this.txtStoreName.Text);
            queryStrings.Add("CellPhone", this.txtCellPhone.Text);
            queryStrings.Add("RealName", this.txtRealName.Text);
            queryStrings.Add("MicroSignal", this.txtMicroSignal.Text);
            queryStrings.Add("Status", this.Status);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }

        private void reDistributor_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Frozen")
            {
                if (!DistributorsBrower.FrozenCommision(int.Parse(e.CommandArgument.ToString()), "1"))
                {
                    this.ShowMsg("冻结失败", false);
                    return;
                }
                this.ShowMsg("冻结成功", true);
                this.ReBind(true);
            }
            if (e.CommandName == "Thaw")
            {
                if (!DistributorsBrower.FrozenCommision(int.Parse(e.CommandArgument.ToString()), "0"))
                {
                    this.ShowMsg("解冻失败", false);
                    return;
                }
                this.ShowMsg("解冻成功", true);
                this.ReBind(true);
            }
            if (e.CommandName == "Forbidden")
            {
                if (DistributorsBrower.FrozenCommision(int.Parse(e.CommandArgument.ToString()), "9"))
                {
                    this.ShowMsg("取消资质成功！", true);
                    try
                    {
                        this.myNotifier.updateAction = UpdateAction.MemberUpdate;
                        this.myNotifier.actionDesc = "取消分销商资质";
                        this.myNotifier.RecDateUpdate = DateTime.Today;
                        this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                        this.myNotifier.UpdateDB();
                    }
                    catch (Exception)
                    {
                    }
                    this.ReBind(true);
                }
                else
                {
                    this.ShowMsg("取消资质失败", false);
                }
            }
        }
    }
}

