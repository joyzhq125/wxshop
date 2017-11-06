namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Web;
    using System.Linq;
    using System.Web.SessionState;
    public class VsiteHandler : IHttpHandler, IRequiresSessionState
    {
        protected string wid;
        public void ProcessRequest(HttpContext context)
        {
            wid = context.Session[DTKeys.SESSION_WEB_ID] as string;
            if (string.IsNullOrEmpty(wid))
            {
                return;
            }
            string str = context.Request.Form["actionName"];
            string s = string.Empty;
            string str4 = str;
            if (str4 != null)
            {
                if (!(str4 == "Topic"))
                {
                    if (str4 == "Vote")
                    {
                        s = JsonConvert.SerializeObject(StoreHelper.GetVoteList());
                    }
                    else if (str4 == "Category")
                    {
                        s = JsonConvert.SerializeObject(from item in CatalogHelper.GetMainCategories(this.wid) select new { CateId = item.CategoryId, CateName = item.Name });
                    }
                    else if (str4 == "Activity")
                    {
                        Array values = Enum.GetValues(typeof(LotteryActivityType));
                        List<EnumJson> list3 = new List<EnumJson>();
                        foreach (Enum enum2 in values)
                        {
                            EnumJson json = new EnumJson {
                                Name = enum2.ToShowText(),
                                Value = enum2.ToString()
                            };
                            list3.Add(json);
                        }
                        s = JsonConvert.SerializeObject(list3);
                    }
                    else if (str4 == "ActivityList")
                    {
                        string str3 = context.Request.Form["acttype"];
                        LotteryActivityType type = (LotteryActivityType) Enum.Parse(typeof(LotteryActivityType), str3);
                        if (type == LotteryActivityType.SignUp)
                        {
                            s = JsonConvert.SerializeObject(from item in VShopHelper.GetAllActivity() select new { ActivityId = item.ActivityId, ActivityName = item.Name });
                        }
                        else
                        {
                            s = JsonConvert.SerializeObject(VShopHelper.GetLotteryActivityByType(type));
                        }
                    }
                    else if (str4 == "AccountTime")
                    {
                        s = s + "{";
                        BalanceDrawRequestQuery entity = new BalanceDrawRequestQuery {
                            RequestTime = "",
                            CheckTime = "",
                            StoreName = "",
                            PageIndex = 1,
                            PageSize = 1,
                            SortOrder = SortAction.Desc,
                            SortBy = "RequestTime",
                            RequestEndTime = "",
                            RequestStartTime = "",
                            IsCheck = "1",
                            UserId = context.Request.Form["UserID"]
                        };
                        Globals.EntityCoding(entity, true);
                        DataTable data = (DataTable) DistributorsBrower.GetBalanceDrawRequest(entity, null).Data;
                        if (data.Rows.Count > 0)
                        {
                            if (data.Rows[0]["MerchantCode"].ToString().Trim() != context.Request.Form["merchantcode"].Trim())
                            {
                                s = s + "\"Time\":\"" + data.Rows[0]["RequestTime"].ToString() + "\"";
                            }
                            else
                            {
                                s = s + "\"Time\":\"\"";
                            }
                        }
                        else
                        {
                            s = s + "\"Time\":\"\"";
                        }
                        s = s + "}";
                    }
                }
                else
                {
                    s = JsonConvert.SerializeObject(VShopHelper.Gettopics());
                }
            }
            context.Response.Write(s);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private class EnumJson
        {
            public string Name { get; set; }

            public string Value { get; set; }
        }
    }
}

