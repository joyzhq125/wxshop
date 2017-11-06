namespace Hidistro.UI.Web.Admin.Member
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditMemberGrade)]
    public class AddMemberGrade : AdminPage
    {
        protected static bool _bAdd = true;
        protected static int _gradeid = 0;
        protected Button btnSubmitMemberRanks;
        protected RadioButtonList chkIsDefault;
        protected Script Script4;
        protected HtmlForm thisForm;
        protected TextBox txt_tradeTimes;
        protected TextBox txt_tradeVol;
        protected TextBox txtRankDesc;
        protected TextBox txtRankName;
        protected TextBox txtValue;
        //protected string wid;
        protected AddMemberGrade() : base("m04", "hyp03")
        {
        }

        private bool bContain(string[] arr, string val)
        {
            foreach (string str in arr)
            {
                if (str.ToLower() == val.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        private bool bDouble(string val, ref double i)
        {
            i = 0.0;
            if (val.Contains("-"))
            {
                return false;
            }
            return double.TryParse(val, out i);
        }

        private bool bInt(string val, ref int i)
        {
            i = 0;
            if (val.Contains(".") || val.Contains("-"))
            {
                return false;
            }
            return int.TryParse(val, out i);
        }

        private void btnSubmitMemberRanks_Click(object sender, EventArgs e)
        {
            string text = this.txtRankName.Text;
            string str2 = this.txt_tradeVol.Text;
            string str3 = this.txt_tradeTimes.Text;
            string str4 = this.txtValue.Text;
            string text1 = this.txtRankDesc.Text;
            if (string.IsNullOrEmpty(text))
            {
                this.ShowMsg("会员等级名称不能为空！", false);
            }
            else if (text.Length > 20)
            {
                this.ShowMsg("会员等级名称需小于20个字符", false);
            }
            else
            {
                double? nullable = null;
                if (!string.IsNullOrEmpty(str2))
                {
                    double num = 0.0;
                    if (!this.bDouble(str2, ref num))
                    {
                        this.ShowMsg("请输入正确的交易额！", false);
                        return;
                    }
                    nullable = new double?(num);
                }
                int? nullable2 = null;
                if (!string.IsNullOrEmpty(str3))
                {
                    int num2 = 0;
                    if (!this.bInt(str3, ref num2))
                    {
                        this.ShowMsg("请输入正确的交易次数！", false);
                        return;
                    }
                    nullable2 = new int?(num2);
                }
                int i = 0;
                if (string.IsNullOrEmpty(str4))
                {
                    this.ShowMsg("会员折扣不能为空！", false);
                }
                else if (!this.bInt(str4, ref i))
                {
                    this.ShowMsg("会员折扣必须是小于100的正整数！", false);
                }
                else if (i > 100)
                {
                    this.ShowMsg("会员折扣必须是小于100的正整数！", false);
                }
                else if (!nullable.HasValue && !nullable2.HasValue)
                {
                    this.ShowMsg("交易额和交易次数同时为空！", false);
                }
                else
                {
                    MemberGradeInfo memberGrade;
                    if (_bAdd)
                    {
                        memberGrade = new MemberGradeInfo();
                    }
                    else
                    {
                        memberGrade = MemberHelper.GetMemberGrade(_gradeid);
                    }
                    memberGrade.wid = this.wid;
                    memberGrade.Name = text;
                    memberGrade.Description = this.txtRankDesc.Text.Trim();
                    memberGrade.IsDefault = Convert.ToBoolean(this.chkIsDefault.SelectedValue);
                    memberGrade.TranVol = nullable;
                    memberGrade.TranTimes = nullable2;
                    memberGrade.Discount = i;
                    if (_bAdd && MemberHelper.IsExist(text,this.wid))
                    {
                        this.ShowMsg("该等级名称已存在，请修改等级名称", false);
                    }
                    else if (MemberHelper.HasSameMemberGrade(memberGrade))
                    {
                        this.ShowMsg("已经存在相同交易额或交易次数的等级，每个会员等级的交易额或交易次数不能相同", false);
                    }
                    else
                    {
                        try
                        {
                            if (_bAdd)
                            {
                                if (MemberHelper.CreateMemberGrade(memberGrade))
                                {
                                    this.ShowMsgAndReUrl("成功添加了一个会员等级", true, "/Admin/Member/MemberGrades.aspx");
                                }
                                else
                                {
                                    this.ShowMsg("添加会员等级失败", false);
                                }
                            }
                            else if (MemberHelper.UpdateMemberGrade(memberGrade))
                            {
                                this.ShowMsgAndReUrl("成功修改了一个会员等级", true, "/Admin/Member/MemberGrades.aspx");
                            }
                            else
                            {
                                this.ShowMsg("修改会员等级失败", false);
                            }
                        }
                        catch (Exception exception)
                        {
                            this.ShowMsg("操作失败,提示：" + exception.Message, false);
                        }
                    }
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSubmitMemberRanks.Click += new EventHandler(this.btnSubmitMemberRanks_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            string[] allKeys = base.Request.Params.AllKeys;
            int i = 0;
            if (allKeys.Contains<string>("id"))
            {
                if (this.bInt(base.Request["id"], ref i))
                {
                    _gradeid = i;
                    _bAdd = false;
                }
                else
                {
                    _bAdd = true;
                }
            }
            else
            {
                _bAdd = true;
            }
            if (!base.IsPostBack && !_bAdd)
            {
                MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(i);
                if (memberGrade == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    Globals.EntityCoding(memberGrade, false);
                    this.txtRankName.Text = memberGrade.Name;
                    this.txtRankDesc.Text = memberGrade.Description;
                    this.txtValue.Text = memberGrade.Discount.ToString();
                    this.txt_tradeVol.Text = memberGrade.TranVol.ToString();
                    this.chkIsDefault.SelectedValue = memberGrade.IsDefault.ToString().ToLower();
                    this.txt_tradeTimes.Text = memberGrade.TranTimes.ToString();
                    if (memberGrade.IsDefault)
                    {
                        this.chkIsDefault.Enabled = false;
                    }
                }
            }
        }

        private bool ValidationMemberGrade(MemberGradeInfo memberGrade)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<MemberGradeInfo>(memberGrade, new string[] { "ValMemberGrade" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

