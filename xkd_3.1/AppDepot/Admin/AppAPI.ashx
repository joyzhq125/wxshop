<%@ WebHandler Language="C#" Class="AppAPI" %>

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using System.Web.SessionState;
using com.force.json;
using Hidistro.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class AppAPI : IHttpHandler ,IRequiresSessionState{
    protected string wid = string.Empty;
    public void ProcessRequest (HttpContext context) {
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
        //this.wid = Globals.GetCurrentWid();
        switch (context.Request["action"])
        {
            case "login":
                login(context);
                break;
            case "userAccResource":
                userAccResource(context);
                break;
            case "goodsListJSON":
                goodsListJSON(context);
                break;
            case "getGoodsSkuJsonById":
                getGoodsSkuJsonById(context);
                break;
            case "colorSelectListJSON":
                colorSelectListJSON(context);
                break;
            case "sizeSelectListJSON":
                sizeSelectListJSON(context);
                break;
            case "getAllBlandJSON":
                getAllBlandJSON(context);
                break;
            case "categoryListJSON":
                categoryListJSON(context);
                break;
            case "getAllUnitJSON":
                getAllUnitJSON(context);
                break;
            case "materialListJSON":
                materialListJSON(context);
                break;
            case "seasonListJSON":
                seasonListJSON(context);
                break;
            case "styleListJSON":
                styleListJSON(context);
                break;
            case "getAllClientItems":
                getAllClientItems(context);
                break;
            case "colorGroupListJSON":
                colorGroupListJSON(context);
                break;
            case "allStorehouseJSON":
                allStorehouseJSON(context);
                break;
            case "sizeAllGroupList":
                sizeAllGroupList(context);
                break;
            case "attrsofType":
                attrsofType(context);
                break;
            case "valuesofType":
                valuesofType(context);
                break;
            case "goodsImgs":
                goodsImgs(context);
                break;
            case "uploadAlbum":
                uploadAlbum(context);
                break;
            case "getUploadUrl":
                getUploadUrl(context);
                break;
            case "deleteRelate":
                deleteRelate(context);
                break;
            case "relateAlbum":
                relateAlbum(context);
                break;
            case "delHisImagine":
                delHisImagine(context);
                break;
            case "listImgHistory":
                listImgHistory(context);
                break;
            default:
                break;
        }
    }


    public void login(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        //context.Response.Write(builder.ToString());
        string userName = context.Request["userName"];
        string psw = context.Request["password"];

        //JSONObject j1 = new JSONObject();
        //j1.Put("num",20);

        //JSONObject j2 = new JSONObject();
        //j2.Put("name", "joy");
        //j2.Put("age", 35);

        //JSONArray ja = new JSONArray();
        //ja.Put(j2);

        //j1.Put("rows", ja);

        //LogHelper.Info(typeof(AppAPI), "testjson:"+userName +psw);
        //return j1.ToString();

        LogHelper.Info(typeof(AppAPI), "login:"+userName +"-"+psw);

        JSONObject j1 = new JSONObject();
        JSONObject j2 = new JSONObject();
        j1.Put("statusCode", 1);
        j1.Put("content","");

        j2.Put("firstLogin",1);
        j1.Put("object", j2);
        context.Response.Write(j1.ToString());
    }

    public void userAccResource(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        StringBuilder builder = new StringBuilder();
        builder.Append("[");
        builder.Append("null,\"goodsSaleReport\",\"wholesaleRejectOrder\",\"supplierFinanceMan\",\"hotOrUnsaleGoodsSaleVolumeView\",\"financeManChargeEdit\",\"wholesaleOrdersPreDelete\",\"purchaseOrderPurchasePrice\",\"purchaseOrderDestroy\",\"goodsManGoodsSaleVolume\",\"financeMan\",\"memberClientEdit\",\"storeTransferOrderDestroy\",\"commodityManView\",\"saleReportPurchasePrice\",\"changeBusinessTime\",\"supplier\",\"wholesaleOrderView\",\"wholesaleOrderCompare\",\"retailHis\",\"retailHisView\",\"wholesalePreOrderEdit\",\"supplierAdd\",\"goodsManExport\",\"storeOutOrderDestroy\",\"purchasePreOrderSaveDraft\",");
        builder.Append("\"tranfersReportExport\",\"system\",\"goodsManRetailPriceFactory\",\"rejectPurchaseSaveDraft\",\"purchaseVsSaleReportView\",\"goodsStoredPrint\",\"storeInOrderView\",\"userGuidView\",\"modifySaler\",\"companyConfigView\",\"retail\",\"financeManChargeDelete\",\"purchaseReportExport\",\"clientAdd\",\"goodsStored\",\"wholesaleOrdersPreSaveDraft\",\"materialEdit\",\"shopConfigEdit\",\"goodsMan\",\"storehouse\",\"wholesaleOrder\",\"wholesaleOrderEdit\",\"storeOrder\",\"shopOrderView\",\"storeOrderEdit\",\"storeTransferOrderDelete\",\"beginFinanceClientAdjustment\",\"purchaseOrderView\",\"seasonView\",\"purchaseOrderCompare\",");
        builder.Append("\"blandView\", \"wholesaleRejectOrdersPreDelete\", \"storeOrderDelete\",  \"storehouseSummaryView\", \"proxyAccountOrdersView\", \"rejectPurchaseOrderDelete\", \"material\",\"clientItemEdit\", \"supplierFinanceManView\", \"clientFinanceMan\", \"clientView\", \"goodsStoredView\",\"purchaseOrderSaveDraft\", \"suppliersManImport\", \"storeReportProfitAmount\", \"clientDelete\", \"shopInfoEdit\",\"clientFinanceManEmail\",\"storeMan\", \"goodsManWholesalePrice\", \"client\", \"fastReceiptMan\",\"clientsManExport\", \"storeRecord\", \"roleMan\", \"memberClientView\", \"totalSaleReportView\",\"approvalConfirmRejectPurchase\", \"financeManPrint\", \"chartMan\",");
        builder.Append("\"salePayRecordView\",\"saleMan\",\"goodsStoredWholesalePrice\",\"storeOrderView\",\"retailView\",\"weChatMan\",\"purchaseVsSaleReportSaleAmount\",\"storeInOrder\",\"supplierFinanceManExport\",\"wholesalePreOrder\",\"goodsStoredSet\",\"systemResetEdit\",\"size\",\"sale\",\"wholesaleRejectOrderView\",\"beginFinanceSupplierAdjustment\",\"clientItemView\",\"colorEdit\",\"commodityMan\",\"purchaseOrderPrint\",\"supplierView\",\"purchaseRejectOrderEdit\",\"commodityManEdit\",\"wholesalePreRejectOrderView\",\"clientFinanceManExport\",\"goodsStoredExport\",\"purchasePreOrderEdit\",\"systemReset\",\"purchaseReport\",\"purchase\",\"memberClient\",\"allAchievementView\",\"barcodeConfig\",\"saleReportGross\",\"style\",\"styleEdit\",\"salePayRecord\",\"barcodeConfigView\",\"storeInOrderApproval\",\"fastReceipt\",\"saleReportExport\",\"myInfoView\",\"wholesaleRejectOrderDestroy\",\"userManEdit\",\"shopInfoView\",\"tranfersReport\",\"unit\",\"color\",\"wholesaleOrdersSaveDraft\",\"shopInfo\",\"supplierFinanceManEdit\",\"goodsManView\",\"goodsStoredRetailPriceFactory\",\"storeTransferOrder\",\"goodsManPrint\",\"commodityManPurchasePrice\",\"financeManView\",\"companyConfig\",\"storeTransferOrderEdit\",\"clientSaleReportView\",\"categoryEdit\",");
        builder.Append("\"goodsManClientSaleVolume\",\"wholesalePreRejectOrder\",\"storeReport\",\"roleManView\",\"purchasePreOrderPrint\",\"totalSaleSummary\",\"report\",\"tranfersReportView\",\"shopConfigView\",\"clientSaleReport\",\"tranToWholesaleRejectOrder\",\"saleReportPurchaseAmount\",\"clientEdit\",\"roleManEdit\",\"accountItem\",\"userManView\",\"storeTransferOrderTradePrice\",\"accountItemView\",\"clientFinanceManPrint\",\"purchasePreOrderDelete\",\"saleReportGrossRate\",\"category\",\"retailHisEdit\",\"categoryView\",\"wholesaleOrderDelete\",\"proxyAccountCashApply\",\"saleReportView\",\"delivery\",\"wholesaleOrderDestroy\",\"purchaseMan\",\"purchasePreOrderTranToPurchase\",\"storehouseMan\",\"userGuid\",\"proxyAccountRecordView\",\"wholesaleRejectOrdersPreSaveDraft\",\"financeManExport\",\"supplierDelete\",\"storeOutOrder\",\"approvalConfirmWholesaleOrders\",\"customerPoint\",\"storeOrderConfirm\",\"goodsManPurchasePrice\",\"deliveryEdit\",\"clientFinanceManView\",\"wholesaleRejectOrderSaveDraft\",\"suppliersManExport\",\"purchasePreOrderEmail\",\"financeManCharge\",\"storeOutOrderView\",\"storehouseManEdit\",\"proxyAccountCash\",\"purchasePreOrderView\",\"wholesalePreOrderView\",\"storeTransferOrderPrint\",\"goodsStoredPurchasePrice\",\"beginFinanceManAdjustment\",\"wholesaleOrderPrint\",\"commodityManRetailPrice\",\"approvalConfirmPurchase\",\"blandEdit\",\"wholesaleRejectOrderEdit\",\"accountEdit\",\"rejectPurchaseOrderDestroy\",\"storeRecordView\",\"clientsManImport\",\"account\",\"finance\",\"shopOrder\",\"tranToRejectWholesaleOrder\",\"goodMan\",\"goodsSaleReportView\",\"bland\",\"commodityManRetailPriceFactory\",\"purchaseReportView\",\"shop\",\"storeTransferOrderConfirm\",\"purchasePreOrder\",\"sizeView\",\"storeInOrderDestroy\",\"purchaseRejectOrderPrint\",\"wholesaleRejectOrderPrint\",\"storeReportView\",\"bulletinView\",\"accountItemEdit\",\"goodsStoredRetailPrice\",\"totalSaleReport\",\"goodsManImport\",\"purchaseOrderEdit\",\"wholesalePreRejectOrderEdit\",\"materialView\",\"proxyAccountCashView\",\"achievement\",\"clientFinanceManReceipt\",\"shopOrderEdit\",\"userMan\",\"wholesaleOrderEmail\",\"storeTransferOrderRetailPrice\",\"printTemplateEdit\",\"approvalConfirmRejectOrders\",\"deliveryView\",\"accountView\",\"storeTransferOrderView\",\"season\",\"purchaseVsSaleReportPurchaseAmount\",\"storeOutOrderApproval\",\"bulletin\",\"storeOrderSaveDraft\",\"purchaseOrder\",\"operateDetailView\",\"unitEdit\",\"tranToWholesaleOrder\",\"other\",\"systemLog\",\"storeReportExport\",\"goods\",\"purchaseOrderDelete\",\"clientFinanceManEdit\",\"supplierFinanceManPay\",\"styleView\",\"hotOrUnsaleGoodsSaleVolume\",\"supplierEdit\",\"storeTransferOrderSaveDraft\",\"unitView\",\"supplierFinanceManPrint\",\"saleReport\",\"purchaseRejectOrderView\",\"storehouseManView\",\"systemLogView\",\"commodityManWholesalePrice\",\"myInfo\",\"sizeEdit\",\"clientItem\",\"shopConfig\",\"clientMan\",\"bulletinEdit\",\"purchaseRejectOrder\",\"colorView\",\"wholesaleRejectOrderDelete\",\"achievementView\",\"seasonEdit\",\"goodsManEdit\",\"goodsManRetailPrice\",\"printTemplate\",\"purchaseVsSaleReport\"");
        builder.Append("]");
        context.Response.Write(builder.ToString());
    }


    public void goodsListJSON(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        int PCnt = 0, RCnt = 0;
        int PageSize = 10;
        string order = "ProductId";
        string ordertype = "asc";
        wid = "WB64X20170623162227";
        string strWhere = "wid='"+wid+"'";
        int PageIndex = SFUtils.ObjToInt(context.Request["page"]);
        int op = SFUtils.ObjToInt(context.Request["operator"]);
        string sidx = SFUtils.ObjectToStr(context.Request["sidx"]);
        string sord = SFUtils.ObjectToStr(context.Request["sord"]);
        string categoryIdsStr = SFUtils.ObjectToStr(context.Request["categoryIdsStr"]);
        string blandId = SFUtils.ObjectToStr(context.Request["blandId"]);
        string isOnSale = SFUtils.ObjectToStr(context.Request["isOnSale"]);

        //LogHelper.Info(typeof(AppAPI), "goodsListJSON:" + PageIndex + "-" + op);

        Chenduo.Bll.Hishop_Products bll = new Chenduo.Bll.Hishop_Products();

        List<Chenduo.Model.Hishop_Products> list = bll.GetList(PageSize,PageIndex,strWhere,order,ordertype,ref PCnt,ref RCnt);

        JSONObject j1 = new JSONObject();
        j1.Put("page", PageIndex);
        j1.Put("pageSize", PageSize);
        j1.Put("records",RCnt);
        j1.Put("total",PCnt);
        j1.Put("summary",null);
        //json字符串转json对象
        JSONArray rows = new JSONArray(JsonUtils.ListToJson<Chenduo.Model.Hishop_Products>(list));
        j1.Put("rows", rows);
        context.Response.Write(j1.ToString());

    }


    public void getGoodsSkuJsonById(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        int productId = SFUtils.ObjToInt(context.Request["id"]);


        //LogHelper.Info(typeof(AppAPI), "getGoodsSkuJsonById:id="+productId);


        Chenduo.Model.Hishop_Products product = new Chenduo.Bll.Hishop_Products().GetModel(productId);
        //LogHelper.Info(typeof(AppAPI), "getGoodsSkuJsonById:stock="+product.Stock);
        DataTable skus = new Chenduo.Bll.ApiData().GetProductSkus(productId);

        JSONObject j1 = new JSONObject();
        JSONObject goodsVO = new JSONObject(JsonConvert.SerializeObject(product));
        JSONArray goodsSkuList = new JSONArray(JsonConvert.SerializeObject(skus));
        j1.Put("goodsVO",goodsVO);
        j1.Put("goodsSkuList", goodsSkuList);
        context.Response.Write(j1.ToString());

    }
    public void colorSelectListJSON(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        //JSONObject j1 = new JSONObject();
        context.Response.ContentType = "application/json";
        StringBuilder builder = new StringBuilder();
        //builder.Append(
        //"{\"totalPage\":1,\"currentPage\":1,\"optionList\":[
        //{\"val\":\"BK\",\"id\":297,\"text\":\"黑色\",\"updateTime\":1412686133000,\"status\":1,\"name\":\"黑色\",\"colorGroupId\":81,\"showOrder\":100,\"colorValue\":\"BK\",\"optimisticLockVersion\":0},");
        //{ "val":"BL","id":298,"text":"蓝色","updateTime":1412686133000,"status":1,"name":"蓝色","colorGroupId":81,"showOrder":100,"colorValue":"BL","optimisticLockVersion":0},
        //{ "val":"GN","id":299,"text":"绿色","updateTime":1412686133000,"status":1,"name":"绿色","colorGroupId":81,"showOrder":100,"colorValue":"GN","optimisticLockVersion":0},
        //{ "val":"GY","id":300,"text":"灰色","updateTime":1412686133000,"status":1,"name":"灰色","colorGroupId":81,"showOrder":100,"colorValue":"GY","optimisticLockVersion":0},
        //{ "val":"OG","id":301,"text":"橙色","updateTime":1412686133000,"status":1,"name":"橙色","colorGroupId":81,"showOrder":100,"colorValue":"OG","optimisticLockVersion":0},
        //{ "val":"PP","id":302,"text":"紫色","updateTime":1412686133000,"status":1,"name":"紫色","colorGroupId":81,"showOrder":100,"colorValue":"PP","optimisticLockVersion":0},
        //{ "val":"YL","id":304,"text":"黄色","updateTime":1412686134000,"status":1,"name":"黄色","colorGroupId":81,"showOrder":100,"colorValue":"YL","optimisticLockVersion":0},
        //{ "val":"BR","id":305,"text":"棕色","updateTime":1412686134000,"status":1,"name":"棕色","colorGroupId":81,"showOrder":100,"colorValue":"BR","optimisticLockVersion":0},
        //{ "val":"BK","id":307,"text":"粉红","updateTime":1412686134000,"status":1,"colorGroupName":"暖色组","name":"粉红","colorGroupId":82,"showOrder":100,"colorValue":"BK","optimisticLockVersion":0},
        //{ "val":"BL","id":308,"text":"桔红","updateTime":1412686134000,"status":1,"colorGroupName":"暖色组","name":"桔红","colorGroupId":82,"showOrder":100,"colorValue":"BL","optimisticLockVersion":0},
        //{ "val":"GN","id":309,"text":"浅橙","updateTime":1412686134000,"status":1,"colorGroupName":"暖色组","name":"浅橙","colorGroupId":82,"showOrder":100,"colorValue":"GN","optimisticLockVersion":0},
        //{ "val":"GY","id":310,"text":"米黄","updateTime":1412686134000,"status":1,"colorGroupName":"暖色组","name":"米黄","colorGroupId":82,"showOrder":100,"colorValue":"GY","optimisticLockVersion":0},
        //{ "val":"OG","id":311,"text":"土黄","updateTime":1412686134000,"status":1,"colorGroupName":"暖色组","name":"土黄","colorGroupId":82,"showOrder":100,"colorValue":"OG","optimisticLockVersion":0},
        //{ "id":357,"text":"白色","updateTime":1413035767000,"status":1,"name":"白色","showOrder":100,"optimisticLockVersion":0},
        //{ "id":358,"text":"红色","updateTime":1413035829000,"status":1,"name":"红色","showOrder":100,"optimisticLockVersion":0},
        //{ "id":360,"text":"中咖927","updateTime":1413624117000,"status":1,"colorGroupName":"袜色组","name":"中咖927","colorGroupId":93,"showOrder":100,"optimisticLockVersion":0},
        //{ "id":361,"text":"浅灰923","updateTime":1413624123000,"status":1,"colorGroupName":"袜色组","name":"浅灰923","colorGroupId":93,"showOrder":100,"optimisticLockVersion":0},
        //{ "id":362,"text":"深灰922","updateTime":1413624128000,"status":1,"colorGroupName":"袜色组","name":"深灰922","colorGroupId":93,"showOrder":100,"optimisticLockVersion":0},
        //{ "id":363,"text":"浅咖918","updateTime":1413624131000,"status":1,"colorGroupName":"袜色组","name":"浅咖918","colorGroupId":93,"showOrder":100,"optimisticLockVersion":0},
        //{ "id":780,"text":"浅花色","updateTime":1422691825000,"status":1,"name":"浅花色","showOrder":100,"optimisticLockVersion":0},
        //{ "id":781,"text":"藏青色","updateTime":1422756311000,"status":1,"name":"藏青色","showOrder":100,"optimisticLockVersion":0}]}
        //");
        context.Response.Write(builder.ToString());
    }


    public void sizeSelectListJSON(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        JSONObject j1 = new JSONObject();
        context.Response.Write(j1.ToString());
    }

    public void getAllBlandJSON(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        JSONObject j1 = new JSONObject();
        context.Response.Write(j1.ToString());
    }

    public void categoryListJSON(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        JSONObject j1 = new JSONObject();
        context.Response.Write(j1.ToString());
    }

    public void getAllUnitJSON(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        JSONObject j1 = new JSONObject();
        context.Response.Write(j1.ToString());
    }

    public void materialListJSON(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        JSONObject j1 = new JSONObject();
        context.Response.Write(j1.ToString());
    }
    public void seasonListJSON(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        JSONObject j1 = new JSONObject();
        context.Response.Write(j1.ToString());
    }
    public void styleListJSON(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        JSONObject j1 = new JSONObject();
        context.Response.Write(j1.ToString());
    }

    public void getAllClientItems(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        JSONObject j1 = new JSONObject();
        context.Response.Write(j1.ToString());
    }
    public void colorGroupListJSON(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        StringBuilder builder = new StringBuilder();
        builder.Append("[{\"optimisticLockVersion\":0,\"name\":\"暖色组\",\"updateTime\":1412686133000,\"id\":82},{\"optimisticLockVersion\":0,\"name\":\"袜色组\",\"updateTime\":1413624112000,\"id\":93}]");
        //JSONObject j1 = new JSONObject();
        context.Response.Write(builder.ToString());
    }

    public void allStorehouseJSON(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        //JSONObject j1 = new JSONObject();
        StringBuilder builder = new StringBuilder();
        builder.Append("[");
        builder.Append("{\"val\":52,\"administratorId\":131,\"createTime\":1413676800000,\"name\":\"四楼仓库\",\"storeName\":\"公共\",\"remark\":\"\",\"updateTime\":1413650167000,\"text\":\"四楼仓库\",\"id\":52,\"storeId\":0,\"status\":\"1\"},{\"val\":51,\"administratorId\":131,\"createTime\":1413650147000,\"name\":\"八楼仓库\",\"storeName\":\"公共\",\"updateTime\":1413650147000,\"text\":\"八楼仓库\",\"id\":51,\"storeId\":0,\"status\":\"1\"}");
        builder.Append("]");

        context.Response.Write(builder.ToString());
    }
    public void sizeAllGroupList(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        StringBuilder builder = new StringBuilder();
        builder.Append("[{\"id\":1211,\"updateTime\":1514320349000,\"name\":\"鞋\",\"optimisticLockVersion\":0}]");
        context.Response.Write(builder.ToString());
    }

    public void attrsofType(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        int typeId = SFUtils.ObjToInt(context.Request["tid"]);
        DataTable attrs = new Chenduo.Bll.ApiData().GetProductAttrsByTypeId(typeId);
        JSONObject j1 = new JSONObject();
        j1.Put("optionList", new JSONArray(JsonConvert.SerializeObject(attrs)));
        context.Response.Write(j1.ToString());

    }
    public void valuesofType(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        int typeId = SFUtils.ObjToInt(context.Request["tid"]);
        DataTable values = new Chenduo.Bll.ApiData().GetProductValuesByTypeId(typeId);
        JSONObject j1 = new JSONObject();
        j1.Put("optionList", new JSONArray(JsonConvert.SerializeObject(values)));
        context.Response.Write(j1.ToString());
    }

    public void goodsImgs(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        int productId = SFUtils.ObjToInt(context.Request["goodsId"]);
        StringBuilder sb = new StringBuilder("[");
        Chenduo.Model.Hishop_Products product = new Chenduo.Bll.Hishop_Products().GetModel(productId);
        //for (int i = 1; i <= 5; i++)
        //{
        if (product.ImageUrl1 != "")
        {
            sb.Append("{");
            sb.AppendFormat("\"goodsId\":{0},", productId);
            //sb.AppendFormat("\"imgId\":{0},", 1);
            sb.AppendFormat("\"url\":\"{0}\"", product.ImageUrl1);
            sb.Append("}");
        }
        if (product.ImageUrl2 != "")
        {
            if (sb.Length > 1)
            {
                sb.Append(",");
            }
            sb.Append("{");
            sb.AppendFormat("\"goodsId\":{0},", productId);
            //sb.AppendFormat("\"imgId\":{0},", 2);
            sb.AppendFormat("\"url\":\"{0}\"", product.ImageUrl2);
            sb.Append("}");
        }
        if (product.ImageUrl3 != "")
        {
            if (sb.Length > 1)
            {
                sb.Append(",");
            }
            sb.Append("{");
            sb.AppendFormat("\"goodsId\":{0},", productId);
            //sb.AppendFormat("\"imgId\":{0},", 3);
            sb.AppendFormat("\"url\":\"{0}\"", product.ImageUrl3);
            sb.Append("}");
        }
        if (product.ImageUrl4 != "")
        {
            if (sb.Length > 1)
            {
                sb.Append(",");
            }
            sb.Append("{");
            sb.AppendFormat("\"goodsId\":{0},", productId);
            //sb.AppendFormat("\"imgId\":{0},", 4);
            sb.AppendFormat("\"url\":\"{0}\"", product.ImageUrl4);
            sb.Append("}");
        }
        if (product.ImageUrl5 != "")
        {
            if (sb.Length > 1)
            {
                sb.Append(",");
            }
            sb.Append("{");
            sb.AppendFormat("\"goodsId\":{0},", productId);
            //sb.AppendFormat("\"imgId\":{0},", 5);
            sb.AppendFormat("\"url\":\"{0}\"", product.ImageUrl5);
            sb.Append("}");
        }
        sb.Append("]");
        //LogHelper.Info(/*this.getType*/typeof(AppAPI), "goodsImgs:" + sb.ToString());
        JSONArray ja = new JSONArray();
        JSONObject jo= new JSONObject();
        jo.Put("page", 1);
        jo.Put("pageSize",10);
        jo.Put("records",1);
        jo.Put("total",1);
        jo.Put("rows",new JSONArray(sb.ToString()));
        context.Response.Write(jo.ToString());
    }


    public void uploadAlbum(HttpContext context)
    {
        //上传文件
        HttpPostedFile file = context.Request.Files["file"];
        int productId = SFUtils.ObjToInt(context.Request["x:goodid"]);
        //LogHelper.Info(typeof(AppAPI), "productId:" + productId);
        if (file == null)
        {
            JSONObject jo = new JSONObject();
            jo.Put("statusCode",0);
            context.Response.Write(jo.ToString());
        }
        else
        {
            string fileName = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "." + file.FileName.Trim().ToLower();
            fileName = fileName.Replace("?", "");
            string fileDstPath = "/Storage/master/product/images/" + fileName;
            //int imgId = 0;

            //string str1 = context.Server.MapPath(fileDstPath);
            //LogHelper.Info(typeof(AppAPI), "Server.mappth:" + str1);
            //string fileFullDstPath = context.Request.MapPath(fileDstPath);       
            //LogHelper.Info(typeof(AppAPI), "Request.mappth:" + fileFullDstPath);

            Chenduo.Bll.Hishop_Products productsBll = new Chenduo.Bll.Hishop_Products();
            Chenduo.Model.Hishop_Products product = productsBll.GetModel(productId);

            if (product.ImageUrl1 == "")
            {
                product.ImageUrl1 = fileDstPath;
                //imgId = 1;
            }
            else if (product.ImageUrl2 == "")
            {
                product.ImageUrl2 = fileDstPath;
                //imgId = 2;
            }
            else if (product.ImageUrl3 == "")
            {
                product.ImageUrl3 = fileDstPath;
                //imgId = 3;
            }
            else if (product.ImageUrl4 == "")
            {
                product.ImageUrl4 = fileDstPath;
                //imgId = 4;
            }
            else if (product.ImageUrl5 == "")
            {
                product.ImageUrl5 = fileDstPath;
                //imgId = 5;
            }
            if(productsBll.Update(product) && new Chenduo.Bll.ApiData().addImgHistory(productId,fileDstPath))
            {
                file.SaveAs(context.Request.MapPath(fileDstPath));

                string url = SFUtils.getWebSite() + fileDstPath;
                StringBuilder sb = new StringBuilder("{");
                //sb.Append("\"name\":\"\",");
                sb.AppendFormat("\"url\":\"{0}\",",url);
                //sb.AppendFormat("\"id\":\"{0}\",",imgId);
                //sb.AppendFormat("\"goodsAlbumId\":\"{0}\"",imgId);
                sb.Append("}");
                JSONObject jo = new JSONObject();
                jo.Put("statusCode",1);
                jo.Put("object",new JSONObject(sb.ToString()));
                context.Response.Write(jo.ToString());
            }
            else
            {
                JSONObject jo = new JSONObject();
                jo.Put("statusCode",0);
                context.Response.Write(jo.ToString());
            }
        }
    }

    public void getUploadUrl(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        StringBuilder sb = new StringBuilder("{");
        sb.Append("\"policy\":\"\",\"accessid\":\"\",\"callback\":\"\",\"signature\":\"\",\"storeName\":\"\",");
        sb.Append("\"host\":\"/Admin/AppAPI.ashx?action=uploadAlbum\"");
        sb.Append("}");
        JSONObject jo = new JSONObject();
        jo.Put("statusCode", 1);
        jo.Put("object", new JSONObject(sb.ToString()));
        context.Response.Write(jo.ToString());
    }

    /// <summary>
    /// 保留历史图片，不删除图片
    /// </summary>
    /// <param name="context"></param>

    public void deleteRelate(HttpContext context)
    {
        string originalPath = "";
        context.Response.ContentType = "application/json";
        int productId = SFUtils.ObjToInt(context.Request["goodsId"]);
        string imgurl = SFUtils.ObjectToStr(context.Request["url"]);

        LogHelper.Info(typeof(AppAPI), "deleteRelate:productId " + productId);
        LogHelper.Info(typeof(AppAPI), "deleteRelate:imgurl " + imgurl);

        Chenduo.Bll.Hishop_Products productsBll = new Chenduo.Bll.Hishop_Products();
        Chenduo.Model.Hishop_Products product = productsBll.GetModel(productId);
        if (product != null)
        {
            if (product.ImageUrl1.Equals(imgurl))
            {
                product.ImageUrl1 = "";
            }
            else if (product.ImageUrl2.Equals(imgurl))
            {
                product.ImageUrl2 = "";
            }
            else if (product.ImageUrl3.Equals(imgurl))
            {
                product.ImageUrl3 = "";
            }
            else if (product.ImageUrl4.Equals(imgurl))
            {
                product.ImageUrl4 = "";
            }
            else if (product.ImageUrl5.Equals(imgurl))
            {
                product.ImageUrl5 = "";
            }

            if (productsBll.Update(product))
            {
                JSONObject jo = new JSONObject();
                jo.Put("statusCode", 1);
                context.Response.Write(jo.ToString());
            }
        }
        else
        {
            JSONObject jo = new JSONObject();
            jo.Put("statusCode", 0);
            context.Response.Write(jo.ToString());

        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void delHisImagine(HttpContext context)
    {
        //string originalPath = "";
        context.Response.ContentType = "application/json";
        int productId = SFUtils.ObjToInt(context.Request["goodsId"]);
        string imgurl = SFUtils.ObjectToStr(context.Request["url"]);

        LogHelper.Info(typeof(AppAPI), "deleteRelate:productId " + productId);
        LogHelper.Info(typeof(AppAPI), "deleteRelate:imgurl " + imgurl);

        Chenduo.Bll.Hishop_Products productsBll = new Chenduo.Bll.Hishop_Products();
        Chenduo.Model.Hishop_Products product = productsBll.GetModel(productId);
        if (product != null)
        {
            if (product.ImageUrl1.Equals(imgurl))
            {
                product.ImageUrl1 = "";
            }
            else if (product.ImageUrl2.Equals(imgurl))
            {
                product.ImageUrl2 = "";
            }
            else if (product.ImageUrl3.Equals(imgurl))
            {
                product.ImageUrl3 = "";
            }
            else if (product.ImageUrl4.Equals(imgurl))
            {
                product.ImageUrl4 = "";
            }
            else if (product.ImageUrl5.Equals(imgurl))
            {
                product.ImageUrl5 = "";
            }

            productsBll.Update(product);
        }
        //删除history表
        if (new Chenduo.Bll.ApiData().delImgHistroy(productId, imgurl.Replace(SFUtils.getWebSite(), "")))
        {
            //删除图片 
            string imagePath = context.Request.MapPath(imgurl.Replace(SFUtils.getWebSite(), ""));
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
        JSONObject jo = new JSONObject();
        jo.Put("statusCode", 1);
        context.Response.Write(jo.ToString());
    }

    /// <summary>
    /// 判断url文件是否存在，如存在则保存起来，不存在返回错误
    /// </summary>
    /// <param name="context"></param>
    public void relateAlbum(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        int productId = SFUtils.ObjToInt(context.Request["goodsId"]);
        string url = SFUtils.ObjectToStr(context.Request["url"]);
        string orgPath = url.Replace(SFUtils.getWebSite(), "");
        string orgFullPath = context.Request.MapPath(url.Replace(SFUtils.getWebSite(), ""));
        if (File.Exists(orgFullPath))
        {
            Chenduo.Bll.Hishop_Products productsBll = new Chenduo.Bll.Hishop_Products();
            Chenduo.Model.Hishop_Products product = productsBll.GetModel(productId);
            if (product != null)
            {

                if (product.ImageUrl1.Equals(""))
                {
                    product.ImageUrl1 = orgPath;
                }
                else if (product.ImageUrl2.Equals(""))
                {
                    product.ImageUrl2 = orgPath;
                }
                else if (product.ImageUrl3.Equals(""))
                {
                    product.ImageUrl3 = orgPath;
                }
                else if (product.ImageUrl4.Equals(""))
                {
                    product.ImageUrl4 = orgPath;
                }
                else if (product.ImageUrl5.Equals(""))
                {
                    product.ImageUrl5 = orgPath;
                }
                if (productsBll.Update(product))
                {
                    JSONObject jo = new JSONObject();
                    jo.Put("statusCode", 1);
                    context.Response.Write(jo.ToString());
                }
            }
            else
            {
                JSONObject jo = new JSONObject();
                jo.Put("statusCode", 0);
                context.Response.Write(jo.ToString());

            }
        }
    }

    public void listImgHistory(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        int productId = SFUtils.ObjToInt(context.Request["goodsId"]);
        List<Chenduo.Model.imgHistory> values = new Chenduo.Bll.ApiData().GetImgHistory(productId);

        JSONObject jo = new JSONObject();
        jo.Put("page",1);
        jo.Put("total", 1);
        jo.Put("rows", new JSONArray(JsonConvert.SerializeObject(values)));
        context.Response.Write(jo.ToString());
    }



    public bool IsReusable {
        get {
            return false;
        }
    }

}