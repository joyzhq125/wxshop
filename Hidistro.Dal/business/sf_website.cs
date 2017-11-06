/**  版本信息模板在安装目录下，可自行修改。
* sf_website.cs
*
* 功 能： N/A
* 类 名： sf_website
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015-08-25 14:59:47   N/A    初版
*
* Copyright (c) 2012 SF Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
//using SF.DBUtility;
//using SF.Common;//Please add references
using Hidistro.Core;
namespace SF.DAL
{
	/// <summary>
	/// 数据访问类:sf_website
	/// </summary>
	public partial class sf_website
	{
		public sf_website()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from sf_website");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.BigInt)
			};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(SF.Model.sf_website model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into sf_website(");
			strSql.Append("businessNum,mid,templatesNum,appid_name,appid_origin_id,weixin_account,avatar,interface_url,token_value,encodingaeskey,appid,appsecret,create_user,create_time,payment_name,state,weixin_pay_account,account_pay_key,send_type,logo,description,wid,sitename,tel,Enableweixinrequest,WeixinCertPassword,Alipay_mid,Alipay_mName,Alipay_Pid ,Alipay_Key,OffLinePayContent,EnableWeixinRed,EnableAlipayRequest,EnablePodRequest,EnableOffLineRequest,WeixinCertPath,OpenManyService,GuidePageSet,EnableGuidePageSet,ManageOpenID,EnableShopMenu,EnableSaleService,entId)");
			strSql.Append(" values (");
			strSql.Append("@businessNum,@mid,@templatesNum,@appid_name,@appid_origin_id,@weixin_account,@avatar,@interface_url,@token_value,@encodingaeskey,@appid,@appsecret,@create_user,@create_time,@payment_name,@state,@weixin_pay_account,@account_pay_key,@send_type,@logo,@description,@wid,@sitename,@tel,@Enableweixinrequest,@WeixinCertPassword,@Alipay_mid,@Alipay_mName,@Alipay_Pid ,@Alipay_Key,@OffLinePayContent,@EnableWeixinRed,@EnableAlipayRequest,@EnablePodRequest,@EnableOffLineRequest,@WeixinCertPath,@OpenManyService,@GuidePageSet,@EnableGuidePageSet,@ManageOpenID,@EnableShopMenu,@EnableSaleService,@entId)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
                    new SqlParameter("@businessNum", SqlDbType.NVarChar,50),
                    new SqlParameter("@mid", SqlDbType.Int,4),
                    new SqlParameter("@templatesNum", SqlDbType.NVarChar,50),
					new SqlParameter("@appid_name", SqlDbType.NVarChar,100),
					new SqlParameter("@appid_origin_id", SqlDbType.NVarChar,100),
					new SqlParameter("@weixin_account", SqlDbType.NVarChar,100),
					new SqlParameter("@avatar", SqlDbType.NVarChar,100),
					new SqlParameter("@interface_url", SqlDbType.NVarChar,200),
					new SqlParameter("@token_value", SqlDbType.NVarChar,100),
					new SqlParameter("@encodingaeskey", SqlDbType.NVarChar,100),
					new SqlParameter("@appid", SqlDbType.NVarChar,100),
					new SqlParameter("@appsecret", SqlDbType.NVarChar,100),
					new SqlParameter("@create_user", SqlDbType.NVarChar,50),
					new SqlParameter("@create_time", SqlDbType.NVarChar,50),
					new SqlParameter("@payment_name", SqlDbType.NVarChar,100),
					new SqlParameter("@state", SqlDbType.Int,4),
					new SqlParameter("@weixin_pay_account", SqlDbType.NVarChar,100),
					new SqlParameter("@account_pay_key", SqlDbType.NVarChar,100),
					new SqlParameter("@send_type", SqlDbType.Int,4),
					new SqlParameter("@logo", SqlDbType.NVarChar,200),
					new SqlParameter("@description", SqlDbType.NVarChar,1000),
                    new SqlParameter("@wid", SqlDbType.NVarChar,50),
                    new SqlParameter("@sitename", SqlDbType.NVarChar,200),
                    new SqlParameter("@tel", SqlDbType.NVarChar,50),                  
                    new SqlParameter("@Enableweixinrequest", SqlDbType.Char,1),

                    new SqlParameter("@WeixinCertPassword", SqlDbType.NVarChar,100),
                    new SqlParameter("@Alipay_mid", SqlDbType.NVarChar,100),
                    new SqlParameter("@Alipay_mName", SqlDbType.NVarChar,100),
                    new SqlParameter("@Alipay_Pid", SqlDbType.NVarChar,100),
                    new SqlParameter("@Alipay_Key", SqlDbType.NVarChar,100),
                    new SqlParameter("@OffLinePayContent", SqlDbType.NVarChar,1000),
                    new SqlParameter("@EnableWeixinRed", SqlDbType.Char,1),
                    new SqlParameter("@EnableAlipayRequest", SqlDbType.Char,1),
                    new SqlParameter("@EnablePodRequest", SqlDbType.Char,1),
                    new SqlParameter("@EnableOffLineRequest", SqlDbType.Char,1),

                    new SqlParameter("@WeixinCertPath", SqlDbType.NVarChar,100),
                    new SqlParameter("@OpenManyService", SqlDbType.NVarChar,1),
                    new SqlParameter("@GuidePageSet", SqlDbType.NVarChar,50),
                    new SqlParameter("@EnableGuidePageSet", SqlDbType.Char,1),
                    new SqlParameter("@ManageOpenID", SqlDbType.NVarChar,100),
                    new SqlParameter("@EnableShopMenu", SqlDbType.Bit,1),

                    new SqlParameter("@EnableSaleService", SqlDbType.Bit,1),
                    new SqlParameter("@entId", SqlDbType.NVarChar,50)
                    //new SqlParameter("@IsValidationService", SqlDbType.Char,1)

            };

            parameters[0].Value = model.businessNum;
            parameters[1].Value = model.mid;
            parameters[2].Value = model.templatesNum;
			parameters[3].Value = model.appid_name;
			parameters[4].Value = model.appid_origin_id;
			parameters[5].Value = model.weixin_account;
			parameters[6].Value = model.avatar;
			parameters[7].Value = model.interface_url;
			parameters[8].Value = model.token_value;
			parameters[9].Value = model.encodingaeskey;
			parameters[10].Value = model.appid;
			parameters[11].Value = model.appsecret;
			parameters[12].Value = model.create_user;
			parameters[13].Value = model.create_time;
			parameters[14].Value = model.payment_name;
			parameters[15].Value = model.state;
			parameters[16].Value = model.weixin_pay_account;
			parameters[17].Value = model.account_pay_key;
			parameters[18].Value = model.send_type;
			parameters[19].Value = model.logo;
			parameters[20].Value = model.description;
            parameters[21].Value = model.wid;
            parameters[22].Value = model.sitename;
            parameters[23].Value = model.tel;
            parameters[24].Value = model.Enableweixinrequest;

            parameters[25].Value = model.WeixinCertPassword;
            parameters[26].Value = model.Alipay_mid;
            parameters[27].Value = model.Alipay_mName;
            parameters[28].Value = model.Alipay_Pid;
            parameters[29].Value = model.Alipay_Key;
            parameters[30].Value = model.OffLinePayContent;
            parameters[31].Value = model.EnableWeixinRed;
            parameters[32].Value = model.EnableAlipayRequest;
            parameters[33].Value = model.EnablePodRequest;
            parameters[34].Value = model.EnableOffLineRequest;

            parameters[35].Value = model.WeixinCertPath;
            parameters[36].Value = model.OpenManyService;
            parameters[37].Value = model.GuidePageSet;
            parameters[38].Value = model.EnableGuidePageSet;
            parameters[39].Value = model.ManageOpenID;
            parameters[40].Value = model.EnableShopMenu;

            parameters[41].Value = model.EnableSaleService;
            parameters[42].Value = model.entId;
            //parameters[40].Value = model.IsValidationService;


            object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt64(obj);
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(SF.Model.sf_website model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update sf_website set ");
            strSql.Append("businessNum=@businessNum,");
            strSql.Append("mid=@mid,");
            strSql.Append("templatesNum=@templatesNum,");
			strSql.Append("appid_name=@appid_name,");
			strSql.Append("appid_origin_id=@appid_origin_id,");
			strSql.Append("weixin_account=@weixin_account,");
			strSql.Append("avatar=@avatar,");
			strSql.Append("interface_url=@interface_url,");
			strSql.Append("token_value=@token_value,");
			strSql.Append("encodingaeskey=@encodingaeskey,");
			strSql.Append("appid=@appid,");
			strSql.Append("appsecret=@appsecret,");
			strSql.Append("create_user=@create_user,");
			strSql.Append("create_time=@create_time,");
			strSql.Append("payment_name=@payment_name,");
			strSql.Append("state=@state,");
			strSql.Append("weixin_pay_account=@weixin_pay_account,");
			strSql.Append("account_pay_key=@account_pay_key,");
			strSql.Append("send_type=@send_type,");
			strSql.Append("logo=@logo,");
			strSql.Append("description=@description, ");
            strSql.Append("wid=@wid, ");
            strSql.Append("sitename=@sitename, ");
            strSql.Append("tel=@tel, ");
            strSql.Append("Enableweixinrequest=@Enableweixinrequest,");
            strSql.Append("WeixinCertPassword=@WeixinCertPassword,");
            strSql.Append("Alipay_mid=@Alipay_mid,");
            strSql.Append("Alipay_mName=@Alipay_mName,");
            strSql.Append("Alipay_Pid=@Alipay_Pid,");
            strSql.Append("Alipay_Key=@Alipay_Key,");
            strSql.Append("OffLinePayContent=@OffLinePayContent,");
            strSql.Append("EnableWeixinRed=@EnableWeixinRed,");
            strSql.Append("EnableAlipayRequest=@EnableAlipayRequest,");
            strSql.Append("EnablePodRequest=@EnablePodRequest,");
            strSql.Append("EnableOffLineRequest=@EnableOffLineRequest, ");

            strSql.Append("WeixinCertPath=@WeixinCertPath,");
            strSql.Append("OpenManyService=@OpenManyService,");
            strSql.Append("GuidePageSet=@GuidePageSet,");
            strSql.Append("EnableGuidePageSet=@EnableGuidePageSet,");
            strSql.Append("ManageOpenID=@ManageOpenID,");
            strSql.Append("IsValidationService=@IsValidationService,");
            strSql.Append("EnableShopMenu=@EnableShopMenu,");

            strSql.Append("EnableSaleService=@EnableSaleService,");
            strSql.Append("entId=@entId ");
            strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@businessNum", SqlDbType.NVarChar,50),
                    new SqlParameter("@mid", SqlDbType.Int, 4),
                    new SqlParameter("@templatesNum", SqlDbType.NVarChar,50),
					new SqlParameter("@appid_name", SqlDbType.NVarChar,100),
					new SqlParameter("@appid_origin_id", SqlDbType.NVarChar,100),
					new SqlParameter("@weixin_account", SqlDbType.NVarChar,100),
					new SqlParameter("@avatar", SqlDbType.NVarChar,100),
					new SqlParameter("@interface_url", SqlDbType.NVarChar,200),
					new SqlParameter("@token_value", SqlDbType.NVarChar,100),
					new SqlParameter("@encodingaeskey", SqlDbType.NVarChar,100),
					new SqlParameter("@appid", SqlDbType.NVarChar,100),
					new SqlParameter("@appsecret", SqlDbType.NVarChar,100),
					new SqlParameter("@create_user", SqlDbType.NVarChar,50),
					new SqlParameter("@create_time", SqlDbType.NVarChar,50),
					new SqlParameter("@payment_name", SqlDbType.NVarChar,100),
					new SqlParameter("@state", SqlDbType.Int,4),
					new SqlParameter("@weixin_pay_account", SqlDbType.NVarChar,100),
					new SqlParameter("@account_pay_key", SqlDbType.NVarChar,100),
					new SqlParameter("@send_type", SqlDbType.Int,4),
					new SqlParameter("@logo", SqlDbType.NVarChar,200),
					new SqlParameter("@description", SqlDbType.NVarChar,1000),
                    new SqlParameter("@wid", SqlDbType.NVarChar,50),
                    new SqlParameter("@sitename", SqlDbType.NVarChar,100),
                    new SqlParameter("@tel", SqlDbType.NVarChar,50),
                    new SqlParameter("@Enableweixinrequest", SqlDbType.Char,1),
                    new SqlParameter("@WeixinCertPassword", SqlDbType.NVarChar,100),
                    new SqlParameter("@Alipay_mid", SqlDbType.NVarChar,100),
                    new SqlParameter("@Alipay_mName", SqlDbType.NVarChar,100),
                    new SqlParameter("@Alipay_Pid", SqlDbType.NVarChar,100),
                    new SqlParameter("@Alipay_Key", SqlDbType.NVarChar,100),
                    new SqlParameter("@OffLinePayContent", SqlDbType.NVarChar,1000),
                    new SqlParameter("@EnableWeixinRed", SqlDbType.Char,1),
                    new SqlParameter("@EnableAlipayRequest", SqlDbType.Char,1),
                    new SqlParameter("@EnablePodRequest", SqlDbType.Char,1),
                    new SqlParameter("@EnableOffLineRequest", SqlDbType.Char,1),

                    new SqlParameter("@WeixinCertPath", SqlDbType.NVarChar,100),
                    new SqlParameter("@OpenManyService", SqlDbType.NVarChar,1),
                    new SqlParameter("@GuidePageSet", SqlDbType.NVarChar,50),
                    new SqlParameter("@EnableGuidePageSet", SqlDbType.Char,1),
                    new SqlParameter("@ManageOpenID", SqlDbType.NVarChar,100),
                    new SqlParameter("@IsValidationService", SqlDbType.Char,1),
                    new SqlParameter("@EnableShopMenu", SqlDbType.Bit,1),
                    new SqlParameter("@EnableSaleService", SqlDbType.Bit,1),
                    new SqlParameter("@entId", SqlDbType.NVarChar,50),

                    new SqlParameter("@id", SqlDbType.BigInt,8)};
            parameters[0].Value = model.businessNum;
            parameters[1].Value = model.mid;
			parameters[2].Value = model.templatesNum;
			parameters[3].Value = model.appid_name;
			parameters[4].Value = model.appid_origin_id;
			parameters[5].Value = model.weixin_account;
			parameters[6].Value = model.avatar;
			parameters[7].Value = model.interface_url;
			parameters[8].Value = model.token_value;
			parameters[9].Value = model.encodingaeskey;
			parameters[10].Value = model.appid;
			parameters[11].Value = model.appsecret;
			parameters[12].Value = model.create_user;
			parameters[13].Value = model.create_time;
			parameters[14].Value = model.payment_name;
			parameters[15].Value = model.state;
			parameters[16].Value = model.weixin_pay_account;
			parameters[17].Value = model.account_pay_key;
			parameters[18].Value = model.send_type;
			parameters[19].Value = model.logo;
			parameters[20].Value = model.description;
            parameters[21].Value = model.wid;
            parameters[22].Value = model.sitename;
            parameters[23].Value = model.tel;
            parameters[24].Value = model.Enableweixinrequest;

            parameters[25].Value = model.WeixinCertPassword;
            parameters[26].Value = model.Alipay_mid;
            parameters[27].Value = model.Alipay_mName;
            parameters[28].Value = model.Alipay_Pid;
            parameters[29].Value = model.Alipay_Key;
            parameters[30].Value = model.OffLinePayContent;
            parameters[31].Value = model.EnableWeixinRed;
            parameters[32].Value = model.EnableAlipayRequest;
            parameters[33].Value = model.EnablePodRequest;
            parameters[34].Value = model.EnableOffLineRequest;
            parameters[35].Value = model.WeixinCertPath;
            parameters[36].Value = model.OpenManyService;
            parameters[37].Value = model.GuidePageSet;
            parameters[38].Value = model.EnableGuidePageSet;
            parameters[39].Value = model.ManageOpenID;
            parameters[40].Value = model.IsValidationService;
            parameters[41].Value = model.EnableShopMenu;
            parameters[42].Value = model.EnableSaleService;
            parameters[43].Value = model.entId;

            parameters[44].Value = model.id;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">修改的模型</param>
        /// <param name="strTables">需要调整AppId的相关表</param>
        /// <returns></returns>
        public bool Update(SF.Model.sf_website model,string [] strTables)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update sf_website set ");
                        strSql.Append("businessNum=@businessNum,");
                        strSql.Append("mid=@mid,");
                        strSql.Append("templatesNum=@templatesNum,");
                        strSql.Append("appid_name=@appid_name,");
                        strSql.Append("appid_origin_id=@appid_origin_id,");
                        strSql.Append("weixin_account=@weixin_account,");
                        strSql.Append("avatar=@avatar,");
                        strSql.Append("interface_url=@interface_url,");
                        strSql.Append("token_value=@token_value,");
                        strSql.Append("encodingaeskey=@encodingaeskey,");
                        strSql.Append("appid=@appid,");
                        strSql.Append("appsecret=@appsecret,");
                        strSql.Append("create_user=@create_user,");
                        strSql.Append("create_time=@create_time,");
                        strSql.Append("payment_name=@payment_name,");
                        strSql.Append("state=@state,");
                        strSql.Append("weixin_pay_account=@weixin_pay_account,");
                        strSql.Append("account_pay_key=@account_pay_key,");
                        strSql.Append("send_type=@send_type,");
                        strSql.Append("logo=@logo,");
                        strSql.Append("description=@description,");
                        strSql.Append("wid=@wid, ");
                        strSql.Append("sitename=@sitename, ");
                        strSql.Append("tel=@tel, ");
                        strSql.Append("Enableweixinrequest=@Enableweixinrequest,");
                        strSql.Append("WeixinCertPassword=@WeixinCertPassword,");
                        strSql.Append("Alipay_mid=@Alipay_mid,");
                        strSql.Append("Alipay_mName=@Alipay_mName,");
                        strSql.Append("Alipay_Pid=@Alipay_Pid,");
                        strSql.Append("Alipay_Key=@Alipay_Key,");
                        strSql.Append("OffLinePayContent=@OffLinePayContent,");
                        strSql.Append("EnableWeixinRed=@EnableWeixinRed,");
                        strSql.Append("EnableAlipayRequest=@EnableAlipayRequest,");
                        strSql.Append("EnablePodRequest=@EnablePodRequest,");
                        strSql.Append("EnableOffLineRequest=@EnableOffLineRequest,");
                        strSql.Append("WeixinCertPath=@WeixinCertPath,");
                        strSql.Append("OpenManyService=@OpenManyService,");
                        strSql.Append("GuidePageSet=@GuidePageSet,");
                        strSql.Append("EnableGuidePageSet=@EnableGuidePageSet,");
                        strSql.Append("ManageOpenID=@ManageOpenID,");
                        strSql.Append("IsValidationService=@IsValidationService,");
                        strSql.Append("EnableShopMenu=@EnableShopMenu, ");
                        strSql.Append("EnableSaleService=@EnableSaleService,");
                        strSql.Append("entId=@entId ");
                        strSql.Append(" where id=@id");
                    SqlParameter[] parameters = {
                    new SqlParameter("@businessNum", SqlDbType.NVarChar,50),
                    new SqlParameter("@mid", SqlDbType.Int,4),
                    new SqlParameter("@templatesNum", SqlDbType.NVarChar,50),
					new SqlParameter("@appid_name", SqlDbType.NVarChar,100),
					new SqlParameter("@appid_origin_id", SqlDbType.NVarChar,100),
					new SqlParameter("@weixin_account", SqlDbType.NVarChar,100),
					new SqlParameter("@avatar", SqlDbType.NVarChar,100),
					new SqlParameter("@interface_url", SqlDbType.NVarChar,200),
					new SqlParameter("@token_value", SqlDbType.NVarChar,100),
					new SqlParameter("@encodingaeskey", SqlDbType.NVarChar,100),
					new SqlParameter("@appid", SqlDbType.NVarChar,100),
					new SqlParameter("@appsecret", SqlDbType.NVarChar,100),
					new SqlParameter("@create_user", SqlDbType.NVarChar,50),
					new SqlParameter("@create_time", SqlDbType.NVarChar,50),
					new SqlParameter("@payment_name", SqlDbType.NVarChar,100),
					new SqlParameter("@state", SqlDbType.Int,4),
					new SqlParameter("@weixin_pay_account", SqlDbType.NVarChar,100),
					new SqlParameter("@account_pay_key", SqlDbType.NVarChar,100),
					new SqlParameter("@send_type", SqlDbType.Int,4),
					new SqlParameter("@logo", SqlDbType.NVarChar,200),
					new SqlParameter("@description", SqlDbType.NVarChar,1000),
                    new SqlParameter("@wid", SqlDbType.NVarChar,50),
                    new SqlParameter("@sitename", SqlDbType.NVarChar,200),
                    new SqlParameter("@tel", SqlDbType.NVarChar,50),
                    new SqlParameter("@Enableweixinrequest", SqlDbType.Char,1),
                    new SqlParameter("@WeixinCertPassword", SqlDbType.NVarChar,100),
                    new SqlParameter("@Alipay_mid", SqlDbType.NVarChar,100),
                    new SqlParameter("@Alipay_mName", SqlDbType.NVarChar,100),
                    new SqlParameter("@Alipay_Pid", SqlDbType.NVarChar,100),
                    new SqlParameter("@Alipay_Key", SqlDbType.NVarChar,100),
                    new SqlParameter("@OffLinePayContent", SqlDbType.NVarChar,1000),
                    new SqlParameter("@EnableWeixinRed", SqlDbType.Char,1),
                    new SqlParameter("@EnableAlipayRequest", SqlDbType.Char,1),
                    new SqlParameter("@EnablePodRequest", SqlDbType.Char,1),
                    new SqlParameter("@EnableOffLineRequest", SqlDbType.Char,1),
                    new SqlParameter("@WeixinCertPath", SqlDbType.NVarChar,100),
                    new SqlParameter("@OpenManyService", SqlDbType.NVarChar,1),
                    new SqlParameter("@GuidePageSet", SqlDbType.NVarChar,50),
                    new SqlParameter("@EnableGuidePageSet", SqlDbType.Char,1),
                    new SqlParameter("@ManageOpenID", SqlDbType.NVarChar,100),
                    new SqlParameter("@IsValidationService", SqlDbType.Char,1),
                    new SqlParameter("@EnableShopMenu", SqlDbType.Bit,1),
                    new SqlParameter("@EnableSaleService", SqlDbType.Bit,1),
                    new SqlParameter("@entId", SqlDbType.NVarChar,50),
                    new SqlParameter("@id", SqlDbType.BigInt,8)};
                        parameters[0].Value = model.businessNum;
                        parameters[1].Value = model.mid;
                        parameters[2].Value = model.templatesNum;
                        parameters[3].Value = model.appid_name;
                        parameters[4].Value = model.appid_origin_id;
                        parameters[5].Value = model.weixin_account;
                        parameters[6].Value = model.avatar;
                        parameters[7].Value = model.interface_url;
                        parameters[8].Value = model.token_value;
                        parameters[9].Value = model.encodingaeskey;
                        parameters[10].Value = model.appid;
                        parameters[11].Value = model.appsecret;
                        parameters[12].Value = model.create_user;
                        parameters[13].Value = model.create_time;
                        parameters[14].Value = model.payment_name;
                        parameters[15].Value = model.state;
                        parameters[16].Value = model.weixin_pay_account;
                        parameters[17].Value = model.account_pay_key;
                        parameters[18].Value = model.send_type;
                        parameters[19].Value = model.logo;
                        parameters[20].Value = model.description;
                        parameters[21].Value = model.wid;
                        parameters[22].Value = model.sitename;
                        parameters[23].Value = model.tel;
                        parameters[24].Value = model.Enableweixinrequest;

                        parameters[25].Value = model.WeixinCertPassword;
                        parameters[26].Value = model.Alipay_mid;
                        parameters[27].Value = model.Alipay_mName;
                        parameters[28].Value = model.Alipay_Pid;
                        parameters[29].Value = model.Alipay_Key;
                        parameters[30].Value = model.OffLinePayContent;
                        parameters[31].Value = model.EnableWeixinRed;
                        parameters[32].Value = model.EnableAlipayRequest;
                        parameters[33].Value = model.EnablePodRequest;
                        parameters[34].Value = model.EnableOffLineRequest;

                        parameters[35].Value = model.WeixinCertPath;
                        parameters[36].Value = model.OpenManyService;
                        parameters[37].Value = model.GuidePageSet;
                        parameters[38].Value = model.EnableGuidePageSet;
                        parameters[39].Value = model.ManageOpenID;
                        parameters[40].Value = model.IsValidationService;
                        parameters[41].Value = model.EnableShopMenu;
                        parameters[42].Value = model.EnableSaleService;
                        parameters[43].Value = model.entId;
                        parameters[42].Value = model.id;

                        int rows = DbHelperSQL.ExecuteSql(conn,trans,strSql.ToString(), parameters);
                        if (rows > 0 && strTables != null && strTables.Length > 0) 
                        {
                            foreach (string table in strTables) 
                            {
                                string tableSQL = string.Format(" update {0} set appNum = '{1}' where {2}", table, model.appid, SFUtils.getWhereByInfo(model.businessNum, model.appid, model.templatesNum));
                                DbHelperSQL.ExecuteSql(conn, trans, tableSQL);
                            }
                        }

                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }

            return true;
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(long id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from sf_website ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.BigInt)
			};
			parameters[0].Value = id;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from sf_website ");
			strSql.Append(" where id in ("+idlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public SF.Model.sf_website GetModel(long id)
		{			
			StringBuilder strSql=new StringBuilder();
            //strSql.Append("select  top 1 id,businessNum,mid,templatesNum,appid_name,appid_origin_id,weixin_account,avatar,interface_url,token_value,encodingaeskey,appid,appsecret,create_user,create_time,payment_name,state,weixin_pay_account,account_pay_key,send_type,logo,description,wid,sitename,tel,Enableweixinrequest,WeixinCertPassword,Alipay_mid,Alipay_mName,Alipay_Pid ,Alipay_Key,OffLinePayContent,EnableWeixinRed,EnableAlipayRequest,EnablePodRequest,EnableOffLineRequest from sf_website ");
            strSql.Append("select  top 1 * from sf_website ");
            strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.BigInt)
			};
			parameters[0].Value = id;

			SF.Model.sf_website model=new SF.Model.sf_website();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SF.Model.sf_website GetModelByMid(long id)
        {

            StringBuilder strSql = new StringBuilder();
            //strSql.Append("select  top 1 id,businessNum,mid,templatesNum,appid_name,appid_origin_id,weixin_account,avatar,interface_url,token_value,encodingaeskey,appid,appsecret,create_user,create_time,payment_name,state,weixin_pay_account,account_pay_key,send_type,logo,description,wid,sitename,tel,Enableweixinrequest,WeixinCertPassword,Alipay_mid,Alipay_mName,Alipay_Pid ,Alipay_Key,OffLinePayContent,EnableWeixinRed,EnableAlipayRequest,EnablePodRequest,EnableOffLineRequest from sf_website ");
            strSql.Append("select  top 1 * from sf_website ");
            strSql.Append(" where mid=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.BigInt)
            };
            parameters[0].Value = id;

            SF.Model.sf_website model = new SF.Model.sf_website();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public SF.Model.sf_website GetModelByNum(string appid_origin_id)
        {

            StringBuilder strSql = new StringBuilder();
            //strSql.Append("select  top 1 id,businessNum,mid,templatesNum,appid_name,appid_origin_id,weixin_account,avatar,interface_url,token_value,encodingaeskey,appid,appsecret,create_user,create_time,payment_name,state,weixin_pay_account,account_pay_key,send_type,logo,description,wid,sitename,tel,Enableweixinrequest,WeixinCertPassword,Alipay_mid,Alipay_mName,Alipay_Pid ,Alipay_Key,OffLinePayContent,EnableWeixinRed,EnableAlipayRequest,EnablePodRequest,EnableOffLineRequest from sf_website ");
            strSql.Append("select  top 1 * from sf_website ");
            strSql.Append(" where appid_origin_id=@appid_origin_id");
            SqlParameter[] parameters = {
					new SqlParameter("@appid_origin_id", SqlDbType.NVarChar)
			};
            parameters[0].Value = appid_origin_id;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="wid"></param>
        /// <returns></returns>
        public SF.Model.sf_website GetModelByWid(string wid)
        {

            StringBuilder strSql = new StringBuilder();
            //strSql.Append("select  top 1 id,businessNum,mid,templatesNum,appid_name,appid_origin_id,weixin_account,avatar,interface_url,token_value,encodingaeskey,appid,appsecret,create_user,create_time,payment_name,state,weixin_pay_account,account_pay_key,send_type,logo,description,wid,sitename,tel,Enableweixinrequest,WeixinCertPassword,Alipay_mid,Alipay_mName,Alipay_Pid ,Alipay_Key,OffLinePayContent,EnableWeixinRed,EnableAlipayRequest,EnablePodRequest,EnableOffLineRequest from sf_website ");
            strSql.Append("select  top 1 * from sf_website ");
            strSql.Append(" where wid=@wid");
            SqlParameter[] parameters = {
                    new SqlParameter("@wid", SqlDbType.NVarChar)
            };
            parameters[0].Value = wid;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public SF.Model.sf_website DataRowToModel(DataRow row)
		{
			SF.Model.sf_website model=new SF.Model.sf_website();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=long.Parse(row["id"].ToString());
				}
				if(row["businessNum"]!=null)
				{
					model.businessNum=row["businessNum"].ToString();
				}
                if (row["mid"] != null)
                {
                    model.mid = long.Parse(row["mid"].ToString());
                }
                if (row["templatesNum"]!=null)
				{
					model.templatesNum=row["templatesNum"].ToString();
				}
				if(row["appid_name"]!=null)
				{
					model.appid_name=row["appid_name"].ToString();
				}
				if(row["appid_origin_id"]!=null)
				{
					model.appid_origin_id=row["appid_origin_id"].ToString();
				}
				if(row["weixin_account"]!=null)
				{
					model.weixin_account=row["weixin_account"].ToString();
				}
				if(row["avatar"]!=null)
				{
					model.avatar=row["avatar"].ToString();
				}
				if(row["interface_url"]!=null)
				{
					model.interface_url=row["interface_url"].ToString();
				}
				if(row["token_value"]!=null)
				{
					model.token_value=row["token_value"].ToString();
				}
				if(row["encodingaeskey"]!=null)
				{
					model.encodingaeskey=row["encodingaeskey"].ToString();
				}
				if(row["appid"]!=null)
				{
					model.appid=row["appid"].ToString();
				}
				if(row["appsecret"]!=null)
				{
					model.appsecret=row["appsecret"].ToString();
				}
				if(row["create_user"]!=null)
				{
					model.create_user=row["create_user"].ToString();
				}
				if(row["create_time"]!=null)
				{
					model.create_time=row["create_time"].ToString();
				}
				if(row["payment_name"]!=null)
				{
					model.payment_name=row["payment_name"].ToString();
				}
				if(row["state"]!=null && row["state"].ToString()!="")
				{
					model.state=int.Parse(row["state"].ToString());
				}
				if(row["weixin_pay_account"]!=null)
				{
					model.weixin_pay_account=row["weixin_pay_account"].ToString();
				}
				if(row["account_pay_key"]!=null)
				{
					model.account_pay_key=row["account_pay_key"].ToString();
				}
				if(row["send_type"]!=null && row["send_type"].ToString()!="")
				{
					model.send_type=int.Parse(row["send_type"].ToString());
				}
				if(row["logo"]!=null)
				{
					model.logo=row["logo"].ToString();
				}
				if(row["description"]!=null)
				{
					model.description=row["description"].ToString();
				}
                if (row["wid"] != null)
                {
                    model.wid = row["wid"].ToString();
                }                
                if (row["sitename"] != null)
                {
                    model.sitename = row["sitename"].ToString();
                }
                if (row["tel"] != null)
                {
                    model.tel = row["tel"].ToString();
                }
                if (row["Enableweixinrequest"] != null)
                {
                    model.Enableweixinrequest = row["Enableweixinrequest"].ToString();
                }

                if (row["WeixinCertPassword"] != null)
                {
                    model.WeixinCertPassword = row["WeixinCertPassword"].ToString();
                }
                if (row["Alipay_mid"] != null)
                {
                    model.Alipay_mid = row["Alipay_mid"].ToString();
                }
                if (row["Alipay_mName"] != null)
                {
                    model.Alipay_mName = row["Alipay_mName"].ToString();
                }
                if (row["Alipay_Pid"] != null)
                {
                    model.Alipay_Pid = row["Alipay_Pid"].ToString();
                }
                if (row["Alipay_Key"] != null)
                {
                    model.Alipay_Key = row["Alipay_Key"].ToString();
                }
                if (row["OffLinePayContent"] != null)
                {
                    model.OffLinePayContent = row["OffLinePayContent"].ToString();
                }
                if (row["EnableWeixinRed"] != null)
                {
                    model.EnableWeixinRed = row["EnableWeixinRed"].ToString();
                }
                if (row["EnableAlipayRequest"] != null)
                {
                    model.EnableAlipayRequest = row["EnableAlipayRequest"].ToString();
                }
                if (row["EnablePodRequest"] != null)
                {
                    model.EnablePodRequest = row["EnablePodRequest"].ToString();
                }
                if (row["EnableOffLineRequest"] != null)
                {
                    model.EnableOffLineRequest = row["EnableOffLineRequest"].ToString();

                }
                if (row["WeixinCertPath"] != null)
                {
                    model.WeixinCertPath = row["WeixinCertPath"].ToString();

                }
                if (row["OpenManyService"] != null)
                {
                    model.OpenManyService = row["OpenManyService"].ToString();

                }
                if (row["GuidePageSet"] != null)
                {
                    model.GuidePageSet = row["GuidePageSet"].ToString();

                }
                if (row["EnableGuidePageSet"] != null)
                {
                    model.EnableGuidePageSet = row["EnableGuidePageSet"].ToString();

                }
                if (row["ManageOpenID"] != null)
                {
                    model.ManageOpenID = row["ManageOpenID"].ToString();

                }
                if (row["IsValidationService"] != null)
                {
                    model.IsValidationService = row["IsValidationService"].ToString();

                }
                if (row["EnableShopMenu"] != null && row["EnableShopMenu"].ToString() != "")
                {
                    model.EnableShopMenu = Boolean.Parse(row["EnableShopMenu"].ToString());
                }
                if (row["EnableSaleService"] != null && row["EnableSaleService"].ToString() != "")
                {
                    model.EnableSaleService = Boolean.Parse(row["EnableSaleService"].ToString());
                }
                if (row["entId"] != null)
                {
                    model.entId = row["entId"].ToString();
                }
            }
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
            //strSql.Append("select id,businessNum,mid,templatesNum,appid_name,appid_origin_id,weixin_account,avatar,interface_url,token_value,encodingaeskey,appid,appsecret,create_user,create_time,payment_name,state,weixin_pay_account,account_pay_key,send_type,logo,description,wid,sitename,tel,Enableweixinrequest,WeixinCertPassword,Alipay_mid,Alipay_mName,Alipay_Pid ,Alipay_Key,OffLinePayContent,EnableWeixinRed,EnableAlipayRequest,EnablePodRequest,EnableOffLineRequest ");
            strSql.Append("select * ");
            strSql.Append(" FROM sf_website ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" id,businessNum,mid,templatesNum,appid_name,appid_origin_id,weixin_account,avatar,interface_url,token_value,encodingaeskey,appid,appsecret,create_user,create_time,payment_name,state,weixin_pay_account,account_pay_key,send_type,logo,description,wid,sitename,tel,Enableweixinrequest,WeixinCertPassword,Alipay_mid,Alipay_mName,Alipay_Pid ,Alipay_Key,OffLinePayContent,EnableWeixinRed,EnableAlipayRequest,EnablePodRequest,EnableOffLineRequest ");
			strSql.Append(" FROM sf_website ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM sf_website ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperSQL.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.id desc");
			}
			strSql.Append(")AS Row, T.*  from sf_website T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQL.Query(strSql.ToString());
		}

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from sf_website");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public SF.Model.sf_website GetModel(string appid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,businessNum,mid,templatesNum,appid_name,appid_origin_id,weixin_account,avatar,interface_url,token_value,encodingaeskey,appid,appsecret,create_user,create_time,payment_name,state,weixin_pay_account,account_pay_key,send_type,logo,description,wid,sitename,tel,Enableweixinrequest,WeixinCertPassword,Alipay_mid,Alipay_mName,Alipay_Pid ,Alipay_Key,OffLinePayContent,EnableWeixinRed,EnableAlipayRequest,EnablePodRequest,EnableOffLineRequest from sf_website ");
            strSql.Append(" where appid=@appid");
            SqlParameter[] parameters = {
					new SqlParameter("@appid", SqlDbType.NVarChar,100)
			};
            parameters[0].Value = appid;

            SF.Model.sf_website model = new SF.Model.sf_website();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public SF.Model.sf_website GetModelbyWhere(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,businessNum,mid,templatesNum,appid_name,appid_origin_id,weixin_account,avatar,interface_url,token_value,encodingaeskey,appid,appsecret,create_user,create_time,payment_name,state,weixin_pay_account,account_pay_key,send_type,logo,description,wid,sitename,tel,Enableweixinrequest,WeixinCertPassword,Alipay_mid,Alipay_mName,Alipay_Pid ,Alipay_Key,OffLinePayContent,EnableWeixinRed,EnableAlipayRequest,EnablePodRequest,EnableOffLineRequest from sf_website ");
            strSql.Append(" where "+where);

            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取公众服务号名称以及其对应的用户数量
        /// </summary>
        /// <param name="businessNum"></param>
        /// <returns></returns>
        public System.Data.DataSet GetAppNumListCount(int mid/*string businessNum*/)
        {
            /*
            string sql = string.Format(@"SELECT app.[id]
                                  ,app.[appid_name]
                                  ,case when u.count > 0 then u.count else 0 end count
                              FROM [sf_website] app 
                              left join (SELECT COUNT([id]) count
                                  ,[appNum]
                              FROM [aspnet_Managers]
                              where userid = '{0}'
                              group by appNum) u 
                              on app.appid = u.appNum
                              where app.businessNum = '{0}'                              
                              order by app.id", businessNum);
            */
            string sql = string.Format(@"select id,appid_name,1 from sf_website where mid={0}", mid);
            return DbHelperSQL.Query(sql);
            
        }

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "sf_website";
			parameters[1].Value = "id";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

