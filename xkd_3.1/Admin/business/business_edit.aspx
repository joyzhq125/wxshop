<%@ Page Language="C#" AutoEventWireup="true" CodeFile="business_edit.aspx.cs" Inherits="XKD.Web.Admin.business_edit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="renderer" content="webkit">   
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>编辑子合同</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>

    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script type="text/javascript">
        $(function () {
            //初始化表单验证
            $("#form1").initValidform();
        });
    </script>
</head>

<body class="mainbody">
<form id="form1" runat="server">
<!--导航栏-->
<div class="location"> 
  <a href="#" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <a href="#"><span>商户基本信息</span></a>
  <i class="arrow"></i>
  <span>编辑内容</span>
</div>
<div class="line10"></div>
<!--/导航栏-->

<!--内容-->
<div id="floatHead" class="content-tab-wrap">
  <div class="content-tab">
    <div class="content-tab-ul-wrap">
      <ul>
        <li><a class="selected" href="javascript:;">商户基本信息</a></li> 
      </ul>
    </div>
  </div>
</div>
<div class="tab-content">
  <dl>
    <dt>商户名称</dt>
    <dd>
      <asp:TextBox ID="txtBusinessName" runat="server" CssClass="input normal" datatype="*2-100" sucmsg=" " />
      <span class="Validform_checktip">*标题最多100个字符</span>
    </dd>
  </dl> 
  <dl>
    <dt>联系电话</dt>
    <dd>
      <asp:TextBox ID="txtTelephone" runat="server" CssClass="input normal" datatype="m" sucmsg=" " />
      <span class="Validform_checktip">*请填写联系电话</span>
    </dd>
  </dl>
  <dl>
    <dt>E-Mail</dt>
    <dd>
      <asp:TextBox ID="txtEMail" runat="server" CssClass="input normal" datatype="e" sucmsg=" " />
      <span class="Validform_checktip">*请输入电子邮件地址</span>
    </dd>
  </dl>  
</div> 
<!--/内容-->

<!--工具栏-->
<div class="page-footer">
  <div class="btn-wrap">
    <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn" OnClick="btnSubmit_Click" />
    <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript:history.back(-1);" />
  </div>
</div>
<!--/工具栏-->

</form>
</body>
</html>
