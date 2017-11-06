<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotifierDemo.aspx.cs" Inherits="Hidistro.UI.Web.Admin.demo.NotifierDemo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="订单变更：通知统计更新" />
        <br />
        <br />
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="会员变更：通知统计更新" />
        <br />
        <br />
        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="产品变更：通知统计更新" />
    
    </div>
    </form>
</body>
</html>
