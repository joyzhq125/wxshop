using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chenduo.Model
{
    [Serializable]
    public class imgHistory
    {
        public imgHistory()
        { }
        #region Model
        private int _goodsid;
        private string _url;
        private DateTime? _addeddate;
        /// <summary>
        /// 
        /// </summary>
        public int goodsid
        {
            set { _goodsid = value; }
            get { return _goodsid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string url
        {
            set { _url = value; }
            get { return _url; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AddedDate
        {
            set { _addeddate = value; }
            get { return _addeddate; }
        }
        #endregion Model

    }
}
