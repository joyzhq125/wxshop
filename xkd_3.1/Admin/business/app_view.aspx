<%@ Page Language="C#" AutoEventWireup="true" CodeFile="app_view.aspx.cs" Inherits="XKD.Web.Admin.app_view" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>编辑公众服务号</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../editor/kindeditor-min.js"></script>

    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
</head>

<body class="mainbody">
<form id="form1" runat="server"> 
<%--<!--导航栏-->
<div class="location"> 
  <a href="#" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <a href="#"><span>微信公众服务号</span></a>
  <i class="arrow"></i>
  <span>编辑内容</span>
</div>
<div class="line10"></div>
<!--/导航栏-->--%>

<!--内容-->
<div id="floatHead" class="content-tab-wrap">
  <div class="content-tab">
    <div class="content-tab-ul-wrap">
      <ul>  
        <li><a class="selected" href="javascript:;">微信公众号</a></li>
        <li><a href="javascript:;">支付方式</a></li>
      </ul>
    </div>
  </div>
</div>
<div class="tab-content">
  <dl>
    <dt>公众账号名称</dt>
    <dd>
      <asp:TextBox ID="txtappid_name" runat="server" CssClass="input normal" datatype="*2-100" sucmsg=" " nullmsg="请填写公众号名称"  />
      <span class="Validform_checktip">*标题最多100个字符</span>
    </dd>
  </dl> 
  <dl>
    <dt>公众号原始ID</dt>
    <dd>
      <asp:TextBox ID="txtappid_origin_id" runat="server" CssClass="input normal" datatype="*" sucmsg=" " nullmsg="请填写公众号原始ID" />
      <span class="Validform_checktip">*</span>
    </dd>
  </dl>
  <dl>
    <dt>微信号</dt>
    <dd>
      <asp:TextBox ID="txtweixin_account" runat="server" CssClass="input normal" datatype="*" sucmsg=" " nullmsg="请填写微信号" />
      <span class="Validform_checktip">*</span>
    </dd>
  </dl>
  <dl>
    <dt>头像</dt>
    <dd>
      <asp:TextBox ID="txtavatar" Text="" CssClass="input normal upload-path" runat="server"></asp:TextBox>
    </dd>
  </dl>
  <dl>
    <dt>接口地址URL</dt>
    <dd>
      <asp:TextBox ID="txtinterface_url" runat="server" CssClass="input normal" sucmsg=" " />
    </dd>
  </dl>
  <dl>
    <dt>TOKEN值</dt>
    <dd>
      <asp:TextBox ID="txttoken_value" runat="server" CssClass="input normal"  sucmsg=" " nullmsg="请填写TOKEN值"/>
      <span class="Validform_checktip">*与公众帐号官方网站上保持一致</span>
   </dd>
  </dl>
  <dl>
    <dt>EncodingAESKey</dt>
    <dd>
      <asp:TextBox ID="txtencodingaeskey" runat="server" CssClass="input normal" sucmsg=" " nullmsg="EncodingAESKey" />
      <span class="Validform_checktip">*与公众帐号官方网站上保持一致,若没有请别填写</span>
  </dd>
  </dl>
 <dl>
    <dd style="color: #16a0d3;">以下为高级功能配置</dd>
 </dl>
  <dl>
    <dt>AppId</dt>
    <dd>
      <asp:TextBox ID="txtappid" runat="server" CssClass="input normal" sucmsg=" " datatype="*"  nullmsg="请填写公众号原始ID" />
      <span class="Validform_checktip">*</span>
    </dd>
  </dl>
  <dl>
    <dt>AppSecret</dt>
    <dd>
      <asp:TextBox ID="txtappsecret" runat="server" CssClass="input normal" sucmsg=" " />
  </dd>
  </dl>
</div> 

<div class="tab-content" style="display:none;">
  <dl>
    <dt>支付名称</dt>
    <dd>
      <asp:TextBox ID="txtpayment_name" runat="server" CssClass="input normal" datatype="*" sucmsg=" " nullmsg="请填写支付名称" />
      <span class="Validform_checktip">*</span>
    </dd>
  </dl> 
  <dl>
    <dt>是否启用</dt>
    <dd>
      <div class="rule-single-checkbox">
          <asp:CheckBox ID="cbstate" runat="server" />
      </div>
      <span class="Validform_checktip">*不启用则不显示该支付方式</span>
    </dd>
  </dl>
  <dl>
    <dt>微信支付商户号</dt>
    <dd>
      <asp:TextBox ID="txtweixin_pay_account" runat="server" CssClass="input normal" sucmsg=" " />
      <span class="Validform_checktip">*微信支付商户号（接口文档中的商户号MCHID）</span>
    </dd>
  </dl>
  <dl>
    <dt>商户支付密钥Key</dt>
    <dd>
      <asp:TextBox ID="txtaccount_pay_key" runat="server" CssClass="input normal" sucmsg=" " />
      <span class="Validform_checktip">登录微信商户后台，进入栏目【账户设置】【密码安全】【API 安全】【API 密钥】</span>
    </dd>
  </dl>
  <dl>
    <dt>发货类型</dt>
    <dd>
       <div class="rule-multi-radio"> 
        <asp:RadioButtonList ID="rblsend_type" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">        
            <asp:ListItem Value="1" Selected="True" >后台管理员发货</asp:ListItem>
            <asp:ListItem Value="2">付款后系统立即发货</asp:ListItem>            
        </asp:RadioButtonList>
      </div>
      <span class="Validform_checktip">*一般来说，虚拟商品或者充值类需要立即发货，实体商品选择后台管理员点击发货</span>
    </dd>
  </dl> 
  <dl>
    <dt>显示图标</dt>
    <dd>
      <asp:TextBox ID="txtlogo" Text="" CssClass="input normal upload-path" runat="server"></asp:TextBox>
    </dd>
  </dl>
  <dl>
    <dt>描述说明</dt>
    <dd>
      <asp:TextBox ID="txtdescription" runat="server" TextMode="MultiLine" Height="100px" CssClass="input normal" sucmsg=" " />
        <span class="Validform_checktip">支付方式的描述说明，支持HTML代码</span>
    </dd>
  </dl>
</div> 
<!--/内容-->

<%--<!--工具栏-->
<div class="page-footer">
  <div class="btn-wrap">
    <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn"  onclick="btnSubmit_Click" />
    <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript:history.back(-1);" />
  </div>
</div>
<!--/工具栏-->--%>

</form>
</body>
</html>
