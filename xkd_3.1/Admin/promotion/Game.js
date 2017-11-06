


function CheckPrzie(prizeType) {
    var prizeName = '';
    switch (prizeType) {
        case 0:
            {
                prizeName = '一等奖';
                break;
            }
        case 1:
            {
                prizeName = '二等奖';
                break;
            }
        case 2:
            {
                prizeName = '三等奖';
                break;
            }
        case 3:
            {
                prizeName = '普通奖';
                break;
            }
    }

    if ($("#rd" + prizeType + "_0")[0].checked) {
        var point0 = $("#txtGivePoint" + prizeType).val();
        if (!CheckValue(point0)) {
            ShowMsg('请输入' + prizeName + '赠送积分!');
            return false;
        }
        if (!$.isNumeric(point0)) {
            ShowMsg(prizeName + '赠送积分格式不对!');
            return false;
        }
    }
    if ($("#rd" + prizeType + "_1")[0].checked) {
        if (!CheckValue($("#seletCouponId" + prizeType).val())) {
            ShowMsg('请选择' + prizeName + '赠送优惠券!');
            return false;
        }
    }
    if ($("#rd" + prizeType + "_2")[0].checked) {
        if (!CheckValue($("#txtShopbookId" + prizeType).val())) {
            ShowMsg('请选择' + prizeName + '赠送商品!');
            return false;
        }
    }

    var prizeCount0 = $("#txtPrizeCount" + prizeType).val();
    if (!CheckValue(prizeCount0)) {
        ShowMsg('请输入' + prizeName + '奖品数量!');
        return false;
    }
    if (!$.isNumeric(prizeCount0)) {
        ShowMsg( prizeName + '奖品数量格式不对!');
        return false;
    }
    var prizeRate0 = $("#txtPrizeRate" + prizeType).val();
    if (!CheckValue(prizeRate0)) {
        ShowMsg('请输入' + prizeName + '中奖率!');
        return false;
    }
    if (!$.isNumeric(prizeRate0)) {
        ShowMsg(prizeName + '中奖率格式不对!');
        return false;
    }
    return true;
}

function CheckPrizeInfo() {
    if (!CheckPrzie(0)) return false;
    if (!CheckPrzie(1)) return false;
    if (!CheckPrzie(2)) return false;
    if (!CheckPrzie(3)) return false;
   
    return true;

}

function CheckValue(value) {
    if (value == '' || value == null) {
        return false;
    }
    return true;
}

function ShowSuccess()
{
    ShowStep2();
}

function ShowStep2() {
    $("#step1").hide();
    $("#step2").hide();
    $("#btnSubmit").show();
    $("#step3").show();
    //显示二维码
    winqrcode();
}

