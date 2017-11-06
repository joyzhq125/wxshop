<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="SaleService.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.SaleService" %>

<%@ Register Src="~/hieditor/ueditor/controls/ucUeditor.ascx" TagName="KindeditorControl" TagPrefix="Kindeditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../css/bootstrapSwitch.css" />
    <script type="text/javascript" src="../js/bootstrapSwitch.js"></script>
    <style type="text/css">
        #ctl00_ContentPlaceHolder1_OpenAccount { margin-left:16px;}
        #ctl00_ContentPlaceHolder1_ChangePwd { margin-left:16px;}
        #custom tbody td {vertical-align:middle;}
    </style>
    <script type="text/javascript">
        $(function () {
            $('#mySwitch').on('switch-change', function (e, data) {
                var type = "0";
                var enable = data.value;
                $.ajax({
                    type: "post",
                    url: "ShopConfigHandler.ashx",
                    data: { type: type, enable: enable },
                    dataType: "json",
                    success: function (data) {
                        if (data.type == "success") {
                            if (enable == true) {
                                $('.maind').css('display', '');
                            }
                            else {
                                $('.maind').css('display', 'none');
                            }
                        }
                        else
                        {
                            ShowMsg("修改失败（" + data.data + ")");
                        }
                    }
                });
            });
        });

        function errAlert(msg) {
            HiTipsShow(msg, 'error');
        }

        function showModel(id) {
            if (parseInt(id) > 0) {
                $.ajax({
                    type: "post",
                    url: "GetCustomerService.ashx?id=" + id,
                    data: {},
                    dataType: "json",
                    success: function (data) {
                        if (data.type == "success") {
                            $('#<%=txt_name.ClientID%>').val(data.nickname);
                            $('#<%=txt_cphone.ClientID%>').val(data.userver);
                            $('#<%=txt_cpwd.ClientID%>').val(data.password);
                        }
                    }
                });
            }

            $('#<%=txt_id.ClientID%>').val(id);

            $('#previewshow').modal('toggle').children().css({
                width: '500px',
                top: '200px'
            });

        }

        function beforeSaveData(obj) {
            var reg = /^(13|15|18|14|17)[0-9]{9}$/;
            var phone = $('#<%=txt_phone.ClientID%>').val();
            var pwd = $('#<%=txt_pwd.ClientID%>').val();
            if ($.trim(phone) == "") {
                errAlert("请输入登录手机号！");
                $('#<%=txt_phone.ClientID%>').focus();
                return false;
            }
            else
            {
                if(!reg.test(phone))
                {
                    errAlert("请输入正确的手机号码！");
                    $('#<%=txt_phone.ClientID%>').focus();
                    return false;
                }
            }
            if ($.trim(pwd) == "") {
                errAlert("请输入密码！");
                $('#<%=txt_pwd.ClientID%>').focus();
                return false;
            }
            return true;
        }

        function AddCustomer()
        {           
            var nickname = $('#<%=txt_name.ClientID%>').val();
            var phone = $('#<%=txt_cphone.ClientID%>').val();
            var pwd = $('#<%=txt_cpwd.ClientID%>').val();
            var reg = /^(13|15|18|14|17)[0-9]{9}$/;
            var unit = $('#<%=txt_phone.ClientID%>').val();
            if ($.trim(unit) == "" || !reg.test(unit)) {
                errAlert("请先开通主账号！");
                return false;
            }

            if ($.trim(nickname) == "")
            {
                errAlert("请输入客服昵称！");
                $('#<%=txt_name.ClientID%>').focus();
                return false;
            }
            if ($.trim(phone) == "") {
                errAlert("请输入登录手机号！");
                $('#<%=txt_cphone.ClientID%>').focus();
                return false;
            }
            else {
                if (!reg.test(phone)) {
                    errAlert("请输入正确的手机号码！");
                    $('#<%=txt_cphone.ClientID%>').focus();
                    return false;
                }
            }
            if ($.trim(pwd) == "") {
                errAlert("请输入密码！");
                $('#<%=txt_cpwd.ClientID%>').focus();
                return false;
            }

            $.ajax({
                type: "post",
                url: "AddCustomerService.ashx",
                data: { id: $('#<%=txt_id.ClientID%>').val(), userver: phone, password: pwd, nickname: nickname },
                dataType: "json",
                success: function (data) {
                    if (data.type == "success") {
                        $('#<%=txt_name.ClientID%>').val("");
                        $('#<%=txt_cphone.ClientID%>').val("");
                        $('#<%=txt_cpwd.ClientID%>').val("");
                        $('#<%=txt_id.ClientID%>').val("");
                        window.location.reload();
                    }
                    else
                    {
                        errAlert(data.data);
                    }
                }
            });
        }

        function DeleteCustomer(id)
        {
            if (parseInt(id) > 0) {
                $.ajax({
                    type: "post",
                    url: "DeleteCustomerService.ashx?id=" + id,
                    data: {},
                    dataType: "json",
                    success: function (data) {
                        if (data.type == "success") {
                            window.location.reload();
                        }
                        else {
                            errAlert(data.data);
                        }
                    }
                });
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>在线客服</h2>
<%--            <small>管理员可管理售后服务QQ帐号和手机号码。</small>--%>
        </div>
        <div class="tab-content">
            <div role="tabpanel" class="exitshopinfo" style="background-color:#fff;" id="setting">                    
                <div class="form-group">
                    <label for="" class="col-xs-2 control-label">开启在线客服：</label>
                    <div class="col-xs-4">
                        <div class="switch" id="mySwitch">
                            <input type="checkbox" <%=enable ? "checked" : ""%> />
                        </div>
                    </div>                    
                </div>
                <div class="form-group">
                    <label for="" class="col-xs-2 control-label">客户端下载：</label>
                    <div class="col-xs-5">           
                        <button type="button" class="btn btn-default"  onclick="window.open('https://s3-qcloud.meiqia.com/download.meiqia/meiqia_for_windows.zip')">Windows版</button>
                        <button type="button" class="btn btn-default"  onclick="window.open('https://itunes.apple.com/cn/app/mei-qia-yi-dong-zai-xian-ke-fu/id1050591118')">IPhone版</button>
                        <%--<button type="button" class="btn btn-default"  onclick="window.open('http://meiqia-s.b0.upaiyun.com/appsite_static/meiqia_for_android_latest.apk')">Android版</button>--%>
                        <button type="button" class="btn btn-default"  onclick="window.open('https://s3-qcloud.meiqia.com/download.meiqia/meiqia_for_android.apk')">Android版</button>
                    </div>                          
                </div>                                
            </div>
            <div class="exitshopinfo maind" style="background-color:#fff;<%=enable ? "" : "display:none" %>">
                <h3>主账号</h3>
                <div class="form-group" style="display:none">
                    <label for="" class="col-xs-2 control-label"><em>*</em>手机号码：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txt_phone" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" style="display:none">
                    <label for="" class="col-xs-2 control-label"><em>*</em>登录密码：</label>
                    <div class="col-xs-4">
                        <input type="password" style="display:none">
                        <asp:TextBox runat="server" class="form-control" ID="txt_pwd" TextMode="Password" autocomplete="off"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <label for="" class="col-xs-2 control-label"><em>*</em>美恰（entId）：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txt_entid" autocomplete="off"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-xs-offset-2">
                        <asp:Button runat="server" CssClass="btn btn-success bigsize" Text="开通主帐号" ID="OpenAccount" Visible="false" OnClick="OpenAccount_Click" OnClientClick="return beforeSaveData(this)" />

                        <asp:Button runat="server" CssClass="btn btn-success bigsize" Text="修改密码" ID="ChangePwd" Visible="false" OnClick="ChangePwd_Click"  OnClientClick="return beforeSaveData(this)" />

                        <asp:Button runat="server" CssClass="btn btn-success bigsize" Text="保存" ID="SaveBtn" OnClick="SaveBtn_Click" />
                    </div>
                </div>
            </div>
            <%--<div class="exitshopinfo maind" style="background-color:#fff;<%=enable ? "" : "display:none" %>">--%>
            <div class="exitshopinfo " style="display:none">
                <h3>客服管理<span style="font-size:13px;color:#999999;">（开通主账号以后才能添加客服）</span></h3>
                <div class="form-group">
                    <div class="col-xs-2">
                        <button type="button" class="btn btn-success bigsize" onclick="showModel(0)">添加客服</button>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-xs-8">
                        <table class="table table-bordered" id="custom">
                            <thead>
                                <tr>
                                    <th>客服昵称</th>
                                    <th>登录手机号</th>
                                    <th>密码</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="grdCustomers" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("nickname") %></td>
                                            <td><%# Eval("userver") %></td>
                                            <td><%# Eval("password") %></td>
                                            <td>
                                                <button type="button" class="btn btn-default" onclick="showModel('<%#Eval("id")%>')">编辑</button>
                                                <button type="button" class="btn btn-default" onclick="DeleteCustomer('<%#Eval("id")%>')">删除</button>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>     
        </div>

        <div class="modal fade" id="previewshow">
            <div class="modal-dialog">
                <div class="modal-content form-horizontal" id="hform">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="modaltitle" style="text-align: left">添加客服</h4>
                    </div>
                    <div class="modal-body">
                        <input type="hidden" id="htxtRoleId" runat="server" />                       

                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>客服昵称：</label>
                            <div class="form-inline">
                                <asp:TextBox runat="server" class="form-control" Width="200px" Style="margin-left: 15px;" ID="txt_name"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>登录手机号：</label>
                            <div class="form-inline">
                                <asp:TextBox runat="server" class="form-control" Width="200px" Style="margin-left: 15px;" ID="txt_cphone"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>登录密码：</label>
                            <div class="form-inline">
                                <asp:TextBox runat="server" class="form-control" Width="200px" Style="margin-left: 15px;" ID="txt_cpwd"></asp:TextBox>                               
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-success" data-dismiss="modal" onclick="AddCustomer();">确 定</button>
                        <asp:TextBox ID="txt_id" runat="server"  style="display:none" ></asp:TextBox>
                        <button type="button" class="btn btn-default" data-dismiss="modal">取 消</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
    </form>
    
</asp:Content>
