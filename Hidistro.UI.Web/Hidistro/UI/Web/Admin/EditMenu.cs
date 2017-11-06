namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Linq;

    public class EditMenu : AdminPage
    {
        protected Button btnAddMenu;
        protected DropDownList ddlType;
        protected DropDownList ddlValue;
        protected Literal lblParent;
        protected HtmlGenericControl liBind;
        protected HtmlGenericControl liParent;
        protected HtmlGenericControl liUrl;
        protected HtmlGenericControl liValue;
        protected TextBox txtMenuName;
        protected TextBox txtUrl;
        protected string wid;
        protected EditMenu() : base("", "")
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
                MenuInfo menu = VShopHelper.GetMenu(base.GetUrlIntParam("MenuId"),this.wid);
                menu.Name = this.txtMenuName.Text;
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
                if (VShopHelper.UpdateMenu(menu))
                {
                    base.Response.Redirect("ManageMenu.aspx");
                }
                else
                {
                    this.ShowMsg("添加失败", false);
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
                int urlIntParam = base.GetUrlIntParam("MenuId");
                MenuInfo menu = VShopHelper.GetMenu(urlIntParam,this.wid);
                this.txtMenuName.Text = menu.Name;
                if (menu.ParentMenuId == 0)
                {
                    if (VShopHelper.GetMenusByParentId(urlIntParam,this.wid).Count > 0)
                    {
                        this.liBind.Visible = false;
                    }
                    this.liParent.Visible = false;
                }
                else
                {
                    this.lblParent.Text = VShopHelper.GetMenu(menu.ParentMenuId,this.wid).Name;
                }
                this.ddlType.SelectedValue = Convert.ToString((int) menu.BindType);
                switch (menu.BindType)
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
                switch (menu.BindType)
                {
                    case BindType.Key:
                        this.ddlValue.DataSource = from a in ReplyHelper.GetAllReply(this.wid)
                            where !string.IsNullOrWhiteSpace(a.Keys)
                            select a;
                        this.ddlValue.DataTextField = "Keys";
                        this.ddlValue.DataValueField = "Id";
                        this.ddlValue.DataBind();
                        this.ddlValue.SelectedValue = menu.ReplyId.ToString();
                        return;

                    case BindType.Topic:
                        this.ddlValue.DataSource = VShopHelper.Gettopics();
                        this.ddlValue.DataTextField = "Title";
                        this.ddlValue.DataValueField = "TopicId";
                        this.ddlValue.DataBind();
                        this.ddlValue.SelectedValue = menu.Content;
                        return;

                    case BindType.Url:
                        this.txtUrl.Text = menu.Content;
                        break;

                    default:
                        return;
                }
            }
        }
    }
}

