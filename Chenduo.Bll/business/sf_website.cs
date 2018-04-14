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
using System.Collections.Generic;
//using SF.Common;
using Chenduo;
namespace Chenduo.BLL
{
	/// <summary>
	/// sf_website
	/// </summary>
	public partial class sf_website
	{
		private readonly DAL.sf_website dal=new DAL.sf_website();
		public sf_website()
		{}
		#region  BasicMethod
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long id)
		{
			return dal.Exists(id);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(Model.sf_website model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Model.sf_website model)
		{
			return dal.Update(model);
		}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.sf_website model,string [] strTables)
        {
            return dal.Update(model,strTables);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(long id)
		{
			
			return dal.Delete(id);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string idlist )
		{
			return dal.DeleteList(idlist );
		}

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.sf_website GetModel(long id)
        {
            return dal.GetModel(id);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.sf_website GetModelByMid(long id)
        {
            return dal.GetModelByMid(id);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.sf_website GetModelbyWhere(string where)
        {
            return dal.GetModelbyWhere(where);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.sf_website GetModelByNum(string appid_origin_id)
        {
            return dal.GetModelByNum(appid_origin_id);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="wid"></param>
        /// <returns></returns>
        public Model.sf_website GetModelByWid(string wid)
        {
            return dal.GetModelByWid(wid);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Model.sf_website> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Model.sf_website> DataTableToList(DataTable dt)
		{
			List<Model.sf_website> modelList = new List<Model.sf_website>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Model.sf_website model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}


        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            return dal.GetList(pageSize, pageIndex, strWhere, filedOrder, out recordCount);
        }

        /// <summary>
        /// 通过微信公众服务号(appid)获取对象实体
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public Model.sf_website GetModel(string appid)
        {
            return dal.GetModel(appid);
        }


        /// <summary>
        /// 获取公众服务号名称以及其对应的用户数量
        /// </summary>
        /// <param name="businessNum"></param>
        /// <returns></returns>
        public System.Data.DataSet GetAppNumListCount(int mid/*string businessNum*/)
        {
            return dal.GetAppNumListCount(mid/*businessNum*/);
        }

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

