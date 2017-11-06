using System;
using System.Collections;
using System.IO;
using System.Web;

public class Uuploader
{
    private string currentType;
    private string filename;
    private string originalName;
    private string state = "SUCCESS";
    private HttpPostedFile uploadFile;
    private string uploadpath;
    private string URL;

    private bool checkSize(int size)
    {
        return (this.uploadFile.ContentLength >= ((size * 0x400) * 0x400));
    }

    private bool checkType(string[] filetype)
    {
        this.currentType = this.getFileExt();
        return (Array.IndexOf<string>(filetype, this.currentType) == -1);
    }

    private void createFolder()
    {
        if (!Directory.Exists(this.uploadpath))
        {
            Directory.CreateDirectory(this.uploadpath);
        }
    }

    public void deleteFolder(string path)
    {
    }

    private string getFileExt()
    {
        string[] strArray = this.uploadFile.FileName.Split(new char[] { '.' });
        return ("." + strArray[strArray.Length - 1].ToLower());
    }

    public string getOtherInfo(HttpContext cxt, string field)
    {
        string str = null;
        if ((cxt.Request.Form[field] != null) && !string.IsNullOrEmpty(cxt.Request.Form[field]))
        {
            str = (field == "fileName") ? cxt.Request.Form[field].Split(new char[] { ',' })[1] : cxt.Request.Form[field];
        }
        return str;
    }

    private Hashtable getUploadInfo()
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add("state", this.state);
        hashtable.Add("url", this.URL);
        hashtable.Add("originalName", this.originalName);
        hashtable.Add("name", Path.GetFileName(this.URL));
        hashtable.Add("size", this.uploadFile.ContentLength);
        hashtable.Add("type", Path.GetExtension(this.originalName));
        return hashtable;
    }

    private string reName()
    {
        return (Guid.NewGuid() + this.getFileExt());
    }

    public Hashtable upFile(HttpContext cxt, string pathbase, string[] filetype, int size)
    {
        pathbase = pathbase + DateTime.Now.ToString("yyyy-MM-dd") + "/";
        this.uploadpath = cxt.Server.MapPath(pathbase);
        try
        {
            this.uploadFile = cxt.Request.Files[0];
            this.originalName = this.uploadFile.FileName;
            this.createFolder();
            if (this.checkType(filetype))
            {
                this.state = "不允许的文件类型";
            }
            if (this.checkSize(size))
            {
                this.state = "文件大小超出网站限制";
            }
            if (this.state == "SUCCESS")
            {
                this.filename = this.reName();
                this.uploadFile.SaveAs(this.uploadpath + this.filename);
                this.URL = pathbase + this.filename;
            }
        }
        catch (Exception)
        {
            this.state = "未知错误";
            this.URL = "";
        }
        return this.getUploadInfo();
    }

    public Hashtable upScrawl(HttpContext cxt, string pathbase, string tmppath, string base64Data)
    {
        pathbase = pathbase + DateTime.Now.ToString("yyyy-MM-dd") + "/";
        this.uploadpath = cxt.Server.MapPath(pathbase);
        FileStream stream = null;
        try
        {
            this.createFolder();
            this.filename = Guid.NewGuid() + ".png";
            stream = File.Create(this.uploadpath + this.filename);
            byte[] buffer = Convert.FromBase64String(base64Data);
            stream.Write(buffer, 0, buffer.Length);
            this.URL = pathbase + this.filename;
        }
        catch (Exception)
        {
            this.state = "未知错误";
            this.URL = "";
        }
        finally
        {
            stream.Close();
            this.deleteFolder(cxt.Server.MapPath(tmppath));
        }
        return this.getUploadInfo();
    }
}

