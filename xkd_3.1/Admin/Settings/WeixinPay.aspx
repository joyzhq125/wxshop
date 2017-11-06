<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master"
    CodeBehind="WeixinPay.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Settings.WeixinPay" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />
    <style type="text/css">
        .lineCss{background-color:green;height:2px;width:auto;}
        .msgDiv{background-color:#D3FBFE; border:1px solid; border-color:#0c9e0e; height:50px; margin-top:10px;margin-bottom:10px;}
    </style>
    <script type="text/javascript">
        function setEnable(obj) {
            var type = "-5";
            var ob = $("#" + obj.id);
            var cls = ob.attr("class");
            var enable = "false";
            if (cls == "switch-btn") {

                ob.empty();
                ob.append("已关闭 <i></i>")
                ob.removeClass();
                ob.addClass("switch-btn off");
                enable = "false";

            }
            else {
                ob.empty();
                ob.append("已开启 <i></i>")
                ob.removeClass();
                ob.addClass("switch-btn");
                enable = "true";
            }
            $.ajax({
                type: "post",
                url: "PayConfigHandler.ashx",
                data: { type: type, enable: enable },
                dataType: "text",
                success: function (data) {
                    if (enable == 'true') {
                        msg('微信支付已开启！');
                        $('#maindiv').css('display', '');
                    }
                    else {
                        msg('微信支付已关闭！');
                        $('#maindiv').css('display', 'none');
                    }
                }
            });
        }

        function msg(msg) {
            HiTipsShow(msg, 'success');
        }

        function errAlert(msg) {
            HiTipsShow(msg, 'error');
        }

        function beforeSaveData(obj) {
            var appid = $('#<%=txt_appid.ClientID%>').val();
            var appsecret = $('#<%=txt_appsecret.ClientID%>').val();
            var mch_id = $('#<%=txt_mch_id.ClientID%>').val();
            var key = $('#<%=txt_key.ClientID%>').val();
            if (appid == "") {
                errAlert("请输入appid!");
                $('#<%=txt_appid.ClientID%>').focus();
                return false;
            }
            if (appsecret == "") {
                errAlert("请输入appsecret!");
                $('#<%=txt_appsecret.ClientID%>').focus();
                return false;
            }
            if (mch_id == "") {
                errAlert("请输入mch_id!");
                $('#<%=txt_mch_id.ClientID%>').focus();
                return false;
            }
            if (key == "") {
                errAlert("请输入Key!");
                $('#<%=txt_key.ClientID%>').focus();
                return false;
            }
            return true;
        }

        $(function () {
            $('#aspnetForm').formvalidation({

                'ctl00$ContentPlaceHolder1$txt_appid': {
                    validators: {
                        notEmpty: {
                            message: '微信公众号身份的唯一标识'
                        }
                        ,
                        stringLength: {
                            min: 1,
                            max: 100,
                            message: ''
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txt_appsecret': {
                    validators: {
                        notEmpty: {
                            message: '审核后在公众平台开启开发模式后可查看'
                        }
                        ,
                        stringLength: {
                            min: 1,
                            max: 100,
                            message: ''
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txt_mch_id': {
                    validators: {
                        notEmpty: {
                            message: '微信支付商户号，审核通过后，在微信发送的邮件中查看。'
                        }
                        ,
                        stringLength: {
                            min: 1,
                            max: 100,
                            message: ''
                        }
                    }
                },

                'ctl00$ContentPlaceHolder1$txt_key': {
                    validators: {
                        notEmpty: {
                            message: '商户支付密钥Key。请登录<a target="_blank" href="https://pay.weixin.qq.com/index.php/home/login?return_url=%2F"> 微信支付商户平台</a> ，在【账户设置-安全设置-API安全】中设置。'
                        }
                    }
                }
            });

        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>设置收款账号</h2>
        </div>
        <div>
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="active"><a href="WeixinPay.aspx">微信支付</a></li>
                <li role="presentation" ><a href="Alipay.aspx">支付宝</a></li>
                <%--<li role="presentation"><a href="ChinaBank.aspx">网银在线</a></li>--%>
                <%--<li role="presentation" ><a href="ShengPay.aspx">盛付通</a></li>--%>
                <li role="presentation" ><a href="OfflinePay.aspx">线下支付</a></li>
                <li role="presentation"><a href="COD.aspx">货到付款</a></li>
            </ul>
            <div class="set-switch">
                <strong>微信支付收款设置</strong>
                <p>还没开通微信支付？<a target="_blank" href="https://mp.weixin.qq.com/cgi-bin/readtemplate?t=news/open-app-apply-guide_tmpl&lang=zh_CN">立即免费申请开通微信支付接口</a></p>
                <p>微信支付只支持会员在微信客户端购买付款，如需支持客户在其他浏览器上购买，请设置其他收款方式。</p>
                <div id="offlineEnable" class="<%=_enable?"switch-btn":"switch-btn off" %>" onclick="setEnable(this)">
                    <%=_enable?"已开启":"已关闭"%>
                    <i></i>
                </div>

            </div>            
    
            <%--<div class="write-form form-horizontal" style="width: 800px; margin-top:10px;">--%>
            <div id="maindiv" style="<%=_enable?"": "display:none" %>">
                <div class="form-group">
                    <label for="inputEmail2" class="col-xs-2 control-label">AppId：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txt_appid"></asp:TextBox>
                        <small class="help-block">微信公众号身份的唯一标识</small>
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputEmail3" class="col-xs-2 control-label">AppSecret：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txt_appsecret"></asp:TextBox>
                        <small class="help-block">审核后在公众平台开启开发模式后可查看</small>
                        
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputEmail3" class="col-xs-2 control-label">
                        MCH_ID：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txt_mch_id"></asp:TextBox>
                        <small class="help-block">微信支付商户号，审核通过后，在微信发送的邮件中查看。</small>
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputEmail3" class="col-xs-2 control-label">Key：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txt_key"></asp:TextBox>
                        <small class="help-block">商户支付密钥Key。请登录<a target="_blank" href="https://pay.weixin.qq.com/index.php/home/login?return_url=%2F"> 微信支付商户平台</a> ，在【账户设置-安全设置-API安全】中设置。</small>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-xs-offset-2 col-xs-10 marginl">
                        <asp:Button runat="server" OnClick="Unnamed_Click" class="btn btn-success inputw100"
                            OnClientClick="return beforeSaveData(this)" Text="保存" />
                    </div>
                </div>
             </div>
            </div>

        <%--</div>--%>
    </form>

</asp:Content>