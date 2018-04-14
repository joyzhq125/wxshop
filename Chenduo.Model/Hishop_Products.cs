using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chenduo.Model
{
	/// <summary>
	/// Hishop_Products:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
    public class Hishop_Products
    {
        public Hishop_Products()
        { }
        #region Model
        private int _productid;
        private string _wid;
        private int _categoryid;
        private int? _typeid;
        private string _productname;
        private string _productcode;
        private string _shortdescription;
        private string _unit;
        private string _description;
        private int _salestatus;
        private DateTime _addeddate;
        private int _visticounts = 0;
        private int _salecounts = 0;
        private int _showsalecounts = 0;
        private int _displaysequence = 0;
        private string _imageurl1;
        private string _imageurl2;
        private string _imageurl3;
        private string _imageurl4;
        private string _imageurl5;
        private string _thumbnailurl40;
        private string _thumbnailurl60;
        private string _thumbnailurl100;
        private string _thumbnailurl160;
        private string _thumbnailurl180;
        private string _thumbnailurl220;
        private string _thumbnailurl310;
        private string _thumbnailurl410;
        private decimal? _marketprice;
        private int? _brandid;
        private string _maincategorypath;
        private string _extendcategorypath;
        private bool _hassku;
        private bool _isfreeshipping;
        private long _taobaoproductid;
        private string _source;
        private decimal _minshowprice = 0;
        private decimal _maxshowprice = 0;
        private int _freighttemplateid = 0;
        private decimal _firstcommission = 0;
        private decimal _secondcommission = 0;
        private decimal _thirdcommission = 0;
        private bool _issetcommission = false;
        private decimal _cubicmeter = 0;
        private decimal _freightweight = 0;
        private string _productshortname;


        private decimal? _saleprice;
        private string _skuid;
        private int? _stock;
        private decimal? _weight;
        private int? _ismaketaobao;

        /// <summary>
        /// 
        /// </summary>
        public int ProductId
        {
            set { _productid = value; }
            get { return _productid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string wid
        {
            set { _wid = value; }
            get { return _wid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CategoryId
        {
            set { _categoryid = value; }
            get { return _categoryid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? TypeId
        {
            set { _typeid = value; }
            get { return _typeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductName
        {
            set { _productname = value; }
            get { return _productname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductCode
        {
            set { _productcode = value; }
            get { return _productcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ShortDescription
        {
            set { _shortdescription = value; }
            get { return _shortdescription; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Unit
        {
            set { _unit = value; }
            get { return _unit; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SaleStatus
        {
            set { _salestatus = value; }
            get { return _salestatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime AddedDate
        {
            set { _addeddate = value; }
            get { return _addeddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int VistiCounts
        {
            set { _visticounts = value; }
            get { return _visticounts; }
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal? SalePrice
        {
            set { _saleprice = value; }
            get { return _saleprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SkuId
        {
            set { _skuid = value; }
            get { return _skuid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Stock
        {
            set { _stock = value; }
            get { return _stock; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Weight
        {
            set { _weight = value; }
            get { return _weight; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? IsMakeTaobao
        {
            set { _ismaketaobao = value; }
            get { return _ismaketaobao; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SaleCounts
        {
            set { _salecounts = value; }
            get { return _salecounts; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ShowSaleCounts
        {
            set { _showsalecounts = value; }
            get { return _showsalecounts; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int DisplaySequence
        {
            set { _displaysequence = value; }
            get { return _displaysequence; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ImageUrl1
        {
            set { _imageurl1 = value; }
            get { return _imageurl1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ImageUrl2
        {
            set { _imageurl2 = value; }
            get { return _imageurl2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ImageUrl3
        {
            set { _imageurl3 = value; }
            get { return _imageurl3; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ImageUrl4
        {
            set { _imageurl4 = value; }
            get { return _imageurl4; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ImageUrl5
        {
            set { _imageurl5 = value; }
            get { return _imageurl5; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ThumbnailUrl40
        {
            set { _thumbnailurl40 = value; }
            get { return _thumbnailurl40; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ThumbnailUrl60
        {
            set { _thumbnailurl60 = value; }
            get { return _thumbnailurl60; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ThumbnailUrl100
        {
            set { _thumbnailurl100 = value; }
            get { return _thumbnailurl100; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ThumbnailUrl160
        {
            set { _thumbnailurl160 = value; }
            get { return _thumbnailurl160; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ThumbnailUrl180
        {
            set { _thumbnailurl180 = value; }
            get { return _thumbnailurl180; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ThumbnailUrl220
        {
            set { _thumbnailurl220 = value; }
            get { return _thumbnailurl220; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ThumbnailUrl310
        {
            set { _thumbnailurl310 = value; }
            get { return _thumbnailurl310; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ThumbnailUrl410
        {
            set { _thumbnailurl410 = value; }
            get { return _thumbnailurl410; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? MarketPrice
        {
            set { _marketprice = value; }
            get { return _marketprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? BrandId
        {
            set { _brandid = value; }
            get { return _brandid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MainCategoryPath
        {
            set { _maincategorypath = value; }
            get { return _maincategorypath; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExtendCategoryPath
        {
            set { _extendcategorypath = value; }
            get { return _extendcategorypath; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool HasSKU
        {
            set { _hassku = value; }
            get { return _hassku; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsfreeShipping
        {
            set { _isfreeshipping = value; }
            get { return _isfreeshipping; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long TaobaoProductId
        {
            set { _taobaoproductid = value; }
            get { return _taobaoproductid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Source
        {
            set { _source = value; }
            get { return _source; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal MinShowPrice
        {
            set { _minshowprice = value; }
            get { return _minshowprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal MaxShowPrice
        {
            set { _maxshowprice = value; }
            get { return _maxshowprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int FreightTemplateId
        {
            set { _freighttemplateid = value; }
            get { return _freighttemplateid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal FirstCommission
        {
            set { _firstcommission = value; }
            get { return _firstcommission; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal SecondCommission
        {
            set { _secondcommission = value; }
            get { return _secondcommission; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal ThirdCommission
        {
            set { _thirdcommission = value; }
            get { return _thirdcommission; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsSetCommission
        {
            set { _issetcommission = value; }
            get { return _issetcommission; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal CubicMeter
        {
            set { _cubicmeter = value; }
            get { return _cubicmeter; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal FreightWeight
        {
            set { _freightweight = value; }
            get { return _freightweight; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductShortName
        {
            set { _productshortname = value; }
            get { return _productshortname; }
        }
        #endregion Model

    }
}


