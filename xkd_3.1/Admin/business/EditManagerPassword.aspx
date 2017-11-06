<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="EditManagerPassword.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Settings.EditManagerPassword" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="renderer" content="webkit">   
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Access-Control-Allow-Origin" content="*">
    <link rel="stylesheet" href="http://apps.bdimg.com/libs/bootstrap/3.3.4/css/bootstrap.min.css">
    <script src="http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js" type="text/javascript"></script>
    <script src="http://apps.bdimg.com/libs/bootstrap/3.3.4/js/bootstrap.min.js" type="text/javascript"></script>
    
    <link rel="stylesheet" type="text/css" href="/admin/css/bootstrap-datetimepicker.min.css">
    <%--<script type="text/javascript" src="../js/bootstrap-datetimepicker.js"></script>
    <script type="text/javascript" src="../js/bootstrap-datetimepicker.zh-CN.js"></script>
    <script type="text/javascript" src="../js/jquery.formvalidation.js"></script>--%>

    <script type="text/javascript" src="/admin/js/bootstrap-datetimepicker.js"></script>
    <script type="text/javascript" src="/admin/js/bootstrap-datetimepicker.zh-CN.js"></script>
    <script type="text/javascript" src="/admin/js/jquery.formvalidation.js"></script>
    <link rel="stylesheet" href="/admin/css/common.css" />
    <script src="/admin/js/Framenew.js"></script>
    <meta name="keywords" content="">
    <meta name="description" content="">
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />
    <style>
    small{font-size:12px}
    .col-xs-3{width:300px}
    </style>
    
</head>

<body>
     <div class="page-header">
                    <h2>编辑管理员信息</h2>
    </div>
    <div class="mate-tabl">
                    <ul class="nav nav-tabs" role="tablist">
                        <li role="presentation" class=""><a href='<%="EditManager.aspx?userId="+Page.Request.QueryString["userId"] %>'>基本信息</a></li>
                        <li role="presentation" class="active"><a href="#messages" aria-controls="messages" role="tab" data-toggle="tab" aria-expanded="false">修改密码</a></li>
                    </ul>
                    <div class="tab-content">
                        <div role="tabpanel" class="tab-pane " id="profile">-</div>
                        <div role="tabpanel" class="tab-pane active" id="messages">

                <!--表单-->
               <form id="thisForm" runat="server" class="form-horizontal" >
                    <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label">用户名：</label>
                        <div class="col-xs-3">
                           <span class="form-control" style="border:none" ><asp:Literal ID="lblLoginNameValue" runat="server" /></span>
                        </div>
                    </div>

                    <div class="form-group" id="panelOld" runat="server">
                        <label for="inputEmail3" class="col-xs-2 control-label"><span style="color:red">*</span>旧密码：</label>
                        <div class="col-xs-3">
                            <asp:TextBox ID="txtOldPassWord" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><span style="color:red">*</span>新密码：</label>
                        <div class="col-xs-3">
                            <asp:TextBox ID="txtNewPassWord" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                            <small class="help-block">密码不能为空，长度在6-20个字符之间</small>
                        </div>
                    </div>

                     <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><span style="color:red">*</span>确认密码：</label>
                        <div class="col-xs-3">
                            <asp:TextBox ID="txtPassWordCompare" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                            <small class="help-block">请输入确认密码</small>
                        </div>
                    </div>

                    
                  
                    <div class="form-group">
                        <div class="col-xs-offset-2 col-xs-10">
                          
                       <asp:Button ID="btnEditPassWordOK" runat="server"  CssClass="btn btn-success" Text="保 存" /> 
                        </div>
                    </div>


    </form>

                        </div>
                    </div>
  </div>

    <script>

        function InitValidators() {

            $('#aspnetForm').formvalidation({
                'ctl00$ContentPlaceHolder1$txtNewPassWord': {
                validators: {
                notEmpty: {
                            message: '密码不能为空'
                },
                    stringLength: {
                            min: 6,
                            max: 20,
                            message: '用户名必须大于6，小于20个字符长'
                    }
                }
            },
               'ctl00$ContentPlaceHolder1$txtPassWordCompare': {
                validators:{
                    notEmpty: {
                            message: '重复密码不能为空'
                    },
                    repeatPass: {
                            message: '密码与上次输入不符'
                    }
                }
            }
         });

        }

        $(document).ready(function () {
            InitValidators();

        });


    </script>

</body>
