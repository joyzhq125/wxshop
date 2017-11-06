<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="EditDistributorGrade.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.EditDistributorGrade" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
    $(function(){
    
    
        $("#Distributorform").formvalidation({
            //'submit': '#ctl00_ContentPlaceHolder1_PassCheck',
            'ctl00$ContentPlaceHolder1$txtName': {
                validators: {
                    notEmpty: {
                        message: '分销商等级名称在20个字符以内！'
                    },
                    stringLength: {
                        min: 2,
                        max: 20,
                        message: '分销商等级名称，2-20个字符！'
                    }
                }
            },
            'ctl00$ContentPlaceHolder1$txtCommissionsLimit': {
                validators: {
                    notEmpty: {
                        message: '佣金满足点不能为空'
                    },
                    regexp: {
                        regexp: /^\d+(\.\d+)?$/,
                        message: '数据类型错误，只能输入实数型数值'
                    }
                }
            },
            'ctl00$ContentPlaceHolder1$txtFirstCommissionRise': {
                validators: {
                    notEmpty: {
                        message: '佣金百分比不能为空'
                    },
                    regexp: {
                        regexp: /^[0-9]\d{0,1}(\.\d+)?$/,
                        message: '数据类型错误，100以内数值'
                    }
                }
            },
            'ctl00$ContentPlaceHolder1$txtSecondCommissionRise': {
                validators: {
                    notEmpty: {
                        message: '佣金百分比不能为空'
                    },
                    regexp: {
                        regexp: /^[0-9]\d{0,1}(\.\d+)?$/,
                        message: '数据类型错误，100以内数值'
                    }
                }
            },
            'ctl00$ContentPlaceHolder1$txtThirdCommissionRise': {
                validators: {
                    notEmpty: {
                        message: '佣金百分比不能为空'
                    },
                    regexp: {
                        regexp: /^[0-9]\d{0,1}(\.\d+)?$/,
                        message: '数据类型错误，100以内数值'
                    }
                }
            }
        });


    });
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="page-header">
            <h2 id="EditTitle" runat="server"><%=htmlOperatorName %>分销商等级</h2>
   </div>

    <form runat="server">




          <div class="form-horizontal" id="Distributorform">

               <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>等级图标：</label>
                        <div class="col-xs-4">
                             <Hi:UpImg runat="server" ID="uploader1" IsNeedThumbnail="false" UploadType="vote"  />
                            <small>（建议上传PNG背景透明的图片，大小50px * 50px）</small>

                        </div>
                </div>

              <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>分销商等级名称：</label>
                        <div class="col-xs-3">
                            <asp:TextBox ID="txtName" runat="server"  CssClass="form-control  inputw120"></asp:TextBox>
                        </div>
                </div>

                <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>佣金满足点：</label>
                        <div class="col-xs-3">
                            <asp:TextBox ID="txtCommissionsLimit" runat="server" CssClass="form-control  inputw120"></asp:TextBox>
                        </div>
                </div>

                <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>成交店铺佣金奖励：</label>
                        <div class="col-xs-3">
                            <asp:TextBox ID="txtFirstCommissionRise" runat="server" CssClass="form-control  inputw120"></asp:TextBox>
                        </div>
                </div>

                <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>上一级佣金奖励：</label>
                        <div class="col-xs-3">
                            <asp:TextBox ID="txtSecondCommissionRise" runat="server" CssClass="form-control  inputw120"></asp:TextBox>
                        </div>
                </div>


                <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>上二级佣金奖励：</label>
                        <div class="col-xs-3">
                            <asp:TextBox ID="txtThirdCommissionRise" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                </div>

               <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>备注：</label>
                        <div class="col-xs-3">
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Width="350" Height="90"></asp:TextBox>
                        </div>
                </div>

              <div class="form-group" id="GIsDefault" runat="server" >
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>设成默认等级：</label>
                        <div class="col-xs-3">
                            <asp:RadioButtonList ID="rbtnlIsDefault"  CssClass="setControl"  runat="server" RepeatDirection="Horizontal">
          <asp:ListItem Value="0">　是　　　</asp:ListItem><asp:ListItem Value="1">　否</asp:ListItem>
          </asp:RadioButtonList>
                        </div>
                </div>

               <div class="form-group">
                        <div class="col-xs-offset-2 col-xs-10">
                             <asp:Button ID="btnEditUser" runat="server" Text="确 定"  CssClass="btn btn-success" />
                        </div>
                 </div>

       


      </div>














    </form>

</asp:Content>
