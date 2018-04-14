using Hidistro.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Chenduo.Bll
{/// <summary>
 /// Hishop_Products
 /// </summary>
    public class Hishop_Products
    {
        private readonly Chenduo.DAL.Hishop_Products dal = new Chenduo.DAL.Hishop_Products();
        public Hishop_Products()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ProductId)
        {
            return dal.Exists(ProductId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Chenduo.Model.Hishop_Products model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Chenduo.Model.Hishop_Products model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ProductId)
        {

            return dal.Delete(ProductId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string ProductIdlist)
        {
            return dal.DeleteList(ProductIdlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Chenduo.Model.Hishop_Products GetModel(int ProductId)
        {

            return dal.GetModel(ProductId);
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
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Chenduo.Model.Hishop_Products> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Chenduo.Model.Hishop_Products> DataTableToList(DataTable dt)
        {
            List<Chenduo.Model.Hishop_Products> modelList = new List<Chenduo.Model.Hishop_Products>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Chenduo.Model.Hishop_Products model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new Chenduo.Model.Hishop_Products();
                    if (dt.Rows[n]["ProductId"].ToString() != "")
                    {
                        model.ProductId = int.Parse(dt.Rows[n]["ProductId"].ToString());
                    }
                    model.wid = dt.Rows[n]["wid"].ToString();
                    if (dt.Rows[n]["CategoryId"].ToString() != "")
                    {
                        model.CategoryId = int.Parse(dt.Rows[n]["CategoryId"].ToString());
                    }
                    if (dt.Rows[n]["TypeId"].ToString() != "")
                    {
                        model.TypeId = int.Parse(dt.Rows[n]["TypeId"].ToString());
                    }
                    model.ProductName = dt.Rows[n]["ProductName"].ToString();
                    model.ProductCode = dt.Rows[n]["ProductCode"].ToString();
                    model.ShortDescription = dt.Rows[n]["ShortDescription"].ToString();
                    model.Unit = dt.Rows[n]["Unit"].ToString();
                    model.Description = dt.Rows[n]["Description"].ToString();
                    if (dt.Rows[n]["SaleStatus"].ToString() != "")
                    {
                        model.SaleStatus = int.Parse(dt.Rows[n]["SaleStatus"].ToString());
                    }
                    if (dt.Rows[n]["AddedDate"].ToString() != "")
                    {
                        model.AddedDate = DateTime.Parse(dt.Rows[n]["AddedDate"].ToString());
                    }
                    if (dt.Rows[n]["VistiCounts"].ToString() != "")
                    {
                        model.VistiCounts = int.Parse(dt.Rows[n]["VistiCounts"].ToString());
                    }

                    if (dt.Rows[n]["SalePrice"].ToString() != "")
                    {
                        model.SalePrice = decimal.Parse(dt.Rows[n]["SalePrice"].ToString());
                    }
                    model.SkuId = dt.Rows[n]["SkuId"].ToString();
                    if (dt.Rows[n]["Stock"].ToString() != "")
                    {
                        model.Stock = int.Parse(dt.Rows[n]["Stock"].ToString());
                    }
                    if (dt.Rows[n]["Weight"].ToString() != "")
                    {
                        model.Weight = decimal.Parse(dt.Rows[n]["Weight"].ToString());
                    }
                    if (dt.Rows[n]["IsMakeTaobao"].ToString() != "")
                    {
                        model.IsMakeTaobao = int.Parse(dt.Rows[n]["IsMakeTaobao"].ToString());
                    }
                    if (dt.Rows[n]["SaleCounts"].ToString() != "")
                    {
                        model.SaleCounts = int.Parse(dt.Rows[n]["SaleCounts"].ToString());
                    }
                    if (dt.Rows[n]["ShowSaleCounts"].ToString() != "")
                    {
                        model.ShowSaleCounts = int.Parse(dt.Rows[n]["ShowSaleCounts"].ToString());
                    }
                    if (dt.Rows[n]["DisplaySequence"].ToString() != "")
                    {
                        model.DisplaySequence = int.Parse(dt.Rows[n]["DisplaySequence"].ToString());
                    }
                    /*
                    model.ImageUrl1 = SFUtils.getWebSite() + dt.Rows[n]["ImageUrl1"].ToString();
                    model.ImageUrl2 = SFUtils.getWebSite() + dt.Rows[n]["ImageUrl2"].ToString();
                    model.ImageUrl3 = SFUtils.getWebSite() + dt.Rows[n]["ImageUrl3"].ToString();
                    model.ImageUrl4 = SFUtils.getWebSite() + dt.Rows[n]["ImageUrl4"].ToString();
                    model.ImageUrl5 = SFUtils.getWebSite() + dt.Rows[n]["ImageUrl5"].ToString();
                    model.ThumbnailUrl40 = SFUtils.getWebSite() + dt.Rows[n]["ThumbnailUrl40"].ToString();
                    model.ThumbnailUrl60 = SFUtils.getWebSite() + dt.Rows[n]["ThumbnailUrl60"].ToString();
                    model.ThumbnailUrl100 = SFUtils.getWebSite() + dt.Rows[n]["ThumbnailUrl100"].ToString();
                    model.ThumbnailUrl160 = SFUtils.getWebSite() + dt.Rows[n]["ThumbnailUrl160"].ToString();
                    model.ThumbnailUrl180 = SFUtils.getWebSite() + dt.Rows[n]["ThumbnailUrl180"].ToString();
                    model.ThumbnailUrl220 = SFUtils.getWebSite() + dt.Rows[n]["ThumbnailUrl220"].ToString();
                    model.ThumbnailUrl310 = SFUtils.getWebSite() + dt.Rows[n]["ThumbnailUrl310"].ToString();
                    model.ThumbnailUrl410 = SFUtils.getWebSite() + dt.Rows[n]["ThumbnailUrl410"].ToString();
                    */
                    model.ImageUrl1 = dt.Rows[n]["ImageUrl1"].ToString() != "" ? SFUtils.getWebSite() + dt.Rows[n]["ImageUrl1"].ToString() : "";
                    model.ImageUrl2 = dt.Rows[n]["ImageUrl2"].ToString() != "" ? SFUtils.getWebSite() + dt.Rows[n]["ImageUrl2"].ToString() : "";
                    model.ImageUrl3 = dt.Rows[n]["ImageUrl3"].ToString() != "" ? SFUtils.getWebSite() + dt.Rows[n]["ImageUrl3"].ToString() : "";
                    model.ImageUrl4 = dt.Rows[n]["ImageUrl4"].ToString() != "" ? SFUtils.getWebSite() + dt.Rows[n]["ImageUrl4"].ToString() : "";
                    model.ImageUrl5 = dt.Rows[n]["ImageUrl5"].ToString() != "" ? SFUtils.getWebSite() + dt.Rows[n]["ImageUrl5"].ToString() : "";
                    model.ThumbnailUrl40 = dt.Rows[n]["ThumbnailUrl40"].ToString() != "" ? SFUtils.getWebSite() + dt.Rows[n]["ThumbnailUrl40"].ToString() : "";
                    model.ThumbnailUrl60 = dt.Rows[n]["ThumbnailUrl60"].ToString() != "" ? SFUtils.getWebSite() + dt.Rows[n]["ThumbnailUrl60"].ToString() : "";
                    model.ThumbnailUrl100 = dt.Rows[n]["ThumbnailUrl100"].ToString() != "" ? SFUtils.getWebSite() + dt.Rows[n]["ThumbnailUrl100"].ToString() : "";
                    model.ThumbnailUrl160 = dt.Rows[n]["ThumbnailUrl160"].ToString() != "" ? SFUtils.getWebSite() + dt.Rows[n]["ThumbnailUrl160"].ToString() : "";
                    model.ThumbnailUrl180 = dt.Rows[n]["ThumbnailUrl180"].ToString() != "" ? SFUtils.getWebSite() + dt.Rows[n]["ThumbnailUrl180"].ToString() : "";
                    model.ThumbnailUrl220 = dt.Rows[n]["ThumbnailUrl220"].ToString() != "" ? SFUtils.getWebSite() + dt.Rows[n]["ThumbnailUrl220"].ToString() : "";
                    model.ThumbnailUrl310 = dt.Rows[n]["ThumbnailUrl310"].ToString() != "" ? SFUtils.getWebSite() + dt.Rows[n]["ThumbnailUrl310"].ToString() : "";
                    model.ThumbnailUrl410 = dt.Rows[n]["ThumbnailUrl410"].ToString() != "" ? SFUtils.getWebSite() + dt.Rows[n]["ThumbnailUrl410"].ToString() : "";
                    if (dt.Rows[n]["MarketPrice"].ToString() != "")
                    {
                        model.MarketPrice = decimal.Parse(dt.Rows[n]["MarketPrice"].ToString());
                    }
                    if (dt.Rows[n]["BrandId"].ToString() != "")
                    {
                        model.BrandId = int.Parse(dt.Rows[n]["BrandId"].ToString());
                    }
                    model.MainCategoryPath = dt.Rows[n]["MainCategoryPath"].ToString();
                    model.ExtendCategoryPath = dt.Rows[n]["ExtendCategoryPath"].ToString();
                    if (dt.Rows[n]["HasSKU"].ToString() != "")
                    {
                        if ((dt.Rows[n]["HasSKU"].ToString() == "1") || (dt.Rows[n]["HasSKU"].ToString().ToLower() == "true"))
                        {
                            model.HasSKU = true;
                        }
                        else
                        {
                            model.HasSKU = false;
                        }
                    }
                    if (dt.Rows[n]["IsfreeShipping"].ToString() != "")
                    {
                        if ((dt.Rows[n]["IsfreeShipping"].ToString() == "1") || (dt.Rows[n]["IsfreeShipping"].ToString().ToLower() == "true"))
                        {
                            model.IsfreeShipping = true;
                        }
                        else
                        {
                            model.IsfreeShipping = false;
                        }
                    }
                    //model.TaobaoProductId=dt.Rows[n]["TaobaoProductId"].ToString();
                    model.Source = dt.Rows[n]["Source"].ToString();
                    if (dt.Rows[n]["MinShowPrice"].ToString() != "")
                    {
                        model.MinShowPrice = decimal.Parse(dt.Rows[n]["MinShowPrice"].ToString());
                    }
                    if (dt.Rows[n]["MaxShowPrice"].ToString() != "")
                    {
                        model.MaxShowPrice = decimal.Parse(dt.Rows[n]["MaxShowPrice"].ToString());
                    }
                    if (dt.Rows[n]["FreightTemplateId"].ToString() != "")
                    {
                        model.FreightTemplateId = int.Parse(dt.Rows[n]["FreightTemplateId"].ToString());
                    }
                    if (dt.Rows[n]["FirstCommission"].ToString() != "")
                    {
                        model.FirstCommission = decimal.Parse(dt.Rows[n]["FirstCommission"].ToString());
                    }
                    if (dt.Rows[n]["SecondCommission"].ToString() != "")
                    {
                        model.SecondCommission = decimal.Parse(dt.Rows[n]["SecondCommission"].ToString());
                    }
                    if (dt.Rows[n]["ThirdCommission"].ToString() != "")
                    {
                        model.ThirdCommission = decimal.Parse(dt.Rows[n]["ThirdCommission"].ToString());
                    }
                    if (dt.Rows[n]["IsSetCommission"].ToString() != "")
                    {
                        if ((dt.Rows[n]["IsSetCommission"].ToString() == "1") || (dt.Rows[n]["IsSetCommission"].ToString().ToLower() == "true"))
                        {
                            model.IsSetCommission = true;
                        }
                        else
                        {
                            model.IsSetCommission = false;
                        }
                    }
                    if (dt.Rows[n]["CubicMeter"].ToString() != "")
                    {
                        model.CubicMeter = decimal.Parse(dt.Rows[n]["CubicMeter"].ToString());
                    }
                    if (dt.Rows[n]["FreightWeight"].ToString() != "")
                    {
                        model.FreightWeight = decimal.Parse(dt.Rows[n]["FreightWeight"].ToString());
                    }
                    model.ProductShortName = dt.Rows[n]["ProductShortName"].ToString();
                    modelList.Add(model);
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
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}
        public List<Chenduo.Model.Hishop_Products> GetList(int PageSize, int PageIndex, string strWhere,string order,string ordertype, ref int PCnt, ref int RCnt)
        {
            DataSet ds = dal.GetList(PageSize, PageIndex, strWhere,order,ordertype, ref PCnt, ref RCnt);
            return DataTableToList(ds.Tables["ds"]);
        }
        #endregion  Method
    }
}
