using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Chenduo;
namespace Chenduo.BLL
{
    public class customer
    {
        private readonly DAL.customer dal = new DAL.customer();
        //private readonly DAL.sf_user_info udal = new DAL.sf_user_info();
        //private readonly DAL.sf_goods_order odal = new DAL.sf_goods_order();

        private string businessNum;
        private string appNum;
        private string tempNum;

        public customer(string _businessNum ,string _appNum , string _tempNum) 
        {
            businessNum = _businessNum;
            appNum = _appNum;
            tempNum = _tempNum;
        }


        private string GetWhere()
        {
            string where = " busnieseNum = '"+this.businessNum+"' and appNum = '"+this.appNum+"' and templatesNum = '"+this.tempNum+"'";
            return where ;
        }


        #region cur_alltop

        /// <summary>
        /// 获取排行榜集合
        /// </summary>
        /// <returns></returns>
        /*
        public List<Model.sf_user_info> getTopList()
        {
            List<Model.sf_user_info> uList = new List<Model.sf_user_info>();

            string strwhere = GetWhere();
            DataTable dt = udal.GetList(strwhere).Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                Model.sf_user_info model = udal.GetModel(Convert.ToInt64(row["id"]));
                decimal money = 0;
                int count = 0;
                string str = "";
                
                DataTable odt = getLevelOrders(row["inviteKey"].ToString(), 4);
                foreach (DataRow r in odt.Rows)
                {
                    money += Convert.ToDecimal(r["totalMoney"]);
                    if (str.IndexOf(r["userNum"].ToString()) < 0)
                    {
                        str += r["userNum"];
                        count++;
                    }
                }

                model.province = money.ToString();
                model.openID = count.ToString();

                if (uList.Count < 1)
                {
                    uList.Add(model);
                }
                else
                {
                    for (int i = 0; i < uList.Count; i++)
                    {
                        if (Convert.ToDecimal(model.province) > Convert.ToDecimal(uList[i].province))
                        {
                            uList.Insert(i, model);
                            break;
                        }
                        else
                        {
                            uList.Add(model);
                            break;
                        }
                    }
                }
            }
            return uList;
        }
        */


        /// <summary>
        /// 获取 参数 用户 的客户订单
        /// </summary>
        /// <param name="inviteKey">邀请码</param>
        /// <param name="level">客户级数</param>
        /// <returns></returns>
        /*
        public DataTable getLevelOrders(string inviteKey, int level)
        {
            DataTable ds = dal.GetList("1!=1").Tables[0];
            DataTable dt = new DataTable();
            string strWhere = GetWhere();
            //判断级别 获取 参数级别的 客户
            if (level == 1)
            {
                dt = udal.GetList(strWhere + " and fatherKey='" + inviteKey + "' ").Tables[0];
            }
            else if (level == 2)
            {
                dt = getTwoUsers(inviteKey);
            }
            else if (level == 3)
            {
                dt = getThreeUsers(inviteKey);
            }
            else if (level == 4)
            {
                dt = udal.GetList(strWhere + " and fatherKey='" + inviteKey + "' ").Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    DataTable dd = odal.GetList("userNum='" + row["userNum"] + "' and ispay = 2 ").Tables[0];
                    foreach (DataRow r in dd.Rows)
                    {
                        DataRow ro = ds.NewRow();
                        ro.ItemArray = r.ItemArray;
                        ds.Rows.Add(ro);
                    }
                }
                dt = getTwoUsers(inviteKey);
                foreach (DataRow row in dt.Rows)
                {
                    DataTable dd = odal.GetList("userNum='" + row["userNum"] + "' and  ispay = 2 ").Tables[0];
                    foreach (DataRow r in dd.Rows)
                    {
                        DataRow ro = ds.NewRow();
                        ro.ItemArray = r.ItemArray;
                        ds.Rows.Add(ro);
                    }
                }
                dt = getThreeUsers(inviteKey);

            }
            foreach (DataRow row in dt.Rows)
            {
                DataTable dd = odal.GetList("userNum='" + row["userNum"] + "' and  ispay = 2 ").Tables[0];
                foreach (DataRow r in dd.Rows)
                {
                    DataRow ro = ds.NewRow();
                    ro.ItemArray = r.ItemArray;
                    ds.Rows.Add(ro);
                }
            }
            return ds;
        }

        /// <summary>
        /// 获取 第二级 客户
        /// </summary>
        /// <param name="inviteKey">邀请码</param>
        public DataTable getTwoUsers(string inviteKey)
        {
            string strWhere = GetWhere();
            DataTable dt = udal.GetList(strWhere + " and fatherKey='" + inviteKey + "' ").Tables[0];
            DataTable ds = udal.GetList("1!=1").Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                DataTable ts = udal.GetList(strWhere + " and fatherKey='" + row["inviteKey"] + "' ").Tables[0];
                foreach (DataRow r in ts.Rows)
                {
                    DataRow ro = ds.NewRow();
                    ro.ItemArray = r.ItemArray;
                    ds.Rows.Add(ro);
                }
            }
            return ds;
        }

        /// <summary>
        /// 获取 第三级 客户
        /// </summary>
        /// <param name="inviteKey">邀请码</param>
        public DataTable getThreeUsers(string inviteKey)
        {
            string strWhere = GetWhere();
            DataTable ds = udal.GetList("1!=1").Tables[0];
            DataTable dt = getTwoUsers(inviteKey);
            foreach (DataRow row in dt.Rows)
            {
                DataTable ts = udal.GetList(strWhere + " and fatherKey='" + row["inviteKey"] + "' ").Tables[0];
                foreach (DataRow r in ts.Rows)
                {
                    DataRow ro = ds.NewRow();
                    ro.ItemArray = r.ItemArray;
                    ds.Rows.Add(ro);
                }
            }
            return ds;
        }

        #endregion

        #region cur_top


        /// <summary>
        /// 客户榜单
        /// 获得查询分页数据
        /// city  上次排名，province 用哪张图片：0-透明，1-上升，2-下降
        /// </summary>
        /// <param name="pageSize">每页数量</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="strWhere">条件</param>
        /// <param name="filedOrder">排序方式</param>
        /// <param name="recordCount">返回数据量</param>
        /// <param name="recordCount">榜单类型（累积排行-0、进步排行-1）</param>
        /// <param name="recordCount">时间类型（属：进步排行，分：月-0、周-1）</param>
        /// <returns></returns>
        public DataSet GetList(int pageSize, int pageIndex,string filterWhere, out int recordCount, int topType, int timeType)
        {
            string timeWhere = " and " +GetWhere();
            string strWhere = GetWhere();

            switch (topType)
            {
                //累积排行
                case 0:
                    DataSet ds = dal.GetList(pageSize, pageIndex, strWhere,filterWhere, out recordCount, timeWhere);
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        item["moneys"] = string.IsNullOrEmpty(item["moneys"].ToString()) ? "0.00" : item["moneys"];
                        //item["role_type"] = 0;
                        item["province"] = 0;
                        //item["role_id"] = item["topNum"];
                        item["city"] = item["row_number"];
                        //item["wechatNum"] = "";
                        item["openid"] = "";
                    }
                    return ds;
                //进步排行
                case 1:
                    DateTime nowDate = DateTime.Now.Date;
                    DateTime betweenDate, oldDate;
                    string oneStr = "", twoStr = "";
                    //月榜
                    if (timeType == 0)
                    {
                        oneStr = "此用户在本月中排名";
                        betweenDate = nowDate.AddMonths(-1);
                        oldDate = nowDate.AddMonths(-2);
                    }
                    //周榜
                    else
                    {
                        oneStr = "此用户在本周中排名";
                        betweenDate = nowDate.AddDays(-7);
                        oldDate = nowDate.AddDays(-14);
                    }

                    //当前排名
                    timeWhere = "and b.payTime>'" + betweenDate + "' and "+GetWhere();
                    DataSet nowdt = dal.GetList(pageSize, pageIndex, strWhere, filterWhere, out recordCount, timeWhere);

                    //上次排名
                    timeWhere = "and b.payTime>'" + oldDate + "' and b.payTime<'" + betweenDate.AddDays(1) + "' and "+GetWhere();

                    //去除所有数据
                    pageSize = 100000;
                    pageIndex = 0;
                    DataSet olddt = dal.GetList(pageSize, pageIndex, strWhere, filterWhere, out recordCount, timeWhere);
                    foreach (DataRow item in nowdt.Tables[0].Rows)
                    {
                        foreach (DataRow old in olddt.Tables[0].Rows)
                        {
                            if (item["userNum"].Equals(old["userNum"]))
                            {
                                item["city"] = old["row_number"];
                                break;
                            }
                        }

                        item["province"] = 0;
                        item["moneys"] = string.IsNullOrEmpty(item["moneys"].ToString()) ? "0.00" : item["moneys"];
                        item["province"] = Convert.ToInt32(item["row_number"]) > Convert.ToInt32(item["city"]) ? "2" : Convert.ToInt32(item["row_number"]) < Convert.ToInt32(item["city"]) ? "1" : "0";
                        twoStr = item["province"].ToString() == "2" ? "下降" : "上升";
                        item["openid"] = oneStr + " " + twoStr + " " + Math.Abs(Convert.ToInt32(item["row_number"]) - Convert.ToInt32(item["city"]));
                    }
                    return nowdt;
                default:
                    break;
            }
            return dal.GetList(pageSize, pageIndex, strWhere, filterWhere, out recordCount, timeWhere);

        }
        */
        #endregion
    }
}
