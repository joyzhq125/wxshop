<%@ Page Language="C#" AutoEventWireup="true" CodeFile="app_list.aspx.cs" Inherits="XKD.Web.Admin.app_list" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>网站管理</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="renderer" content="webkit">    
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimum-scale=1.0, maximum-scale=1.0">
    <link rel="stylesheet" type="text/css" href="css/index.css" media="all">
    <script type="text/javascript">
        function addAppInfo() {
            location.href = 'app_edit.aspx?action=<%=Hidistro.Core.DTEnums.ActionEnum.Add %>';
        }

        function redirect(id) {
            parent.redirect(id);
        }
    </script>
</head>
<body>
    <div class="portals_container">
        <asp:Repeater ID="rptList" runat="server">
            <ItemTemplate>

                <div class="portal fl style_<%# Eval("index") %>"> 
                    <img class="fl ico" src="imgs/<%# Eval("index1") %>.png">
                    <div class="right fl">
<%--                   <span class="icon_<%# Eval("index1") %>"><%# Eval("appid_name") %></span>
                       <p class="weixinid">微信ID：<%# Eval("weixin_account") %></p>
                       <p class="gzhid">公众号原始ID：<%# Eval("appid_origin_id") %></p>--%>
                       <span class="icon_<%# Eval("index1") %>"><%# Eval("sitename") %></span>
                        <p class="weixinid">编号：<%# Eval("wid") %></p>
                    </div>
                    <div class="clearfl"></div> 
                    <div class="btn">
                        <div class="fr guanli"><img src="imgs/guanli.png"><strong><a href="app_list.aspx?action=setting&id=<%#Eval("id")%>">管理</a></strong></div>
                        <div class="fr xiugai"><img src="imgs/xiugai.png"><strong><a href="app_edit.aspx?action=<%=Hidistro.Core.DTEnums.ActionEnum.Edit %>&id=<%#Eval("id")%>">修改</a></strong></div>
                        <div class="clearfl"></div>
                    </div>
                    <div class="clearfl"></div>
                </div>

            </ItemTemplate>
         </asp:Repeater> 

        <div class="portal add" id="divAdd" onclick="javascript:addAppInfo();" runat="server">
            <a href="#"><div class="addimg"></div></a>
        </div>
    </div>
</body>
</html>
