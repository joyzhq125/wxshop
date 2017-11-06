namespace Hidistro.UI.Web.API
{
    using System;
    using System.IO;

    public class TLog
    {
        public static string LogFile = ("JobLog" + DateTime.Now.ToString("yyyyMM") + ".log");

        public static void SaveLog(string cdmstring)
        {
            try
            {
                FileInfo info = new FileInfo(LogFile);
                using (FileStream stream = info.OpenWrite())
                {
                    StreamWriter writer = new StreamWriter(stream);
                    writer.BaseStream.Seek(0L, SeekOrigin.End);
                    writer.WriteLine("=====================================");
                    writer.Write("时间:" + DateTime.Now.ToString() + "\r\n");
                    writer.Write("内容:" + cdmstring + "\r\n");
                    writer.WriteLine("=====================================");
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception exception)
            {
                exception.ToString();
            }
        }
    }
}

