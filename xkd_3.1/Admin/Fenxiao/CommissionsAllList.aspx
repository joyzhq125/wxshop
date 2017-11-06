<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="CommissionsAllList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.CommissionsAllList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register src="../Ascx/ucDateTimePicker.ascx" tagname="DateTimePicker" tagprefix="Hi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
          <div class="page-header">
            <h2>佣金明细</h2>
         </div>
    <form runat="server">
    <div class="set-switch">
                    <div class="form-horizontal clearfix">
                       <div class="form-inline mb10">
                            <div class="form-group">
                                <label　 for="sellshop1" style="margin-left:10px">　店铺名：</label>
                                <asp:TextBox ID="txtStoreName" CssClass="form-control resetSize inputw150" runat="server" />
                            </div>
                            <div class="form-group" style="padding-left:1px">
                                <label for="sellshop2">　订单号：</label>
                                <asp:TextBox ID="txtOrderId"  CssClass="form-control  resetSize  inputw100" runat="server"  Width="150" />
                            </div>
                            
                        </div>

                        <div class="form-inline  mr20">
                            <label class="col-xs-1 pad control-label resetSize" for="setdate">时间范围：</label>
                            <div class="form-inline journal-query">
                                <div class="form-group" style="padding-left:4px">
                                   <Hi:DateTimePicker CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="form-control resetSize inputw150" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;至&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   <Hi:DateTimePicker ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="form-control resetSize inputw150" />&nbsp;&nbsp;&nbsp;
                                </div>
                                <asp:Button ID="btnQueryLogs" runat="server" class="btn resetSize btn-primary" Text="查询" OnClick="btnQueryLogs_Click" />&nbsp;&nbsp;
                                <div class="form-group">
                                    <label for="exampleInputName2">快速查看</label>
                                    <asp:Button ID="Button1" runat="server" class="btn resetSize btn-default" Text="最近7天" OnClick="Button1_Click1"  />
                                    <asp:Button ID="Button4" runat="server" class="btn resetSize btn-default" Text="最近一个月" OnClick="Button4_Click1" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>



    <!--数据列表-->
     <asp:Repeater ID="reCommissions"  runat="server" >
         <HeaderTemplate>
             <div>
             <table class="table table-hover mar table-bordered" style="table-layout:fixed">
                        <thead>
                            <tr>
                                <th  width="100">店铺</th>  
                                <th  width="120">订单号</th>
                                 <th  width="100">订单金额</th>
                                <th  width="100">佣金</th>
                                <th width="120">时间</th>
                            </tr>
                        </thead>
                        <tbody>
         </HeaderTemplate>
     <ItemTemplate>
      <tr  class="td_bg">
          <td>
                                    &nbsp;<%# getNextName(Eval("fromStoreName").ToString(),Eval("UserId").ToString(),Eval("ReferralUserId").ToString(),Eval("ReferralPath").ToString())%></td>
                                 <td>
                                    &nbsp;<a href="/Admin/trade/OrderDetails.aspx?OrderId=<%# Eval("OrderId")%>" target="_blank"><%# Eval("OrderId")%></a></td>
                                  <td>
                                    &nbsp;￥<%# Eval("OrderTotal", "{0:F2}")%></td>
                                <td>
                                    &nbsp;￥<%# Eval("CommTotal","{0:F2}")%></td>
                               
                               
        <td width="200">
                                 &nbsp;<%# Eval("TradeTime", "{0:yyyy-MM-dd hh:mm:ss}")%></td>
           </tr>
     </ItemTemplate>
     <FooterTemplate>
         </tbody>
     </table>
     </div>
     </FooterTemplate>
     </asp:Repeater>

         <!--数据列表底部功能区域-->
  <br />
        <div class="select-page clearfix">
                    <div class="form-horizontal fl">
                       &nbsp;佣金总额：￥<%=Math.Round(CurrentTotal,2) %>
                    </div>
                    <div  class="page fr">
                         <div class="pageNumber">
                        <div class="pagination" style="margin:0px">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />　
                       </div>
                      </div>
                    </div>
                </div>

        <div class="clearfix" style="height:30px"></div>
        </form>
</asp:Content>
