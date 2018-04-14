//using SF.Common;
//using SF.DBUtility;
using Hidistro.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Chenduo;
namespace Chenduo.DAL
{
    public class customer
    {
        #region sf_user_info

        /// <summary>
        /// 客户榜单
        /// 获得查询分页数据
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="strWhere">条件</param>
        /// <param name="recordCount">返回数据总条数</param>
        /// <param name="timeWhere">时间条件</param>
        /// <returns></returns>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filterWhere, out int recordCount, string timeWhere)
        {
            string strSql = "select (select sum(totalMoney) from sf_goods_order as b where b.userNum = a.userNum and ispay=2 " + timeWhere + ") as moneys,[id],busnieseNum,appNum,templatesNum,userNum,nickName,province,city,addTime,openID,photoUri,isAttention,sellModel,inviteKey,fatherKey from sf_user_info as a";
            if (strWhere.Trim() != "")
            {
                strSql += " where " + strWhere;
            }
            string temp_SQL = strSql;
            if (filterWhere.Length > 0) 
            {
                strSql = "select * from (" + strSql + ") t Where " + filterWhere;
            }

            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));

            strSql = temp_SQL;
            return DbHelperSQL.Query(CreatePagingSql(recordCount, pageSize, pageIndex, strSql, "(select sum(totalMoney) from sf_goods_order as b where b.userNum = a.userNum and ispay=2 " + timeWhere + ") desc,addtime desc",filterWhere));
        }


        /// <summary>
        /// 获取分页SQL语句，默认row_number为关健字，所有表不允许使用该字段名
        /// </summary>
        /// <param name="_recordCount">记录总数</param>
        /// <param name="_pageSize">每页记录数</param>
        /// <param name="_pageIndex">当前页数</param>
        /// <param name="_safeSql">SQL查询语句</param>
        /// <param name="_orderField">排序字段，多个则用“,”隔开</param>
        /// <returns>分页SQL语句</returns>
        public static string CreatePagingSql(int _recordCount, int _pageSize, int _pageIndex, string _safeSql, string _orderField,string filterWhere)
        {
            //计算总页数
            _pageSize = _pageSize == 0 ? _recordCount : _pageSize;
            int pageCount = (_recordCount + _pageSize - 1) / _pageSize;

            //检查当前页数
            if (_pageIndex < 1)
            {
                _pageIndex = 1;
            }
            else if (_pageIndex > pageCount)
            {
                _pageIndex = pageCount;
            }
            //拼接SQL字符串，加上ROW_NUMBER函数进行分页
            StringBuilder newSafeSql = new StringBuilder();
            newSafeSql.AppendFormat("SELECT ROW_NUMBER() OVER(ORDER BY {0}) as row_number,", _orderField);
            newSafeSql.Append(_safeSql.Substring(_safeSql.ToUpper().IndexOf("SELECT") + 6));

            //拼接成最终的SQL语句
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * FROM (");
            if (filterWhere.Length > 0)
            {
                sbSql.Append("Select * from (");
                sbSql.Append(newSafeSql.ToString());
                sbSql.Append(") t  Where " + filterWhere);
            }
            else 
            {
                sbSql.Append(newSafeSql.ToString());
            }

            sbSql.Append(") AS T");

            if (filterWhere.Length == 0) 
            { 
                sbSql.AppendFormat(" WHERE row_number between {0} and {1}", ((_pageIndex - 1) * _pageSize) + 1, _pageIndex * _pageSize);
            }

            return sbSql.ToString();
        }


        #endregion

        #region sf_goods_order


        #endregion
    }
}
