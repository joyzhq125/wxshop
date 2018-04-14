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
	/// 数据访问类:sf_business_settlement_notice
	/// </summary>
	public partial class sf_business_settlement_notice
	{
		public sf_business_settlement_notice()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from sf_business_settlement_notice");
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
		public long Add(Model.sf_business_settlement_notice model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into sf_business_settlement_notice(");
			strSql.Append("sf_finance_settlement_num,businessNum,content,type,user_name,time)");
			strSql.Append(" values (");
			strSql.Append("@sf_finance_settlement_num,@businessNum,@content,@type,@user_name,@time)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@sf_finance_settlement_num", SqlDbType.NVarChar,100),
					new SqlParameter("@businessNum", SqlDbType.NVarChar,50),
					new SqlParameter("@content", SqlDbType.NVarChar,500),
					new SqlParameter("@type", SqlDbType.Int,4),
					new SqlParameter("@user_name", SqlDbType.NVarChar,50),
					new SqlParameter("@time", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.sf_finance_settlement_num;
			parameters[1].Value = model.businessNum;
			parameters[2].Value = model.content;
			parameters[3].Value = model.type;
			parameters[4].Value = model.user_name;
			parameters[5].Value = model.time;

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
		public bool Update(Model.sf_business_settlement_notice model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update sf_business_settlement_notice set ");
			strSql.Append("sf_finance_settlement_num=@sf_finance_settlement_num,");
			strSql.Append("businessNum=@businessNum,");
			strSql.Append("content=@content,");
			strSql.Append("type=@type,");
			strSql.Append("user_name=@user_name,");
			strSql.Append("time=@time");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@sf_finance_settlement_num", SqlDbType.NVarChar,100),
					new SqlParameter("@businessNum", SqlDbType.NVarChar,50),
					new SqlParameter("@content", SqlDbType.NVarChar,500),
					new SqlParameter("@type", SqlDbType.Int,4),
					new SqlParameter("@user_name", SqlDbType.NVarChar,50),
					new SqlParameter("@time", SqlDbType.NVarChar,50),
					new SqlParameter("@id", SqlDbType.BigInt,8)};
			parameters[0].Value = model.sf_finance_settlement_num;
			parameters[1].Value = model.businessNum;
			parameters[2].Value = model.content;
			parameters[3].Value = model.type;
			parameters[4].Value = model.user_name;
			parameters[5].Value = model.time;
			parameters[6].Value = model.id;

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
			strSql.Append("delete from sf_business_settlement_notice ");
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
			strSql.Append("delete from sf_business_settlement_notice ");
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
		public Model.sf_business_settlement_notice GetModel(long id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,sf_finance_settlement_num,businessNum,content,type,user_name,time from sf_business_settlement_notice ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.BigInt)
			};
			parameters[0].Value = id;

			Model.sf_business_settlement_notice model=new Model.sf_business_settlement_notice();
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
		public Model.sf_business_settlement_notice DataRowToModel(DataRow row)
		{
			Model.sf_business_settlement_notice model=new Model.sf_business_settlement_notice();
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
				if(row["businessNum"]!=null)
				{
					model.businessNum=row["businessNum"].ToString();
				}
				if(row["content"]!=null)
				{
					model.content=row["content"].ToString();
				}
				if(row["type"]!=null && row["type"].ToString()!="")
				{
					model.type=int.Parse(row["type"].ToString());
				}
				if(row["user_name"]!=null)
				{
					model.user_name=row["user_name"].ToString();
				}
				if(row["time"]!=null)
				{
					model.time=row["time"].ToString();
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
			strSql.Append("select id,sf_finance_settlement_num,businessNum,content,type,user_name,time ");
			strSql.Append(" FROM sf_business_settlement_notice ");
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
			strSql.Append(" id,sf_finance_settlement_num,businessNum,content,type,user_name,time ");
			strSql.Append(" FROM sf_business_settlement_notice ");
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
			strSql.Append("select count(1) FROM sf_business_settlement_notice ");
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
			strSql.Append(")AS Row, T.*  from sf_business_settlement_notice T ");
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
            strSql.Append("select * from sf_business_settlement_notice");
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
			parameters[0].Value = "sf_business_settlement_notice";
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

