/**  版本信息模板在安装目录下，可自行修改。
* sf_business_settlement_notice.cs
*
* 功 能： N/A
* 类 名： sf_business_settlement_notice
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015-08-25 22:33:44   N/A    初版
*
* Copyright (c) 2012 SF Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace SF.Model
{
	/// <summary>
	/// sf_business_settlement_notice:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class sf_business_settlement_notice
	{
		public sf_business_settlement_notice()
		{}
		#region Model
		private long _id;
		private string _sf_finance_settlement_num;
		private string _businessnum;
		private string _content;
		private int? _type;
		private string _user_name;
		private string _time;
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
		public string sf_finance_settlement_num
		{
			set{ _sf_finance_settlement_num=value;}
			get{return _sf_finance_settlement_num;}
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
		public string content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string user_name
		{
			set{ _user_name=value;}
			get{return _user_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string time
		{
			set{ _time=value;}
			get{return _time;}
		}
		#endregion Model

	}
}

