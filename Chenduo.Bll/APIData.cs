using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Chenduo.Bll
{
    public class ApiData
    {
        
        private readonly Chenduo.Dal.ApiData dal = new Chenduo.Dal.ApiData();
        public DataTable GetProductSkus(int ProductId)
        {
            return dal.GetProductSkus(ProductId);
        }

        public DataTable GetProductAttrsByTypeId(int typeId)
        {
            return dal.GetProductAttrsByTypeId(typeId);
        }

        public DataTable GetProductValuesByTypeId(int typeId)
        {
            return dal.GetProductValuesByTypeId(typeId);
        }

        public bool addImgHistory(int productId, string imgUrl)
        {
            return dal.addImgHistory(productId,imgUrl);
        }

        public bool delImgHistroy(int productId, string imgUrl)
        {
            return dal.delImgHistroy(productId, imgUrl);
        }

        public List<Chenduo.Model.imgHistory> GetImgHistory(int productId)
        {
            return dal.GetImgHistory(productId);
             
        }

    }
}
