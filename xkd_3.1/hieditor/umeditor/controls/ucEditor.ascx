<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEditor.ascx.cs" Inherits="Hidistro.UI.Web.umeditor.controls.ucEditor" %>
<asp:TextBox ID="txtMemo" runat="server" TextMode="MultiLine" ></asp:TextBox>
<%if(IsFirstEdit){ %>
    <link href="/hieditor/umeditor/themes/default/css/umeditor.css" type="text/css" rel="stylesheet">
    <script type="text/javascript" charset="utf-8" src="/hieditor/umeditor/umeditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="/hieditor/umeditor/umeditor.min.js"></script>
    <script type="text/javascript" src="/hieditor/umeditor/lang/zh-cn/zh-cn.js"></script><%} %>
<script type="text/javascript">
    //实例化编辑器
    var um = UM.getEditor('<%=txtMemo.ClientID%>');
    
    </script>