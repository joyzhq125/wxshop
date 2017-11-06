$(function () {
    $.get("/Api/Hi_Ajax_OnlineServiceConfig.ashx", function (data) {
        //$(data).appendTo('head');
        $(data).appendTo('body');
    });
});