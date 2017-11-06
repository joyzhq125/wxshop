<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master"
    CodeBehind="ImageData.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.ImageData" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <Hi:Script ID="Script2" runat="server" Src="/Utility/swfupload/handlers.js"></Hi:Script>
    <Hi:Script ID="Script5" runat="server" Src="/utility/jquery.artDialog.js" />     
    <Hi:Script ID="Script7" runat="server" Src="/utility/iframeTools.js" />
    <Hi:Script ID="Script6" runat="server" Src="/utility/Window.js" />
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />   
    <Hi:Script ID="Script1" runat="server" Src="/Utility/swfupload/swfupload.js"></Hi:Script>
    
    <%--<link rel="stylesheet" href="/admin/css/css.css" type="text/css" media="screen" />--%>
    <link rel="stylesheet" href="/utility/skins/blue.css" type="text/css" media="screen" />

    <script type="text/javascript" src="/Utility/swfupload/swfobject.js"></script>
    <style type="text/css">
        .txtCss{color:#ffad34; font-size:14px; font-style:normal;}  
        .verticalalignTop{ vertical-align:top;}    
        .numCss{color:#ff6600; font-size:12px;}  
        .aCss{font-size:12px;}
        .typNameCss{color:#0b5ba5;}
        .liCss{margin-bottom:3px;}
        .table td {width:217px;height:217px;}
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#typeDiv').find('span').each(function () {
                if ($(this).attr('id') != 'ctl00_ContentPlaceHolder1_ImageTypeID')
                {
                    $(this).removeClass();
                    $(this).addClass('numCss');
                    $(this).css('margin-left', '2px');
                }                
            });
            $('#typeDiv').find('a').each(function () {
                $(this).removeClass();
                $(this).addClass('aCss');
                var url = $(this).attr('href');
                url = url.replace('/store/ImageData.aspx', '/Shop/ImageData.aspx');
                $(this).attr('href',url);
            });
            $('#typeDiv').find('li').each(function () {
                $(this).removeClass();
                $(this).addClass('liCss');
            });
        });


        function copySuccess() {
            alert("该地址已经复制，你可以使用Ctrl+V 粘贴！");
        }
        var myHerf = window.location.host;
        var myproto = window.location.protocol;
        applicationPath = "";
        function bindFlashCopyButton(value, containerID) {
            var flashvars = {
                content: encodeURIComponent(myproto + "//" + myHerf + applicationPath + value),
                uri: '/Utility/swfupload/flash_copy_btn.png'
            };
            var params = {
                wmode: "transparent",
                allowScriptAccess: "always"
            };
            swfobject.embedSWF("/Utility/swfupload/clipboard.swf", containerID, "23", "12", "9.0.0", null, flashvars, params);
        }

        function setdisplay(obj,val)
        {
            var a = $(obj).children("em")
            for(i=0;i<a.length;i++)
            {
                a[0].style.display = val==0?'none':'';
            }
        }
      


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">

        <div class="page-header">
            <h2>图片库</h2>
            <%--<small>查看服务器上所有图片进行管理和操作</small>--%>
        </div>

        <div class="form-inline" style="margin-left: 5px;">           
                
                    <label>图片数量：</label>
                    <span>
                        <a><asp:Label ID="lblImageData" runat="server" Text="111"></asp:Label></a>
                    </span>
                    
           
        </div>               
                        
            <!--结束-->      
            <table style="width:100%;margin-top:5px;">
                <tr>
                    <td width="77%">
                        <ul>
                            <li class="batchHandleButton">
                                <span class="signicon"></span>

                                <button type="button" class="btn btn-primary resetSize" onclick="CheckClickAll()">
                                    全选
                                    <%--<span class="glyphicon glyphicon-ok" aria-hidden="true"></span>--%>
                                </button>
                                <button type="button" class="btn btn-primary resetSize" onclick="CheckReverse()">
                                    反选
                                    <%--<span class="glyphicon glyphicon-fullscreen" aria-hidden="true"></span>--%>
                                </button>
                                <span style="padding:0px 5px 0px 5px;"><img src="../images/u5_line.png"></span>
                                <button type="button" class="btn btn-primary resetSize" onclick="MoveImg()">
                                    移动到
                                    <%--<span class="glyphicon glyphicon-ok" aria-hidden="true"></span>--%>
                                </button>
                                                                                              
                                <button type="button" id="btnDelete1" runat="server" class="btn btn-danger resetSize" onclick="javascript:if (!confirm('确定要执行该删除操作吗？删除后将不可以恢复！')) {return false;}">
                                    删除
                                    <%--<span class="glyphicon glyphicon-trash" aria-hidden="true"></span>--%>
                                </button>
                                <span style="padding:0px 5px 0px 5px;"><img src="../images/u5_line.png"></span>
                                <span><a href="imageftp.aspx">上传图片</a></span>
                                <span id="swfu_container"><span><span id="spanButtonPlaceholder"></span></span><span
                                    id="divFileProgressContainer"></span></span>

                            </li>
                        </ul>
                    </td>
                    <td width="20%" nowrap="nowrap">
                        <div class="form-inline">
                            <%--<label>请选择上传图片的分类：</label>
                            <Hi:ImageDataGradeDropDownList ID="dropImageFtp2" CssClass="form-control resetSize"
                                runat="server" NullToDisplay="请选择上传图片的路径" AutoPostBack="true" />--%>
                            <label>显示顺序:</label>
                            <Hi:ImageOrderDropDownList AutoPostBack="true" CssClass="form-control resetSize" runat="server"
                                ID="ImageOrder" />
                            <span>
                                <asp:TextBox ID="txtWordName" CssClass="form-control resetSize" Width="110" runat="server" />
                            </span>                    
                            <span>
                                <asp:Button CssClass="btn btn-primary resetSize" ID="btnImagetSearch" runat="server" Text="查询" />
                            </span>
                        </div>                                                                                              
                    </td>
                </tr>
            </table>

        <table style="margin-top:5px; margin-bottom:5px;">
            <tr>
                <td  style="width:80%;">      
                    <div id="imgDatadiv" style="width: auto; ">
                        <!--图片列表begin-->
                        <asp:DataList ID="photoDataList" runat="server" RepeatColumns="4" ShowFooter="False"
                            CssClass="table table-hover mar table-bordered"
                            ShowHeader="False" DataKeyField="PhotoId" CellPadding="0" RepeatDirection="Horizontal">
                            <ItemTemplate>
                                <div style="width: 200px; height: 200px; text-align: center; " onmouseover="setdisplay(this,1)" onmouseout="setdisplay(this,0)">
                                    <div style="width: 150px; height: 150px; margin-left: 25px; margin-right: 25px;">
                                        <a href='<%=GlobalsPath%><%# Eval("PhotoPath")%>' target="_blank" title="<%# Eval("PhotoName")%>">
                                            <img  src='<%=GlobalsPath%>/Admin/PicRar.aspx?P=<%# Eval("PhotoPath")%>&W=140&H=110' />
                                            <%--<img src="<%# Eval("PhotoPath")%>" style="width: 150px; height: 150px;" />--%>
                                            <asp:HiddenField ID="HiddenFieldImag" Value='<%# DataBinder.Eval(Container.DataItem, "PhotoPath")%>' runat="server" />
                                        </a>
                                    </div>
                                    <%# TruncStr(DataBinder.Eval(Container.DataItem, "PhotoName").ToString(), 20)%><br />
                                    <label>
                                        <asp:CheckBox ID="checkboxCol" runat="server" />选择</label>&nbsp;
                                 
                            <script> bindFlashCopyButton("<%# DataBinder.Eval(Container.DataItem, "PhotoPath")%>", 'spcopy<%#Container.ItemIndex+1 %>');</script>

                                    <br />
                                    <em style="display: none;">
                                        <span id="spcopy<%#Container.ItemIndex+1 %>" class="txtCss">复制</span>
                                        <a class="txtCss" href="javascript:RePlaceImg('<%# DataBinder.Eval(Container.DataItem, "PhotoPath")%>','<%# DataBinder.Eval(Container.DataItem, "PhotoId")%>')"
                                            style="margin: 0;">替换
                                        </a>
                                        <a class="txtCss" href="javascript:ReImgName('<%# DataBinder.Eval(Container.DataItem, "PhotoName")%>','<%# DataBinder.Eval(Container.DataItem, "PhotoId")%>')"
                                            style="margin: 0;">改名</a>
                                        <Hi:ImageLinkButton ID="btnDelPhoto" CssClass="txtCss" runat="server" Text="删除" IsShow="true" />
                                    </em>

                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                        <!--图片列表-->
                    </div>  
                     
                </td>
                <td style="width: 200px;vertical-align: top;">
                    <div style="margin-left: 5px;background-color: #F7F7F7;text-align: center;">
                        <div>
                            <label>图片分组</label>
                        </div>
                        <div id="typeDiv" style="width:100%;min-height:162px;">
                            <Hi:ImageTypeLabel runat="server" ID="ImageTypeID" />                           
                        </div>
                        <div style="padding-bottom:10px;">
                            <input type="button" class="btn btn-primary resetSize" value="分组管理" onclick="location.href = 'ImageType.aspx';" />
                        </div>
                    </div>
                </td>
            </tr>
            <tr style="display:none;">
                <td style="width: 30%; ">
                    <table style="width:100%;margin-top:5px;">
                        <tr>
                            <td>
                                <ul>
                                    <li>
                                        <span class="bottomSignicon"></span>


                                        <button type="button" class="btn btn-success btn-xs" onclick="CheckClickAll()">
                                            全选
                                    <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>
                                        </button>
                                        <button type="button" class="btn btn-success btn-xs" onclick="CheckReverse()">
                                            反选
                                    <span class="glyphicon glyphicon-fullscreen" aria-hidden="true"></span>
                                        </button>

                                        <button type="button" class="btn btn-success btn-xs" onclick="MoveImg()">
                                            移动到
                                            <%--<span class="glyphicon glyphicon-ok" aria-hidden="true"></span>--%>
                                        </button> 
                                        
                                         <button type="button" id="btnDelete2" runat="server" class="btn btn-danger btn-xs">
                                                    删除
                                            <span class="glyphicon glyphicon-trash" aria-hidden="true"></span>
                                         </button>

                              
                                    </li>
                                </ul>
                            </td>

                        </tr>
                    </table>
                </td>
                <td style="text-align: right;"></td>
            </tr>
            <tr>
                <td>
                    <!--翻页页码-->
                    <div class="pageNumber">
                        <div class="pagination">
                            <UI:Pager runat="server" ShowTotalPages="true" ID="pager" DefaultPageSize="20" />
                        </div>
                    </div>
                </td>
            </tr>
        </table>

    <div style="display: none">
        <asp:Button ID="btnSaveImageDataName" runat="server" Text="更换图片" CssClass="submit_sure" />
        <asp:Button ID="btnMoveImageData" runat="server" Text="文件移动" CssClass="submit_sure" />
    </div>


    <!--更改图片名称-->
    <div id="ImageDataWindowName" style="display: none">
        <div style="width: 430px">
            <div class="form-group">
                <asp:HiddenField ID="ReImageDataNameId" Value='' runat="server" />
                <label for="inputEmail3" class="col-xs-3 control-label">图片名称：</label>
                <div class="col-xs-3" style="width: 42%">
                    <asp:TextBox ID="ReImageDataName" runat="server" CssClass="form-control" Width="300px"></asp:TextBox>
                    <small class="help-block">图片名称不能为空,长度限制在30个字符以内</small>
                </div>
            </div>
        </div>
    </div>


    <!--图片路径替换-->
    <div id="ImageDataWindowFtp" style="display: none">
        <div class="frame-content">
            <asp:HiddenField ID="RePlaceImg" Value='' runat="server" />
            <asp:HiddenField ID="RePlaceId" Value='' runat="server" />
            <p>
                <span class="frame-span frame-input90">上传图片：<em>*</em></span>
                <asp:FileUpload ID="FileUpload" runat="server" onchange="FileExtChecking(this)" />
            </p>
        </div>
    </div>
    <!--文件移动-->
    <div id="ImageDataWindowMove" style="display: none">
        <div style="width: 430px">
            <div class="form-group">
                <label for="inputEmail3" class="col-xs-3 control-label">选择分组：</label>
                <div class="col-xs-3" style="width:75%">
                    <Hi:ImageDataGradeDropDownList ID="dropImageFtp" CssClass="form-control" runat="server" />
                 </div>
                
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" language="javascript">

        var formtype = "change";

        function validatorForm() {
            var imgsrc = "", imgid = "";
            switch (formtype) {
                case "change":
                    imgsrc = $("#ctl00_contentHolder_ReImageDataName").val().replace(/\s/g, "");
                    if (imgsrc.length <= 0) {
                        alert("图片名称不允许为空！");
                        return false;
                    }
                    break;
                case "remove":
                    if (!confirm("您确定要移动选中的图片吗？")) {
                        return false;
                    }
                    setArryText('ctl00_contentHolder_dropImageFtp', $("#ctl00_contentHolder_dropImageFtp").val());
                    break;
            };
            return true;
        }

        //文件移动
        
        //替换
        function RePlaceImg(imgSrc, imgId) {
            DialogFrame("../store/ImageReplace.aspx?imgsrc=" + imgSrc + "&imgId=" + imgId, '图片替换', 335, 140);
        }

        //反选
        function CheckReverse() {
            var frm = document.aspnetForm;
            for (i = 0; i < frm.length; i++) {
                e = frm.elements[i];
                if (e.type == 'checkbox' && e.name.indexOf('checkboxCol') != -1) {
                    if (e.checked == false)
                        e.checked = true;
                    else
                        e.checked = false;
                }
            }
        }

        //全选
        function CheckClickAll() {
            var frm = document.aspnetForm;
            for (i = 0; i < frm.length; i++) {
                e = frm.elements[i];
                if (e.type == 'checkbox' && e.name.indexOf('checkboxCol') != -1) {
                    e.checked = true;
                }
                if (e.type == 'checkbox' && e.name.indexOf('checkboxHead') != -1)
                    e.checked = false;
            }
        }

        var queueErrorArray;
        var swfu;
        $(function () {

            function loader(thing) {
                var settings = {
                    // Backend Settings
                    upload_url: "ImageData.aspx",
                    use_query_string: false,
                    post_params: {
                        iscallback: "true",
                        typeId: thing,
                        "ASPSESSID": "<%=Session.SessionID %>"
                    },
                    // File Upload Settings
                    file_size_limit: "501",
                    file_types: "*.jpg;*.gif;*.png;*.jpeg",
                    file_types_description: "JPG Images",
                    file_upload_limit: "0",    // Zero means unlimited

                    // Event Handler Settings - these functions as defined in Handlers.js
                    // The handlers are not part of SWFUpload but are part of my website and control how
                    // my website reacts to the SWFUpload events.
                    file_queue_error_handler: fileQueueError,
                    file_dialog_complete_handler: fileDialogComplete,
                    upload_progress_handler: uploadProgress,
                    upload_error_handler: uploadError,
                    upload_success_handler: uploadSuccess,
                    upload_complete_handler: uploadComplete,

                    // Button settings
                    button_image_url: "/DialogTemplates/images/swfupload_uploadBtn2.png",
                    button_placeholder_id: "spanButtonPlaceholder",
                    button_width: 63,
                    button_height: 22,

                    default_preview: "/DialogTemplates/images/07.png",

                    // Flash Settings
                    flash_url: "/DialogTemplates/swfupload/swfupload.swf", // Relative to this file
                    custom_settings: { upload_target: "divFileProgressContainer" }
                };
                //swfu = new SWFUpload(settings);

            };
            $("#ctl00_contentHolder_dropImageFtp2").change(function () {
                swfu.setPostParams({ "typeId": this.value, "iscallback": "true" });
            });
            loader(0);
        });


        //改名
        function ReImgName(imgName, imgId) {
            vilidsetings = {
                'ctl00$ContentPlaceHolder1$ImageDataWindowName': {
                    validators: {
                        notEmpty: {
                            message: '填写文件名称'
                        },
                        stringLength: {
                            min: 1,
                            max:30,
                            message: '长度不能超过30个字符'
                        }
                    }
                }
            };
            arrytext = null;
            formtype = "change";
            setArryText('ctl00_ContentPlaceHolder1_ReImageDataName', imgName);
            setArryText('ctl00_ContentPlaceHolder1_ReImageDataNameId', imgId);
            DialogShowNew('文件名称更改', 'imagecmp', 'ImageDataWindowName', 'ctl00_ContentPlaceHolder1_btnSaveImageDataName');
                                                                              
        }

        function MoveImg() {
            formtype = "remove";
            var frm = document.aspnetForm;
            var isFlag = false;
            for (i = 0; i < frm.length; i++) {
                var e = frm.elements[i];
                if (e.checked) {
                    isFlag = true;
                    break;
                }
            }
            if (isFlag) {
                setArryText('ctl00_ContentPlaceHolder1_dropImageFtp', '0');
                DialogShowNew("移动图片管理", "imagecmp", 'ImageDataWindowMove', 'ctl00_ContentPlaceHolder1_btnMoveImageData');
            }
            else
                alert("请选择需要移动的图片！");
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
                            //else if(btnId='ctl00_ContentPlaceHolder1_btnMoveImageData')
                            //{
                            //    setShowText(temparrytext);
                            //    this.close();
                            //    getArryText(temparrytext);
                            //    $("#" + btnId).trigger("click");
                            //}
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
    </script>
</asp:Content>


