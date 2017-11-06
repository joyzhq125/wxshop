<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Hidistro.UI.Web.umeditor.test" %>

<%@ Register src="controls/ucEditor.ascx" tagname="ucEditor" tagprefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div><uc1:ucEditor ID="ucEditor1" runat="server" Width="700" />
    </div>
        <asp:Label ID="lblTest" runat="server"></asp:Label>
        <asp:Button ID="btnSubmit" runat="server" Text="测试" OnClick="btnSubmit_Click" />
    </form>
    
</body>
</html>
