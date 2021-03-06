﻿namespace Hidistro.Entities
{
    using Hidistro.Entities.Sales;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Xml;

    public static class ExpressHelper
    {
        private static string path = HttpContext.Current.Request.MapPath("~/Express.xml");

        public static void AddExpress(string name, string kuaidi100Code, string taobaoCode)
        {
            XmlDocument xmlNode = GetXmlNode();
            XmlNode node = xmlNode.SelectSingleNode("companys");
            XmlElement newChild = xmlNode.CreateElement("company");
            newChild.SetAttribute("name", name);
            newChild.SetAttribute("Kuaidi100Code", kuaidi100Code);
            newChild.SetAttribute("TaobaoCode", taobaoCode);
            newChild.SetAttribute("New", "Y");
            node.AppendChild(newChild);
            xmlNode.Save(path);
        }

        public static void DeleteExpress(string name)
        {
            XmlDocument xmlNode = GetXmlNode();
            XmlNode node = xmlNode.SelectSingleNode("companys");
            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (node2.Attributes["name"].Value == name)
                {
                    node.RemoveChild(node2);
                    break;
                }
            }
            xmlNode.Save(path);
        }

        public static ExpressCompanyInfo FindNode(string company)
        {
            ExpressCompanyInfo info = null;
            XmlDocument xmlNode = GetXmlNode();
            string xpath = string.Format("//company[@name='{0}']", company);
            XmlNode node = xmlNode.SelectSingleNode(xpath);
            if (node != null)
            {
                info = new ExpressCompanyInfo {
                    Name = company,
                    Kuaidi100Code = node.Attributes["Kuaidi100Code"].Value,
                    TaobaoCode = node.Attributes["TaobaoCode"].Value
                };
            }
            return info;
        }

        public static ExpressCompanyInfo FindNodeByCode(string code)
        {
            ExpressCompanyInfo info = null;
            XmlDocument xmlNode = GetXmlNode();
            string xpath = string.Format("//company[@TaobaoCode='{0}']", code);
            XmlNode node = xmlNode.SelectSingleNode(xpath);
            if (node != null)
            {
                info = new ExpressCompanyInfo {
                    Name = node.Attributes["name"].Value,
                    Kuaidi100Code = node.Attributes["Kuaidi100Code"].Value,
                    TaobaoCode = code
                };
            }
            return info;
        }

        public static IList<ExpressCompanyInfo> GetAllExpress()
        {
            IList<ExpressCompanyInfo> list = new List<ExpressCompanyInfo>();
            foreach (XmlNode node2 in GetXmlNode().SelectSingleNode("companys").ChildNodes)
            {
                ExpressCompanyInfo item = new ExpressCompanyInfo {
                    Name = node2.Attributes["name"].Value,
                    Kuaidi100Code = node2.Attributes["Kuaidi100Code"].Value,
                    TaobaoCode = node2.Attributes["TaobaoCode"].Value
                };
                list.Add(item);
            }
            return list;
        }

        public static IList<string> GetAllExpressName()
        {
            IList<string> list = new List<string>();
            foreach (XmlNode node2 in GetXmlNode().SelectSingleNode("companys").ChildNodes)
            {
                list.Add(node2.Attributes["name"].Value);
            }
            return list;
        }

        public static string GetDataByKuaidi100(string computer, string expressNo)
        {
            HttpWebResponse response;
            string str = "29833628d495d7a5";
            XmlNode node = GetXmlNode().SelectSingleNode("companys");
            if (node != null)
            {
                str = node.Attributes["Kuaidi100NewKey"].Value;
            }
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(string.Format("http://kuaidi100.com/api?com={0}&nu={1}&show=2&id={2}", computer, expressNo, str));
            request.Timeout = 0x1f40;
            string str2 = "暂时没有此快递单号的信息";
            try
            {
                response = (HttpWebResponse) request.GetResponse();
            }
            catch
            {
                return str2;
            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                str2 = reader.ReadToEnd().Replace("&amp;", "").Replace("&nbsp;", "").Replace("&", "");
            }
            return str2;
        }

        public static string GetExpressData(string computer, string expressNo)
        {
            return GetDataByKuaidi100(computer, expressNo);
        }

        public static DataTable GetExpressTable()
        {
            DataTable table = new DataTable();
            XmlNode node = GetXmlNode().SelectSingleNode("companys");
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("Name");
            table.Columns.Add("Kuaidi100Code");
            table.Columns.Add("TaobaoCode");
            table.Columns.Add("New");
            int num = 0;
            foreach (XmlNode node2 in node.ChildNodes)
            {
                DataRow row = table.NewRow();
                row["id"] = num;
                row["Name"] = node2.Attributes["name"].Value;
                row["Kuaidi100Code"] = node2.Attributes["Kuaidi100Code"].Value;
                row["TaobaoCode"] = node2.Attributes["TaobaoCode"].Value;
                if (node2.Attributes["New"] != null)
                {
                    row["New"] = node2.Attributes["New"].Value;
                }
                else
                {
                    row["New"] = "N";
                }
                table.Rows.Add(row);
                num++;
            }
            return table;
        }

        private static XmlDocument GetXmlNode()
        {
            XmlDocument document = new XmlDocument();
            if (!string.IsNullOrEmpty(path))
            {
                document.Load(path);
            }
            return document;
        }

        public static bool IsExitExpress(string name)
        {
            foreach (XmlNode node2 in GetXmlNode().SelectSingleNode("companys").ChildNodes)
            {
                if (node2.Attributes["name"].Value == name)
                {
                    return true;
                }
            }
            return false;
        }

        public static void UpdateExpress(string oldcompanyname, string name, string kuaidi100Code, string taobaoCode)
        {
            XmlDocument xmlNode = GetXmlNode();
            foreach (XmlNode node2 in xmlNode.SelectSingleNode("companys").ChildNodes)
            {
                if (node2.Attributes["name"].Value == oldcompanyname)
                {
                    node2.Attributes["name"].Value = name;
                    node2.Attributes["Kuaidi100Code"].Value = kuaidi100Code;
                    node2.Attributes["TaobaoCode"].Value = taobaoCode;
                    break;
                }
            }
            xmlNode.Save(path);
        }
    }
}

