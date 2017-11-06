<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientSet.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master"
    Inherits="Hidistro.UI.Web.Admin.Member.ClientSet" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/utility/skins/blue.css" type="text/css" media="screen" />
    <Hi:Script ID="Script5" runat="server" Src="/utility/jquery.artDialog.js" />
    <Hi:Script ID="Script6" runat="server" Src="/utility/Window.js" />
    <style type="text/css">
        h2{font-size:15px; font-weight:bold;}
  
        .splitLine{ background-color:#cccccc; height:1px; margin-top:5px; }
    </style>
    <script>
        function validForm() {
            var reg1 = /^\d+$/;
            var re = /^\d+(?=\.{0,1}\d+$|$)/
            if ($("#ctl00_contentHolder_txtactivyorder").val().replace(/\s/g, "") != "") {
                if (!reg1.test($("#ctl00_contentHolder_txtactivyorder").val().replace(/\s/g, ""))) {
                    alert("下单次数非法");
                    return false;
                }
            }

            if ($("#ctl00_contentHolder_txtactivymoney").val().replace(/\s/g, "") != "") {
                if (!re.test($("#ctl00_contentHolder_txtactivymoney").val())) {
                    alert("下单金额非法");
                    return false;
                }
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">


                <div class="page-header">
                    <h2>客户分组配置</h2>
                    <small>按照客户的采购时间、采购频次、采购金额等字段进行分组管理多维度筛选条件,
                并可以对这些客户分组进行更加精准的短信关怀和营销。</small>
                </div>
           
              
          
                    <div class="form-group" style="margin-left:5px;">
                        <small>1.如果设置的数字为空或为0，则视为没有设置。   </small>
                        <small>2.设置条件后，系统会自动按照过滤条件给筛选出所匹配的客户。</small>
                    </div>
                    <div class="form-group" style="margin-left: 5px;">
                        <h2>新客户过滤配置 <font color="red">（二选一）</font></h2>
                        <div class="splitLine"></div>
                    </div>
                    <div class="form-inline" style="margin-left: 5px; margin-bottom:5px;">
                        <input type="radio" id="radnewtime" name="radionew" checked="true" runat="server" />
                        <label>满足注册时间 从 </label>
                        <UI:WebCalendar runat="server" CssClass="form-control resetSize" ID="calendarStartDate" />
                        <label>至</label>   
                        <UI:WebCalendar runat="server" CssClass="form-control resetSize" ID="calendarEndDate" />
                        <label>   的客户 </label>
                    </div>
                    <div class="form-inline" style="margin-left: 5px; margin-bottom: 5px;">
                        <input type="radio" id="radnewday" name="radionew" runat="server" />
                        <label> 满足</label>
                            <select name="slsnewregist" id="slsnewregist" class="form-control resetSize" runat="server">
                            <option value="3">3天</option>
                            <option value="7">7天</option>
                            <option value="14">14天</option>
                        </select>
                        <label>
                        内注册的客户
                        </label>
                    </div>

                    <div class="form-group" style="margin-left: 5px; margin-top:20px;">
                        <h2>活跃客户过滤配置 <font color="red">（二选一）</font></h2>
                        <div class="splitLine"></div>
                    </div>

                    <div class="form-inline" style="margin-left: 5px; margin-bottom: 5px;">             
                         <input id="radioactivyorder" name="radioactivy" type="radio" checked="true" runat="server" />
                         <label> 满足下单次数</label>    
                        
                        <select id="slsactivyorder" name="slsactivyorder" class="form-control resetSize" runat="server">
                            <option value="7">7天</option>
                            <option value="14">14天</option>
                            <option value="30">30天</option>
                        </select>
            
                        <label>内采购</label>     
                   
                        <select name="slsactivyorderchar" id="slsactivyorderchar" class="form-control resetSize" runat="server">
                            <option value=">" selected >大于</option>
 
                        </select>
                    
                        <input type="text" id="txtactivyorder" runat="server" class="form-control resetSize" 
                            style="float: none" />
                        <label>次的客户</label>  
                        
                    </div>



                    <div class="form-inline" style="margin-left: 5px; margin-bottom: 5px;">
                         <input id="radioactivymoney" name="radioactivy" type="radio" runat="server" />
                         <label>满足采购金额 </label>                    
                        <select name="slsactivymoney" id="slsactivymoney" class="form-control resetSize" runat="server">
                            <option value="7">7天</option>
                            <option value="14">14天</option>
                            <option value="30">30天</option>
                        </select>          
                        <label>内采购</label>                       
                    
                        <select id="slsactivymoneychar" runat="server" class="form-control">
                            <option value=">" selected>大于</option>

                        </select>
                   
                         <input type="text" id="txtactivymoney" runat="server" class="form-control resetSize" style="float: none" />
                        <label>元的客户</label>                     
               
                    </div>



                <div class="form-group" style="margin-left: 5px; margin-top: 20px;">
                    <h2>休眠客户配置</h2>
                    <div class="splitLine"></div>
                </div>

  
                <div class="form-inline" style="margin-left: 5px; margin-bottom: 5px;">
                        <label>满足　</label>        
    
                        <select name="slssleep" id="slssleep" runat="server" class="form-control resetSize">
                            <option value="7">7天</option>
                            <option value="14">14天</option>
                            <option value="30">30天</option>
                        </select>
            
                    <label> 内无采购记录的客户</label>      
               </div>
         
                <div class="btn_bottom" style="margin-top: 5px; text-align: center; width:60%;">
                    <asp:Button ID="btnSaveClientSettings" runat="server" Text="保 存" CssClass="btn btn-success inputw100"
                                 />
               </div>
           

          

     
    </form>
</asp:Content>
