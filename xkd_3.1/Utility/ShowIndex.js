

$(function () {
    jQuery.getJSON("/api/Hi_Ajax_NavMenu.ashx", function (settingjson) {
        //底部导航
        menu(settingjson);
        //弹出圆圈菜单
        if (window.location.pathname.toLowerCase() == "/default.aspx") {
            AddMenu(settingjson);
        }
    });
})




function menu(settingjson) {
    if (window.location.pathname.toLowerCase() == "/default.aspx") {
        //if (settingjson.ShopDefault) {
        if (settingjson.EnableShopMenu) {
            var a = _.template($("#menu").html())(settingjson);
            //console.log(a);
            //导航代码
            //alert(_.template($("menu").html(), settingjson));
            //console.log(_.template($("#menu").html(), settingjson));
            $(_.template($("#menu").html())(settingjson)).appendTo('body');
            GetUICss();
        }
    }
    //else if (window.location.pathname == "/Vshop/MemberCenter.aspx") {
    //    if (settingjson.MemberDefault) {
    //        //个人中心
    //        $(_.template($("#menu").html())(settingjson)).appendTo('body');
    //        GetUICss();
    //    }
    //}
    else if (window.location.pathname.toLowerCase() == "/productsearch.aspx") {
        if (settingjson.GoodsType) {
            //分类首页
            $(_.template($("#menu").html())(settingjson)).appendTo('body');
            GetUICss();
        }
    }
    else if (window.location.pathname.toLowerCase() == "/productlist.aspx") {
        if (settingjson.GoodsListMenu) {
            //所有商品
            $(_.template($("#menu").html())(settingjson)).appendTo('body');
            GetUICss();
        }
    }
    else if (window.location.pathname.toLowerCase() == "/branddetail.aspx") {
        if (settingjson.BrandMenu) {
            //品牌专题
            $(_.template($("#menu").html())(settingjson)).appendTo('body');
            GetUICss();
        }
    }
    else if (window.location.pathname.toLowerCase() == "/activities.aspx") {
        if (settingjson.ActivityMenu) {
            //店铺活动
            $(_.template($("#menu").html())(settingjson)).appendTo('body');
            GetUICss();
        }
    }
    else if (window.location.pathname.toLowerCase() == "/productsearch.aspx") {
        if (settingjson.GoodsType) {
            //分类查询首页
            $(_.template($("#menu").html())(settingjson)).appendTo('body');
            GetUICss();
        }
    }
}


function GetUICss() {
    var oUl = $("#ul");
    var len = parseInt($("#ul > li").length);
    for (i = 0; i < $("#ul > li").length; i++) {
        var width = 100 / len;
        width+="%"
        $("#ul > li").eq(i).css("width", width);
    
    
    }
    $("#menuNav  div[data='1']").click(function () {
        if ($(this).find(".childNav").css("display") == "none") {
            $(this).find(".childNav").css("display", "block");
        } else {
            $(this).find(".childNav").css("display", "none");
        }
    });
}

