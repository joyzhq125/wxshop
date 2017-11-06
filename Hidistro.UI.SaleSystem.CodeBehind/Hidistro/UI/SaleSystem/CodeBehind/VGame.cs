namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.Common.Controls;
    using System;

    public class VGame : VMemberTemplatedWebControl
    {
        private string htmlTitle = string.Empty;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle(this.htmlTitle);
        }

        protected override void OnInit(EventArgs e)
        {
            GameType type = GameType.幸运大转盘;
            try
            {
                type = (GameType) Enum.Parse(typeof(GameType), this.Page.Request.QueryString["type"]);
            }
            catch (Exception)
            {
                base.GotoResourceNotFound("");
            }
            this.htmlTitle = type.ToString();
            if (this.SkinName == null)
            {
                switch (type)
                {
                    case GameType.幸运大转盘:
                        this.SkinName = "skin-vGameZhuangPan.html";
                        goto Label_00C4;

                    case GameType.疯狂砸金蛋:
                        this.SkinName = "skin-vGameEgg.html";
                        goto Label_00C4;

                    case GameType.好运翻翻看:
                        this.SkinName = "skin-vGameHaoYun.html";
                        goto Label_00C4;

                    case GameType.大富翁:
                        this.SkinName = "skin-vGameDaFuWen.html";
                        goto Label_00C4;

                    case GameType.刮刮乐:
                        this.SkinName = "skin-vGameGuaGuaLe.html";
                        goto Label_00C4;
                }
                base.GotoResourceNotFound("");
            }
        Label_00C4:
            base.OnInit(e);
        }
    }
}

