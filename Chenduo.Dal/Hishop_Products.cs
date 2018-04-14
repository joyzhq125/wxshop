using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Hidistro.Core;
namespace Chenduo.DAL
{
    /// <summary>
    /// 数据访问类:Hishop_Products
    /// </summary>
    public class Hishop_Products
    {
        public Hishop_Products()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ProductId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Hishop_Products");
            strSql.Append(" where ProductId=@ProductId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", SqlDbType.Int,4)};
            parameters[0].Value = ProductId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Chenduo.Model.Hishop_Products model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Hishop_Products(");
            strSql.Append("wid,CategoryId,TypeId,ProductName,ProductCode,ShortDescription,Unit,Description,SaleStatus,AddedDate,VistiCounts,SaleCounts,ShowSaleCounts,DisplaySequence,ImageUrl1,ImageUrl2,ImageUrl3,ImageUrl4,ImageUrl5,ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,ThumbnailUrl410,MarketPrice,BrandId,MainCategoryPath,ExtendCategoryPath,HasSKU,IsfreeShipping,TaobaoProductId,Source,MinShowPrice,MaxShowPrice,FreightTemplateId,FirstCommission,SecondCommission,ThirdCommission,IsSetCommission,CubicMeter,FreightWeight,ProductShortName)");
            strSql.Append(" values (");
            strSql.Append("@wid,@CategoryId,@TypeId,@ProductName,@ProductCode,@ShortDescription,@Unit,@Description,@SaleStatus,@AddedDate,@VistiCounts,@SaleCounts,@ShowSaleCounts,@DisplaySequence,@ImageUrl1,@ImageUrl2,@ImageUrl3,@ImageUrl4,@ImageUrl5,@ThumbnailUrl40,@ThumbnailUrl60,@ThumbnailUrl100,@ThumbnailUrl160,@ThumbnailUrl180,@ThumbnailUrl220,@ThumbnailUrl310,@ThumbnailUrl410,@MarketPrice,@BrandId,@MainCategoryPath,@ExtendCategoryPath,@HasSKU,@IsfreeShipping,@TaobaoProductId,@Source,@MinShowPrice,@MaxShowPrice,@FreightTemplateId,@FirstCommission,@SecondCommission,@ThirdCommission,@IsSetCommission,@CubicMeter,@FreightWeight,@ProductShortName)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@wid", SqlDbType.NVarChar,50),
                    new SqlParameter("@CategoryId", SqlDbType.Int,4),
                    new SqlParameter("@TypeId", SqlDbType.Int,4),
                    new SqlParameter("@ProductName", SqlDbType.NVarChar,200),
                    new SqlParameter("@ProductCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@ShortDescription", SqlDbType.NVarChar,2000),
                    new SqlParameter("@Unit", SqlDbType.NVarChar,50),
                    new SqlParameter("@Description", SqlDbType.NText),
                    new SqlParameter("@SaleStatus", SqlDbType.Int,4),
                    new SqlParameter("@AddedDate", SqlDbType.DateTime),
                    new SqlParameter("@VistiCounts", SqlDbType.Int,4),
                    new SqlParameter("@SaleCounts", SqlDbType.Int,4),
                    new SqlParameter("@ShowSaleCounts", SqlDbType.Int,4),
                    new SqlParameter("@DisplaySequence", SqlDbType.Int,4),
                    new SqlParameter("@ImageUrl1", SqlDbType.NVarChar,255),
                    new SqlParameter("@ImageUrl2", SqlDbType.NVarChar,255),
                    new SqlParameter("@ImageUrl3", SqlDbType.NVarChar,255),
                    new SqlParameter("@ImageUrl4", SqlDbType.NVarChar,255),
                    new SqlParameter("@ImageUrl5", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl40", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl60", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl100", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl160", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl180", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl220", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl310", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl410", SqlDbType.NVarChar,255),
                    new SqlParameter("@MarketPrice", SqlDbType.Money,8),
                    new SqlParameter("@BrandId", SqlDbType.Int,4),
                    new SqlParameter("@MainCategoryPath", SqlDbType.NVarChar,256),
                    new SqlParameter("@ExtendCategoryPath", SqlDbType.NVarChar,256),
                    new SqlParameter("@HasSKU", SqlDbType.Bit,1),
                    new SqlParameter("@IsfreeShipping", SqlDbType.Bit,1),
                    new SqlParameter("@TaobaoProductId", SqlDbType.BigInt,8),
                    new SqlParameter("@Source", SqlDbType.VarChar,1),
                    new SqlParameter("@MinShowPrice", SqlDbType.Money,8),
                    new SqlParameter("@MaxShowPrice", SqlDbType.Money,8),
                    new SqlParameter("@FreightTemplateId", SqlDbType.Int,4),
                    new SqlParameter("@FirstCommission", SqlDbType.Decimal,9),
                    new SqlParameter("@SecondCommission", SqlDbType.Decimal,9),
                    new SqlParameter("@ThirdCommission", SqlDbType.Decimal,9),
                    new SqlParameter("@IsSetCommission", SqlDbType.Bit,1),
                    new SqlParameter("@CubicMeter", SqlDbType.Decimal,9),
                    new SqlParameter("@FreightWeight", SqlDbType.Decimal,9),
                    new SqlParameter("@ProductShortName", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.wid;
            parameters[1].Value = model.CategoryId;
            parameters[2].Value = model.TypeId;
            parameters[3].Value = model.ProductName;
            parameters[4].Value = model.ProductCode;
            parameters[5].Value = model.ShortDescription;
            parameters[6].Value = model.Unit;
            parameters[7].Value = model.Description;
            parameters[8].Value = model.SaleStatus;
            parameters[9].Value = model.AddedDate;
            parameters[10].Value = model.VistiCounts;
            parameters[11].Value = model.SaleCounts;
            parameters[12].Value = model.ShowSaleCounts;
            parameters[13].Value = model.DisplaySequence;
            parameters[14].Value = model.ImageUrl1;
            parameters[15].Value = model.ImageUrl2;
            parameters[16].Value = model.ImageUrl3;
            parameters[17].Value = model.ImageUrl4;
            parameters[18].Value = model.ImageUrl5;
            parameters[19].Value = model.ThumbnailUrl40;
            parameters[20].Value = model.ThumbnailUrl60;
            parameters[21].Value = model.ThumbnailUrl100;
            parameters[22].Value = model.ThumbnailUrl160;
            parameters[23].Value = model.ThumbnailUrl180;
            parameters[24].Value = model.ThumbnailUrl220;
            parameters[25].Value = model.ThumbnailUrl310;
            parameters[26].Value = model.ThumbnailUrl410;
            parameters[27].Value = model.MarketPrice;
            parameters[28].Value = model.BrandId;
            parameters[29].Value = model.MainCategoryPath;
            parameters[30].Value = model.ExtendCategoryPath;
            parameters[31].Value = model.HasSKU;
            parameters[32].Value = model.IsfreeShipping;
            parameters[33].Value = model.TaobaoProductId;
            parameters[34].Value = model.Source;
            parameters[35].Value = model.MinShowPrice;
            parameters[36].Value = model.MaxShowPrice;
            parameters[37].Value = model.FreightTemplateId;
            parameters[38].Value = model.FirstCommission;
            parameters[39].Value = model.SecondCommission;
            parameters[40].Value = model.ThirdCommission;
            parameters[41].Value = model.IsSetCommission;
            parameters[42].Value = model.CubicMeter;
            parameters[43].Value = model.FreightWeight;
            parameters[44].Value = model.ProductShortName;

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
        public bool Update(Chenduo.Model.Hishop_Products model)
        {

            string website = SFUtils.getWebSite();
            model.ImageUrl1 = model.ImageUrl1 != "" ? model.ImageUrl1.Replace(website, "") : "";
            model.ImageUrl2 = model.ImageUrl2 != "" ? model.ImageUrl2.Replace(website, "") : "";
            model.ImageUrl3 = model.ImageUrl3 != "" ? model.ImageUrl3.Replace(website, "") : "";
            model.ImageUrl4 = model.ImageUrl1 != "" ? model.ImageUrl4.Replace(website, "") : "";
            model.ImageUrl5 = model.ImageUrl1 != "" ? model.ImageUrl5.Replace(website, "") : "";
            model.ThumbnailUrl40 = model.ThumbnailUrl40 != "" ? model.ThumbnailUrl40.Replace(website, "") : "";
            model.ThumbnailUrl60 = model.ThumbnailUrl60 != "" ? model.ThumbnailUrl60.Replace(website, "") : "";
            model.ThumbnailUrl100 = model.ThumbnailUrl100 != "" ? model.ThumbnailUrl100.Replace(website, "") : "";
            model.ThumbnailUrl160 = model.ThumbnailUrl160 != "" ? model.ThumbnailUrl160.Replace(website, "") : "";
            model.ThumbnailUrl180 = model.ThumbnailUrl180 != "" ? model.ThumbnailUrl180.Replace(website, "") : "";
            model.ThumbnailUrl220 = model.ThumbnailUrl220 != "" ? model.ThumbnailUrl220.Replace(website, "") : "";
            model.ThumbnailUrl310 = model.ThumbnailUrl310 != "" ? model.ThumbnailUrl310.Replace(website, "") : "";
            model.ThumbnailUrl410 = model.ThumbnailUrl410 != "" ? model.ThumbnailUrl410.Replace(website, "") : "";

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Hishop_Products set ");
            strSql.Append("wid=@wid,");
            strSql.Append("CategoryId=@CategoryId,");
            strSql.Append("TypeId=@TypeId,");
            strSql.Append("ProductName=@ProductName,");
            strSql.Append("ProductCode=@ProductCode,");
            strSql.Append("ShortDescription=@ShortDescription,");
            strSql.Append("Unit=@Unit,");
            strSql.Append("Description=@Description,");
            strSql.Append("SaleStatus=@SaleStatus,");
            strSql.Append("AddedDate=@AddedDate,");
            strSql.Append("VistiCounts=@VistiCounts,");
            strSql.Append("SaleCounts=@SaleCounts,");
            strSql.Append("ShowSaleCounts=@ShowSaleCounts,");
            strSql.Append("DisplaySequence=@DisplaySequence,");
            strSql.Append("ImageUrl1=@ImageUrl1,");
            strSql.Append("ImageUrl2=@ImageUrl2,");
            strSql.Append("ImageUrl3=@ImageUrl3,");
            strSql.Append("ImageUrl4=@ImageUrl4,");
            strSql.Append("ImageUrl5=@ImageUrl5,");
            strSql.Append("ThumbnailUrl40=@ThumbnailUrl40,");
            strSql.Append("ThumbnailUrl60=@ThumbnailUrl60,");
            strSql.Append("ThumbnailUrl100=@ThumbnailUrl100,");
            strSql.Append("ThumbnailUrl160=@ThumbnailUrl160,");
            strSql.Append("ThumbnailUrl180=@ThumbnailUrl180,");
            strSql.Append("ThumbnailUrl220=@ThumbnailUrl220,");
            strSql.Append("ThumbnailUrl310=@ThumbnailUrl310,");
            strSql.Append("ThumbnailUrl410=@ThumbnailUrl410,");
            strSql.Append("MarketPrice=@MarketPrice,");
            strSql.Append("BrandId=@BrandId,");
            strSql.Append("MainCategoryPath=@MainCategoryPath,");
            strSql.Append("ExtendCategoryPath=@ExtendCategoryPath,");
            strSql.Append("HasSKU=@HasSKU,");
            strSql.Append("IsfreeShipping=@IsfreeShipping,");
            strSql.Append("TaobaoProductId=@TaobaoProductId,");
            strSql.Append("Source=@Source,");
            strSql.Append("MinShowPrice=@MinShowPrice,");
            strSql.Append("MaxShowPrice=@MaxShowPrice,");
            strSql.Append("FreightTemplateId=@FreightTemplateId,");
            strSql.Append("FirstCommission=@FirstCommission,");
            strSql.Append("SecondCommission=@SecondCommission,");
            strSql.Append("ThirdCommission=@ThirdCommission,");
            strSql.Append("IsSetCommission=@IsSetCommission,");
            strSql.Append("CubicMeter=@CubicMeter,");
            strSql.Append("FreightWeight=@FreightWeight,");
            strSql.Append("ProductShortName=@ProductShortName");
            strSql.Append(" where ProductId=@ProductId");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", SqlDbType.Int,4),
                    new SqlParameter("@wid", SqlDbType.NVarChar,50),
                    new SqlParameter("@CategoryId", SqlDbType.Int,4),
                    new SqlParameter("@TypeId", SqlDbType.Int,4),
                    new SqlParameter("@ProductName", SqlDbType.NVarChar,200),
                    new SqlParameter("@ProductCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@ShortDescription", SqlDbType.NVarChar,2000),
                    new SqlParameter("@Unit", SqlDbType.NVarChar,50),
                    new SqlParameter("@Description", SqlDbType.NText),
                    new SqlParameter("@SaleStatus", SqlDbType.Int,4),
                    new SqlParameter("@AddedDate", SqlDbType.DateTime),
                    new SqlParameter("@VistiCounts", SqlDbType.Int,4),
                    new SqlParameter("@SaleCounts", SqlDbType.Int,4),
                    new SqlParameter("@ShowSaleCounts", SqlDbType.Int,4),
                    new SqlParameter("@DisplaySequence", SqlDbType.Int,4),
                    new SqlParameter("@ImageUrl1", SqlDbType.NVarChar,255),
                    new SqlParameter("@ImageUrl2", SqlDbType.NVarChar,255),
                    new SqlParameter("@ImageUrl3", SqlDbType.NVarChar,255),
                    new SqlParameter("@ImageUrl4", SqlDbType.NVarChar,255),
                    new SqlParameter("@ImageUrl5", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl40", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl60", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl100", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl160", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl180", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl220", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl310", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl410", SqlDbType.NVarChar,255),
                    new SqlParameter("@MarketPrice", SqlDbType.Money,8),
                    new SqlParameter("@BrandId", SqlDbType.Int,4),
                    new SqlParameter("@MainCategoryPath", SqlDbType.NVarChar,256),
                    new SqlParameter("@ExtendCategoryPath", SqlDbType.NVarChar,256),
                    new SqlParameter("@HasSKU", SqlDbType.Bit,1),
                    new SqlParameter("@IsfreeShipping", SqlDbType.Bit,1),
                    new SqlParameter("@TaobaoProductId", SqlDbType.BigInt,8),
                    new SqlParameter("@Source", SqlDbType.VarChar,1),
                    new SqlParameter("@MinShowPrice", SqlDbType.Money,8),
                    new SqlParameter("@MaxShowPrice", SqlDbType.Money,8),
                    new SqlParameter("@FreightTemplateId", SqlDbType.Int,4),
                    new SqlParameter("@FirstCommission", SqlDbType.Decimal,9),
                    new SqlParameter("@SecondCommission", SqlDbType.Decimal,9),
                    new SqlParameter("@ThirdCommission", SqlDbType.Decimal,9),
                    new SqlParameter("@IsSetCommission", SqlDbType.Bit,1),
                    new SqlParameter("@CubicMeter", SqlDbType.Decimal,9),
                    new SqlParameter("@FreightWeight", SqlDbType.Decimal,9),
                    new SqlParameter("@ProductShortName", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ProductId;
            parameters[1].Value = model.wid;
            parameters[2].Value = model.CategoryId;
            parameters[3].Value = model.TypeId;
            parameters[4].Value = model.ProductName;
            parameters[5].Value = model.ProductCode;
            parameters[6].Value = model.ShortDescription;
            parameters[7].Value = model.Unit;
            parameters[8].Value = model.Description;
            parameters[9].Value = model.SaleStatus;
            parameters[10].Value = model.AddedDate;
            parameters[11].Value = model.VistiCounts;
            parameters[12].Value = model.SaleCounts;
            parameters[13].Value = model.ShowSaleCounts;
            parameters[14].Value = model.DisplaySequence;
            parameters[15].Value = model.ImageUrl1;
            parameters[16].Value = model.ImageUrl2;
            parameters[17].Value = model.ImageUrl3;
            parameters[18].Value = model.ImageUrl4;
            parameters[19].Value = model.ImageUrl5;
            parameters[20].Value = model.ThumbnailUrl40;
            parameters[21].Value = model.ThumbnailUrl60;
            parameters[22].Value = model.ThumbnailUrl100;
            parameters[23].Value = model.ThumbnailUrl160;
            parameters[24].Value = model.ThumbnailUrl180;
            parameters[25].Value = model.ThumbnailUrl220;
            parameters[26].Value = model.ThumbnailUrl310;
            parameters[27].Value = model.ThumbnailUrl410;
            parameters[28].Value = model.MarketPrice;
            parameters[29].Value = model.BrandId;
            parameters[30].Value = model.MainCategoryPath;
            parameters[31].Value = model.ExtendCategoryPath;
            parameters[32].Value = model.HasSKU;
            parameters[33].Value = model.IsfreeShipping;
            parameters[34].Value = model.TaobaoProductId;
            parameters[35].Value = model.Source;
            parameters[36].Value = model.MinShowPrice;
            parameters[37].Value = model.MaxShowPrice;
            parameters[38].Value = model.FreightTemplateId;
            parameters[39].Value = model.FirstCommission;
            parameters[40].Value = model.SecondCommission;
            parameters[41].Value = model.ThirdCommission;
            parameters[42].Value = model.IsSetCommission;
            parameters[43].Value = model.CubicMeter;
            parameters[44].Value = model.FreightWeight;
            parameters[45].Value = model.ProductShortName;

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
        public bool Delete(int ProductId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Hishop_Products ");
            strSql.Append(" where ProductId=@ProductId");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", SqlDbType.Int,4)
};
            parameters[0].Value = ProductId;

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
        public bool DeleteList(string ProductIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Hishop_Products ");
            strSql.Append(" where ProductId in (" + ProductIdlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
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
        public Chenduo.Model.Hishop_Products GetModel(int ProductId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ProductId,wid,CategoryId,TypeId,ProductName,ProductCode,ShortDescription,Unit,Description,SaleStatus,AddedDate,VistiCounts,SaleCounts,ShowSaleCounts,DisplaySequence,ImageUrl1,ImageUrl2,ImageUrl3,ImageUrl4,ImageUrl5,ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,ThumbnailUrl410,MarketPrice,BrandId,MainCategoryPath,ExtendCategoryPath,HasSKU,IsfreeShipping,TaobaoProductId,Source,MinShowPrice,MaxShowPrice,FreightTemplateId,FirstCommission,SecondCommission,ThirdCommission,IsSetCommission,CubicMeter,FreightWeight,ProductShortName,SalePrice,SkuId,Stock,Weight,IsMakeTaobao from vw_Hishop_BrowseProductList ");
            strSql.Append(" where ProductId=@ProductId");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", SqlDbType.Int,4)
            };
            parameters[0].Value = ProductId;

            Chenduo.Model.Hishop_Products model = new Chenduo.Model.Hishop_Products();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ProductId"].ToString() != "")
                {
                    model.ProductId = int.Parse(ds.Tables[0].Rows[0]["ProductId"].ToString());
                }
                model.wid = ds.Tables[0].Rows[0]["wid"].ToString();
                if (ds.Tables[0].Rows[0]["CategoryId"].ToString() != "")
                {
                    model.CategoryId = int.Parse(ds.Tables[0].Rows[0]["CategoryId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TypeId"].ToString() != "")
                {
                    model.TypeId = int.Parse(ds.Tables[0].Rows[0]["TypeId"].ToString());
                }
                model.ProductName = ds.Tables[0].Rows[0]["ProductName"].ToString();
                model.ProductCode = ds.Tables[0].Rows[0]["ProductCode"].ToString();
                model.ShortDescription = ds.Tables[0].Rows[0]["ShortDescription"].ToString();
                model.Unit = ds.Tables[0].Rows[0]["Unit"].ToString();
                model.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                if (ds.Tables[0].Rows[0]["SaleStatus"].ToString() != "")
                {
                    model.SaleStatus = int.Parse(ds.Tables[0].Rows[0]["SaleStatus"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AddedDate"].ToString() != "")
                {
                    model.AddedDate = DateTime.Parse(ds.Tables[0].Rows[0]["AddedDate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["VistiCounts"].ToString() != "")
                {
                    model.VistiCounts = int.Parse(ds.Tables[0].Rows[0]["VistiCounts"].ToString());
                }

                if (ds.Tables[0].Rows[0]["SalePrice"].ToString() != "")
                {
                    model.SalePrice = decimal.Parse(ds.Tables[0].Rows[0]["SalePrice"].ToString());
                }
                model.SkuId = ds.Tables[0].Rows[0]["SkuId"].ToString();
                if (ds.Tables[0].Rows[0]["Stock"].ToString() != "")
                {
                    model.Stock = int.Parse(ds.Tables[0].Rows[0]["Stock"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Weight"].ToString() != "")
                {
                    model.Weight = decimal.Parse(ds.Tables[0].Rows[0]["Weight"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsMakeTaobao"].ToString() != "")
                {
                    model.IsMakeTaobao = int.Parse(ds.Tables[0].Rows[0]["IsMakeTaobao"].ToString());
                }

                if (ds.Tables[0].Rows[0]["SaleCounts"].ToString() != "")
                {
                    model.SaleCounts = int.Parse(ds.Tables[0].Rows[0]["SaleCounts"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ShowSaleCounts"].ToString() != "")
                {
                    model.ShowSaleCounts = int.Parse(ds.Tables[0].Rows[0]["ShowSaleCounts"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DisplaySequence"].ToString() != "")
                {
                    model.DisplaySequence = int.Parse(ds.Tables[0].Rows[0]["DisplaySequence"].ToString());
                }
                model.ImageUrl1 = ds.Tables[0].Rows[0]["ImageUrl1"].ToString() != ""? SFUtils.getWebSite() + ds.Tables[0].Rows[0]["ImageUrl1"].ToString(): "";
                model.ImageUrl2 = ds.Tables[0].Rows[0]["ImageUrl2"].ToString() != ""? SFUtils.getWebSite() + ds.Tables[0].Rows[0]["ImageUrl2"].ToString():"";
                model.ImageUrl3 = ds.Tables[0].Rows[0]["ImageUrl3"].ToString()!=""? SFUtils.getWebSite() + ds.Tables[0].Rows[0]["ImageUrl3"].ToString():"";
                model.ImageUrl4 = ds.Tables[0].Rows[0]["ImageUrl4"].ToString()!=""? SFUtils.getWebSite() + ds.Tables[0].Rows[0]["ImageUrl4"].ToString():"";
                model.ImageUrl5 = ds.Tables[0].Rows[0]["ImageUrl5"].ToString()!=""? SFUtils.getWebSite() + ds.Tables[0].Rows[0]["ImageUrl5"].ToString():"";
                model.ThumbnailUrl40 = ds.Tables[0].Rows[0]["ThumbnailUrl40"].ToString()!=""? SFUtils.getWebSite() + ds.Tables[0].Rows[0]["ThumbnailUrl40"].ToString():"";
                model.ThumbnailUrl60 = ds.Tables[0].Rows[0]["ThumbnailUrl60"].ToString()!=""? SFUtils.getWebSite() + ds.Tables[0].Rows[0]["ThumbnailUrl60"].ToString():"";
                model.ThumbnailUrl100 = ds.Tables[0].Rows[0]["ThumbnailUrl100"].ToString()!=""? SFUtils.getWebSite() + ds.Tables[0].Rows[0]["ThumbnailUrl100"].ToString():"";
                model.ThumbnailUrl160 = ds.Tables[0].Rows[0]["ThumbnailUrl160"].ToString()!=""? SFUtils.getWebSite() + ds.Tables[0].Rows[0]["ThumbnailUrl160"].ToString():"";
                model.ThumbnailUrl180 = ds.Tables[0].Rows[0]["ThumbnailUrl180"].ToString()!=""? SFUtils.getWebSite() + ds.Tables[0].Rows[0]["ThumbnailUrl180"].ToString():"";
                model.ThumbnailUrl220 = ds.Tables[0].Rows[0]["ThumbnailUrl220"].ToString()!=""? SFUtils.getWebSite() + ds.Tables[0].Rows[0]["ThumbnailUrl220"].ToString():"";
                model.ThumbnailUrl310 = ds.Tables[0].Rows[0]["ThumbnailUrl310"].ToString()!=""? SFUtils.getWebSite() + ds.Tables[0].Rows[0]["ThumbnailUrl310"].ToString():"";
                model.ThumbnailUrl410 = ds.Tables[0].Rows[0]["ThumbnailUrl410"].ToString()!=""? SFUtils.getWebSite() + ds.Tables[0].Rows[0]["ThumbnailUrl410"].ToString():"";
                if (ds.Tables[0].Rows[0]["MarketPrice"].ToString() != "")
                {
                    model.MarketPrice = decimal.Parse(ds.Tables[0].Rows[0]["MarketPrice"].ToString());
                }
                if (ds.Tables[0].Rows[0]["BrandId"].ToString() != "")
                {
                    model.BrandId = int.Parse(ds.Tables[0].Rows[0]["BrandId"].ToString());
                }
                model.MainCategoryPath = ds.Tables[0].Rows[0]["MainCategoryPath"].ToString();
                model.ExtendCategoryPath = ds.Tables[0].Rows[0]["ExtendCategoryPath"].ToString();
                if (ds.Tables[0].Rows[0]["HasSKU"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["HasSKU"].ToString() == "1") || (ds.Tables[0].Rows[0]["HasSKU"].ToString().ToLower() == "true"))
                    {
                        model.HasSKU = true;
                    }
                    else
                    {
                        model.HasSKU = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["IsfreeShipping"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["IsfreeShipping"].ToString() == "1") || (ds.Tables[0].Rows[0]["IsfreeShipping"].ToString().ToLower() == "true"))
                    {
                        model.IsfreeShipping = true;
                    }
                    else
                    {
                        model.IsfreeShipping = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["TaobaoProductId"].ToString() != "")
                {
                    model.TaobaoProductId = long.Parse(ds.Tables[0].Rows[0]["TaobaoProductId"].ToString());
                }
                model.Source = ds.Tables[0].Rows[0]["Source"].ToString();
                if (ds.Tables[0].Rows[0]["MinShowPrice"].ToString() != "")
                {
                    model.MinShowPrice = decimal.Parse(ds.Tables[0].Rows[0]["MinShowPrice"].ToString());
                }
                if (ds.Tables[0].Rows[0]["MaxShowPrice"].ToString() != "")
                {
                    model.MaxShowPrice = decimal.Parse(ds.Tables[0].Rows[0]["MaxShowPrice"].ToString());
                }
                if (ds.Tables[0].Rows[0]["FreightTemplateId"].ToString() != "")
                {
                    model.FreightTemplateId = int.Parse(ds.Tables[0].Rows[0]["FreightTemplateId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["FirstCommission"].ToString() != "")
                {
                    model.FirstCommission = decimal.Parse(ds.Tables[0].Rows[0]["FirstCommission"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SecondCommission"].ToString() != "")
                {
                    model.SecondCommission = decimal.Parse(ds.Tables[0].Rows[0]["SecondCommission"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ThirdCommission"].ToString() != "")
                {
                    model.ThirdCommission = decimal.Parse(ds.Tables[0].Rows[0]["ThirdCommission"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsSetCommission"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["IsSetCommission"].ToString() == "1") || (ds.Tables[0].Rows[0]["IsSetCommission"].ToString().ToLower() == "true"))
                    {
                        model.IsSetCommission = true;
                    }
                    else
                    {
                        model.IsSetCommission = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["CubicMeter"].ToString() != "")
                {
                    model.CubicMeter = decimal.Parse(ds.Tables[0].Rows[0]["CubicMeter"].ToString());
                }
                if (ds.Tables[0].Rows[0]["FreightWeight"].ToString() != "")
                {
                    model.FreightWeight = decimal.Parse(ds.Tables[0].Rows[0]["FreightWeight"].ToString());
                }
                model.ProductShortName = ds.Tables[0].Rows[0]["ProductShortName"].ToString();
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ProductId,wid,CategoryId,TypeId,ProductName,ProductCode,ShortDescription,Unit,Description,SaleStatus,AddedDate,VistiCounts,SaleCounts,ShowSaleCounts,DisplaySequence,ImageUrl1,ImageUrl2,ImageUrl3,ImageUrl4,ImageUrl5,ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,ThumbnailUrl410,MarketPrice,BrandId,MainCategoryPath,ExtendCategoryPath,HasSKU,IsfreeShipping,TaobaoProductId,Source,MinShowPrice,MaxShowPrice,FreightTemplateId,FirstCommission,SecondCommission,ThirdCommission,IsSetCommission,CubicMeter,FreightWeight,ProductShortName ");
            strSql.Append(" FROM Hishop_Products ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
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
            strSql.Append(" ProductId,wid,CategoryId,TypeId,ProductName,ProductCode,ShortDescription,Unit,Description,SaleStatus,AddedDate,VistiCounts,SaleCounts,ShowSaleCounts,DisplaySequence,ImageUrl1,ImageUrl2,ImageUrl3,ImageUrl4,ImageUrl5,ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,ThumbnailUrl410,MarketPrice,BrandId,MainCategoryPath,ExtendCategoryPath,HasSKU,IsfreeShipping,TaobaoProductId,Source,MinShowPrice,MaxShowPrice,FreightTemplateId,FirstCommission,SecondCommission,ThirdCommission,IsSetCommission,CubicMeter,FreightWeight,ProductShortName ");
            strSql.Append(" FROM Hishop_Products ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, string order,string ordertype,ref int PCnt, ref int RCnt)
        {
            SqlParameter[] parameters = {

                new SqlParameter {ParameterName="@PCount", DbType=DbType.Int32, Direction=ParameterDirection.Output},
                new SqlParameter {ParameterName="@RCount", DbType=DbType.Int32, Direction=ParameterDirection.Output},
                new SqlParameter {ParameterName="@sys_Table", DbType=DbType.String,Size=100,Direction=ParameterDirection.Input},
                new SqlParameter {ParameterName="@sys_Fields",DbType=DbType.String,Size=500,Direction=ParameterDirection.Input},
                new SqlParameter {ParameterName="@sys_Where", DbType=DbType.String,Size=3000,Direction=ParameterDirection.Input},
                new SqlParameter {ParameterName="@sys_Order", DbType=DbType.String,Size=100,Direction=ParameterDirection.Input},
                new SqlParameter {ParameterName="@sys_OrderType", DbType=DbType.String,Size=100,Direction=ParameterDirection.Input},
                new SqlParameter {ParameterName="@sys_Begin", DbType=DbType.Int32,Direction=ParameterDirection.Input},
                new SqlParameter {ParameterName="@sys_PageIndex", DbType=DbType.Int32,Direction=ParameterDirection.Input},
                new SqlParameter {ParameterName="@sys_PageSize", DbType=DbType.Int32,Direction=ParameterDirection.Input}

            };
            //parameters[0].Value = "Hishop_Products";
            //parameters[1].Value = "ProductId";
            parameters[2].Value = "vw_Hishop_BrowseProductList";
            parameters[3].Value = "*";
            parameters[4].Value = strWhere;
            parameters[5].Value = order;
            parameters[6].Value = ordertype;
            parameters[7].Value = 1;
            parameters[8].Value = PageIndex;
            parameters[9].Value = PageSize;
            DataSet ds = DbHelperSQL.RunProcedure("sys_Page_v2", parameters, "ds");
            PCnt = Convert.ToInt32(parameters[0].Value);
            RCnt = Convert.ToInt32(parameters[1].Value);
            return ds;
        }

        #endregion  Method
    }
}
