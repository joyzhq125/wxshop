<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.Goods.ProductConsultationsReplyed" MasterPageFile="~/Admin/AdminNew.Master" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*.table_title{background:#f2f2f2}
        .table td,th{text-align:center}*/
        #ctl00_ContentPlaceHolder1_grdConsultation th {margin:0px;border-left:0px;border-right:0px;background-color:#F7F7F7;text-align:left; vertical-align:middle;}
        #ctl00_ContentPlaceHolder1_grdConsultation td {margin:0px;border-left:0px;border-right:0px;vertical-align:middle;}
        .username {margin-left:10px;}
         td{word-break: break-all;}
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
          <div class="page-header">
            <h2>客户咨询</h2>
<%--            <small>管理店铺的所有商品咨询，您可以查询或删除商品咨询</small>--%>
        </div>
        <div id="mytabl">
            <div class="table-page">
                <!-- Nav tabs -->
                <ul class="nav nav-tabs">
                    <li><a href="ProductConsultations.aspx">未回复咨询</a></li>
                    <li class="active"><a href="#home">已回复咨询</a></li>
                </ul>
                <div class="page-box">
                    <div class="page fr">
                        <div class="form-group">
                            <label for="exampleInputName2">每页显示数量：</label>
                            <UI:PageSize runat="server" ID="hrefPageSize" />
                        </div>
                    </div>
                </div>
            </div>
            <!-- Tab panes -->
            <div class="tab-content">
                <div class="tab-pane active">
                    <div class="set-switch">
                    <div class="form-inline">
                        <div class="form-group mr20">
                            <label for="exampleInputName2">商品名称</label>
                            <asp:TextBox ID="txtSearchText" runat="server" CssClass="form-control resetSize" placeholder="" />
                        </div>
                        <div class="form-group mr20">
                            <label for="exampleInputName2">商品分类</label>
                            <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" CssClass="form-control resetSize" Style="padding: 5px 0px;" />
                        </div>
                        <div class="form-group mr20">
                            <label for="exampleInputName2">商家编码</label>
                            <asp:TextBox ID="txtSKU" runat="server" CssClass="form-control resetSize" placeholder="" />                          
                        </div>
                        <div class="form-group">
                            <asp:Button ID="btnSearch" runat="server" Text="查询" class="btn resetSize btn-primary" />
                        </div>
                    </div>
                    </div>

                    <div class="select-page clearfix" style="margin-top: 20px;">
                    </div>
                    <UI:Grid ID="grdConsultation" runat="server" ShowHeader="true" CssClass="table mar table-bordered" AutoGenerateColumns="false" DataKeyNames="ConsultationId" HeaderStyle-CssClass="table_title"   GridLines="None" Width="100%">
                        <Columns>
                            <asp:TemplateField SortExpression="ProductName" HeaderText="商品" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="25%">
                                <ItemStyle CssClass="Name" />
                                <ItemTemplate>                                
                                    <div style="float: left;">                                      
                                        <a href='<%#string.Format("../../ProductDetails.aspx?productId={0}&wid={1}",Eval("productId"),this.wid)%>' target="_blank">
                                            <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl60"  Width="60" Height="60"/>
                                            <asp:Literal ID="lblProductName" runat="server" Text='<%# Eval("ProductName") %>' />
                                        </a>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="客户/问题" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="70%">
                                <ItemTemplate>
                                    <asp:Label ID="lblConsultationText" runat="server" Text='<%# Eval("ConsultationText") %>' CssClass="line"></asp:Label>
                                    <br />
                                    <span style="color: #999;">
                                    <Hi:FormatedTimeLabel ID="ConsultationDateTime" Time='<%# Eval("ConsultationDate") %>' runat="server"></Hi:FormatedTimeLabel>
                                    <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("UserName").ToString() %>' CssClass="username"></asp:Label>
                                    </span>
                                    <br />
                                    <div style="background-color:#f2f2f2;color: #F00;">
                                        <span style="word-break:break-all; ">[回复]<%#Eval("ReplyText").ToString() %></span>
                                        <br />
                                        <span>[<%#Eval("ReplyDate").ToString() %>]</span>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff">
                                <ItemTemplate>
                                    <span class="submit_shanchu">
                                        <Hi:ImageLinkButton ID="ilikbReplyDelete" runat="server" CommandName="Delete" IsShow="true" CommandArgument='<%# Eval("ConsultationId")%>' OnClientClick="" Text="删除" CssClass="btn btn-danger resetSize" />
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </UI:Grid>
                    <div class="page">
                        <div class="bottomPageNumber clearfix">
                            <div class="pageNumber">
                                <div class="pagination">
                                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

            </div>
        </div>
 
         <div class="page">
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination">
                            <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                        </div>
                    </div>
                </div>
            </div>
    </form>
</asp:Content>



