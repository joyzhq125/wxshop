namespace Hidistro.SqlDal.Weibo
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Weibo;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class ArticleDao
    {
        private string articledetailUrl = ("http://" + Globals.DomainName + Globals.ApplicationPath + "/vshop/ArticleDetail.aspx?$1");
        private Database database = DatabaseFactory.CreateDatabase();
        //StringBuilder builder;
        public int AddMultiArticle(ArticleInfo article)
        {
            int num;
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO vshop_Article(").Append("Title,ArticleType,LinkType,Content,ImageUrl,Url,Memo,PubTime,wid)").Append(" VALUES (@Title,@ArticleType,@LinkType,@Content,@ImageUrl,@Url,@Memo,@PubTime,@wid);select @@identity");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, article.Title);
            this.database.AddInParameter(sqlStringCommand, "ArticleType", DbType.Int32, article.ArticleType);
            this.database.AddInParameter(sqlStringCommand, "LinkType", DbType.Int32, article.LinkType);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, article.Content);
            this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, article.ImageUrl);
            this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, article.Url);
            this.database.AddInParameter(sqlStringCommand, "Memo", DbType.String, article.Memo);
            this.database.AddInParameter(sqlStringCommand, "PubTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, article.wid);
            if (int.TryParse(this.database.ExecuteScalar(sqlStringCommand).ToString(), out num))
            {
                if (article.LinkType == LinkType.ArticleDetail)
                {
                    string str = "update vshop_Article set Url=@Url where ArticleId=@ArticleId";
                    sqlStringCommand = this.database.GetSqlStringCommand(str);
                    article.ArticleId = num;
                    article.Url = this.articledetailUrl.Replace("$1", "sid=" + article.ArticleId.ToString());
                    this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, article.Url);
                    this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, article.ArticleId);
                    this.database.ExecuteNonQuery(sqlStringCommand);
                }
                foreach (ArticleItemsInfo info in article.ItemsInfo)
                {
                    builder = new StringBuilder();
                    builder.Append("insert into vshop_ArticleItems(");
                    builder.Append("ArticleId,Title,Content,ImageUrl,Url,LinkType)");
                    builder.Append(" values (");
                    builder.Append("@ArticleId,@Title,@Content,@ImageUrl,@Url,@LinkType);select @@identity");
                    sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
                    this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, num);
                    this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, info.Title);
                    this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, info.Content);
                    this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, info.ImageUrl);
                    this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, info.Url);
                    this.database.AddInParameter(sqlStringCommand, "LinkType", DbType.Int32, info.LinkType);
                    int num2 = int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
                    if (info.LinkType == LinkType.ArticleDetail)
                    {
                        string str2 = "update vshop_ArticleItems set Url=@Url where Id=@Id";
                        sqlStringCommand = this.database.GetSqlStringCommand(str2);
                        info.Id = num2;
                        info.Url = this.articledetailUrl.Replace("$1", "iid=" + info.Id.ToString());
                        this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, info.Url);
                        this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, info.Id);
                        this.database.ExecuteNonQuery(sqlStringCommand);
                    }
                }
            }
            return num;
        }

        public int AddSingerArticle(ArticleInfo article)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO vshop_Article(").Append("Title,ArticleType,LinkType,Content,ImageUrl,Url,Memo,PubTime,wid)").Append(" VALUES (@Title,@ArticleType,@LinkType,@Content,@ImageUrl,@Url,@Memo,@PubTime,@wid);select @@identity");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, article.Title);
            this.database.AddInParameter(sqlStringCommand, "ArticleType", DbType.Int32, article.ArticleType);
            this.database.AddInParameter(sqlStringCommand, "LinkType", DbType.Int32, article.LinkType);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, article.Content);
            this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, article.ImageUrl);
            this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, article.Url);
            this.database.AddInParameter(sqlStringCommand, "Memo", DbType.String, article.Memo);
            this.database.AddInParameter(sqlStringCommand, "PubTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, article.wid);
            int num = int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
            if (article.LinkType == LinkType.ArticleDetail)
            {
                string str = "update vshop_Article set Url=@Url where ArticleId=@ArticleId";
                sqlStringCommand = this.database.GetSqlStringCommand(str);
                article.ArticleId = num;
                article.Url = this.articledetailUrl.Replace("$1", "sid=" + article.ArticleId.ToString());
                this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, article.Url);
                this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, article.ArticleId);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
            return num;
        }

        public bool DeleteArticle(int articleId)
        {
            string str = "DELETE FROM vshop_Article WHERE ArticleId=@ArticleId;DELETE FROM vshop_ArticleItems WHERE ArticleId=@ArticleId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articleId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public ArticleInfo GetArticleInfo(int articleid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ArticleId,Title,ArticleType,LinkType,Content,ImageUrl,Url,Memo,PubTime,MediaId,wid from vshop_Article ");
            builder.Append(" where ArticleId=@ArticleId ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articleid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return this.ReaderBind(reader);
            }
        }

        public IList<ArticleItemsInfo> GetArticleItems(int articleid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select Id,ArticleId,Title,Content,ImageUrl,Url,LinkType,MediaId from vshop_ArticleItems ");
            builder.Append(" where ArticleId=@ArticleId order by ID asc ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articleid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<ArticleItemsInfo>(reader);
            }
        }

        public ArticleItemsInfo GetArticleItemsInfo(int itemid)
        {
            StringBuilder builder = new StringBuilder();
            ArticleItemsInfo info = new ArticleItemsInfo();
            builder.Append("select Id,ArticleId,Title,Content,ImageUrl,Url,LinkType,PubTime,mediaid from vshop_ArticleItems ");
            builder.Append(" where Id=@Id order by ID asc ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, itemid);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            info.Id = int.Parse(table.Rows[0]["ID"].ToString());
            info.Title = table.Rows[0]["Title"].ToString();
            info.LinkType = (LinkType) int.Parse(table.Rows[0]["LinkType"].ToString());
            info.ArticleId = int.Parse(table.Rows[0]["ArticleId"].ToString());
            info.ImageUrl = table.Rows[0]["ImageUrl"].ToString();
            info.Url = table.Rows[0]["Url"].ToString();
            info.Content = table.Rows[0]["Content"].ToString();
            info.PubTime = DateTime.Parse(table.Rows[0]["PubTime"].ToString());
            if (table.Rows[0]["MediaId"] != null)
            {
                info.MediaId = table.Rows[0]["MediaId"].ToString();
                return info;
            }
            info.MediaId = "";
            return info;
        }

        public DbQueryResult GetArticleRequest(ArticleQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.ArticleType > 0)
            {
                builder.AppendFormat(" ArticleType = {0} ", query.ArticleType);
            }
            if(!string.IsNullOrEmpty(query.wid))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" WID = '{0}' ", query.wid);
            }
            if (!string.IsNullOrEmpty(query.Title))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("( Title LIKE '%{0}%' or Memo like '%{0}%' or exists (select 1 from  vshop_ArticleItems where title like '%{0}%' and ArticleId=vshop_Article.ArticleId))", DataHelper.CleanSearchString(query.Title));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vshop_Article ", "ArticleId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public ArticleInfo ReaderBind(IDataReader dataReader)
        {
            ArticleInfo info = null;
            if (dataReader.Read())
            {
                info = new ArticleInfo();
                object obj2 = dataReader["ArticleId"];
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    info.ArticleId = (int) obj2;
                }
                info.Title = dataReader["Title"].ToString();
                obj2 = dataReader["ArticleType"];
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    info.ArticleType = (ArticleType) obj2;
                }
                obj2 = dataReader["LinkType"];
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    info.LinkType = (LinkType) obj2;
                }
                info.Content = dataReader["Content"].ToString();
                info.ImageUrl = dataReader["ImageUrl"].ToString();
                info.Url = dataReader["Url"].ToString();
                info.Memo = dataReader["Memo"].ToString();
                obj2 = dataReader["PubTime"];
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    info.PubTime = (DateTime) obj2;
                }
                if (info.ArticleType == ArticleType.List)
                {
                    info.ItemsInfo = this.GetArticleItems(info.ArticleId);
                }
                obj2 = dataReader["MediaId"];
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    info.MediaId = obj2.ToString();
                    return info;
                }
                info.MediaId = "";
                info.wid = dataReader["wid"].ToString();
            }
            return info;
        }

        public void UpdateArticleItem(ArticleItemsInfo iteminfo)
        {
            string str = string.Empty;
            if (iteminfo.Id > 0)
            {
                str = "update vshop_ArticleItems set Title=@Title,Content=@Content,ImageUrl=@ImageUrl,Url=@Url,LinkType=@LinkType,PubTime=@PubTime where Id=@Id";
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
                this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, iteminfo.Id);
                this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, iteminfo.Title);
                this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, iteminfo.Content);
                this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, iteminfo.ImageUrl);
                this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, iteminfo.Url);
                this.database.AddInParameter(sqlStringCommand, "LinkType", DbType.Int32, iteminfo.LinkType);
                this.database.AddInParameter(sqlStringCommand, "PubTime", DbType.DateTime, iteminfo.PubTime);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("insert into vshop_ArticleItems(");
                builder.Append("ArticleId,Title,Content,ImageUrl,Url,LinkType,PubTime)");
                builder.Append(" values (");
                builder.Append("@ArticleId,@Title,@Content,@ImageUrl,@Url,@LinkType,@PubTime);select @@identity");
                DbCommand command2 = this.database.GetSqlStringCommand(builder.ToString());
                this.database.AddInParameter(command2, "ArticleId", DbType.Int32, iteminfo.ArticleId);
                this.database.AddInParameter(command2, "Title", DbType.String, iteminfo.Title);
                this.database.AddInParameter(command2, "Content", DbType.String, iteminfo.Content);
                this.database.AddInParameter(command2, "ImageUrl", DbType.String, iteminfo.ImageUrl);
                this.database.AddInParameter(command2, "Url", DbType.String, iteminfo.Url);
                this.database.AddInParameter(command2, "LinkType", DbType.Int32, iteminfo.LinkType);
                this.database.AddInParameter(command2, "PubTime", DbType.DateTime, iteminfo.PubTime);
                int num = int.Parse(this.database.ExecuteScalar(command2).ToString());
                if (iteminfo.LinkType == LinkType.ArticleDetail)
                {
                    string str2 = "update vshop_ArticleItems set Url=@Url where Id=@Id";
                    command2 = this.database.GetSqlStringCommand(str2);
                    iteminfo.Id = num;
                    iteminfo.Url = this.articledetailUrl.Replace("$1", "iid=" + iteminfo.Id.ToString());
                    this.database.AddInParameter(command2, "Url", DbType.String, iteminfo.Url);
                    this.database.AddInParameter(command2, "Id", DbType.Int32, iteminfo.Id);
                    this.database.ExecuteNonQuery(command2);
                }
            }
        }

        public bool UpdateMedia_Id(int type, int id, string mediaid)
        {
            StringBuilder builder = new StringBuilder();
            if (type == 0)
            {
                builder.Append("UPDATE vshop_Article SET mediaid=@mediaid WHERE ArticleId=@ID");
            }
            else
            {
                builder.Append("UPDATE vshop_ArticleItems SET mediaid=@mediaid WHERE Id=@ID");
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, id);
            this.database.AddInParameter(sqlStringCommand, "mediaid", DbType.String, mediaid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateMultiArticle(ArticleInfo article)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE vshop_Article SET ").Append("Title=@Title,").Append("ArticleType=@ArticleType,").Append("LinkType=@LinkType,").Append("Content=@Content,").Append("ImageUrl=@ImageUrl,").Append("Url=@Url,").Append("Memo=@Memo,").Append("PubTime=@PubTime,").Append("wid=@wid").Append(" WHERE ArticleId=@ArticleId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, article.Title);
            this.database.AddInParameter(sqlStringCommand, "ArticleType", DbType.Int32, article.ArticleType);
            this.database.AddInParameter(sqlStringCommand, "LinkType", DbType.Int32, article.LinkType);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, article.Content);
            this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, article.ImageUrl);
            this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, article.Url);
            this.database.AddInParameter(sqlStringCommand, "Memo", DbType.String, article.Memo);
            this.database.AddInParameter(sqlStringCommand, "PubTime", DbType.DateTime, article.PubTime);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, article.wid);
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, article.ArticleId);
            bool flag = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
            if (flag)
            {
                foreach (ArticleItemsInfo info in article.ItemsInfo)
                {
                    info.ArticleId = article.ArticleId;
                    if (info.LinkType == LinkType.ArticleDetail)
                    {
                        info.Url = this.articledetailUrl.Replace("$1", "iid=" + info.Id.ToString());
                    }
                    this.UpdateArticleItem(info);
                }
                string str = "delete from vshop_ArticleItems WHERE ArticleId=@ArticleId and PubTime<>@PubTime";
                if (article.LinkType == LinkType.ArticleDetail)
                {
                    str = str + ";update vshop_Article set Url=@Url where ArticleId=@ArticleId";
                }
                sqlStringCommand = this.database.GetSqlStringCommand(str);
                article.Url = this.articledetailUrl.Replace("$1", "sid=" + article.ArticleId.ToString());
                this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, article.Url);
                this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, article.ArticleId);
                this.database.AddInParameter(sqlStringCommand, "PubTime", DbType.DateTime, article.PubTime);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
            return flag;
        }

        public bool UpdateSingleArticle(ArticleInfo article)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE vshop_Article SET ").Append("Title=@Title,").Append("ArticleType=@ArticleType,").Append("LinkType=@LinkType,").Append("Content=@Content,").Append("ImageUrl=@ImageUrl,").Append("Url=@Url,").Append("Memo=@Memo,").Append("PubTime=@PubTime,").Append("wid=@wid").Append(" WHERE ArticleId=@ArticleId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            if (article.LinkType == LinkType.ArticleDetail)
            {
                article.Url = this.articledetailUrl.Replace("$1", "sid=" + article.ArticleId.ToString());
            }
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, article.Title);
            this.database.AddInParameter(sqlStringCommand, "ArticleType", DbType.Int32, article.ArticleType);
            this.database.AddInParameter(sqlStringCommand, "LinkType", DbType.Int32, article.LinkType);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, article.Content);
            this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, article.ImageUrl);
            this.database.AddInParameter(sqlStringCommand, "Url", DbType.String, article.Url);
            this.database.AddInParameter(sqlStringCommand, "Memo", DbType.String, article.Memo);
            this.database.AddInParameter(sqlStringCommand, "PubTime", DbType.DateTime, article.PubTime);
            this.database.AddInParameter(sqlStringCommand, "wid", DbType.String, article.wid);
            this.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, article.ArticleId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

