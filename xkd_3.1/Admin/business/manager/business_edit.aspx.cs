using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XKD.Web.Admin
{
    public partial class business_edit : AdminPage
    {
        string defaultpassword = "0|0|0|0"; //默认显示密码
        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                if (!int.TryParse(Request.QueryString["id"] as string, out this.id))
                {
                    JscriptMsg("传输参数不正确！", "back");
                    return;
                }
                if (!new SF.BLL.manager().Exists(this.id))
                {
                    JscriptMsg("记录不存在或已被删除！", "back");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("business_list", DTEnums.ActionEnum.View.ToString()); //检查权限
                ManagerInfo model = GetAdminInfo(); //取得管理员信息
                RoleBind(ddlRoleId);
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
            }
        }

        #region 角色类型=================================
        private void RoleBind(DropDownList ddl)
        {
            SF.BLL.manager_role bll = new SF.BLL.manager_role();
            string whereStr = "id>1";
            if (this.id == 1)
            {
                whereStr = "id=1";
            }
            DataTable dt = bll.GetList(whereStr).Tables[0];

            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("请选择角色...", ""));
            foreach (DataRow dr in dt.Rows)
            {
                //if (Convert.ToInt32(dr["role_type"]) >= role_type)
                {
                    ddl.Items.Add(new ListItem(dr["role_name"].ToString(), dr["id"].ToString()));
                }
            }
        }
        #endregion

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            SF.BLL.manager bll = new SF.BLL.manager();
            ManagerInfo model = bll.GetModel(_id);
            ddlRoleId.SelectedValue = model.RoleId.ToString();
            if (model.islock == "0")
            {
                cbIsLock.Checked = true;
            }
            else
            {
                cbIsLock.Checked = false;
            }
            txtUserName.Text = model.UserName;
            txtUserName.ReadOnly = true;
            txtUserName.Attributes.Remove("ajaxurl");
            if (!string.IsNullOrEmpty(model.Password))
            {
                txtPassword.Attributes["value"] = txtPassword1.Attributes["value"] = defaultpassword;
            }
            txtRealName.Text = model.realname;
            txtTelephone.Text = model.telephone;
            txtEmail.Text = model.Email;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            ManagerInfo model = new ManagerInfo();
            //SF.Model.manager model = new SF.Model.manager();
            SF.BLL.manager bll = new SF.BLL.manager();
            model.RoleId = int.Parse(ddlRoleId.SelectedValue);
            //model.role_type = new SF.BLL.manager_role().GetModel(model.role_id).role_type;
            //model.busnieseNum = "B" + DateTime.Now.ToString("yyyyMMddHHmmss");
            
            //model.type = 1;
            if (cbIsLock.Checked == true)
            {
                model.islock = "0";
            }
            else
            {
                model.islock = "1";
            }
            //检测用户名是否重复
            if (bll.Exists(txtUserName.Text.Trim()))
            {
                return false;
            }
            model.UserName = txtUserName.Text.Trim();
            //获得6位的salt加密字符串
            //model.salt = SFUtils.GetCheckCode(6);
            //以随机生成的6位字符串做为密钥加密
            model.Password = HiCryptographer.Md5Encrypt(txtPassword.Text.Trim());//DESEncrypt.Encrypt(txtPassword.Text.Trim(), model.salt);
            model.realname = txtRealName.Text.Trim();
            model.telephone = txtTelephone.Text.Trim();
            model.Email = txtEmail.Text.Trim();
            model.CreateDate = DateTime.Now;

            if (bll.Add(model) > 0)
            {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加商户:" + model.UserName); //记录日志
                return true;
            }
            return false;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            bool result = false;
            SF.BLL.manager bll = new SF.BLL.manager();
            ManagerInfo model = bll.GetModel(_id);

            model.RoleId = int.Parse(ddlRoleId.SelectedValue);
            //model.role_type = new BLL.manager_role().GetModel(model.role_id).role_type;
            if (cbIsLock.Checked == true)
            {
                model.islock = "0";
            }
            else
            {
                model.islock = "1";
                if (_id == 1)
                {
                    return false;
                }
            }
            //判断密码是否更改
            if (txtPassword.Text.Trim() != defaultpassword)
            {
                //获取用户已生成的salt作为密钥加密
                model.Password = HiCryptographer.Md5Encrypt(txtPassword.Text.Trim());//DESEncrypt.Encrypt(txtPassword.Text.Trim(), model.salt);
            }
            model.realname = txtRealName.Text.Trim();
            model.telephone = txtTelephone.Text.Trim();
            model.Email = txtEmail.Text.Trim();

            if (bll.Update(model))
            {
                AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改商户:" + model.UserName); //记录日志
                result = true;
            }

            return result;
        }
        #endregion

        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("business_list", DTEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    if (this.id == 1)
                    {
                        JscriptMsg("不可禁用超级用户！", "business_list.aspx");
                    }
                    else
                    {
                        JscriptMsg("保存过程中发生错误！", "");
                    }
                    return;
                }
                JscriptMsg("修改商户信息成功！", "business_list.aspx");
            }
            else //添加
            {
                ChkAdminLevel("business_list", DTEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("添加商户信息成功！请尽快到合同管理填写合同信息....", "business_list.aspx");
            }
        }
    }
}