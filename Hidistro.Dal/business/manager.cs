using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Hidistro.Core;
using Hidistro.Entities.Store;
//using SF.DBUtility;
//using SF.Common;

namespace SF.DAL
{
    /// <summary>
    /// 数据访问类:管理员
    /// </summary>
    public partial class manager
    {
        private string databaseprefix; //数据库表名前缀
        public manager(string _databaseprefix)
        {
            databaseprefix = _databaseprefix;
        }

        #region 基本方法=============================
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from aspnet_Managers");
            strSql.Append(" where UserId=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 查询用户名是否存在
        /// </summary>
        public bool Exists(string user_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from aspnet_Managers");
            strSql.Append(" where UserName=@user_name ");
            SqlParameter[] parameters = {
					new SqlParameter("@user_name", SqlDbType.NVarChar,100)};
            parameters[0].Value = user_name;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 根据用户名取得用户真实姓名
        /// </summary>
        /// <param name="user_name"></param>
        /// <returns></returns>
        public string GetRealName(string user_name) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select realname from aspnet_Managers");
            strSql.Append(" where UserName=@user_name ");
            SqlParameter[] parameters = {
					new SqlParameter("@user_name", SqlDbType.NVarChar,100)};
            parameters[0].Value = user_name;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj != null)
            {
                return obj.ToString();
            }
            else 
            {
                return "";
            }
        }

        /// <summary>
        /// 根据用户名取得Salt
        /// </summary>
        public string GetSalt(string user_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 salt from aspnet_Managers");
            strSql.Append(" where UserName=@user_name");
            SqlParameter[] parameters = {
                    new SqlParameter("@user_name", SqlDbType.NVarChar,100)};
            parameters[0].Value = user_name;
            string salt = Convert.ToString(DbHelperSQL.GetSingle(strSql.ToString(), parameters));
            if (string.IsNullOrEmpty(salt))
            {
                return "";
            }
            return salt;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ManagerInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into aspnet_Managers(");
            strSql.Append("RoleId,UserName,PassWord,salt,realname,telephone,Email,islock,CreateDate)");
            strSql.Append(" values (");
            strSql.Append("@RoleId,@UserName,@PassWord,@salt,@realname,@telephone,@Email,@islock,@CreateDate)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@RoleId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,100),
					new SqlParameter("@PassWord", SqlDbType.NVarChar,100),
					new SqlParameter("@salt", SqlDbType.NVarChar,20),
					new SqlParameter("@realname", SqlDbType.NVarChar,50),
					new SqlParameter("@telephone", SqlDbType.NVarChar,30),
					new SqlParameter("@Email", SqlDbType.NVarChar,30),
					new SqlParameter("@islock", SqlDbType.Int,4),
					new SqlParameter("@CreateDate", SqlDbType.DateTime)};
            parameters[0].Value = model.RoleId;;
            parameters[1].Value = model.UserName;
            parameters[2].Value = model.Password;
            parameters[3].Value = model.salt;
            parameters[4].Value = model.realname;
            parameters[5].Value = model.telephone;
            parameters[6].Value = model.Email;
            parameters[7].Value = model.islock;
            parameters[8].Value = model.CreateDate;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
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
        /// 更新一条数据
        /// </summary>
        public bool Update(ManagerInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update aspnet_Managers set ");
            strSql.Append("RoleId=@RoleId,");
            strSql.Append("UserName=@UserName,");
            strSql.Append("PassWord=@PassWord,");
            strSql.Append("salt=@salt,");
            strSql.Append("realname=@realname,");
            strSql.Append("telephone=@telephone,");
            strSql.Append("Email=@Email,");
            strSql.Append("islock=@islock,");
            strSql.Append("CreateDate=@CreateDate");
            strSql.Append(" where UserId=@UserId");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@RoleId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,100),
					new SqlParameter("@PassWord", SqlDbType.NVarChar,100),
                    new SqlParameter("@salt",SqlDbType.NVarChar,20),
					new SqlParameter("@realname", SqlDbType.NVarChar,50),
					new SqlParameter("@telephone", SqlDbType.NVarChar,30),
					new SqlParameter("@Email", SqlDbType.NVarChar,30),
					new SqlParameter("@islock", SqlDbType.Int,4),
					new SqlParameter("@CreateDate", SqlDbType.DateTime)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.RoleId;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.Password;
            parameters[4].Value = model.salt;
            parameters[5].Value = model.realname;
            parameters[6].Value = model.telephone;
            parameters[7].Value = model.Email;
            parameters[8].Value = model.islock;
            parameters[9].Value = model.CreateDate;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
        public bool Delete(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from aspnet_Managers");
            strSql.Append(" where UserId=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
        public ManagerInfo GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 UserId,RoleId,UserName,Password,salt,Email,CreateDate,realname,telephone,islock from aspnet_Managers ");
            strSql.Append(" where UserId=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            ManagerInfo model = new ManagerInfo();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UserId"].ToString() != "")
                {
                    model.RoleId = int.Parse(ds.Tables[0].Rows[0]["RoleId"].ToString());
                }
                model.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                model.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                model.salt = ds.Tables[0].Rows[0]["salt"].ToString();
                model.realname = ds.Tables[0].Rows[0]["realname"].ToString();
                model.telephone = ds.Tables[0].Rows[0]["telephone"].ToString();
                model.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                if (ds.Tables[0].Rows[0]["islock"].ToString() != "")
                {
                    model.islock = ds.Tables[0].Rows[0]["islock"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CreateDate"].ToString() != "")
                {
                    model.CreateDate = DateTime.Parse(ds.Tables[0].Rows[0]["CreateDate"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }
    
        /// <summary>
        /// 根据用户名密码返回一个实体
        /// </summary>
        public ManagerInfo GetModel(string user_name, string password)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id from aspnet_Managers");
            strSql.Append(" where UserName=@user_name and Password=@password and islock='0'");
            SqlParameter[] parameters = {
					new SqlParameter("@user_name", SqlDbType.NVarChar,100),
                    new SqlParameter("@password", SqlDbType.NVarChar,100)};
            parameters[0].Value = user_name;
            parameters[1].Value = password;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj != null)
            {
                return GetModel(Convert.ToInt32(obj));
            }
            return null;
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" UserId,RoleId,UserName,Password,salt,Email,CreateDate,realname,telephone,islock ");
            strSql.Append(" FROM aspnet_Managers ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM aspnet_Managers");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
        }

        /// <summary>
        /// 新增一个获取编号方法
        /// 时间:2015-8-24
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataSet GetList(string str) 
        {
            string sql = @"select * from aspnet_Managers where {0}";
            if (str.Trim() != "") 
            {
                sql = string.Format(sql, str);
            }
            return DbHelperSQL.Query(sql.ToString());
        }

        /// <summary>
        /// 获取id方法
        /// 时间:2015-8-24
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public object GetNum(string str) 
        {
            string sql = @"select * from aspnet_Managers where busniesenum = '{0}'";
            if (str.Trim() != "") 
            {
                sql = string.Format(sql, str);
            }
            return DbHelperSQL.GetSingle(sql.ToString());
        }

        #endregion
    }
}