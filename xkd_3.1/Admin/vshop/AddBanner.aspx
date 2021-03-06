﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddBanner.aspx.cs" Inherits="Hidistro.UI.Web.Admin.vshop.AddBanner" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
<script type="text/javascript">
    var auth = "<%=(Request.Cookies[FormsAuthentication.FormsCookieName]==null ? string.Empty : Request.Cookies[FormsAuthentication.FormsCookieName].Value) %>";
</script>
    <script src="../js/swfupload/swfupload.js" type="text/javascript"></script>
    <script src="../js/UploadHandler.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1>添加轮播图</h1>
            <span class="font">添加轮播图</span>
      </div>
          <div class="formitem validator2">
            <ul>
              <li><span class="formitemtitle Pw_100">轮播图描述：</span>
                <asp:TextBox ID="txtBannerDesc" runat="server" Width="600px" CssClass="forminput" />
              </li>              
              <li runat="server" id="liParent"><span class="formitemtitle Pw_100">上传图片：</span>
                <span id="spanButtonPlaceholder"></span>
                                <span id="divFileProgressContainer"></span>
								<div>图片建议尺寸：650px * 320px</div>
              </li>  
              <li id="smallpic" style="display: none;"> 
                             <!--封面上传后，返回的图片地址，填充下面的input对象。-->
                           </li>      
              <li> <span class="formitemtitle Pw_100">跳转至：</span>
                <asp:DropDownList ID="ddlType" runat="server" CssClass="forminput droptype" ClientIDMode="Static">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlSubType" name="ddlSubType" runat="server"  CssClass="forminput droptype" style="display:none"   ClientIDMode="Static">
                </asp:DropDownList>
                 <asp:DropDownList ID="ddlThridType" name="ddlThridType" runat="server"  CssClass="forminput droptype" style="display:none"   ClientIDMode="Static">
                </asp:DropDownList>
              <asp:TextBox ID="Tburl" style="display:none; Width:350px" CssClass="forminput" runat="server" ClientIDMode="Static"></asp:TextBox>
                                  <span ID="navigateDesc" runat="server" style="display:none;"><a target="_blank" href="http://www.hishop.com.cn/bbs/thread-193492-1-1.html">获取导航地址</a></span>
              </li>
            </ul>
              <ul class="btn Pa_100 clearfix">
                <asp:Button ID="btnAddBanner" runat="server" 
                      OnClientClick="return GetLoctionUrl();" Text="确 定"  
                      CssClass="submit_DAqueding float" onclick="btnAddBanner_Click" />
         </ul>
         <!--隐藏图片地址-->
               <input id="fmSrc" runat="server" clientidmode="Static" type="hidden" value="" />
               <input id="locationUrl" runat="server" clientidmode="Static" type="hidden" value="" />   
          </div>
  </div>    
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="../js/UploadBanner.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            BindType();
            $("#ddlType").val("Link").trigger("change");
            ShowThirdDropDown();
        }
        );
        
    </script>
    <script src="../js/LocationType.js" type="text/javascript"></script>
</asp:Content>

