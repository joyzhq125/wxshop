namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class UCGameInfo : UserControl
    {
        private Hidistro.Entities.Promotions.GameInfo _gameInfo;
        private IList<MemberGradeItem> _memberGrades = new List<MemberGradeItem>();
        protected CheckBox cbOnlyGiveNotPrizeMember;
        protected ucDateTimePicker dateBeginTime;
        protected ucDateTimePicker dateEndTime;
        protected HiddenField hfGameId;
        protected HiddenField hfKeyWord;
        protected bool isAllCheck;
        protected RadioButton rbdPlayType0;
        protected RadioButton rbdPlayType1;
        protected RadioButton rbdPlayType2;
        protected RadioButton rbdPlayType3;
        protected TextBox txtDescription;
        protected TextBox txtGameTitle;
        protected TextBox txtGameUrl;
        protected TextBox txtGivePoint;
        protected TextBox txtNeedPoint;
        public string wid;
        private void BindDate()
        {
            if (this._gameInfo == null)
            {
                string keyWord = Guid.NewGuid().ToString().Replace("-", "");
                this.hfKeyWord.Value = keyWord;
                string str2 = this.CreateGameUrl(keyWord);
                this.txtGameUrl.Text = str2;
            }
            else
            {
                this.hfGameId.Value = this._gameInfo.GameId.ToString();
                this.txtGameTitle.Text = this._gameInfo.GameTitle;
                this.dateBeginTime.SelectedDate = new DateTime?(this._gameInfo.BeginTime);
                this.dateEndTime.SelectedDate = new DateTime?(this._gameInfo.EndTime);
                this.txtDescription.Text = this._gameInfo.Description.Replace("<br/>", "\n");
                this.txtNeedPoint.Text = this._gameInfo.NeedPoint.ToString();
                this.txtGivePoint.Text = this._gameInfo.GivePoint.ToString();
                this.cbOnlyGiveNotPrizeMember.Checked = this._gameInfo.OnlyGiveNotPrizeMember;
                this.rbdPlayType0.Checked = false;
                this.rbdPlayType1.Checked = false;
                this.rbdPlayType2.Checked = false;
                this.rbdPlayType3.Checked = false;
                switch (this._gameInfo.PlayType)
                {
                    case PlayType.一天一次:
                        this.rbdPlayType0.Checked = true;
                        break;

                    case PlayType.一次:
                        this.rbdPlayType1.Checked = true;
                        break;

                    case PlayType.一天两次:
                        this.rbdPlayType2.Checked = true;
                        break;

                    case PlayType.两次:
                        this.rbdPlayType3.Checked = true;
                        break;
                }
                if (string.Equals(this._gameInfo.ApplyMembers, "0"))
                {
                    this.isAllCheck = true;
                }
                this.txtGameUrl.Text = this._gameInfo.GameUrl;
            }
            foreach (MemberGradeInfo info in MemberHelper.GetMemberGrades(this.wid))
            {
                MemberGradeItem item = new MemberGradeItem();
                if (!this.isAllCheck)
                {
                    item.IsCheck = this.IsCheck(info.GradeId.ToString());
                }
                else
                {
                    item.IsCheck = this.isAllCheck;
                }
                item.Name = info.Name;
                item.GradeId = info.GradeId.ToString();
                this._memberGrades.Add(item);
            }
        }

        private string CreateGameUrl(string keyWord)
        {
            Uri url = HttpContext.Current.Request.Url;
            string str = (url.Port == 80) ? string.Empty : (":" + url.Port.ToString(CultureInfo.InvariantCulture));
            string str2 = string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[] { url.Scheme, Globals.DomainName, str });
            return string.Format("{0}{1}/Game.aspx?gamid={2}&type={3}", new object[] { str2, Globals.ApplicationPath, keyWord, (int) this.GameType });
        }

        private void GetDate()
        {
            if (this._gameInfo == null)
            {
                this._gameInfo = new Hidistro.Entities.Promotions.GameInfo();
            }
            try
            {
                this._gameInfo.GameId = int.Parse(this.hfGameId.Value);
            }
            catch (Exception)
            {
                this._gameInfo.GameId = 0;
                this._gameInfo.GameType = this.GameType;
            }
            this._gameInfo.GameTitle = this.txtGameTitle.Text;
            try
            {
                this._gameInfo.BeginTime = this.dateBeginTime.SelectedDate.Value;
            }
            catch (InvalidOperationException)
            {
                throw new Exception("活动时间期的开始日期不能为空！");
            }
            try
            {
                this._gameInfo.EndTime = this.dateEndTime.SelectedDate.Value;
            }
            catch (InvalidOperationException)
            {
                throw new Exception("活动时间的结束日期不能为空！");
            }
            try
            {
                this._gameInfo.NeedPoint = int.Parse(this.txtNeedPoint.Text);
            }
            catch (FormatException)
            {
                throw new Exception("活动消耗积分格式不对！");
            }
            try
            {
                this._gameInfo.GivePoint = int.Parse(this.txtGivePoint.Text);
            }
            catch (FormatException)
            {
                throw new Exception("活动参与送积分格式不对！");
            }
            this._gameInfo.Description = this.txtDescription.Text.Replace("\n", "<br/>");
            if (this.rbdPlayType0.Checked)
            {
                this._gameInfo.PlayType = PlayType.一天一次;
            }
            if (this.rbdPlayType1.Checked)
            {
                this._gameInfo.PlayType = PlayType.一次;
            }
            if (this.rbdPlayType2.Checked)
            {
                this._gameInfo.PlayType = PlayType.一天两次;
            }
            if (this.rbdPlayType3.Checked)
            {
                this._gameInfo.PlayType = PlayType.两次;
            }
            this._gameInfo.OnlyGiveNotPrizeMember = this.cbOnlyGiveNotPrizeMember.Checked;
            this._gameInfo.ApplyMembers = base.Request["allApplyMembers"];
            if (string.IsNullOrEmpty(this._gameInfo.ApplyMembers))
            {
                this._gameInfo.ApplyMembers = base.Request["applyMembers"];
            }
            this._gameInfo.GameUrl = this.txtGameUrl.Text.Trim();
            this._gameInfo.KeyWork = this.hfKeyWord.Value;
        }

        private bool IsCheck(string gradeId)
        {
            if (this._gameInfo != null)
            {
                string[] source = this._gameInfo.ApplyMembers.Split(new char[] { ',' });
                for (int i = 0; i < source.Count<string>(); i++)
                {
                    if (string.Equals(source[i], gradeId))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindDate();
            }
        }

        public Hidistro.Entities.Promotions.GameInfo GameInfo
        {
            get
            {
                this.GetDate();
                return this._gameInfo;
            }
            set
            {
                this._gameInfo = value;
            }
        }

        public Hidistro.Entities.Promotions.GameType GameType { get; set; }

        protected IList<MemberGradeItem> MemberGrades
        {
            get
            {
                return this._memberGrades;
            }
        }
    }
}

