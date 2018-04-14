/**  版本信息模板在安装目录下，可自行修改。
* sf_business_settlement.cs
*
* 功 能： N/A
* 类 名： sf_business_settlement
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015-08-26 20:44:34   N/A    初版
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
using Chenduo;
namespace Chenduo.DAL
{
	/// <summary>
	/// 数据访问类:sf_business_settlement
	/// </summary>
	public partial class sf_business_settlement
	{
		public sf_business_settlement()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from sf_business_settlement");
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
		public long Add(Model.sf_business_settlement model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into sf_business_settlement(");
			strSql.Append("sf_finance_settlement_num,busnieseNum,businessName,sf_contract_num,settlement_times,user_name,total_amount,state,pay_num,pay_time,notice_count,create_time)");
			strSql.Append(" values (");
			strSql.Append("@sf_finance_settlement_num,@busnieseNum,@businessName,@sf_contract_num,@settlement_times,@user_name,@total_amount,@state,@pay_num,@pay_time,@notice_count,@create_time)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@sf_finance_settlement_num", SqlDbType.NVarChar,100),
					new SqlParameter("@busnieseNum", SqlDbType.NVarChar,50),
					new SqlParameter("@businessName", SqlDbType.NVarChar,50),
					new SqlParameter("@sf_contract_num", SqlDbType.NVarChar,50),
					new SqlParameter("@settlement_times", SqlDbType.NVarChar,100),
					new SqlParameter("@user_name", SqlDbType.NVarChar,100),
					new SqlParameter("@total_amount", SqlDbType.Decimal,9),
					new SqlParameter("@state", SqlDbType.Int,4),
					new SqlParameter("@pay_num", SqlDbType.NVarChar,100),
					new SqlParameter("@pay_time", SqlDbType.DateTime),
					new SqlParameter("@notice_count", SqlDbType.Int,4),
					new SqlParameter("@create_time", SqlDbType.DateTime)};
			parameters[0].Value = model.sf_finance_settlement_num;
			parameters[1].Value = model.busnieseNum;
			parameters[2].Value = model.businessName;
			parameters[3].Value = model.sf_contract_num;
			parameters[4].Value = model.settlement_times;
			parameters[5].Value = model.user_name;
			parameters[6].Value = model.total_amount;
			parameters[7].Value = model.state;
			parameters[8].Value = model.pay_num;
			parameters[9].Value = model.pay_time;
			parameters[10].Value = model.notice_count;
			parameters[11].Value = model.create_time;

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
		public bool Update(Model.sf_business_settlement model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update sf_business_settlement set ");
			strSql.Append("sf_finance_settlement_num=@sf_finance_settlement_num,");
			strSql.Append("busnieseNum=@busnieseNum,");
			strSql.Append("businessName=@businessName,");
			strSql.Append("sf_contract_num=@sf_contract_num,");
			strSql.Append("settlement_times=@settlement_times,");
			strSql.Append("user_name=@user_name,");
			strSql.Append("total_amount=@total_amount,");
			strSql.Append("state=@state,");
			strSql.Append("pay_num=@pay_num,");
			strSql.Append("pay_time=@pay_time,");
			strSql.Append("notice_count=@notice_count,");
			strSql.Append("create_time=@create_time");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@sf_finance_settlement_num", SqlDbType.NVarChar,100),
					new SqlParameter("@busnieseNum", SqlDbType.NVarChar,50),
					new SqlParameter("@businessName", SqlDbType.NVarChar,50),
					new SqlParameter("@sf_contract_num", SqlDbType.NVarChar,50),
					new SqlParameter("@settlement_times", SqlDbType.NVarChar,100),
					new SqlParameter("@user_name", SqlDbType.NVarChar,100),
					new SqlParameter("@total_amount", SqlDbType.Decimal,9),
					new SqlParameter("@state", SqlDbType.Int,4),
					new SqlParameter("@pay_num", SqlDbType.NVarChar,100),
					new SqlParameter("@pay_time", SqlDbType.DateTime),
					new SqlParameter("@notice_count", SqlDbType.Int,4),
					new SqlParameter("@create_time", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.BigInt,8)};
			parameters[0].Value = model.sf_finance_settlement_num;
			parameters[1].Value = model.busnieseNum;
			parameters[2].Value = model.businessName;
			parameters[3].Value = model.sf_contract_num;
			parameters[4].Value = model.settlement_times;
			parameters[5].Value = model.user_name;
			parameters[6].Value = model.total_amount;
			parameters[7].Value = model.state;
			parameters[8].Value = model.pay_num;
			parameters[9].Value = model.pay_time;
			parameters[10].Value = model.notice_count;
			parameters[11].Value = model.create_time;
			parameters[12].Value = model.id;

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
		/// 删除一条数据
		/// </summary>
		public bool Delete(long id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from sf_business_settlement ");
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
			strSql.Append("delete from sf_business_settlement ");
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
		public Model.sf_business_settlement GetModel(long id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,sf_finance_settlement_num,busnieseNum,businessName,sf_contract_num,settlement_times,user_name,total_amount,state,pay_num,pay_time,notice_count,create_time from sf_business_settlement ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.BigInt)
			};
			parameters[0].Value = id;

			Model.sf_business_settlement model=new Model.sf_business_settlement();
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
		/// 得到一个对象实体
		/// </summary>
		public Model.sf_business_settlement DataRowToModel(DataRow row)
		{
			Model.sf_business_settlement model=new Model.sf_business_settlement();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=long.Parse(row["id"].ToString());
				}
				if(row["sf_finance_settlement_num"]!=null)
				{
					model.sf_finance_settlement_num=row["sf_finance_settlement_num"].ToString();
				}
				if(row["busnieseNum"]!=null)
				{
					model.busnieseNum=row["busnieseNum"].ToString();
				}
				if(row["businessName"]!=null)
				{
					model.businessName=row["businessName"].ToString();
				}
				if(row["sf_contract_num"]!=null)
				{
					model.sf_contract_num=row["sf_contract_num"].ToString();
				}
				if(row["settlement_times"]!=null)
				{
					model.settlement_times=row["settlement_times"].ToString();
				}
				if(row["user_name"]!=null)
				{
					model.user_name=row["user_name"].ToString();
				}
				if(row["total_amount"]!=null && row["total_amount"].ToString()!="")
				{
					model.total_amount=decimal.Parse(row["total_amount"].ToString());
				}
				if(row["state"]!=null && row["state"].ToString()!="")
				{
					model.state=int.Parse(row["state"].ToString());
				}
				if(row["pay_num"]!=null)
				{
					model.pay_num=row["pay_num"].ToString();
				}
				if(row["pay_time"]!=null && row["pay_time"].ToString()!="")
				{
					model.pay_time=DateTime.Parse(row["pay_time"].ToString());
				}
				if(row["notice_count"]!=null && row["notice_count"].ToString()!="")
				{
					model.notice_count=int.Parse(row["notice_count"].ToString());
				}
				if(row["create_time"]!=null && row["create_time"].ToString()!="")
				{
					model.create_time=DateTime.Parse(row["create_time"].ToString());
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
			strSql.Append("select id,sf_finance_settlement_num,busnieseNum,businessName,sf_contract_num,settlement_times,user_name,total_amount,state,pay_num,pay_time,notice_count,create_time ");
			strSql.Append(" FROM sf_business_settlement ");
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
			strSql.Append(" id,sf_finance_settlement_num,busnieseNum,businessName,sf_contract_num,settlement_times,user_name,total_amount,state,pay_num,pay_time,notice_count,create_time ");
			strSql.Append(" FROM sf_business_settlement ");
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
			strSql.Append("select count(1) FROM sf_business_settlement ");
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
			strSql.Append(")AS Row, T.*  from sf_business_settlement T ");
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
            strSql.Append("select * from sf_business_settlement");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
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
			parameters[0].Value = "sf_business_settlement";
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

