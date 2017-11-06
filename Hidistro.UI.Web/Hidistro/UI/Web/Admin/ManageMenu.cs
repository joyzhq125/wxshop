namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Weixin.MP.Api;
    using Hishop.Weixin.MP.Domain;
    using Hishop.Weixin.MP.Domain.Menu;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ManageMenu : AdminPage
    {
        protected Button btnSubmit;
        protected Grid grdMenu;
        protected ManageMenu() : base("", "")
        {
        }

        private void BindData()
        {
            this.grdMenu.DataSource = VShopHelper.GetMenus(this.wid);
            this.grdMenu.DataBind();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            IList<MenuInfo> initMenus = VShopHelper.GetInitMenus(this.wid);
            Hishop.Weixin.MP.Domain.Menu.Menu menu = new Hishop.Weixin.MP.Domain.Menu.Menu();
            foreach (MenuInfo info in initMenus)
            {
                if ((info.Chilren == null) || (info.Chilren.Count == 0))
                {
                    menu.menu.button.Add(this.BuildMenu(info));
                }
                else
                {
                    SubMenu item = new SubMenu {
                        name = info.Name
                    };
                    foreach (MenuInfo info2 in info.Chilren)
                    {
                        item.sub_button.Add(this.BuildMenu(info2));
                    }
                    menu.menu.button.Add(item);
                }
            }
            string json = JsonConvert.SerializeObject(menu.menu);
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false,wid);
            if (string.IsNullOrEmpty(masterSettings.WeixinAppId) || string.IsNullOrEmpty(masterSettings.WeixinAppSecret))
            {
                base.Response.Write("<script>alert('您的服务号配置存在问题，请您先检查配置！');location.href='PayConfig.aspx'</script>");
            }
            else if (MenuApi.CreateMenus(JsonConvert.DeserializeObject<Token>(TokenApi.GetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret)).access_token, json).Contains("ok"))
            {
                this.ShowMsg("成功的把自定义菜单保存到了微信", true);
            }
            else
            {
                this.ShowMsg("操作失败!服务号配置信息错误或没有微信自定义菜单权限", false);
            }
        }

        private SingleButton BuildMenu(MenuInfo menu)
        {
            switch (menu.BindType)
            {
                case BindType.Key:
                    return new SingleClickButton { name = menu.Name, key = menu.MenuId.ToString() };

                case BindType.Topic:
                case BindType.HomePage:
                case BindType.ProductCategory:
                case BindType.ShoppingCar:
                case BindType.OrderCenter:
                case BindType.MemberCard:
                case BindType.Url:
                    return new SingleViewButton { name = menu.Name, url = menu.Url };
            }
            return new SingleClickButton { name = menu.Name, key = "None" };
        }

        private void grdMenu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            int menuId = (int) this.grdMenu.DataKeys[rowIndex].Value;
            if (e.CommandName == "Fall")
            {
                VShopHelper.SwapMenuSequence(menuId, false,this.wid);
            }
            else if (e.CommandName == "Rise")
            {
                VShopHelper.SwapMenuSequence(menuId, true,this.wid);
            }
            else if (e.CommandName == "DeleteMenu")
            {
                if (VShopHelper.DeleteMenu(menuId))
                {
                    this.ShowMsg("成功删除了指定的分类", true);
                }
                else
                {
                    this.ShowMsg("分类删除失败，未知错误", false);
                }
            }
            this.BindData();
        }

        private void grdMenu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int num = (int) DataBinder.Eval(e.Row.DataItem, "ParentMenuId");
                string str = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                if (num == 0)
                {
                    str = "<b>" + str + "</b>";
                }
                if (num > 0)
                {
                    str = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + str;
                }
                Literal literal = e.Row.FindControl("lblCategoryName") as Literal;
                literal.Text = str;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.grdMenu.RowCommand += new GridViewCommandEventHandler(this.grdMenu_RowCommand);
            this.grdMenu.RowDataBound += new GridViewRowEventHandler(this.grdMenu_RowDataBound);
            this.btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

        protected string RenderInfo(object menuIdObj)
        {
            ReplyInfo reply = ReplyHelper.GetReply((int) menuIdObj);
            if (reply != null)
            {
                return reply.Keys;
            }
            return string.Empty;
        }
    }
}

