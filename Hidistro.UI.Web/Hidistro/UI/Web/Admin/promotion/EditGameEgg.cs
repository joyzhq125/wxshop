namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditGameEgg : AdminPage
    {
        private int _gameId;
        protected Button btnSubmit1;
        protected GameType gameType;
        protected Label lbBeginTime;
        protected Label lbEedTime;
        protected Label lbGameDescription;
        protected Label lbPrizeGade0;
        protected Label lbPrizeGade1;
        protected Label lbPrizeGade2;
        protected Label lbPrizeGade3;
        protected HtmlForm thisForm;
        protected Hidistro.UI.Web.Admin.promotion.UCGameInfo UCGameInfo;
        protected Hidistro.UI.Web.Admin.promotion.UCGamePrizeInfo UCGamePrizeInfo;

        protected EditGameEgg() : base("m08", "yxp08")
        {
            this.gameType = GameType.疯狂砸金蛋;
            this._gameId = -1;
        }

        private void BindDate()
        {
            GameInfo modelByGameId = GameHelper.GetModelByGameId(this._gameId);
            if (modelByGameId != null)
            {
                this.UCGameInfo.GameInfo = modelByGameId;
                IList<GamePrizeInfo> gamePrizeListsByGameId = GameHelper.GetGamePrizeListsByGameId(this._gameId);
                this.UCGamePrizeInfo.PrizeLists = gamePrizeListsByGameId;
                this.UCGamePrizeInfo.NotPrzeDescription = modelByGameId.NotPrzeDescription;
                try
                {
                    this.lbPrizeGade0.Text = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => (p.PrizeGrade == PrizeGrade.一等奖)).PrizeType.ToString();
                    this.lbPrizeGade1.Text = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => (p.PrizeGrade == PrizeGrade.二等奖)).PrizeType.ToString();
                    this.lbPrizeGade2.Text = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => (p.PrizeGrade == PrizeGrade.三等奖)).PrizeType.ToString();
                    this.lbPrizeGade3.Text = gamePrizeListsByGameId.FirstOrDefault<GamePrizeInfo>(p => (p.PrizeGrade == PrizeGrade.普通奖)).PrizeType.ToString();
                    this.lbBeginTime.Text = modelByGameId.BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                    this.lbEedTime.Text = modelByGameId.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    this.lbGameDescription.Text = Globals.HtmlDecode(modelByGameId.Description);
                }
                catch (Exception)
                {
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.SaveDate();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this._gameId = int.Parse(base.Request.QueryString["gameId"]);
            }
            catch (Exception)
            {
                base.GotoResourceNotFound();
                return;
            }
            if (!this.Page.IsPostBack)
            {
                this.BindDate();
            }
        }

        private void SaveDate()
        {
            try
            {
                GameInfo gameInfo = this.UCGameInfo.GameInfo;
                gameInfo.NotPrzeDescription = this.UCGamePrizeInfo.NotPrzeDescription;
                IList<GamePrizeInfo> prizeLists = this.UCGamePrizeInfo.PrizeLists;
                if (!GameHelper.Update(gameInfo))
                {
                    throw new Exception("更新失败！");
                }
                foreach (GamePrizeInfo info2 in prizeLists)
                {
                    if (!GameHelper.UpdatePrize(info2))
                    {
                        throw new Exception("更新奖品信息时失败！");
                    }
                }
                this.Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ShowSuccess", "<script>$(function () { ShowStep2(); })</script>");
            }
            catch (Exception exception)
            {
                this.ShowMsg(exception.Message, false);
            }
        }
    }
}

