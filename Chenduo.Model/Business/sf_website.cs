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
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace Chenduo.Model
{
	/// <summary>
	/// sf_website:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class sf_website
	{
		public sf_website()
		{}
		#region Model
		private long _id;
        private long _mid;
        private string _wid;
        private string _businessnum;
		private string _templatesnum;
		private string _appid_name;
		private string _appid_origin_id;
		private string _weixin_account;
		private string _avatar;
		private string _interface_url;
		private string _token_value;
		private string _encodingaeskey;
		private string _appid;
		private string _appsecret;
		private string _create_user;
		private string _create_time;
		private string _payment_name;
		private int _state;
		private string _weixin_pay_account;
		private string _account_pay_key;
		private int _send_type;
		private string _logo;
		private string _description;
        private string _sitename;
        private string _tel;
        private string _enableweixinrequest;

        private string _WeixinCertPassword;
        private string _Alipay_mid;
        private string _Alipay_mName;
        private string _Alipay_Pid;
        private string _Alipay_Key;
        private string _OffLinePayContent;
        private string _EnableWeixinRed;
        private string _EnableAlipayRequest;
        private string _EnablePodRequest;
        private string _EnableOffLineRequest;

        //待加入
        private string _WeixinCertPath;

        //多客服
        private string _OpenManyService;

        //一键关注
        private string _GuidePageSet;
        private string _EnableGuidePageSet;
        //管理openid
        private string _ManageOpenID;
        //会员登录权限
        private string _IsValidationService;

        //店铺菜单
        private bool _EnableShopMenu;
        
        /// <summary>
        /// 在线客服
        /// </summary>
        private bool _EnableSaleService;

        /// <summary>
        /// 美恰ID
        /// </summary>
        private string _entId;

        /// <summary>
        /// 
        /// </summary>
        public long id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string businessNum
		{
			set{ _businessnum=value;}
			get{return _businessnum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string templatesNum
		{
			set{ _templatesnum=value;}
			get{return _templatesnum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string appid_name
		{
			set{ _appid_name=value;}
			get{return _appid_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string appid_origin_id
		{
			set{ _appid_origin_id=value;}
			get{return _appid_origin_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string weixin_account
		{
			set{ _weixin_account=value;}
			get{return _weixin_account;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string avatar
		{
			set{ _avatar=value;}
			get{return _avatar;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string interface_url
		{
			set{ _interface_url=value;}
			get{return _interface_url;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string token_value
		{
			set{ _token_value=value;}
			get{return _token_value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string encodingaeskey
		{
			set{ _encodingaeskey=value;}
			get{return _encodingaeskey;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string appid
		{
			set{ _appid=value;}
			get{return _appid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string appsecret
		{
			set{ _appsecret=value;}
			get{return _appsecret;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string create_user
		{
			set{ _create_user=value;}
			get{return _create_user;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string create_time
		{
			set{ _create_time=value;}
			get{return _create_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string payment_name
		{
			set{ _payment_name=value;}
			get{return _payment_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int state
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string weixin_pay_account
		{
			set{ _weixin_pay_account=value;}
			get{return _weixin_pay_account;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string account_pay_key
		{
			set{ _account_pay_key=value;}
			get{return _account_pay_key;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int send_type
		{
			set{ _send_type=value;}
			get{return _send_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string logo
		{
			set{ _logo=value;}
			get{return _logo;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string description
		{
			set{ _description=value;}
			get{return _description;}
		}

        public long mid
        {
            get
            {
                return _mid;
            }

            set
            {
                _mid = value;
            }
        }

        public string wid
        {
            get
            {
                return _wid;
            }

            set
            {
                _wid = value;
            }
        }

        public string sitename
        {
            get
            {
                return _sitename;
            }

            set
            {
                _sitename = value;
            }
        }

        public string tel
        {
            get
            {
                return _tel;
            }

            set
            {
                _tel = value;
            }
        }

        public string Enableweixinrequest
        {
            get
            {
                return _enableweixinrequest;
            }

            set
            {
                _enableweixinrequest = value;
            }
        }

        public string WeixinCertPassword
        {
            get
            {
                return _WeixinCertPassword;
            }

            set
            {
                _WeixinCertPassword = value;
            }
        }

        public string Alipay_mid
        {
            get
            {
                return _Alipay_mid;
            }

            set
            {
                _Alipay_mid = value;
            }
        }

        public string Alipay_mName
        {
            get
            {
                return _Alipay_mName;
            }

            set
            {
                _Alipay_mName = value;
            }
        }

        public string Alipay_Pid
        {
            get
            {
                return _Alipay_Pid;
            }

            set
            {
                _Alipay_Pid = value;
            }
        }

        public string Alipay_Key
        {
            get
            {
                return _Alipay_Key;
            }

            set
            {
                _Alipay_Key = value;
            }
        }

        public string OffLinePayContent
        {
            get
            {
                return _OffLinePayContent;
            }

            set
            {
                _OffLinePayContent = value;
            }
        }

        public string EnableWeixinRed
        {
            get
            {
                return _EnableWeixinRed;
            }

            set
            {
                _EnableWeixinRed = value;
            }
        }

        public string EnableAlipayRequest
        {
            get
            {
                return _EnableAlipayRequest;
            }

            set
            {
                _EnableAlipayRequest = value;
            }
        }

        public string EnablePodRequest
        {
            get
            {
                return _EnablePodRequest;
            }

            set
            {
                _EnablePodRequest = value;
            }
        }

        public string EnableOffLineRequest
        {
            get
            {
                return _EnableOffLineRequest;
            }

            set
            {
                _EnableOffLineRequest = value;
            }
        }

        public string WeixinCertPath
        {
            get
            {
                return _WeixinCertPath;
            }

            set
            {
                _WeixinCertPath = value;
            }
        }
        public string OpenManyService
        {
            get
            {
                return _OpenManyService;
            }

            set
            {
                _OpenManyService = value;
            }
        }

        public string GuidePageSet
        {
            get
            {
                return _GuidePageSet;
            }

            set
            {
                _GuidePageSet = value;
            }
        }

        public string EnableGuidePageSet
        {
            get
            {
                return _EnableGuidePageSet;
            }

            set
            {
                _EnableGuidePageSet = value;
            }
        }

        public string ManageOpenID
        {
            get
            {
                return _ManageOpenID;
            }

            set
            {
                _ManageOpenID = value;
            }
        }

        public string IsValidationService
        {
            get
            {
                return _IsValidationService;
            }

            set
            {
                _IsValidationService = value;
            }
        }

        public bool EnableShopMenu
        {
            get
            {
                return _EnableShopMenu;
            }

            set
            {
                _EnableShopMenu = value;
            }
        }

        public string entId
        {
            get
            {
                return _entId;
            }

            set
            {
                _entId = value;
            }
        }

        public bool EnableSaleService
        {
            get
            {
                return _EnableSaleService;
            }

            set
            {
                _EnableSaleService = value;
            }
        }
        #endregion Model

    }
}

