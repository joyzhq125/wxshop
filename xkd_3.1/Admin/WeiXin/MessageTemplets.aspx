<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="MessageTemplets.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WeiXin.MessageTemplets" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script>
    $(function () {
        $('[MessageType="会员注册时"]').hide();
    });
</script>	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="page-header">
            <h2>消息模板设置</h2>
            <small>通过接入模板消息接口，公众号可向关注其帐号的用户发送预设模板的消息，以便向用户及时发送重要的服务通知。</small>
        </div>
        <div class="titileBorderBox">
            <h3><strong>管理员订单提醒设置</strong></h3>
            <div class="contentBox">
                <div class="form-inline">
                    <div class="form-group">
                        <label>微信OpenID：<asp:TextBox ID="txtManageOpenID" CssClass="form-control inputw300" runat="server"/></label>
                    </div><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn btn-success inputw100" OnClick="btnSave_Click" />
                </div>
                <small class="mt10">该配置用于指定接收订单提醒的微信账户，openid号可以在 <a href="../member/managemembers.aspx" title="会员列表中有会员的详细信息，包括会员关注微信公众号后自动获取到的openID，暂时会员列表页还没有，先链接到现有的会员列表页做参考">会员列表</a> 中指定的会员一栏中查看并复制</small>
                

            </div>
            <h3><strong>
                <img src="../images/wechat.jpg" /> 微信模板消息设置</strong> <a href="WeixinSettings.html" target="_blank">帮助指引</a></h3>
            <div class="contentBox">
                <asp:Repeater ID="rptList" runat="server" OnItemDataBound="rptList_ItemDataBound">
                    <HeaderTemplate>
                        <table class="table table-bordered table-hover">
                            <thead>
                                <tr>
                                    <th>消息类型</th>
                                    <th>微信模板ID</th>
                                    <th>是否开启</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%#Eval("Name") %><asp:HiddenField ID="hdfMessageType" runat="server" />
                            </td>
                            <td><%#Eval("WeixinTemplateId") %></td>
                            <td><label><asp:CheckBox runat="server" ID="chkWeixinMessage" MessageType='<%# Eval("Name")%>' Checked='<%# Eval("SendWeixin")%>'/> 开启</label></td>
                            <td><span class="submit_bianji" messagetype='<%# Eval("Name")%>'><a href='<%# "EditTemplateId.aspx?MessageType="+Eval("MessageType")%>'><%#Eval("WeixinTemplateId").ToString()==""?"添加":"编辑" %></a></span></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
	</table>
                    </FooterTemplate>

                </asp:Repeater>
                <div style="text-align: center;">
                    <asp:Button ID="btnSaveSendSetting" runat="server" Text="保存设置" CssClass="btn btn-success" /></div>
            </div>
        </div>
    </form>
</asp:Content>
