namespace Hidistro.Entities.Weibo
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ArticleQuery : Pagination
    {
        public int ArticleType { get; set; }

        public string Memo { get; set; }

        public string Title { get; set; }

        public string wid { get; set; }
    }
}

