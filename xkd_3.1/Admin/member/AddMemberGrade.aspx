<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddMemberGrade.aspx.cs"
    MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.Member.AddMemberGrade" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />

    <script type="text/javascript">

        $(function () {
            $('#radioDiv').find('label').each(function () {
                $(this).css('margin-right', '20px');
            });

            $('#aspnetForm').formvalidation({
                'ctl00$ContentPlaceHolder1$txtRankName': {
                    validators: {
                        notEmpty: {
                            message: "会员等级名称不能为空，长度限制在20字符以内"
                        }
                        ,
                        stringLength: {
                            min: 1,
                            max: 20,
                            message: '会员等级名称不能为空，长度限制在20字符以内'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txt_tradeVol': {
                    validators: {              
                        regexp: {
                            regexp: /^[0-9]+\.{0,1}[0-9]{0,2}$/,
                            message: '请输入数字'
                        }
                    }
                },

                'ctl00$ContentPlaceHolder1$txt_tradeTimes': {
                    validators: {
                        regexp: {
                            regexp: /^[0-9]*$/,
                            message: '请输入数字'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtValue': {
                    validators: {
                        notEmpty: {
                            message: '等级折扣为不能为空，且是数字'
                        }
                        ,
                        regexp: {
                            regexp: /^[0-9]*$/,
                            message: '请输入数字'
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
            <h2>添加会员等级</h2>
        </div>
    <div>

            <div class="form-group" style="margin-left: 5px; margin-bottom:30px;">
                        <div style="font-size:15px;"> <span style="color:red;margin-right: 10px;font-weight :900;">|</span>基本信息 </div>
                        <div class="splitLine"></div>
                    </div>
        <div class="form-group">
            <label for="inputEmail1" class="col-xs-2 control-label">会员等级名称：</label>
            <div class="col-xs-4">
                <asp:TextBox ID="txtRankName" CssClass="form-control" runat="server" />
            </div>
        </div>
              <div class="form-group">
            <label for="inputEmail1" class="col-xs-2 control-label">会员等级价格：</label>
            <div class="col-xs-4">
                       <span style="float:left; padding-top:10px; margin-right:10px;"> 现价×</span>  <div style="float:left;"><asp:TextBox ID="txtValue" CssClass="form-control" Width="90px" runat="server"   /></div><span style="float:left;padding-top:10px; margin-left:5px;" >%</span>

            </div>
        </div>
    
        
        <div  class="form-group">
            <label for="inputEmail1" class="col-xs-2 control-label">设为默认：</label>
            <div id="radioDiv" class="col-xs-4">
                <asp:RadioButtonList ID="chkIsDefault" runat="server" RepeatDirection ="Horizontal">
                    <asp:ListItem Text="是" Value="true" ></asp:ListItem>
                    <asp:ListItem Text="否" Value="false" Selected></asp:ListItem>
                </asp:RadioButtonList>
              
            </div>
        </div>
  
        <div class="form-group" style="margin-left: 5px; margin-bottom:30px;">
            <div style="font-size:15px;"> <span style="color:red;margin-right: 10px;font-weight :900;">|</span>自动升级 </div>
            <div class="splitLine"></div>
        </div>
              <div class="form-group">
            <label for="inputEmail1" class="col-xs-2 control-label">交易次数：</label>
            <div class="col-xs-4">
                <asp:TextBox ID="txt_tradeTimes" CssClass="form-control" runat="server" />
            </div>
        </div>
      <div class="form-group ">
            <label for="inputEmail1" class="col-xs-2 control-label">交易额：</label>
            <div class="col-xs-4">
                <asp:TextBox ID="txt_tradeVol" CssClass="form-control" runat="server" />
            </div>
        </div>

        <div class="form-group" style ="display:none;">
            <label for="inputEmail1" class="col-xs-2 control-label">备注：</label>
            <div class="col-xs-4" style="margin-left:5px;">
                <asp:TextBox ID="txtRankDesc" runat="server" TextMode="MultiLine" CssClass="form-group"
                    Width="450" Height="120"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <div class="col-xs-offset-2 marginl">
                <asp:Button runat="server" ID="btnSubmitMemberRanks" class="btn btn-success bigsize"
                  Text="确定" />
            </div>
        </div>
    </div>
</form>
 

</asp:Content>

