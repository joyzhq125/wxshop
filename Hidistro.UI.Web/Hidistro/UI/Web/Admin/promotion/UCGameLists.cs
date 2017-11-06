namespace Hidistro.UI.Web.Admin.promotion
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Web.UI.WebControls;

    public class UCGameLists : BaseUserControl
    {
        protected Button btnDel;
        protected Button btnSeach;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected Grid grdGameLists;
        protected PageSize hrefPageSize;
        protected string isFinished = "0";
        private int pageIndex = 1;
        protected Pager pager1;
        private int pageSize = 10;

        private void BindData()
        {
            GameSearch search = new GameSearch
            {
                SortBy = "GameId",
                PageIndex = this.pageIndex,
                PageSize = this.pageSize,
                GameType = (int?)this.PGameType.Value,
                BeginTime = this.calendarStartDate.SelectedDate,
                EndTime = this.calendarEndDate.SelectedDate
            };
            string isFinished = this.isFinished;
            if (!string.IsNullOrEmpty(isFinished))
            {
                search.Status = isFinished;
            }
            DbQueryResult gameListByView = GameHelper.GetGameListByView(search);
            DataTable data = (DataTable)gameListByView.Data;
            this.grdGameLists.DataSource = data;
            this.grdGameLists.DataBind();
            this.pager1.TotalRecords = gameListByView.TotalRecords;
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            List<int> list = new List<int>();
            foreach (GridViewRow row in this.grdGameLists.Rows)
            {
                if (row.RowIndex >= 0)
                {
                    CheckBox box = row.Cells[0].Controls[0] as CheckBox;
                    if (box.Checked)
                    {
                        list.Add(int.Parse(this.grdGameLists.DataKeys[row.RowIndex].Value.ToString()));
                    }
                }
            }
            if (list.Count <= 0)
            {
                this.ShowMsg("请至少选择一条要删除的数据！", false);
            }
            else
            {
                try
                {
                    if (!GameHelper.Delete(list.ToArray()))
                    {
                        throw new Exception("操作失败！");
                    }
                    this.ShowMsg("操作成功！", true);
                    this.BindData();
                }
                catch (Exception exception)
                {
                    this.ShowMsg(exception.Message, false);
                }
            }
        }

        protected void btnSeach_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        protected string GetEditUrl(string gameId)
        {
            GameType valueOrDefault = this.PGameType.GetValueOrDefault();
            if (this.PGameType.HasValue)
            {
                switch (valueOrDefault)
                {
                    case GameType.幸运大转盘:
                        return string.Format("EditGame.aspx?gameId={0}", gameId);

                    case GameType.疯狂砸金蛋:
                        return string.Format("EditGameEgg.aspx?gameId={0}", gameId);

                    case GameType.好运翻翻看:
                        return string.Format("EditGameXingYun.aspx?gameId={0}", gameId);

                    case GameType.大富翁:
                        return string.Format("EditGameDaFuWen.aspx?gameId={0}", gameId);

                    case GameType.刮刮乐:
                        return string.Format("EditGameGuaGuaLe.aspx?gameId={0}", gameId);
                }
            }
            return "";
        }

        protected string GetPrizeListsUrl(string gameId)
        {
            GameType valueOrDefault = this.PGameType.GetValueOrDefault();
            if (this.PGameType.HasValue)
            {
                switch (valueOrDefault)
                {
                    case GameType.幸运大转盘:
                        return string.Format("PrizeLists.aspx?gameId={0}", gameId);

                    case GameType.疯狂砸金蛋:
                        return string.Format("PrizeListsEgg.aspx?gameId={0}", gameId);

                    case GameType.好运翻翻看:
                        return string.Format("PrizeListsHaoYun.aspx?gameId={0}", gameId);

                    case GameType.大富翁:
                        return string.Format("PrizeListsDaFuWen.aspx?gameId={0}", gameId);

                    case GameType.刮刮乐:
                        return string.Format("PrizeListsGuaGuaLe.aspx?gameId={0}", gameId);
                }
            }
            return "";
        }

        protected void grdGameLists_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex >= 0)
            {
                if (int.Parse(((HiddenField)e.Row.FindControl("hfStatus")).Value) == 0)
                {
                    ((LinkButton)e.Row.FindControl("lkDelete")).Visible = false;
                }
                else
                {
                    ((LinkButton)e.Row.FindControl("FinishBtn")).Visible = false;
                }
            }
        }

        protected void grdGameLists_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string str = this.grdGameLists.DataKeys[e.RowIndex].Value.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    if (!GameHelper.Delete(new int[] { int.Parse(str) }))
                    {
                        throw new Exception("操作失败！");
                    }
                    this.ShowMsg("操作成功！", true);
                    this.BindData();
                }
                catch (Exception exception)
                {
                    this.ShowMsg(exception.Message, false);
                }
            }
        }

        protected void grdGameLists_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string str = this.grdGameLists.DataKeys[e.RowIndex].Value.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    if (!GameHelper.UpdateStatus(int.Parse(str), GameStatus.结束))
                    {
                        throw new Exception("操作失败！");
                    }
                    this.ShowMsg("操作成功！", true);
                    this.BindData();
                }
                catch (Exception exception)
                {
                    this.ShowMsg(exception.Message, false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.isFinished = base.Request.QueryString["isFinished"];
            if (string.IsNullOrEmpty(this.isFinished))
            {
                this.isFinished = "0";
            }
            try
            {
                this.pageIndex = int.Parse(base.Request["pageindex"]);
            }
            catch (Exception)
            {
                this.pageIndex = 1;
            }
            try
            {
                this.pageSize = int.Parse(base.Request.QueryString["pagesize"]);
            }
            catch (Exception)
            {
                this.pageSize = 10;
            }
            this.pager1.DefaultPageSize = this.pageSize;
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }

        public GameType? PGameType { get; set; }
    }
}

