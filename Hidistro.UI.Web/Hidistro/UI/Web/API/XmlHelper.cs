namespace Hidistro.UI.Web.API
{
    using System;
    using System.Xml;

    public class XmlHelper
    {
        private static XmlDocument doc = new XmlDocument();

        public static string GetNodeValue(string XmlFile, string NodeName)
        {
            if ((doc == null) || (doc.DocumentElement == null))
            {
                LoadXml(XmlFile);
            }
            XmlNodeList elementsByTagName = doc.GetElementsByTagName("Job");
            if (elementsByTagName.Count > 0)
            {
                foreach (XmlElement element in elementsByTagName[0])
                {
                    if (element.Name == NodeName)
                    {
                        return element.InnerText;
                    }
                }
            }
            return "";
        }

        private static bool LoadXml(string XmlFile)
        {
            try
            {
                doc.Load(XmlFile);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

