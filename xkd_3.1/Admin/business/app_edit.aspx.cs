using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.Entities.Store;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Globalization;
using Hidistro.Entities.Members;
using Hidistro.ControlPanel.Members;

using Hishop.Weixin.MP.Test;

namespace XKD.Web.Admin
{
    public partial class app_edit : AdminPage /*System.Web.UI.Page*/
    {
        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            action = DTRequest.GetQueryString("action");
            id = DTRequest.GetQueryInt("id");
            if (!Page.IsPostBack)
            {
                //ChkAdminLevel("app_list", DTEnums.ActionEnum.View.ToString()); //检查权限
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
                else
                {
                    //if (string.IsNullOrEmpty(txttoken_value.Text))
                    //{
                    //    txttoken_value.Text = this.CreateKey(8);
                    //}
                }
            }
        }
        private string CreateKey(int len)
        {
            byte[] data = new byte[len];
            new RNGCryptoServiceProvider().GetBytes(data);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(string.Format("{0:X2}", data[i]));
            }
            return builder.ToString();
        }

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            SF.BLL.sf_website bll = new SF.BLL.sf_website();
            SF.Model.sf_website model = bll.GetModel(_id);
            txtsitename.Text = model.sitename;
            //txtappid_name.Text = model.appid_name;
            //txtappid_origin_id.Text = model.appid_origin_id;
            //txtweixin_account.Text = model.weixin_account;
            //txtavatar.Text = model.avatar;
            //txtinterface_url.Text = model.interface_url;
            //if(string.IsNullOrEmpty(model.token_value))
            //{
            //    //model.token_value=this.CreateKey(8);
            //}
            //txttoken_value.Text = model.token_value;
            //txtencodingaeskey.Text = model.encodingaeskey;
            //txtappid.Text = model.appid;
            //txtappsecret.Text = model.appsecret;

            //txtpayment_name.Text = model.payment_name;
            //cbstate.Checked = model.state == 1 ? true : false;
            //txtweixin_pay_account.Text = model.weixin_pay_account;
            //txtaccount_pay_key.Text = model.account_pay_key;
            //rblsend_type.SelectedValue = model.send_type.ToString();
            //txtlogo.Text = model.logo;
            //txtdescription.Text = model.description;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            bool result = false;
            SF.BLL.sf_website bll = new SF.BLL.sf_website();
            SF.Model.sf_website model = new SF.Model.sf_website();
            ManagerInfo model1 = (ManagerInfo)Session[DTKeys.SESSION_ADMIN_INFO];

            //model.businessNum = model1.businessNum;
            model.sitename = txtsitename.Text.Trim();
            //model.appid_name = txtappid_name.Text.Trim();
            //model.appid_origin_id = txtappid_origin_id.Text.Trim();
            //model.weixin_account = txtweixin_account.Text.Trim();
            //model.avatar = txtavatar.Text;
            //model.interface_url = txtinterface_url.Text.Trim();
            //model.token_value = txttoken_value.Text.Trim();
            //if (string.IsNullOrEmpty(model.token_value))
            //{
            //    //model.token_value = this.CreateKey(8);
            //}
            //model.encodingaeskey = txtencodingaeskey.Text.Trim();
            //model.appid = txtappid.Text.Trim().Trim();
            //model.appsecret = txtappsecret.Text.Trim();

            //model.payment_name = txtpayment_name.Text;
            ////model.state = cbstate.Checked ? 1 : 0;
            //model.weixin_pay_account = txtweixin_pay_account.Text.Trim();
            //model.account_pay_key = txtaccount_pay_key.Text.Trim();
            //model.send_type = int.Parse(rblsend_type.SelectedValue);
            //model.logo = txtlogo.Text;
            //model.description = txtdescription.Text;

            model.create_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            model.create_user = model1.realname;
            model.mid = model1.UserId;
            model.wid = "W" + SFUtils.GetCheckCode(4).ToString() + DateTime.Now.ToString("yyyyMMddHHmmss");
            if (bll.Add(model) > 0)
            {
                //AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加公众服务号:" + model.appid_name); //记录日志
                //1.拷贝默认模板
                CopyTemplate(model.wid);

                //2.增加默认会员类别
                MemberGradeInfo memberGrade = new MemberGradeInfo();
                memberGrade.wid = model.wid;
                memberGrade.Name = "普通会员";
                memberGrade.Description = "普通会员";
                memberGrade.IsDefault = true;
                memberGrade.TranVol = 999999999.00;
                memberGrade.TranTimes = 999999999;
                //折扣
                memberGrade.Discount = 100;
                result = MemberHelper.CreateMemberGrade(memberGrade);
            }
            return result;
        }
        #endregion


        void CopyTemplate(string wid)
        {
            if(Directory.Exists(base.Server.MapPath("/Templates/vshop/" + wid + "/")))
            {
                Directory.Delete(base.Server.MapPath("/Templates/vshop/" + wid + "/"), true);
            }
            Directory.CreateDirectory(base.Server.MapPath("/Templates/vshop/" + wid + "/"));
            CopyOldLabFilesToNewLab(base.Server.MapPath("/Templates/vshop_bak/"), base.Server.MapPath("/Templates/vshop/" + wid + "/"));
            //string[] strArray = Directory.Exists(base.Server.MapPath("/Templates/vshop/")) ? Directory.GetDirectories(base.Server.MapPath("/Templates/vshop/")) : null;
            //if(strArray != null)
            //{
            //    Directory.CreateDirectory("/Templates/vsho/" + wid + "/");
            //    foreach (string str in strArray)
            //    {
            //        DirectoryInfo info = new DirectoryInfo(str);
            //        //获取文件夹名
            //        string str2 = info.Name.ToLower(CultureInfo.InvariantCulture);
            //        if ((str2.Length > 0) && str2.StartsWith("t"))
            //        {
            //            //copy 文件夹
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="sourcePath">lab文件所在目录(@"~\labs\oldlab")</param>
        /// <param name="savePath">保存的目标目录(@"~\labs\newlab")</param>
        /// <returns>返回:true-拷贝成功;false:拷贝失败</returns>
        public bool CopyOldLabFilesToNewLab(string sourcePath, string savePath)
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            #region //拷贝labs文件夹到savePath下
            try
            {
                string[] labDirs = Directory.GetDirectories(sourcePath);//目录
                string[] labFiles = Directory.GetFiles(sourcePath);//文件
                if (labFiles.Length > 0)
                {
                    for (int i = 0; i < labFiles.Length; i++)
                    {
                        if (Path.GetFileName(labFiles[i]) != ".lab")//排除.lab文件
                        {
                            File.Copy(sourcePath + "\\" + Path.GetFileName(labFiles[i]), savePath + "\\" + Path.GetFileName(labFiles[i]), true);
                        }
                    }
                }
                if (labDirs.Length > 0)
                {
                    for (int j = 0; j < labDirs.Length; j++)
                    {
                        Directory.GetDirectories(sourcePath + "\\" + Path.GetFileName(labDirs[j]));

                        //递归调用
                        CopyOldLabFilesToNewLab(sourcePath + "\\" + Path.GetFileName(labDirs[j]), savePath + "\\" + Path.GetFileName(labDirs[j]));
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            #endregion
            return true;
        }

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            bool result = false;
            //调整AppId时，需要调整的表
            string[] strTables = null;
            SF.BLL.sf_website bll = new SF.BLL.sf_website();
            SF.Model.sf_website model = bll.GetModel(_id);

            //if (txtappid.Text.Trim().Equals(model.appid) == false)
            //{
            //    //strTables = new string[] { "sf_contract_child", "sf_user_info", "sf_goods_info", "sf_goods_amounts", "sf_goods_cart", "sf_goods_color", "sf_goods_img", "sf_goods_index_pics", "sf_goods_order", "sf_goods_orderdetal", "sf_goods_size", "sf_goods_types", "sf_user_address", "sf_user_bank", "sf_user_withdraw_order", "sf_auto_reply" };
            //}

            model.id = _id;
            model.sitename = txtsitename.Text.Trim();
            //model.appid_name = txtappid_name.Text;
            //model.appid_origin_id = txtappid_origin_id.Text;
            //model.weixin_account = txtweixin_account.Text;
            //model.avatar = txtavatar.Text;
            //model.interface_url = txtinterface_url.Text;
            //model.token_value = txttoken_value.Text;
            //if (string.IsNullOrEmpty(model.token_value))
            //{
            //    //model.token_value = this.CreateKey(8);
            //}
            //model.encodingaeskey = txtencodingaeskey.Text;
            //model.appid = txtappid.Text.Trim();
            //model.appsecret = txtappsecret.Text;

            //model.payment_name = txtpayment_name.Text;
            ////model.state = cbstate.Checked ? 1 : 0;
            //model.weixin_pay_account = txtweixin_pay_account.Text;
            //model.account_pay_key = txtaccount_pay_key.Text;
            //model.send_type = int.Parse(rblsend_type.SelectedValue);
            //model.logo = txtlogo.Text;
            //model.description = txtdescription.Text;

            if (bll.Update(model, strTables) == true)
            {
                //AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改公众服务号:" + model.appid_name); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //验证appid是否重复
            bool repeat = false;
            SF.BLL.sf_website bll = new SF.BLL.sf_website();

            if (action == DTEnums.ActionEnum.Add.ToString())
            {
                //System.Data.DataSet ds = bll.GetList(" appid = '" + txtappid.Text.Trim() + "' ");
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    repeat = true;
                //}
            }
            else if (action == DTEnums.ActionEnum.Edit.ToString())
            {
                //System.Data.DataSet ds = bll.GetList(" id <> " + this.id + " and appid = '" + txtappid.Text.Trim() + "' ");
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    repeat = true;
                //}
            }

            if (repeat == true)
            {
                JscriptMsg("提示：AppId在系统中重复,<br>请检查您的AppId或者联系官方系统管理员！", string.Empty);
                return;
            }

            if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
                //ChkAdminLevel("app_edit", DTEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误啦！", string.Empty);
                    return;
                }
                JscriptMsg("修改信息成功！", "app_list.aspx");
            }
            else //添加
            {
                //ChkAdminLevel("app_edit", DTEnums.ActionEnum.Add.ToString()); //检查权限 

                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", string.Empty);
                    return;
                }
                JscriptMsg("添加信息成功！", "app_list.aspx");
            }
        }

        protected void test_Click(object sender, EventArgs e)
        {
            new Utils().Test02();
        }
    }
}