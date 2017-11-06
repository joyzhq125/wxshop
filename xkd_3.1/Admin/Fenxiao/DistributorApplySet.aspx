<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="DistributorApplySet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.DistributorApplySet" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/hieditor/ueditor/controls/ucUeditor.ascx" TagName="KindeditorControl" TagPrefix="Kindeditor" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <Hi:Style ID="Style1"  runat="server" Href="/admin/css/bootstrapSwitch.css" />
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/bootstrapSwitch.js" />
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
                    <h2>分销设置</h2>
    </div>
       <form id="thisForm" runat="server" class="form-horizontal" >
        <div class="mate-tabl" id="mytab">
                    <ul class="nav nav-tabs" role="tablist" >
                        <li role="presentation" class="active"><a href="#profile" aria-controls="profile" role="tab" data-toggle="tab" aria-expanded="false">基本设置</a></li>
                        <li role="presentation" class=""><a  href="#messages" aria-controls="messages"  role="tab" data-toggle="tab" >分销说明</a></li>
                       
                    </ul>
                    <div class="tab-content">
                        <div role="tabpanel" class="tab-pane active" id="profile">
           <!--表单-->
   
                    <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>开启三级分佣：</label>
                        <div class="col-xs-5">
                            <div class="switch">
                                <input id="radioCommissionon" type="checkbox" name="radioCommission" runat="server"/>
                            </div>
                            <small>开启三级分佣，成交店铺和其上两级分销商都能获得相应的佣金<br />关闭三级分佣，则只有成交店铺可获得相应的佣金</small>
                        </div>
                    </div>

                    <div class="form-group" style="display:none">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>允许自主申请分销商：</label>
                        <div class="col-xs-5">
                            <div class="switch" id="mySwitch">
                                <input id="radiorequeston" type="checkbox" name="radiorequest" runat="server"/>
                            </div>
                            <small>关闭以后，管理员可以使用批量生成账号来操作</small>
                        </div>
                    </div>
                     <div class="form-group" id="conditionsGroup">
                        <label for="inputEmail3" class="col-xs-2 control-label"><span style="color:red">*</span>分销商申请条件：</label>
                        <div class="col-xs-5">
                            <div class="form-group">
                                &nbsp;&nbsp; <input type="radio" name="conditions" value="true" id="NoConditions" runat="server" checked />
                            <label for="NoConditions">　无门槛，可随时申请</label>
                                 </div>
                            <div class="form-group">
                                &nbsp;&nbsp; <input type="radio"  name="conditions" value="false" id="HasConditions"  runat="server" /><label for="HasConditions">　累计消费额达到&nbsp;</label><input type="text" id="txtrequestmoney" runat="server" style="width:100px"/>&nbsp;元以后才能申请
                            </div>
                        </div>
                    </div>
                    
                   
                    <div class="form-group">
                        <div class="col-xs-offset-2 col-xs-10">
                         <asp:Button ID="btnSave" runat="server" OnClientClick="return PageIsValid();" OnClick="btnSave_Click"
                        Text="保存" CssClass="btn btn-success inputw100"  />
                        </div>
                    </div>

     </div>
                        <div role="tabpanel" class="tab-pane " id="messages">

                      <div class="edit-text clearfix">
                        <div class="edit-text-left">
                            <div class="mobile-border">
                                <div class="mobile-d">
                                    <div class="mobile-header">
                                        <i></i>
                                        <div class="mobile-title">分销说明</div>
                                    </div>
                                    <div class="upshop-view">
                                        <div class="img-info">
                                            <p>基本信息区</p>
                                            <p>固定样式，显示商品主图、价格等信息</p>
                                        </div>
                                        <div class="exit-shop-info" id="fckDescriptionShow">
                                            内容区
                                        </div>
                                    </div>
                                    <div class="mobile-footer"></div>
                                </div>
                            </div>
                        </div>
                        <div class="edit-text-right">
                            <div class="edit-inner">
                                <Kindeditor:KindeditorControl ID="fckDescription" runat="server" Height="300" Width="570" />
                            </div>
                          
                        </div>
                    </div>

                   <div class="footer-btn navbar-fixed-bottom">
                         <asp:Button runat="server" ID="Button1" Text="保存" OnClientClick="return checkDescription();"  OnClick="btnSave_Description" CssClass="btn btn-success inputw100" />
                  </div>

              </div>

           
     </div>
  </div>

    </form>






    <script>

     
        $('#mySwitch').on('switch-change', function (e, data) {



            if (data.value) {
                $("#conditionsGroup").show();
            } else {
                $("#conditionsGroup").hide();
            };
 
        });


        $(function () {
            if ($("#ctl00_ContentPlaceHolder1_radiorequeston")[0].checked) { $("#conditionsGroup").show(); }


            /*编辑器监听事件*/
            um.addListener('ready', function (editor) {
                $("#fckDescriptionShow").html(um.getContent());
            });
            um.addListener('selectionchange', function () {
                $("#fckDescriptionShow").html(um.getContent());
            });


            var tabnum ="<%= tabnum%>";
            $("#mytab li a").eq(tabnum).tab("show");

        });

        function checkDescription() {

            if (um.getContent() == "") {
                HiTipsShow("分销说明内容不能为空", 'error');
             return false;
            };
            
        }

      



    </script>
</asp:Content>
