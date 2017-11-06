using System;
using System.Collections.Generic;
using System.Web;

namespace Senparc.Weixin.MP.TenPayLibV3
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}