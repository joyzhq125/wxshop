<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberGrades.aspx.cs"
    MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.Member.MemberGrades" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .thCss{text-align:center;}
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>会员等级管理</h2>
  
        </div>
        <div style="margin-bottom:10px;"> <a href="/Admin/member/AddMemberGrade.aspx" class="btn resetSize btn-primary">新增会员等级</a></div>
	  <!--数据列表区域-->
      <div id="datalist">
       
        <UI:Grid ID="grdMemberRankList" runat="server" AutoGenerateColumns="false" ShowHeader="true"
            DataKeyNames="GradeId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title"
            CssClass="table table-hover mar table-bordered">
                  <Columns>
                      <asp:TemplateField HeaderText="等级名称" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="14%" >
                            <ItemTemplate>
                               <asp:Literal ID="litName" Text='<%# Convert.ToBoolean(Eval("IsDefault")) == true ? Eval("Name")+"<span style=\"color:blue;\">(默认等级)" +"</span>" : Eval("Name")%>' runat="server"></asp:Literal>
                            </ItemTemplate>
                      </asp:TemplateField>

                 
                    <asp:TemplateField HeaderText="会员人数" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px"
                          HeaderStyle-CssClass="td_right td_left" >
                          <ItemTemplate>
                              <asp:Literal ID="litUsers" Text='<%# SelectUserCountGrades(Convert.ToInt32( Eval("GradeId"))) %>' runat="server"></asp:Literal>
                          </ItemTemplate>
                      </asp:TemplateField>

                      <asp:TemplateField HeaderText="满足交易额" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px"
                          HeaderStyle-CssClass="td_right td_left" >
                          <ItemTemplate>
                              <asp:Literal ID="litPoints" Text='<%# "￥"+Eval("TranVol") %>' runat="server"></asp:Literal>
                          </ItemTemplate>
                      </asp:TemplateField>


                      <asp:TemplateField HeaderText="满足交易次数" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px"
                          HeaderStyle-CssClass="td_right td_left" >
                          <ItemTemplate>
                              <asp:Literal ID="litTranTimes" Text='<%#Eval("TranTimes") %>' runat="server"></asp:Literal>
                          </ItemTemplate>
                      </asp:TemplateField>



                      <asp:TemplateField HeaderText="折扣" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="11%">
                            <ItemTemplate>
                             <asp:Literal ID="litDiscountRate" Text='<%# Eval("Discount").ToString()!="100"?"现价×"+Eval("Discount")+"%":"-" %>'  runat="server"></asp:Literal>
                            </ItemTemplate>
                      </asp:TemplateField>
                
                      <asp:TemplateField HeaderText="" HeaderStyle-Width="95" ItemStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                                <div style='<%# Convert.ToInt32( Eval("GradeId")) == 2?"display:none":"display:display"%>'>
                                     <span class="submit_bianji"><asp:HyperLink ID="lkbViewAttribute" runat="server" Text="编辑" NavigateUrl='<%# "AddMemberGrade.aspx?id="+Eval("GradeId")%>' ></asp:HyperLink></span>
                                     <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="lkDelete" CommandName="Delete" IsShow="true" Text="删除" /></span>
                                </div>
                           </ItemTemplate>
                         
                      </asp:TemplateField>
                  </Columns>
                </UI:Grid>	   
        </div>
       </form>


    <script type="text/javascript">
        $(document).ready(function () {
            $('#datalist').find('th').each(function () {
                $(this).addClass('thCss');
            });
        })
    </script>
</asp:Content>

