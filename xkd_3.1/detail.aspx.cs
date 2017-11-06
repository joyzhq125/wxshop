using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using MxWeiXinPF.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MxWeiXinPF.Web
{
    public partial class detail : TBasePage
    {
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            tPath = SFUtils.GetRootPath() + "/templates/common/detail/type1/news_show.html";
            TemplateMgr template = new TemplateMgr(tPath, this.wid);
            template.tType = TemplateType.News;
            //template.openid = MyCommFun.RequestOpenid();
            template.OutPutHtml("type1", this.wid);
        }
    }
}