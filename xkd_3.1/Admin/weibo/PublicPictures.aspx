<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master" CodeBehind="PublicPictures.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Weibo.PublicPictures" %>
 <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
      <link rel="stylesheet" href="/Admin/shop/Public/css/dist/component-min.css">
    <link rel="stylesheet" href="/Admin/shop/Public/plugins/jbox/jbox-min.css">

    <!-- diy css start-->
    <link rel="stylesheet" href="/Admin/shop/PublicMob/css/style.css">
  
    <link rel="stylesheet" href="/Admin/shop/Public/plugins/uploadify/uploadify-min.css">
    <script type="text/javascript" src="../js/weiboHelper.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     
    <!-- end ImgPicker -->
   
    <!-- end ModulePicker -->
    

    <script type="text/j-template" id="tpl_albums_main">
        <div id="albums" style="border:none;box-shadow:none;top:0;margin:-7px 0 0 -520px;  left: 521px;">
            <div class="albums-container clearfix">
                <div class="albums-cl fl">
                    <div class="albums-cl-actions">
                        <a href="javascript:;" id="j-addFolder"><i class="gicon-plus"></i><span>添加</span></a>
                        <a href="javascript:;" id="j-renameFolder"><i class="gicon-pencil"></i><span>重命名</span></a>
                        <a href="javascript:;" id="j-delFolder"><i class="gicon-trash"></i><span>删除</span></a>
                    </div>
                    <div class="albums-cl-tree" id="j-panelTree">
                        <p class="txtCenter pdt10 loading j-loading"><i class="icon-loading"></i></p>
                    </div>
                </div>
                <div class="albums-cr fl">
                    <div class="albums-cr-actions">
                        <input type="file" name="imgpicker_upload_input" id="j-addImg">
                        <a href="javascript:;" id="j-moveImg" class="btn btn-primary mgl10">移动图片到</a>
                        <a href="javascript:;" id="j-cateImg" class="btn btn-primary mgl5">移动分类图片到</a>
                        <a href="javascript:;" id="j-delImg" class="btn btn-danger mgl5">删除所选图片</a>
                        <input type="text" placeholder="请输入图片名称" style="width: 210px;padding:6px;vertical-align: 0;border-radius: 2px;border: 1px solid #ccc;"><a href="javascript:;" id="j-delImg" class="btn btn-primary mgl10 searchImg">搜索</a>
                    </div>
                    <div class="albums-cr-imgs" id="j-panelImgs">
                        <p class="txtCenter pdt10 loading j-loading"><i class="icon-loading"></i></p>
                    </div>
                    <div class="albums-cr-ctrls clearfix">
                        <a href="javascript:;" id="j-useImg" class="btn btn-primary fl">使用选中的图片</a>
                        <div class="paginate fr" id="j-panelPaginate"></div>
                    </div>
                </div>
            </div>
        </div>
    </script>
    <!-- end tpl_albums_main -->

    <script type="text/j-template" id="tpl_albums_overlay">
      <%--  <div id="albums-overlay"></div>--%>
    </script>
    <!-- end tpl_albums_overlay -->

    <script type="text/j-template" id="tpl_albums_tree">
        <dl class="j-albumsNodes">
            <dt data-id="-1" data-add="1" data-rename="0" data-del="0">
                <i class="icon-folder open"></i>
                <span class="j-treeShowTxt"><em class="j-name">所有图片</em>(<em class="j-num"><#=dataset.total#></em>)</span>
            </dt>
            <dd><#=nodes#></dd>
        </dl>
    </script>
    <!-- end tpl_albums_tree -->

    <script type="text/j-template" id="tpl_albums_tree_fn">
        <# _.each(dataset,function(item){#>
        <dl>
            <#if(item.id==0){#>
            <dt data-id="<#=item.id#>" data-add="0" data-rename="0" data-del="0">
                <#}else{#>
            <dt data-id="<#=item.id#>" data-add="1" data-rename="1" data-del="1">
                <#}#>
                <#if(item.subFolder && item.subFolder.length){#>
                <i class="icon-folder open"></i>
                <#}else{#>
                <i class="icon-folder"></i>
                <#}#>
                <span class="j-treeShowTxt"><em class="j-name"><#=item.name#></em>(<em class="j-num"><#=item.picNum#></em>)</span>
                <#if(item.id!=0){#>
                <input type="text" class="ipt j-ip" maxlength="10" value="<#=item.name#>"><i class="icon-loading j-loading"></i>
                <#}#>
            </dt>
            <dd>
                <#if(item.subFolder && item.subFolder.length){#>
                <#= templateFn({dataset:item.subFolder, templateFn:templateFn}) #>
                <#}#>
            </dd>
        </dl>
        <#})#>
    </script>

    <script type="text/j-template" id="tpl_albums_delFolder">
        <div>
            <p class="ftsize14 bold">删除该文件夹同时会删除其子文件夹，是否继续？</p>
            <div class="radio-group mgt5">
                <label><input type="radio" name="isDelImgs" value="1" checked>不删除图片</label>
                <label><input type="radio" name="isDelImgs" value="2">同时删除图片</label>
            </div>
        </div>
    </script>
    <!-- end tpl_albums_delFolder -->

    <script type="text/j-template" id="tpl_albums_imgs">
        <#if(dataset){#>
        <ul class="clearfix">

            <# _.each(dataset,function(item,index){ #>
            <li class="fl" data-id="<#=item.id#>">
                <img src="<#=item.file#>">
                <div class="albums-cr-imgs-selected"><i></i></div>
                <div class="albums-edit">
                    <span><i class="gicon-pencil edit-img-name"></span></i>
                    <p><#=item.name#></p>
                    <div class="img-name-edit">
                        <input type="text" value="<#=item.name#>" style="width:60%;" name="rename" class="file_name" />
                        <a href="javascript:;" class="renameImg">确定</a>
                    </div>
                </div>
            </li>
            <# }) #>
        </ul>
        <#}else{#>
        <p class="albums-cr-imgs-noPic j-noPic">暂无图片</p>
        <#}#>
    </script>
    <!-- end tpl_albums_imgs -->
     <script src="/Admin/shop/Public/js/dist/lib-min.js"></script>
    <script src="/Admin/shop/Public/plugins/jbox/jquery.jbox-min.js"></script>
    <script src="/Admin/shop/Public/plugins/zclip/jquery.zclip-min.js"></script>
    <script src="/Admin/shop/Public/js/dist/component-min.js"></script>
    <script src="/Admin/shop/Public/plugins/uploadify/jquery.uploadify.min.js?ver=940"></script>
    <script src="/Admin/shop/Public/js/jquery-ui/jquery-ui.min.js"></script>
    <script src="/Admin/shop/Public/js/config.js"></script>
    


    <script type="text/javascript">
        $(document).ready(function () {
            $(".ui-header.navbar-fixed-top").css("display", 'none');
            HiShop.popbox.ImgPicker(function (a) {
              
                gotovalue(a);
            });
            $('.container').css({
                marginTop: -10,
                width: "1040px"
 
            });
            function gotovalue(obj) {
              
                window.parent.closeModalPic("myModal", obj);
            }
        })
    </script>



</asp:Content>