<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Hidistro.UI.Web.Admin.demo.test" %>
<%@ Register src="../Ascx/ucDateTimePicker.ascx" tagname="DateTimePicker" tagprefix="Hi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
    测试
     

        <Hi:DateTimePicker ID="ucDateTimePicker1" runat="server" CssClass="form-control " DateFormat="yyyy-MM-dd" PlaceHolder="kkdkd"/>
        <script type="text/javascript">
            <%=ucDateTimePicker1.ClientID%>_txtDateTimePicker_obj.on('changeDate', function (ev) {
                alert(ev.date)
    //if (ev.date.valueOf() < date-start-display.valueOf()){
        
    //}
});

        </script>
        <asp:Button ID="btnSave" runat="server" Text="测试" OnClick="btnSave_Click" />

        <br /><br />
        <asp:Label ID="lblTest" runat="server"></asp:Label>
    </form>
     

</asp:Content>
