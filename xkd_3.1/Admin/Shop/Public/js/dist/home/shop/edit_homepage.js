$(function () {
    //var a = $("#j-initdata").val(),
    //b = $("#j-pageID").val();

    //a = a.length ? $.parseJSON(a) : Defaults[b],
    //$(".j-pagetitle").text(a.page.title),
    //$(".j-pagetitle-ipt").val(a.page.title),
    var a;
    var b = $(pageID).val();
    var c = $(webID).val();
    $.ajax({
        url: HiShop.Config.AjaxUrl.getPage,
        type: 'GET',
        dataType: 'text',
        data: {
            id: b,
            wid:c
        },
        success: function (data) {
            a = data.length ? $.parseJSON(data) : Defaults[b];
            $(".j-pagetitle").text(a.page.title);
            $(".j-pagetitle-ipt").val(a.page.title);
            _.each(a.PModules,
             function (a, b) {
                 var c = 0 == b ? !0 : !1;
                 HiShop.DIY.add(a, c);
             });
             _.each(a.LModules,
             function (a) {
                 HiShop.DIY.add(a);
             });
        }
    }),
 
    $("#j-savePage").click(function () {
        //HiShop.Convert.ToHtml();
        return HiShop.DIY.Unit.getData() ? ($.ajax({
            url: HiShop.Config.AjaxUrl.savePage,
            type: "post",
            dataType: "json",
            data: {
                content: JSON.stringify(HiShop.DIY.Unit.getData()),
                id: b,
                getGoodGroupUrl: HiShop.Config.CodeBehind.getGoodGroupUrl,
                getGoodUrl: HiShop.Config.CodeBehind.getGoodUrl,
                is_preview: 0,
                wid: c
            },
            beforeSend: function () {
                $.jBox.showloading()
            },
            success: function (a) {
                1 == a.status ? HiShop.hint("success", "恭喜您，保存成功！") : HiShop.hint("danger", "对不起，保存失败：" + a.msg),
                $.jBox.hideloading()
            }
        }), !1) : void 0
    }),
    $("#j-saveAndPrvPage").click(function () {
        return HiShop.DIY.Unit.getData() ? ($.ajax({
            url: HiShop.Config.AjaxUrl.savePage,
            type: "post",
            dataType: "json",
            data: {
                content: JSON.stringify(HiShop.DIY.Unit.getData()),
                id: b,
                is_preview: 1,
                getGoodUrl: HiShop.Config.CodeBehind.getGoodUrl,
                getGoodGroupUrl: HiShop.Config.CodeBehind.getGoodGroupUrl
            },
            beforeSend: function () {
                $.jBox.showloading()
            },
            success: function (a) {
                1 == a.status ? (HiShop.hint("success", "恭喜您，保存成功！"), setTimeout(function () {
                    window.open(a.link)
                },
                1e3)) : HiShop.hint("danger", "对不起，保存失败：" + a.msg),
                $.jBox.hideloading()
            }
        }), !1) : void 0
    }),
    $("#j-homeRecover").click(function () {
        var a = ($(this), $(pageID).val());
        return $.jBox.show({
            title: "还原模板",
            content: "确认还原为初始状态吗？",
            btnOK: {
                onBtnClick: function (b) {
                    $.jBox.close(b),
                    $.ajax({
                        url: "/Shop/home_page_recover",
                        type: "post",
                        dataType: "json",
                        data: {
                            id: a,
                            getGoodGroupUrl:HiShop.Config.CodeBehind.getGoodGroupUrl
                        },
                        beforeSend: function () {
                            $.jBox.showloading()
                        },
                        success: function (a) {
                            1 == a.status ? (HiShop.hint("success", "恭喜您，恢复成功！"), setTimeout(function () {
                                window.location.reload()
                            },
                            1e3)) : HiShop.hint("danger", "对不起，恢复失败：" + a.msg),
                            $.jBox.hideloading()
                        }
                    })
                }
            }
        }),
        !1
    })
});