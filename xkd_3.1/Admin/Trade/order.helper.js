function CreateSelectToOrderListObj(obj, jsondata) {
    var selHtml = "";
    if ($(obj).attr("oid") != undefined && $(obj).attr("shipnum") != undefined) {
        selHtml = "<select style='height: 26px;margin-left: 12px;' name='comp' onchange='chageLogistics(this,\"" + $(obj).attr("oid") + "\")'><option value=''>请选择</option>";
        for (var i = 0; i < jsondata.length; i++) {
            if (jsondata[i].code == $(obj).attr("tid")) {
                selHtml += "<option value='" + jsondata[i].code + "' selected='selected'>" + jsondata[i].name + "</option>";
            } else {
                selHtml += "<option value='" + jsondata[i].code + "'>" + jsondata[i].name + "</option>";
            }
        }
        selHtml += "</select> <input type='text' class='resetText' value='" + $(obj).attr("shipnum") + "' maxlength='20' onkeyup=\"value=value.replace(/[W]/g,'')\">";
        $(obj).html(selHtml);
    }
}
function chageLogistics(obj, orderid) {
    if (obj.type == "select-one") {
        var objval = $(obj).val();
        var objname = $(obj).find("option:selected").text()
        if (objval.length > 0 && objname.length > 0 && orderid.length > 0) {
            var jdata = "{\"orderid\":\"" + orderid + "\",\"compname\":\"" + objname + "\",\"shipordernumber\":\"\",\"companycode\":\"" + objval + "\"}";
            var jsonTips = "[" + jdata + "]";
            $.ajax({
                url: "sendordergoods.aspx",
                type: "post",
                data: "posttype=saveoneorders&data=" + jsonTips,
                datatype: "json",
                success: function (json) {
                    if (json.type == "1") {
                        ShowMsg(json.tips, true);
                    } else {
                        ShowMsg(json.tips, false);
                    }
                }
            });
        }
    }
}
$(document).ready(function () {
    /*获取物流公司选择*/
    //$.ajax({
    //    url: "sendordergoods.aspx",
    //    type: "post",
    //    data: "posttype=getcompany&t=" + (new Date()).getTime(),
    //    datatype: "json",
    //    success: function (json) {
    //        if (json[0].type == "1") {
    //            var datalist = json[0].data;
    //            $(".logistics").each(function () {
    //                CreateSelectToOrderListObj(this, datalist);
    //            })
    //        }
    //    }
    //});
})