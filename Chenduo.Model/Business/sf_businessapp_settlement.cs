/**  版本信息模板在安装目录下，可自行修改。
* sf_businessapp_settlement.cs
*
* 功 能： N/A
* 类 名： sf_businessapp_settlement
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015-08-26 20:44:35   N/A    初版
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
	/// sf_businessapp_settlement:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class sf_businessapp_settlement
	{
		public sf_businessapp_settlement()
		{}
		#region Model
		private long _id;
		private string _sf_finance_settlement_num;
		private string _busniesenum;
		private string _business_name;
		private string _appnum;
		private string _appid_name;
		private string _templatesnum;
		private string _templates_name;
		private string _sf_contract_num;
		private string _settlement_times;
		private string _user_name;
		private decimal? _total_amount;
		private DateTime? _create_time;
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
		public string busnieseNum
		{
			set{ _busniesenum=value;}
			get{return _busniesenum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string business_name
		{
			set{ _business_name=value;}
			get{return _business_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string appNum
		{
			set{ _appnum=value;}
			get{return _appnum;}
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
		public string templatesNum
		{
			set{ _templatesnum=value;}
			get{return _templatesnum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string templates_name
		{
			set{ _templates_name=value;}
			get{return _templates_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sf_contract_num
		{
			set{ _sf_contract_num=value;}
			get{return _sf_contract_num;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string settlement_times
		{
			set{ _settlement_times=value;}
			get{return _settlement_times;}
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
		public decimal? total_amount
		{
			set{ _total_amount=value;}
			get{return _total_amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? create_time
		{
			set{ _create_time=value;}
			get{return _create_time;}
		}
		#endregion Model

	}
}

