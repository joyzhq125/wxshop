<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Hidistro.UI.Web.Vshop.Default" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.CodeBehind" Assembly="Hidistro.UI.SaleSystem.CodeBehind" %>
<%@ Register TagPrefix="H2" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<!DOCTYPE html>
<html lang="en-US">
<head>
    <meta charset="UTF-8" />
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=1" name="viewport" />
    <meta http-equiv="content-script-type" content="text/javascript">
    <meta name="format-detection" content="telephone=no" />
    <!-- uc强制竖屏 -->
    <meta name="screen-orientation" content="portrait">
    <!-- QQ强制竖屏 -->
    <meta name="x5-orientation" content="portrait">
    <title>店铺首页</title>
    <!-- <link rel="stylesheet" href="/PublicMob/css/style.css"> -->
    <link rel="stylesheet" href="/Admin/Shop/PublicMob/css/dist/style.css">
    <link rel="stylesheet" href="/Admin/Shop/PublicMob/css/SpsBtn.css">
     <link rel="stylesheet" href="/Admin/Shop/PublicMob/css/Menu.css">
    <link rel="stylesheet" href="<%=cssSrc %>">
</head>
<body style="">
    
    <div class="membersbox pad50 " id ="divCommon" style="max-width:640px; margin:0 auto;position:relative;">
            <Hi:HomePage runat ="server" ID="H_Page" ></Hi:HomePage>
<%--        <section class="members_bottom">
            <section>
                <a href="/Default.aspx">商城首页<i></i></a>
                <a href="/ProductList.aspx">所有商品<i></i></a>
                <a href="/Vshop/MemberCenter.aspx">会员中心<i></i></a>
                <a href="/Vshop/ShoppingCart.aspx">购物车<i></i></a>
                <a href="/Vshop/DistributorCenter.aspx">分销申请<i></i></a>
            </section>
        </section> --%>   
        <section class="members_bottom">
            <section style="height:48px;"><a href="http://www.hishop.com.cn/support/" target="_blank" style="color: #b3b3b3;">辰多技术支持 </a></section>
        </section>
    </div>

    <div id="mmexport"><img src="/Admin/Shop/PublicMob/images/mmexport.png" width="100%" alt=""></div>

    <!--关注-->
    <!-- 悬浮按钮 -->
    <div class="mask_menu" id="menubtn" style="display: none;"></div>
    <div class="menu" id="menufloat"><i class="icon-menu"></i></div>
    <div class="menu-c notel nofocus" id="menufloat-c" style="">
        <div class="menu-c-out">
            <div class="menu-c-inner" id ="menuIn">
            
            </div>
        </div>
    </div>    <!-- 收藏 -->
    <div class="collectbg"><img src="/Admin/Shop/PublicMob/images/collectbg.png" width="100%" alt=""><a href="javascript:;" class="a-know">我知道了</a></div>
    <!-- 分享 -->
    <div class="sharebg"><img src="/Admin/Shop/PublicMob/images/mmexport.png" width="100%" alt=""><a href="javascript:;" class="a-know">我知道了</a></div>
    <script src="/Admin/Shop/PublicMob/js/dist/lib-min.js"></script>
    <script src="/Admin/Shop/PublicMob/js/dist/main.js"></script>
     
    <!--<script src="/PublicMob/js/dist/cbb_jssdk.js"></script>-->
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <H2:weixinset ID ="weixin" runat="server"></H2:weixinset>
      <script src="/Utility/OnlineCustomerService.js"></script>
      <%--<script src="/Admin/shop/Public/js/dist/lib-min.js"></script>--%>
      <script src="/Admin/shop/Public/js/dist/underscore.js"></script>
      <script src="/Utility/ShowIndex.js"></script>
      <script src="/Utility/IndexSuspendMenu.js"></script>
  
   
    <script>
        $(function () {
            //AddMenu();           
            // 点击我要分销功能
            $(document).on('click', '#fxBtn', function () {
                $("#mmexport").show();
            });
            $(document).on('touchend', '#fxBtn', function () {
                $('#fxBtn').click();
            })
            // $('#fxBtn').on('touchend',function(){
            //     $(this).click();
            // })
            $(document).on('touchend', "#mmexport", function () {
                $(this).hide();
            });
           (function () {
                 // 控制添加商品的图片显示高度，确保商品布局正常
                 $('.b_mingoods,.mingoods').each(function (index, el) {
                     var me = $(this),
                         imgHeight = me.find('img').width();
                     me.find('img').closest('a').height(imgHeight);
                 });
                 $('.board3').each(function (index, el) {
                     var me = $(this);
                     var bwidth = me.width();
                     if (me.hasClass('small_board') || !me.hasClass('big_board')) {
                         me.children('span').attr('style', 'height:' + bwidth + 'px !important;overflow:hidden;');
                     }
                     if (me.hasClass('big_board')) {
                         me.children('span').attr('style', 'height:' + (bwidth * 2 + 10) + 'px !important;overflow:hidden;');
                     }
                 });
                 $('.mdetail_goodsimg ul li').each(function (index, el) {
                     var me = $(this);
                     var imgWidth = me.width();
                     me.height(imgWidth);
                 });
                 // 悬浮按钮
                 var SpsBtn = {
                     config: {
                         menu: document.getElementById('menufloat'),
                         menusub: document.getElementById('menufloat-c'),
                         menubtn: document.getElementById('menubtn')
                     },
                     init: function () {
                         this.SpsbtnClick();
                         this.touchMove();
                     },
                     touchMove: function () {
                         var _this = this;
                         _this.config.menu.addEventListener('touchmove', function (e) {
                             e.preventDefault();
                             var touch = e.touches[0],
                                 moveX, moveY,
                                 winWh = window.innerWidth - 50,
                                 winHt = window.innerHeight - 50;
                             moveX = touch.clientX - 25;
                             moveY = touch.clientY - 25;
                             moveY = moveY < 0 ? 0 : moveY;
                             moveX = moveX < 0 ? 0 : moveX;
                             moveY = moveY > winHt ? winHt : moveY;
                             moveX = moveX > winWh ? winWh : moveX;
                             _this.config.menu.style.left = moveX + 'px';
                             _this.config.menu.style.top = moveY + 'px';
                             _this.config.menusub.style.left = moveX + 10 + 'px';
                             _this.config.menusub.style.top = moveY - 190 + 'px';
                         }, false)


                     },
                     SpsbtnClick: function () {
                         var _this = this;
                         this.config.menu.addEventListener('click', function () {
                             var me = $(this);
                             // console.log(me)
                             if (!me.hasClass('show')) {
                                 me.addClass('show');
                                 me.siblings('.mask_menu,#menufloat-c').show();
                                 me.siblings('#menufloat-c').find('.menu-c-inner').addClass('in').removeClass('outer')
                             } else {
                                 me.removeClass('show');
                                 me.siblings('.mask_menu,#menufloat-c').hide();
                                 me.siblings('#menufloat-c').find('.menu-c-inner').removeClass('in').addClass('outer')
                             }
                         }, false);
                         this.config.menubtn.addEventListener('click', function () {
                             $(_this.config.menu).removeClass('show');
                             $(_this.config.menu).siblings('.mask_menu,#menufloat-c').hide();
                             $(_this.config.menu).siblings('#menufloat-c').find('.menu-c-inner').removeClass('in').addClass('outer')
                         })
                     }
                 }
                 SpsBtn.init();          
  
                 $('.a-know').click(function (event) {
                     $(this).parent('.collectbg,.sharebg').hide();
                 });
             
             })();
        });
    </script>

    <script type="text/javascript" src="/Admin/Shop/PublicMob/plugins/swipe/swipe.js"></script>
    <script>
        // var elem = document.getElementById('mySwipe');
        // window.mySwipe = Swipe(elem, {
        //    startSlide: 0,
        //    auto: 3000,
        //    callback: function(m) {
        //    	 indexs.eq(m).addClass('cur').siblings().removeClass('cur')
        //    },
        // });
        // window.indexs = $(".members_flash_time span");
        $(function () {
            $.getJSON("/api/Hi_Ajax_GetProductsCount.ashx", function (data) {
                $("#goodsCount").html(data.count);
                if ($("#StoreName").length > 0)
                {
                    $("#StoreName").html(data.storeName);
                }
            })
            $('.j-swipe').each(function (index, el) {
                var me = $(this);
                me.attr('id', 'Swiper' + index);
                var id = me.attr('id');
                // alert(id)
                var elem = document.getElementById(id);
                window.mySwipe = Swipe(elem, {
                    startSlide: 0,
                    auto: 3000,
                    callback: function (m) {
                        $(elem).find('.members_flash_time').children('span').eq(m).addClass('cur').siblings().removeClass('cur')
                    },
                });
            });
        });


        wx.ready(function () {
            wx.onMenuShareAppMessage({
                title: '<%=siteName%>',
                desc: '<%=Desc%>',
                link: window.location.href+"?userID=<%=memberID%>",
                imgUrl: '<%=imgUrl%>',
                type: "",
                dataUrl: "",
                success: function () { },
                cancel: function () { }
            }),
            wx.onMenuShareTimeline({
                title: '<%=siteName%>',
                link: window.location.href + "?userID=<%=memberID%>",
                imgUrl: '<%=imgUrl%>',
                success: function () { },
                cancel: function () { }
            }),
            wx.onMenuShareQQ({
                title: '<%=siteName%>',
                desc: '<%=Desc%>',
                link: window.location.href + "?userID=<%=memberID%>",
                imgUrl: '<%=imgUrl%>',
                success: function () { },
                cancel: function () { }
            }),
            wx.onMenuShareWeibo({
                title: '<%=siteName%>',
                desc: '<%=Desc%>',
                link: window.location.href + "?userID=<%=memberID%>",
                imgUrl: '<%=imgUrl%>',
                success: function () { },
                cancel: function () { }
            })
        })
    </script>
    <!--在线客服 -->
    <script type="text/javascript">
       
    </script>


<%--<script type='text/javascript'>
    (function(m, ei, q, i, a, j, s) {
        m[i] = m[i] || function() {
            (m[i].a = m[i].a || []).push(arguments)
        };
        j = ei.createElement(q),
        s = ei.getElementsByTagName(q)[0];
        j.async = true;
        j.charset = 'UTF-8';
        j.src = '//static.meiqia.com/dist/meiqia.js?_=t';
        s.parentNode.insertBefore(j, s);
    })(window, document, 'script', '_MEIQIA');
    _MEIQIA('entId', 64612);
</script>--%>


<script type="text/j-template" id="menu">
  <div class="menuNav" id="menuNav">
      <ul id="ul">
                <# _.each(menuList,function(item){ #>
                <li>
                   
                      <# if(item.SubMenus.length > 0){ #>
                        <div class="navcontent" data="1">
                        <# if(item.ShopMenuPic.length > 0){ #>
                            <img src="<#= item.ShopMenuPic#>" style="width :20px;height:20px;" />
                        <# } #>
                         <#= item.Name #>
                        
    	                <div class="childNav">
                        <ul>
                             <# _.each(item.SubMenus,function(chlitem){ #>
                                <li><a href="<#= chlitem.Content #>"><#= chlitem.Name #></a></li>
                             <# }) #>
                       </ul>
                       </div>
                
                    <# } else{ #>
                           <div class="navcontent" data="0">
                             <a href="<#= item.Content #>">
                           <# if(item.ShopMenuPic.length > 0){ #>
                            <img src="<#= item.ShopMenuPic#>" style="width :20px;height:20px;" />
                            <# } #>
                           <#= item.Name #></a>
                     <# } #>
                    </div>
                </li>
                <# }) #>
        </ul>
</div>
</script>
</body>
</html>