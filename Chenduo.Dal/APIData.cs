using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Hidistro.Core;
namespace Chenduo.Dal
{
    public class ApiData
    {
        public DataTable GetProductSkus(int ProductId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sk.skuid,sk.productid,sk.sku,sk.weight,sk.stock,sk.costprice,sk.saleprice,dbo.F_SkuConcatAttr(sk.skuid) attrIds,dbo.F_SkuConcatVal(sk.skuid) valueIds ");
            strSql.Append(" from Hishop_SKUs sk left join Hishop_SKUItems si on sk.SkuId = si.SkuId ");
            strSql.Append(" where productId=@ProductId ");
            strSql.Append(" group by sk.skuid,sk.productid,sk.sku,sk.weight,sk.stock,sk.costprice,sk.saleprice");
            SqlParameter[] parameters =
            {
                new SqlParameter("@ProductId", SqlDbType.Int, 4)
            };
            parameters[0].Value = ProductId;
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            return ds.Tables[0];
        }

        public DataTable GetProductAttrsByTypeId(int typeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select distinct AttributeId,AttributeName from Hishop_Attributes " );
            strSql.Append(@" where typeid = @TypeId");
            SqlParameter[] parameters =
            {
                new SqlParameter("@TypeId", SqlDbType.Int, 4)
            };
            parameters[0].Value = typeId;
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            return ds.Tables[0];
        }
        public DataTable GetProductValuesByTypeId(int typeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select distinct av.AttributeId,av.ValueId,av.ValueStr from Hishop_Attributes ab ,Hishop_AttributeValues av ");
            strSql.Append(@" where ab.AttributeId = av.AttributeId and ab.TypeId = @TypeId ");
            SqlParameter[] parameters =
            {
                new SqlParameter("@TypeId", SqlDbType.Int, 4)
            };
            parameters[0].Value = typeId;
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            return ds.Tables[0];
        }

        public List<Chenduo.Model.imgHistory> GetImgHistory(int productId)
        {
            List<Chenduo.Model.imgHistory> list = new List<Chenduo.Model.imgHistory>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from imgHistory where goodsid=@ProductId");
            SqlParameter[] parameters =
            {
                new SqlParameter("@ProductId", SqlDbType.Int, 4)
            };
            parameters[0].Value = productId;
            DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
            Chenduo.Model.imgHistory model = null;
            for (int n = 0;n < dt.Rows.Count;n++)
            {
                model = new Chenduo.Model.imgHistory();
                if (dt.Rows[n]["goodsid"].ToString() != "")
                {
                    model.goodsid = SFUtils.ObjToInt(dt.Rows[n]["goodsid"]);
                }
                model.url = dt.Rows[n]["url"].ToString() != ""
                    ? SFUtils.getWebSite() + dt.Rows[n]["url"].ToString()
                    : "";

                list.Add(model);
            }
            return list;
        }


        public bool addImgHistory(int productId,string imgUrl)
        {
            if (!existImgHistory(productId, imgUrl))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into imgHistory(goodsid,url,AddedDate) values(@goodsid,@url,@AddedDate)");// productId,imgUrl,DateTime.Now);
                SqlParameter[] parameters =
                {
                    new SqlParameter("@goodsid", SqlDbType.Int, 4),
                    new SqlParameter("@url", SqlDbType.NVarChar, 255),
                    new SqlParameter("@AddedDate", SqlDbType.DateTime)
                };
                parameters[0].Value = productId;
                parameters[1].Value = imgUrl;
                parameters[2].Value = DateTime.Now;
                if (DbHelperSQL.ExecuteSql(strSql.ToString(),parameters) != 1)
                {
                    return false;
                }
            }
            return true;
        }

        public bool delImgHistroy(int productId, string imgUrl)
        {
            if (existImgHistory(productId, imgUrl))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("delete from imgHistory where goodsid={0} and url='{1}'", productId, imgUrl);
                if (DbHelperSQL.ExecuteSql(strSql.ToString()) != 1)
                {
                    return false;
                }
            }
            return true;
        }

        public bool existImgHistory(int productId, string imgUrl)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select count(*) from imgHistory where goodsid={0} and url='{1}'", productId,
                imgUrl);
            return  DbHelperSQL.Exists(strSql.ToString());
        }
    }
}
