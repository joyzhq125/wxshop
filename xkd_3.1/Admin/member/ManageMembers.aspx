<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageMembers.aspx.cs"
    MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.member.ManageMembers" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/utility/skins/blue.css" type="text/css" media="screen" />
    <Hi:Script ID="Script5" runat="server" Src="/utility/jquery.artDialog.js" />
    <Hi:Script ID="Script6" runat="server" Src="/utility/Window.js" />
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />
  
    <style type="text/css">
        /*.selectthis {border-color:red; color:red; border:1px solid;}*/
        .tdClass{text-align:center;}
        .labelClass{margin-right:10px;}
        .thCss{text-align:center;}
        .selectthis{border:1px solid;border-color:#999999; color:#c93027;margin-right:2px;}
        .selectthis:hover {border:1px solid;border-color:#999999; color:#c93027;margin-right:2px;}
        .aClass{border:1px solid;border-color:#999999; color:#999999;margin-right:2px;}
        .aClass:hover{border:1px solid;border-color:#999999; color:#999999;margin-right:2px;}
        #datalist td{word-break: break-all;}
        #ctl00_ContentPlaceHolder1_grdMemberList th {margin:0px;border-left:0px;border-right:0px;background-color:#F7F7F7;text-align:center; vertical-align:middle;}
        #ctl00_ContentPlaceHolder1_grdMemberList td {margin:0px;border-left:0px;border-right:0px;text-align:center;vertical-align:middle;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">                     
        <div>
            <div class="page-header">
                <h2>会员列表</h2>
            </div>
        <!--搜索-->

        <!--数据列表区域-->
        <div>
            <ul class="nav nav-tabs">
                <li dataID="normal"><a href="/admin/Member/ManageMembers.aspx"><asp:Literal ID="ListActive" Text="所有会员" runat="server"></asp:Literal></a></li>
                <li dataID="new"><a href="/admin/Member/ManageMembers.aspx?clientType=new"><asp:Literal ID="Listfrozen" Text="新会员"  runat="server"></asp:Literal></a></li>
                <li dataID="activy"><a href="/admin/Member/ManageMembers.aspx?clientType=activy"><asp:Literal ID="Literal1" Text="活跃会员"  runat="server"></asp:Literal></a></li>
                <li dataID="sleep"><a href="/admin/Member/ManageMembers.aspx?clientType=sleep"><asp:Literal ID="Literal2" Text="沉睡会员"  runat="server"></asp:Literal></a></li>
            </ul>
            <div class="form-inline mb10">
                 <div class="set-switch">
                    <div class="form-inline  mb10">
                    <div class="form-group mr20" style="margin-left:0px;">
                        <label for="sellshop1" class="ml10">昵称：</label>
                        <asp:TextBox ID="txtSearchText" CssClass="form-control resetSize"  runat="server" /> 
                    </div>
                    <div class="form-group mr20" style ="margin-left :30px;">
                        <label for="sellshop1">手机号码：</label>
                        <asp:TextBox ID="txtPhone" CssClass="form-control resetSize" runat="server" />
                    </div>
                    <div class="form-group mr20" style ="margin-left :30px;">
                        <a class="bl mb5" href="/admin/Member/ManageMembers.aspx" style="cursor: pointer">清除条件</a>
                    </div>  
                    </div>  
                    <div class="form-inline ">
                        <div class="form-group">
                            <label class="ml10">用户名：</label>                      
                            <asp:TextBox ID="txtRealName" CssClass="form-control resetSize" runat="server" />
                        </div>
                        <div class="form-group" style ="margin-left :30px;">
                            <label>会员等级：</label>                 
                            <Hi:MemberGradeDropDownList ID="rankList" Width="170" runat="server" CssClass="form-control resetSize" AllowNull="true" NullToDisplay="全部" />
                        </div>   
                        <div class="form-group" style ="margin-left :30px;">                       
                            <asp:Button ID="btnSearchButton" runat="server" CssClass="btn resetSize btn-primary" Text="搜索" />
                        </div>
                                   
                    </div>   
                </div>          
             </div>

            <div>
                    <div class="form-inline" id="clickTopDown" style="display:none;">
                        <input type="image" src="../images/top.gif" style="border-width: 0px;">                
                        <strong class="fonts">导出会员信息</strong>
                    </div>

                    <div id="dataArea" class="form-group" style="display: none;">
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-3 control-label">请选择需要导出的信息：</label>
                            <div class="col-xs-3" id="checkListDiv" style="width: 42%">
                                <Hi:ExportFieldsCheckBoxList ID="exportFieldsCheckBoxList" runat="server"></Hi:ExportFieldsCheckBoxList>
                            </div>
                        </div>
                        <div class="form-group" >
                            <label for="inputEmail3" class="col-xs-3 control-label">请选择导出格式：</label>
                            <div  id="typeSelectdiv" class="col-xs-3" style="width: 42%;margin-top:7px;">
                                <Hi:ExportFormatRadioButtonList ID="exportFormatRadioButtonList" runat="server" />
                            </div>
                        </div>
                        <div class="form-group" >
                            <div class="col-xs-offset-2 marginl">
                                <asp:Button ID="btnExport" runat="server" CssClass="btn btn-success bigsize" Text="导出" />
                            </div>
                        </div>             
                    </div>
            </div>

            <div style="margin-bottom:5px;  margin-top:10px;">        
                <div class="form-inline" id="pagesizeDiv" style="float: left; width:100%; margin-bottom:5px;">                
                </div> 
                  <div class="page-box">
                    <div class="page fr">
                        <div class="form-group" style="margin-right:0px;margin-left:0px;">
                            <label for="exampleInputName2">每页显示数量：</label>
                       <UI:PageSize runat="server"  ID="hrefPageSize" />
                        </div>
                    </div>
                </div>
                <div class="pageNumber" style="float: right;  height:29px; margin-bottom:5px; display:none;" >
                    <label>每页显示数量：</label>
                    <div class="pagination" style="display:none;">
                        <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
                    </div>
                </div>
                
                <div class="form-inline" style="text-align:left; margin-top:5px;">
                    <input type="checkbox" id="selectAll" /> 全选    
                    <button type="button" class="btn resetSize btn-primary" onclick ="ShowReferral();" >
                        批量设置上级
                    </button> 
                    <button type="button" class="btn resetSize btn-primary" onclick ="ShowGrade();" >
                        批量设置等级
                    </button>     

                    <button type="button" class="btn resetSize btn-primary" onclick ="ShowSMS();" >
                        群发短信
                    </button>  

                    <Hi:ImageLinkButton ID="lkbDelectCheck1" class="btn resetSize btn-primary" runat="server" Text="批量删除" IsShow="true"  DeleteMsg="确认删除选中的用户！"/>                                           
                </div>
                <!--结束-->                           
            </div>
            <div id="datalist">
            <UI:Grid ID="grdMemberList" runat="server" ShowHeader="true" AutoGenerateColumns="false"
                DataKeyNames="UserId" HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered"
                GridLines="None" Width="100%" >
                <Columns>
                    <UI:CheckBoxColumn  CellWidth="50"  ItemStyle-HorizontalAlign="Center"/>
                    <asp:TemplateField HeaderText="微信头像"  ItemStyle-Width ="80" SortExpression="UserName"  HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>                                                       
                            <img alt="头像" src="<%# Eval("UserHead") %>" style="height:60px;width:60px;border-width:0px;"/>
                            <input type ="text" value="<%# Eval("UserId") %>" style="display :none;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="昵称/手机"  SortExpression="UserName"  HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <p><asp:Literal ID="lblUserName" runat="server" Text='<%# Eval("UserName").ToString()==""?"未设置":Eval("UserName") %>' /></p>
                            <p><%# Eval("CellPhone").ToString()==""?"未绑定":Eval("CellPhone") %></p>
                            <asp:HiddenField Value='<%# Eval("CellPhone")%>' runat="server" ID="hidCellPhone" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="微信OPenID" ItemStyle-Width ="200"  HeaderStyle-HorizontalAlign="Center" ShowHeader="true">
                        <ItemTemplate>
                            <%# Eval("OpenID").ToString()==""?"未绑定":Eval("OpenID")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="用户名" ShowHeader="true">
                        <ItemTemplate>
                            <%# Eval("UserBindName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="会员等级"  ItemStyle-Width ="100" ShowHeader="true">
                        <ItemTemplate>
                            <asp:Literal ID="lblGradeName" runat="server" Text='<%# Eval("GradeName") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="上级" ItemStyle-Width="100px" SortExpression="Balance">
                        <ItemTemplate>
                            <%# Eval("StoreName").ToString()==""?"主店": Eval("StoreName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="订单数/消费额" ItemStyle-HorizontalAlign="Center" ShowHeader="true">
                       <ItemTemplate>
                           <p >
                               <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/trade/manageorder.aspx?UserId={0}",Server.UrlEncode(Eval("UserId").ToString()))) %>' style="text-decoration: underline; color:#F60;">
                                   <asp:Label ID="lblOrderNumberBandField" class="order-span" Text='<%# Eval("OrderCount") %>' runat="server"></asp:Label>
                               </a>
                           </p>
                           <p style="color:#F60;">
                           ￥<%# Eval("OrderTotal").ToString ()==""?"0.00":Convert.ToDouble ( Eval("OrderTotal")).ToString ("f2") %></p>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="注册时间" HeaderStyle-HorizontalAlign="Center" SortExpression="GradeName">
                        <ItemTemplate>
                             <ItemTemplate><%# Eval("CreateDate")%></ItemTemplate>
                        </ItemTemplate>
                    </asp:TemplateField>                                 
                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="border_top border_bottom" HeaderStyle-Width="95">
                        <ItemStyle CssClass="spanD spanN" />
                        <ItemTemplate>
                            <p>
                                <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/member/MemberDetails.aspx?userId={0}", Eval("UserId")))%>'>详情</a> |
                                <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/member/EditMember.aspx?userId={0}", Eval("UserId")))%>'>编辑</a>
                            </p>
                            <p><a href="javascript:ShowGradeUser('<%# Eval("UserId") %>','<%# Eval("GradeId") %>')">设置等级</a></p>
                            <p><a href='javascript:void(0);' dataID="<%#Eval("UserId") %>">删除会员</a></p>                            
                            <span class="submit_shanchu" style="display: none">
                                <Hi:ImageLinkButton runat="server" ID="Delete" IsShow="true" Text="删除" CommandName="Delete" />
                            </span>
                            <input id="hdUserId" type="hidden" value="<%# Eval("UserID") %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </UI:Grid>   

            </div>      
        </div>
        <!--数据列表底部功能区域-->
    
        <div class="bottomPageNumber clearfix">
            <div class="pageNumber">
                <div class="pagination" style="width: auto">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
            </div>
        </div>
       <!--会员短信群发-->
<%--        <div id="div_sendmsg" style="display: none; width:600px;height:600px;">
            <div style="display:none">
                <p>短信群发<span>(您还剩余短信<font color="red"><asp:Literal ID="litsmscount" runat="server" Text="0"></asp:Literal></font>条)</span>
                </p>
                <p>
                    <h4>发送对象(共<font style="color: Red">0</font>个会员)</h4>
                </p>
                <div id="send_member" style="overflow-x: hidden; overflow-y: auto; margin-bottom: 20px">
                    <ul class="menber"></ul>
                </div>
                <p>
                    <textarea id="txtmsgcontent" runat="server" style="width: 350px; height:150px;"
                        class="forminput" value="输入发送内容……" onfocus="javascript:addfocus(this);" onblur="javascript:addblur(this);"></textarea>
                </p>
            </div>
        </div>--%>
        </div>

        <!-- dialog start-->
        <!-- 批量设置等级-->
        <div class="modal fade" id="GradeShow">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" style="text-align:left" >批量设置等级</h4>
              </div>
                <div class="modal-body form-horizontal" >
                    <div class="form-group">
                                <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>请选择等级：</label>
                                <div class="col-xs-6">
                                  <Hi:MemberGradeDropDownList ID="GradeCheckList"  CssClass="form-control inputw120" runat="server" AllowNull="False" />
                                </div>
                        </div>
                </div>
              <div class="modal-footer">
                  <asp:Button ID="GradeCheck"  class="btn btn-primary" Text="确定修改"  runat="server"  />
                <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
              </div>
            </div>
          </div>
        </div>

               <div class="modal fade" id="Referral">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" style="text-align:left" >修改会员上级</h4>
              </div>
                <div class="modal-body form-horizontal" >
                             <div class="form-group">
                                <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>分销商：</label>
                                <div class="col-xs-6">
                                <input id="txtRid" type="text" placeholder="店铺名"  class="form-control resetSize"/>
                                    
                                </div>
                                 <input id ="btn_selectReferral" type="button" value="搜索" class="btn resetSize btn-primary" />
                        </div>
                    <div class="form-group">
                                <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>请选择：</label>
                                <div class="col-xs-6">
                                    <asp:DropDownList  CssClass="form-control inputw120" runat="server"  ID ="DDL_ReferralUser">

                                    </asp:DropDownList>
       
                                </div>
                        </div>
                </div>
              <div class="modal-footer">
                  <input  class="btn btn-primary" type ="button" ID="btn_SetDistributors" value="确定修改" />
                <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
              </div>
            </div>
          </div>
        </div>



            <div class="modal fade" id="GradeShowUser">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" style="text-align:left" >设置等级</h4>
              </div>
                <div class="modal-body form-horizontal" >
                    <div class="form-group">
                                <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>请选择等级：</label>
                                <div class="col-xs-6">
                                  <Hi:MemberGradeDropDownList ID="DDL_User"  CssClass="form-control inputw120" runat="server" AllowNull="False" />
                                </div>
                        </div>
                </div>
              <div class="modal-footer">
                  <input type ="text" id ="txtUserID" value ="" style ="display:none;" />
                  <input type ="button" id ="btnUserGrade" class="btn btn-primary" value="确定修改" />
                 
                <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
              </div>
            </div>
          </div>
        </div>




        <div class="modal fade" id="SendSMS">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" style="text-align:left" >群发短信</h4>
              </div>
                <div class="modal-body form-horizontal" >
                     <div class="form-group">
                         <label for="inputEmail3" ><em></em>剩余短信条数：<%=ValidSmsNum%></label><br />
                    </div>
                    <div class="form-group">
                        
                         <label for="inputEmail3" ><em>*</em>短信内容：</label>
                        <div style="margin:10px;">
                                <asp:TextBox TextMode="MultiLine" ID="txtContent" runat="server"  Height="80px"  Visible="false" ></asp:TextBox>
                               <textarea id="txtmsgcontent" runat="server" style="height:140px;" class="forminput" value="" onfocus="javascript:addfocus(this);" onblur="javascript:addblur(this);"></textarea>
                        </div>
                    </div>
                </div>
              <div class="modal-footer">
                  <asp:Button ID="btnSendMessage" runat="server"  CssClass="btn btn-primary"   Text="确定发送" />
                 
                <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
              </div>
            </div>
          </div>
        </div>

         <!-- dialog end-->

        </form>
    <script type="text/javascript">
        var formtype = "";
        //绑定上级下拉列表
        function SelectReferral()
        {
            var title = $("#txtRid").val();
            if (title < 1) {
                ShowMsg("请输入用户名或者分销商名称", false);
                return;
            }
            $.getJSON("/admin/Member/api/Hi_Ajax_GetDistributors.ashx", { userName: title }, function (data) {
                if (data.length > 0) {
                    $("#<%=DDL_ReferralUser.ClientID%>").empty();
                }
                for (var i = 0; i < data.length; i++) {
                  
                    $("<option value='" + data[i].UserId + "'>" + data[i].StoreName + "</option>").appendTo("#<%=DDL_ReferralUser.ClientID%>");
                }
            });
        }

        function SetReferral() {
            if ($('#datalist input[type="checkbox"]:checked').length < 1) {
                ShowMsg("请先选择要修改分销商的用户！",false);
                return;
            }
            var rid =$("#<%=DDL_ReferralUser.ClientID%>").val();
            if (rid.length <1){
                ShowMsg("请选择分销商！", false);
                return;
            }
            var ids = "";
            for (var i = 0; i < $('#datalist input[type="checkbox"]:checked').length; i++) {
               
                ids += $('#datalist input[type="checkbox"]:checked').eq(i).parent().next().find("input").val().trim() + ",";
            }
            var msg = ManageMembers.SetDistributors(ids, rid).value;
            if (msg == "success") {
                ShowMsgAndReUrl("成功修改了" + $('#datalist input[type="checkbox"]:checked').length + "个用户的分销商！", true, "/admin/Member/ManageMembers.aspx", null);

            } else{
                ShowMsg(msg, false);
            }

        }

        $(document).ready(function () {
            $("#ctl00_ContentPlaceHolder1_grdMemberList img").each(function () {
                if ($(this).attr("src") == "") {
                    $(this).attr("src", "/Utility/pics/imgnopic.jpg");
                }
            })

            $("<option value='0'>主店</option>").appendTo("#<%=DDL_ReferralUser.ClientID%>");
            AddDialog("div_del_user", "<strong>删除用户后将不可恢复!</strong><p>确认删除该用户吗？</p>", DelUser)
            $("#btn_selectReferral").click(SelectReferral);

            $("#btn_SetDistributors").click(SetReferral);

            if (GetQueryString("clientType").length > 0) {
                var clientType = GetQueryString("clientType");
                $(".nav-tabs > li[dataID='" + GetQueryString("clientType") + "']").addClass("active");
                $(".nav-tabs > li[dataID='" + GetQueryString("clientType") + "'] > a").attr("href", "#");
            } else {
                $(".nav-tabs > li[dataID='normal']").addClass("active");
                $(".nav-tabs > li[dataID='normal'] > a").attr("href", "#");
            }

            var status = 1;
            $("#clickTopDown").click(function () {
                $("#dataArea").toggle(500, changeClass)
            })

            changeClass = function () {
                if (status == 1) {
                    status = 0;
                }
                else {
                    status = 1;
                }
            }


            $("#checkListDiv").find('td').each(function () {
                $(this).removeClass();
                $(this).addClass('tdClass');
            });

            $("#typeSelectdiv").find('label').each(function () {
                $(this).removeClass();
                $(this).addClass('labelClass');
            });

            $('#datalist').find('th').each(function () {
                $(this).css('text-align', 'center');
            });

            $('#pagesizeDiv').find('a').each(function () {
                if ($(this).attr("class") != "selectthis") {
                    $(this).removeClass();
                    $(this).addClass('aClass');
                }

            });
            $("#datalist  a[dataID]").click(function () {
               
                $("#btn_div_del_user").attr("dataID", $(this).attr("dataID"));
                $('#div_del_user').modal('toggle').children().css({
                    width: '400px'
                })

            });

            $('#selectAll').click(function () {
                var check = $(this).prop('checked');
                $("input[type='checkbox']").each(function () {
                    $(this).prop('checked', check);
                });
            });
        });

     


        //样式控制
        function showcss(divobj, rownumber) {
            if (rownumber > 12) {
                $("#" + divobj).css("height", 100);
            }
        }

        function ShowGradeUser(id,gradeID) {
            $("#txtUserID").val(id);
            $("#<%=DDL_User.ClientID%>").val(gradeID)
            $('#GradeShowUser').modal('toggle').children().css({
                width: '400px', top: "170px"
            });

            $("#btnUserGrade").click(function () {
                var result = ManageMembers.GradeCheckUser($("#txtUserID").val(), $("#<%=DDL_User.ClientID%>").val()).value;
                if (result == "success") {
                    ShowMsgAndReUrl("设置成功", true, "/admin/Member/ManageMembers.aspx", null);
                } else{
                    ShowMsg("设置失败",false);
                }
            });
        }
        function DelUser() {
            var result = ManageMembers.DelUser($(this).attr("dataID")).value;
            if (result == "success") {
                ShowMsgAndReUrl("删除成功", true, "/admin/Member/ManageMembers.aspx", null);
            } else if (result == "isExistVShop") {
                ShowMsg("该用户是分销商,请取消分销商资质后再删除！", false);
            }
            else {
                ShowMsg("设置失败", false);
            }
        }
        function ShowGrade() {

            if ($('#datalist input[type="checkbox"]:checked').length < 1) {
                alert("请先选择要修改用户等级的用户！");
                return;
            }


            $('#GradeShow').modal('toggle').children().css({
                width: '400px', top: "170px"
            });

        }

        function ShowReferral() {

            if ($('#datalist input[type="checkbox"]:checked').length < 1) {
                alert("请先选择要修改分销商的用户！");
                return;
            }
            $("#Referral .modal-title").html("修改会员上级 (会员数 " + $('#datalist input[type="checkbox"]:checked').length + " 人)");
            $('#Referral').modal('toggle').children().css({
                width: '400px', top: "170px"
            });

        }

        function ShowSMS() {

            if ($('#datalist input[type="checkbox"]:checked').length < 1) {
                alert("请先选择要群发短信的用户！");
                return;
            }


            $('#SendSMS').modal('toggle').children().css({
                width: '400px', top: "170px"
            });

        }

        function validatorForm() {

            $("#hform").find(":input").trigger("blur"); //触发验证
            var numError = $("#hform").find('.has-error').length;
            if (numError) return false; //验证未通过
            return true;
        }


        function DialogShowNew(hishop_titile, hishop_id, hishop_div, btnId) {

            var tform = $("<form id='hform'><form>"); //构造form,方便绑定验证方法
            tform.append($("#" + hishop_div).html());

            dialog = art.dialog({
                id: hishop_id,
                title: hishop_titile,
                content: tform[0],
                init: function () {
                    if (arrytext != null) {
                        getArryText(arrytext);
                    }
                },
                resize: true,
                fixed: true,
                close: function () {
                    arrytext = null;
                },
                button: [{
                    name: '确 认', callback: function () {
                        var istag = validatorForm();
                        if (istag) {
                            var temparrytext = arrytext;
                            if (temparrytext != null) {
                                setShowText(temparrytext);
                                this.close();
                                getArryText(temparrytext);

                                $("#" + btnId).trigger("click");

                            }
                        } else {
                            return false;
                        }
                    }, focus: true
                },
                        { name: '取 消' }
                ]
            });


            $('#hform').formvalidation(vilidsetings);//绑定验证方法

        }

        //----------------------
        function ShowAddDiv(roleId, name, description) {

            //验证方法'
            vilidsetings = {
                'ctl00$ContentPlaceHolder1$txt_AddImageTypeName': {
                    validators: {
                        notEmpty: {
                            message: '填写分组名称'
                        },
                        stringLength: {
                            min: 1,
                            max: 20,
                            message: '长度不能超过20个字符'
                        }
                    }
                }
            };

            arrytext = null;
            formtype = "add";
        
        }
    </script>
</asp:Content>
