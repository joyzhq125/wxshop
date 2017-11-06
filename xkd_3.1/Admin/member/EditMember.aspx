<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditMember.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.Member.EditMember" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        
        $(document).ready(function () {
            $('#datadiv').find('select').each(function () {
                $(this).removeClass();
                $(this).addClass('form-control');
            });
            InitValidators();
        });

        function InitValidators() {
            $('#aspnetForm').formvalidation({
                'ctl00$ContentPlaceHolder1$txtRealName': {
                    validators: {
                        stringLength: {
                            min: 1,
                            max: 20,
                            message: '姓名长度在20个字符以内'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtprivateEmail': {
                    validators: {
                        stringLength: {
                            min: 1,
                            max: 256,
                            message: '请输入正确电子邮件，长度在1-256个字符以内'
                        },
                        regexp: {
                            regexp: /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+$/,
                            message: ''
                        }
                    }
                },

                'ctl00$ContentPlaceHolder1$txtAddress': {
                    validators: {
                        stringLength: {
                            min: 1,
                            max: 100,
                            message: '街道地址必须控制在100个字符以内'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtQQ': {
                    validators: {
                        stringLength: {
                            min: 3,
                            max: 20,
                            message: 'QQ号长度限制在3-20个字符之间，只能输入数字'
                        }
                        ,
                        regexp: {
                            regexp: /^[0-9]*$/,
                            message: ''
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtCellPhone': {
                    validators: {
                        stringLength: {
                            min: 3,
                            max: 20,
                            message: '手机号码长度限制在3-20个字符之间,只能输入数字'
                        }
                        ,
                        regexp: {
                            regexp: /^[0-9]*$/,
                            message: ''
                        }
                    }
                }
            });
          }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div id="datadiv">
            <div class="page-header">
                <h2>编辑会员信息</h2>
                <small>编辑会员各项信息资料</small>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">昵称：</label>
                <div class="col-xs-4" style="margin-top:5px;">
                    <asp:Literal ID="lblLoginNameValue" runat="server"></asp:Literal>                
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">会员等级：</label>
                <div class="col-xs-4">
                    <Hi:MemberGradeDropDownList ID="drpMemberRankList" runat="server" AllowNull="false" />
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">姓名：</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtRealName" CssClass="form-control" runat="server" />
                    <small class="help-block">姓名长度在20个字符以内</small>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">电子邮件地址：</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtprivateEmail" CssClass="form-control" runat="server" />
                    <small class="help-block">请输入正确电子邮件，长度在1-256个字符以内</small>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">详细地址：</label>
                <div class="col-xs-4">
                    <Hi:RegionSelector runat="server" ID="rsddlRegion" />           
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">街道地址：</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtAddress" CssClass="form-control" runat="server" />
                    <small class="help-block">街道地址必须控制在100个字符以内</small>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">QQ：</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtQQ" CssClass="form-control" runat="server" />
                    <small class="help-block">QQ号长度限制在3-20个字符之间，只能输入数字</small>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">手机号码：</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtCellPhone" CssClass="form-control" runat="server" />
                    <small class="help-block">手机号码长度限制在3-20个字符之间,只能输入数字</small>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">注册日期：</label>
                <div class="col-xs-4" style="margin-top: 5px;">
                    <Hi:FormatedTimeLabel ID="lblRegsTimeValue"  runat="server" />
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label">总消费金额：</label>
                <div class="col-xs-4" style="margin-top: 5px;">
                    <asp:Literal ID="lblTotalAmountValue" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="form-group">
                <div class="col-xs-offset-2 marginl">
                    <asp:Button runat="server" ID="btnEditUser" class="btn btn-success bigsize"
                        Text="确定" />
                </div>
            </div>
        </div>
</form>

</asp:Content>

