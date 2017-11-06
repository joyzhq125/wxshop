namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Linq;

    public class AddMenu : AdminPage
    {
        protected Button btnAddMenu;
        protected DropDownList ddlType;
        protected DropDownList ddlValue;
        protected Literal lblParent;
        protected HtmlGenericControl liParent;
        protected HtmlGenericControl liUrl;
        protected HtmlGenericControl liValue;
        protected TextBox txtCategoryName;
        protected TextBox txtUrl;
        protected string wid;
        protected AddMenu() : base("", "")
        {
        }

        private void btnAddMenu_Click(object sender, EventArgs e)
        {
            if ((this.ddlType.SelectedValue == "1") && (this.ddlValue.Items.Count <= 0))
            {
                this.ShowMsg("关键字不能为空", false);
            }
            else
            {
                MenuInfo menu = new MenuInfo {
                    Name = this.txtCategoryName.Text,
                    ParentMenuId = base.GetUrlIntParam("pid"),
                    wid=this.wid
                };
                if (!VShopHelper.CanAddMenu(menu.ParentMenuId,this.wid))
                {
                    this.ShowMsg("一级菜单不能超过三个，对应二级菜单不能超过五个", false);
                }
                else
                {
                    menu.Bind = Convert.ToInt32(this.ddlType.SelectedValue);
                    BindType bindType = menu.BindType;
                    switch (bindType)
                    {
                        case BindType.Key:
                            menu.ReplyId = Convert.ToInt32(this.ddlValue.SelectedValue);
                            break;

                        case BindType.Topic:
                            menu.Content = this.ddlValue.SelectedValue;
                            break;

                        default:
                            if (bindType == BindType.Url)
                            {
                                menu.Content = this.txtUrl.Text.Trim();
                            }
                            break;
                    }
                    menu.Type = "click";
                    if (menu.ParentMenuId == 0)
                    {
                        menu.Type = "view";
                    }
                    else if (string.IsNullOrEmpty(this.ddlType.SelectedValue) || (this.ddlType.SelectedValue == "0"))
                    {
                        this.ShowMsg("二级菜单必须绑定一个对象", false);
                        return;
                    }
                    if (VShopHelper.SaveMenu(menu))
                    {
                        base.Response.Redirect("ManageMenu.aspx");
                    }
                    else
                    {
                        this.ShowMsg("添加失败", false);
                    }
                }
            }
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindType type = (BindType) Convert.ToInt32(this.ddlType.SelectedValue);
            switch (type)
            {
                case BindType.Key:
                case BindType.Topic:
                    this.liUrl.Visible = false;
                    this.liValue.Visible = true;
                    break;

                case BindType.Url:
                    this.liUrl.Visible = true;
                    this.liValue.Visible = false;
                    break;

                default:
                    this.liUrl.Visible = false;
                    this.liValue.Visible = false;
                    break;
            }
            switch (type)
            {
                case BindType.Key:
                    this.ddlValue.DataSource = from a in ReplyHelper.GetAllReply(this.wid)
                        where !string.IsNullOrWhiteSpace(a.Keys)
                        select a;
                    this.ddlValue.DataTextField = "Keys";
                    this.ddlValue.DataValueField = "Id";
                    this.ddlValue.DataBind();
                    return;

                case BindType.Topic:
                    this.ddlValue.DataSource = VShopHelper.Gettopics();
                    this.ddlValue.DataTextField = "Title";
                    this.ddlValue.DataValueField = "TopicId";
                    this.ddlValue.DataBind();
                    return;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wid = GetCurWebId();
            if (string.IsNullOrEmpty(wid)) return;
            this.btnAddMenu.Click += new EventHandler(this.btnAddMenu_Click);
            if (!this.Page.IsPostBack)
            {
                this.liValue.Visible = false;
                this.liUrl.Visible = false;
                if (base.GetUrlIntParam("pid") == 0)
                {
                    this.liParent.Visible = false;
                }
                else
                {
                    this.lblParent.Text = VShopHelper.GetMenu(base.GetUrlIntParam("pid"),this.wid).Name;
                }
            }
        }
    }
}

