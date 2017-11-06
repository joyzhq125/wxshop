<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ValidationService.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WeiXin.ValidationService" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function setEnable(obj) {
            var ob = $("#" + obj.id);
            var cls = ob.attr("class");
            var enable = 0;
            if (cls != "switch-btn") {
                enable = 1;
            }
            $.ajax({
                type: "post",
                url: "validationservice.aspx",
                data: { enable: enable, action: "setenable" },
                dataType: "json",
                success: function (json) {
                    if (json.type == 1) {
                        var opname = "已关闭";
                        if (cls == "switch-btn") {
                            ob.empty();
                            ob.append(opname + " <i></i>")
                            ob.removeClass();
                            ob.addClass("switch-btn off");
                        }
                        else {
                            opname = "已开启";
                            ob.empty();
                            ob.append(opname + " <i></i>")
                            ob.removeClass();
                            ob.addClass("switch-btn");
                        }

                        ShowMsg("会员公众号登录授权" + opname + '！', true);
                    }
                    else {
                        ShowMsg('操作失败！', false);
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>会员登录授权</h2>
    </div>
    <div class="set-switch">
        <p>开启以后，会员可在微信端通过微信授权快速登录系统。</p>
        <p style="color: red">(仅认证服务号可用）</p>
        <div id="guidepageEnable" class="<%=enableValidationService?"switch-btn":"switch-btn off" %>" onclick="setEnable(this)">
            <%=enableValidationService?"已开启":"已关闭"%>
            <i></i>
        </div>
    </div>
</asp:Content>
