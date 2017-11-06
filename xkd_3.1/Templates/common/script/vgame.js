var gameData;

function InitInfo() {
    var gameId = GetGameId();
    $.ajax({
        url: "/API/Hi_Ajax_Game.ashx",
        type: 'post', dataType: 'text',
        data: { "action": "getprizelists", "gameId": gameId },
        async: false,
        success: function (data) {
            data = eval('(' + data + ')');
            if (data.status == '0') {
                alert_h(data.Desciption);
            } else {
                gameData = data;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log("请求出错了");
        }

    });
}

//判断是否还能玩
function IsCanPlay()
{
    var result = false;
    var gameId = GetGameId();
    $.ajax({
        url: "/API/Hi_Ajax_Game.ashx",
        type: 'post', dataType: 'json',
        data: { "action": "checkusercanplay", "gameId": gameId },
        async: false,
        success: function (data)
        {
            if (data.status == 1) {
                result = true;
            } else {
                alert_h(data.Desciption);
            }
        }
    });
    return result;
}
//抽奖
function GetPrize() {
    var gameId = GetGameId();
    var index = -1;
    $.ajax({
        url: "/API/Hi_Ajax_Game.ashx",
        type: 'post', dataType: 'json',
        data: { "action": "getprizeinfo", "gameId": gameId },
        async: false,
        success: function (data) {
            if (data.status == 0) {
                alert_h(data.Desciption);
            } else {
                index = Number(data.prizeIndex) + 1;
            }
        }
    });
    return index;

}
var pageIndex = 1;
var pageSize = 7;
function UserPrizeLists() {
    var gameId = GetGameId();
    $.ajax({
        url: "/API/Hi_Ajax_Game.ashx",
        type: 'post', dataType: 'json',
        data: { "action": "getuserprizelists", "gameId": gameId,"pageIndex":pageIndex,"pageSize":pageSize },
        async: false,
        success: function (data) {
            var emlement = "";
            $.each(data.lists, function (index, item) {
                emlement += " <ul><li>" + item.UserName + "</li><li>" + item.PrizeName + "</li><li>" + item.PrizeGrade + "</li><li>" + item.DateTime + "</li></ul>";
            });
            $("#userPrizeLists").append(emlement);
        }
    });
    pageIndex++;
    nDivHight = $(".nameList").outerHeight();
}

function GetGameId() {
    var gameId = window.location.search.substr(window.location.search.indexOf("=") + 1);
    if (gameId.indexOf("&") > 0)
        gameId = gameId.substr(0, gameId.indexOf("&"));
    return gameId;
}

var nDivHight;
$(function () {
    var nScrollHight = 0; //滚动距离总长(注意不是滚动条的长度)
    var nScrollTop = 0;   //滚动到的当前位置
    $(".nameList").scroll(function () {
        nScrollHight = $(this)[0].scrollHeight;
        nScrollTop = $(this)[0].scrollTop;
        if (nScrollTop + nDivHight >= nScrollHight)
            UserPrizeLists();
    });
});
