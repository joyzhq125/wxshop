namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities.Promotions;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class UCGamePrizeInfo : UserControl
    {
        private List<ListItem> _couponList = new List<ListItem>();
        private IList<GamePrizeInfo> _prizeLists;
        protected int prizeTypeValue0;
        protected int prizeTypeValue1;
        protected int prizeTypeValue2;
        protected int prizeTypeValue3;
        protected TextBox txtNotPrzeDescription;

        private void BindDdlCouponId()
        {
            DataTable unFinishedCoupon = CouponHelper.GetUnFinishedCoupon(DateTime.Now);
            if (unFinishedCoupon != null)
            {
                foreach (DataRow row in unFinishedCoupon.Rows)
                {
                    ListItem item = new ListItem {
                        Text = row["CouponName"].ToString(),
                        Value = row["CouponId"].ToString()
                    };
                    this._couponList.Add(item);
                }
            }
        }

        private bool GetDate()
        {
            if (!this.Page.IsPostBack)
            {
                return true;
            }
            this._prizeLists = new List<GamePrizeInfo>();
            this._prizeLists.Add(this.GetModel(PrizeGrade.一等奖));
            this._prizeLists.Add(this.GetModel(PrizeGrade.二等奖));
            this._prizeLists.Add(this.GetModel(PrizeGrade.三等奖));
            this._prizeLists.Add(this.GetModel(PrizeGrade.普通奖));
            return true;
        }

        private GamePrizeInfo GetModel(PrizeGrade prizeGrade)
        {
            GamePrizeInfo info = new GamePrizeInfo {
                PrizeGrade = prizeGrade
            };
            int num = (int) prizeGrade;
            PrizeType type = PrizeType.赠送积分;
            try
            {
                type = (PrizeType) Enum.Parse(typeof(PrizeType), base.Request[string.Format("prizeType_{0}", num)]);
            }
            catch (Exception)
            {
            }
            info.PrizeType = type;
            switch (type)
            {
                case PrizeType.赠送积分:
                    try
                    {
                        info.GivePoint = int.Parse(base.Request[string.Format("txtGivePoint{0}", num)]);
                        goto Label_0112;
                    }
                    catch (Exception)
                    {
                        throw new Exception(string.Format("{0}的赠送积分格式不对!", prizeGrade.ToString()));
                    }
                    break;

                case PrizeType.赠送优惠劵:
                    break;

                case PrizeType.赠送商品:
                    info.GiveShopBookId = base.Request[string.Format("txtShopbookId{0}", num)];
                    info.GriveShopBookPicUrl = base.Request[string.Format("txtProductPic{0}", num)];
                    goto Label_0112;

                default:
                    goto Label_0112;
            }
            info.GiveCouponId = base.Request[string.Format("seletCouponId{0}", num)];
        Label_0112:
            try
            {
                info.PrizeCount = int.Parse(base.Request[string.Format("txtPrizeCount{0}", num)]);
            }
            catch (Exception)
            {
                throw new Exception(string.Format("{0}的奖品数量格式不对!", prizeGrade.ToString()));
            }
            try
            {
                info.PrizeRate = int.Parse(base.Request[string.Format("txtPrizeRate{0}", num)]);
            }
            catch (Exception)
            {
                throw new Exception(string.Format("{0}的中奖率格式不对!", prizeGrade.ToString()));
            }
            try
            {
                info.PrizeId = int.Parse(base.Request[string.Format("prizeInfoId{0}", num)]);
            }
            catch (Exception)
            {
                info.PrizeId = 0;
            }
            try
            {
                info.GameId = int.Parse(base.Request[string.Format("prizeGameId{0}", num)]);
            }
            catch (Exception)
            {
                info.GameId = 0;
            }
            return info;
        }

        protected string GetPrizeInfoHtml(PrizeGrade prizeGrade, GamePrizeInfo model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<div class='tabContent'>");
            builder.Append("<div class='form-horizontal clearfix'>");
            builder.Append("<div class='form-group setmargin'>");
            builder.Append("<label class='col-xs-3 pad resetSize control-label'><em>*</em>&nbsp;&nbsp;奖品类别：</label>");
            builder.Append("<div class='form-inline col-xs-9'>");
            builder.Append("<div class='resetradio selectradio pt3'>");
            builder.Append("<label class=\"mr20\">");
            if (model != null)
            {
                builder.AppendFormat(" <input type=\"radio\" id=\"rd{0}_0\" name=\"prizeType_{0}\" {1} value=\"0\" />赠送积分</label>", (int) prizeGrade, (model.PrizeType == PrizeType.赠送积分) ? "checked=\"checked\"" : "");
                builder.AppendFormat(" <label class=\"mr20\"> <input type=\"radio\" id=\"rd{0}_1\" name=\"prizeType_{0}\" {1} value=\"1\" />赠送优惠券</label>", (int) prizeGrade, (model.PrizeType == PrizeType.赠送优惠劵) ? "checked=\"checked\"" : "");
                builder.AppendFormat(" <label class=\"mr20\"> <input type=\"radio\" id=\"rd{0}_2\" name=\"prizeType_{0}\" {1} value=\"2\" />赠送商品</label>", (int) prizeGrade, (model.PrizeType == PrizeType.赠送商品) ? "checked=\"checked\"" : "");
                builder.AppendFormat("<input type=\"hidden\" id=\"prizeTypeValue{0}\" value=\"{1}\" />", (int) prizeGrade, (int) model.PrizeType);
                builder.AppendFormat("<input type=\"hidden\" name=\"prizeInfoId{0}\" value=\"{1}\" />", (int) prizeGrade, model.PrizeId);
                builder.AppendFormat("<input type=\"hidden\" name=\"prizeGameId{0}\" value=\"{1}\" />", (int) prizeGrade, model.GameId);
            }
            else
            {
                builder.AppendFormat(" <input type=\"radio\" id=\"rd{0}_0\" name=\"prizeType_{0}\" checked=\"checked\" value=\"0\" />赠送积分</label>", (int) prizeGrade);
                builder.AppendFormat(" <label class=\"mr20\"> <input type=\"radio\" id=\"rd{0}_1\" name=\"prizeType_{0}\" value=\"1\" />赠送优惠券</label>", (int) prizeGrade);
                builder.AppendFormat(" <label class=\"mr20\"> <input type=\"radio\" id=\"rd{0}_2\" name=\"prizeType_{0}\" value=\"2\" />赠送商品</label>", (int) prizeGrade);
            }
            builder.Append(" </div></div></div>");
            if ((model != null) && (model.PrizeType == PrizeType.赠送积分))
            {
                builder.Append(" <div class=\"form-group setmargin give giveint\"  style=\"display:normal\">");
            }
            else
            {
                builder.Append(" <div class=\"form-group setmargin give giveint\">");
            }
            builder.Append(" <label class=\"col-xs-3 pad resetSize control-label\" for=\"pausername\"><em>*</em>&nbsp;&nbsp");
            builder.Append("赠送积分：</label> <div class=\"form-inline col-xs-9\">");
            if (model != null)
            {
                builder.AppendFormat(" <input type=\"text\" name=\"txtGivePoint{0}\" id=\"txtGivePoint{0}\" class=\"form-control resetSize\" value=\"{1}\" />", (int) prizeGrade, model.GivePoint);
            }
            else
            {
                builder.AppendFormat(" <input type=\"text\" name=\"txtGivePoint{0}\" id=\"txtGivePoint{0}\" class=\"form-control resetSize\" value=\"0\" />", (int) prizeGrade);
            }
            if ((model != null) && (model.PrizeType == PrizeType.赠送优惠劵))
            {
                builder.Append(" </div> </div><div class=\"form-group setmargin give givecop\" style=\"display:normal\">");
            }
            else
            {
                builder.Append(" </div> </div><div class=\"form-group setmargin give givecop\">");
            }
            builder.Append(" <label class=\"col-xs-3 pad resetSize control-label\" for=\"pausername\"><em>*</em>&nbsp;&nbsp;赠送优惠券：</label> <div class=\"form-inline col-xs-9\">");
            builder.AppendFormat(" <select name=\"seletCouponId{0}\" id=\"seletCouponId{0}\" class=\"form-control resetSize\">", (int) prizeGrade);
            if (model != null)
            {
                foreach (ListItem item in this.CouponIdList)
                {
                    if (string.Equals(model.GiveCouponId, item.Value))
                    {
                        builder.AppendFormat(" <option value=\"{0}\" selected=\"selected\">{1}</option>", item.Value, item.Text);
                    }
                    else
                    {
                        builder.AppendFormat(" <option value=\"{0}\">{1}</option>", item.Value, item.Text);
                    }
                }
            }
            else
            {
                foreach (ListItem item2 in this.CouponIdList)
                {
                    builder.AppendFormat(" <option value=\"{0}\">{1}</option>", item2.Value, item2.Text);
                }
            }
            builder.Append(" </select> </div>  </div> ");
            if ((model != null) && (model.PrizeType == PrizeType.赠送商品))
            {
                builder.Append("<div class=\"form-group setmargin give giveshop\" style=\"display:normal\">");
            }
            else
            {
                builder.Append("<div class=\"form-group setmargin give giveshop\">");
            }
            builder.Append("<label class=\"col-xs-3 pad resetSize control-label\" for=\"pausername\"><em>*</em>&nbsp;&nbsp;赠送商品：</label>");
            builder.Append("<div class=\"form-inline col-xs-9\"><div class=\"pt3\">");
            if (model != null)
            {
                builder.AppendFormat("<img id=\"imgProduct{0}\" style=\"width:59px; height:59px;\" name=\"imgProduct{0}\"  src=\"{1}\"onclick=\"SelectShopBookId({0});\" />", (int) prizeGrade, string.IsNullOrEmpty(model.GriveShopBookPicUrl) ? "../images/u100.png" : model.GriveShopBookPicUrl);
                builder.AppendFormat("<input type=\"hidden\" name=\"txtShopbookId{0}\" id=\"txtShopbookId{0}\"  value=\"{1}\" />", (int) prizeGrade, model.GiveShopBookId);
                builder.AppendFormat("<input type=\"hidden\" id=\"txtProductPic{0}\" name=\"txtProductPic{0}\"  value=\"{1}\" />", (int) prizeGrade, string.IsNullOrEmpty(model.GriveShopBookPicUrl) ? "../images/u100.png" : model.GriveShopBookPicUrl);
            }
            else
            {
                builder.AppendFormat("<img id=\"imgProduct{0}\" style=\"width:59px; height:59px;\" name=\"imgProduct{0}\" src=\"../images/u100.png\" onclick=\"SelectShopBookId({0});\" />", (int) prizeGrade);
                builder.AppendFormat("<input type=\"hidden\" name=\"txtShopbookId{0}\" id=\"txtShopbookId{0}\" />", (int) prizeGrade);
                builder.AppendFormat("<input type=\"hidden\" id=\"txtProductPic{0}\" name=\"txtProductPic{0}\"  />", (int) prizeGrade);
            }
            builder.Append("</div> </div></div>");
            builder.Append("<div class=\"form-group setmargin\">");
            builder.Append(" <label class=\"col-xs-3 pad resetSize control-label\" for=\"pausername\"><em>*</em>&nbsp;&nbsp;奖品数量：</label> <div class=\"form-inline col-xs-9\">");
            if (model != null)
            {
                builder.AppendFormat("<input type=\"text\" name=\"txtPrizeCount{0}\" id=\"txtPrizeCount{0}\" class=\"form-control resetSize\" value=\"{1}\"/>", (int) prizeGrade, model.PrizeCount);
            }
            else
            {
                builder.AppendFormat("<input type=\"text\" name=\"txtPrizeCount{0}\" id=\"txtPrizeCount{0}\" class=\"form-control resetSize\" value=\"0\"/>", (int) prizeGrade);
            }
            builder.Append("  <small>奖品数量为0时不设此奖项</small> </div> </div>");
            builder.Append("<div class=\"form-group setmargin\">");
            builder.Append("<label class=\"col-xs-3 pad resetSize control-label\" for=\"pausername\"><em>*</em>&nbsp;&nbsp;中奖率：</label> <div class=\"form-inline col-xs-9\">");
            if (model != null)
            {
                builder.AppendFormat("<input type=\"text\" name=\"txtPrizeRate{0}\" id=\"txtPrizeRate{0}\" class=\"form-control resetSize\" value=\"{1}\" />&nbsp;%", (int) prizeGrade, model.PrizeRate);
            }
            else
            {
                builder.AppendFormat("<input type=\"text\" name=\"txtPrizeRate{0}\" id=\"txtPrizeRate{0}\" class=\"form-control resetSize\" value=\"0\" />&nbsp;%", (int) prizeGrade);
            }
            builder.Append("<small>中奖率必须为整数</small></div> </div></div> ");
            builder.Append("</div>");
            return builder.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindDdlCouponId();
            }
        }

        protected string PrizeInfoHtml()
        {
            StringBuilder builder = new StringBuilder();
            if (this.PrizeLists != null)
            {
                builder.Append(this.GetPrizeInfoHtml(PrizeGrade.一等奖, this.PrizeLists.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.一等奖)));
                builder.Append(this.GetPrizeInfoHtml(PrizeGrade.二等奖, this.PrizeLists.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.二等奖)));
                builder.Append(this.GetPrizeInfoHtml(PrizeGrade.三等奖, this.PrizeLists.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.三等奖)));
                builder.Append(this.GetPrizeInfoHtml(PrizeGrade.普通奖, this.PrizeLists.FirstOrDefault<GamePrizeInfo>(p => p.PrizeGrade == PrizeGrade.普通奖)));
            }
            else
            {
                builder.Append(this.GetPrizeInfoHtml(PrizeGrade.一等奖, null));
                builder.Append(this.GetPrizeInfoHtml(PrizeGrade.二等奖, null));
                builder.Append(this.GetPrizeInfoHtml(PrizeGrade.三等奖, null));
                builder.Append(this.GetPrizeInfoHtml(PrizeGrade.普通奖, null));
            }
            return builder.ToString();
        }

        protected IList<ListItem> CouponIdList
        {
            get
            {
                return this._couponList;
            }
        }

        public string NotPrzeDescription
        {
            get
            {
                return this.txtNotPrzeDescription.Text.Trim();
            }
            set
            {
                this.txtNotPrzeDescription.Text = value;
            }
        }

        public IList<GamePrizeInfo> PrizeLists
        {
            get
            {
                this.GetDate();
                return this._prizeLists;
            }
            set
            {
                this._prizeLists = value;
            }
        }
    }
}

