<%@ Page Language="C#" AutoEventWireup="true" CodeFile="first.aspx.cs" Inherits="XKD.Web.Admin.first" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta charset="UTF-8">
    <title>AppId管理中心</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../css/first.css" media="all">
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>

    <script type="text/javascript" charset="utf-8" src="../statistics/js/highcharts.js"></script>
    <script type="text/javascript" charset="utf-8" src="../statistics/js/exporting.js"></script>

    <script type="text/javascript">
        //BI显示
        function ShowReport(_title, _url) {
            var winDialog = top.dialog({
                title: _title,
                url: _url,
                width: 700,
                height: 450
            }).showModal();
        }

        function showAppDetails(id) {
            var dialog = top.dialog({
                title: "服务号信息",
                url: "app_view.aspx?id=" + id,
                width: 700,
                height: 600
            }).showModal();
        }

        function appNumListManage() {
            parent.location.href = 'frame.aspx';
        }



        $(function () {
            $('#container').highcharts({
                chart: {
                    type: 'spline'
                },
                title: {
                    text: '',
                    x: -20 //center
                },

                xAxis: {
                    categories: [<%=orders_Tendency[0] %>]
                },
                yAxis: {
                    title: {
                        text: ''
                    },
                    min: 0,
                    allowDecimals: false,
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                credits: {
                    enabled: false // 禁用版权信息
                },
                exporting: {
                    enabled: false
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },
                series: [{
                    name: "订单数",
                    data: [<%=orders_Tendency[1] %>],
                    showInLegend: false
                }]
            });

            Highcharts.theme = {
                colors: ["#66cc33", "#ffcc33", "#ff3333", "#7798BF", "#aaeeee", "#ff0066", "#eeaaee",
                    "#55BF3B", "#DF5353", "#7798BF", "#aaeeee"],
                chart: {
                    backgroundColor: null,
                    style: {
                        fontFamily: "Dosis, sans-serif"
                    }
                },
                title: {
                    style: {
                        fontSize: '16px',
                        fontWeight: 'bold',
                        textTransform: 'uppercase'
                    }
                },
                tooltip: {
                    borderWidth: 0,
                    backgroundColor: 'rgba(219,219,216,0.8)',
                    shadow: false
                },
                legend: {
                    itemStyle: {
                        fontWeight: 'bold',
                        fontSize: '13px'
                    }
                },
                xAxis: {
                    gridLineWidth: 1,
                    labels: {
                        style: {
                            fontSize: '12px'
                        }
                    }
                },
                yAxis: {
                    minorTickInterval: 'auto',
                    title: {
                        style: {
                            textTransform: 'uppercase'
                        }
                    },
                    labels: {
                        style: {
                            fontSize: '12px'
                        }
                    }
                },
                plotOptions: {
                    candlestick: {
                        lineColor: '#404048'
                    }
                },


                // General
                background2: '#F0F0EA'

            };

            // Apply the theme
            Highcharts.setOptions(Highcharts.theme);


            $('#container1').highcharts({
                chart: {
                    type: 'spline',
                    animation: Highcharts.svg, // don't animate in old IE
                    marginRight: 10,
                    events: {
                        load: function () {

                            // set up the updating of the chart each second
                            var series0 = this.series[0];
                            var series1 = this.series[1];
                            var series2 = this.series[2];
                            setInterval(function () {
                                var x = (new Date()).getTime(); // current time
                                x += 28800000;
                                y0 = Math.random();
                                y1 = Math.random();
                                y2 = Math.random();
                                series0.addPoint([x, y0], true, true);
                                series1.addPoint([x, y1], true, true);
                                series2.addPoint([x, y2], true, true);
                            }, 1000);
                        }
                    }
                },
                title: {
                    text: ''
                },
                xAxis: {
                    type: 'datetime',
                    tickPixelInterval: 150
                },
                yAxis: {
                    title: {
                        text: ''
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                tooltip: {
                    formatter: function () {
                        return '<b>' + this.series.name + '</b><br/>' +
                            Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x) + '<br/>' +
                            Highcharts.numberFormat(this.y, 2);
                    }
                },
                legend: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                series: [{
                    name: 'Random data',
                    data: (function () {
                        // generate an array of random data
                        var data = [],
                            time = (new Date()).getTime(),
                            i;

                        for (i = -19; i <= 0; i += 1) {
                            data.push({
                                x: time + i * 1000 + 28800000,
                                y: Math.random()
                            });
                        }
                        return data;
                    }())
                }, {
                    name: 'Random data',
                    data: (function () {
                        // generate an array of random data
                        var data = [],
                            time = (new Date()).getTime(),
                            i;

                        for (i = -19; i <= 0; i += 1) {
                            data.push({
                                x: time + i * 1000 + 28800000,
                                y: Math.random()
                            });
                        }
                        return data;
                    }())
                }, {
                    name: 'Random data',
                    data: (function () {
                        // generate an array of random data
                        var data = [],
                            time = (new Date()).getTime(),
                            i;

                        for (i = -19; i <= 0; i += 1) {
                            data.push({
                                x: time + i * 1000 + 28800000,
                                y: Math.random()
                            });
                        }
                        return data;
                    }())
                }]
            });


        });
    </script>
</head>
<body>

    <div class="main-area">
        <div id="main">
            <div class="index-overview">
                <div style="min-height: 838px;" class="content-wrap">
                    <div class="account-preview">
                        <div class="account-left">
                            <div class="inner-panel">
                                <i class="avatar" style="background:url('<%=avatar_url %>') no-repeat scroll 0px 0px;"></i>
                                <a class="user-name-link" href="###" title="<%=business_name %>">
                                    <span class="user-name"><%=business_name %></span>
                                </a>

                                <a id="overview-verify" href="###" title="通过官方实名认证">
                                    <i class="iconfont icon-verify verify-pass"></i>
                                </a>

                                <a id="phoneBound" href="###" title="通过手机认证">
                                    <i class="iconfont icon-verify-mobile <%=phone_verify %>"></i>
                                </a>
                                <a id="emailBound" href="###" title="通过邮箱认证">
                                    <i class="iconfont icon-verify-email <%=email_verify %>"></i>
                                </a>
                                <div class="item cash-balance">
                                    <span>总营业额：</span>
                                    <span class="money">￥</span>
                                    <a href="#" id="cash-label"><%=total_money %></a>

                                    <a class="btn-recharge" href="###" onclick="appNumListManage();">服务号管理</a>
                                    <a class="btn-recharge" style="display: none;" href="###">资料修改</a>
                                </div>
                                <div class="item coupon-balance">
                                    <span>总订单数：</span>
                                    <!--  <a id="coupon-num" href="###">0</a>-->
                                    <a id="A2" href="###"><%=total_orders_count %></a>
                                    <span>笔</span>&nbsp;&nbsp;&nbsp;

                                     <span>总客户数：</span>
                                    <a id="A1" href="###"><%=total_customers_count %></a>
                                    <span>人</span>&nbsp;&nbsp;&nbsp;

                                    <span>服务号：</span>
                                    <a id="A3" href="###"><%=total_appid_count %></a>
                                    <span>个</span>&nbsp;&nbsp;&nbsp;

                                </div>
                                <div class="item" ><a href="###">今日新增：</a></div>
                            </div>
                            <div class="bottom-panel">
                                <a href="manager/message_list.aspx" class="message">
                                    <i class="iconfont icon-ses"></i>通知<b id="mes-num"><%=today_notices_count %></b>条
                                </a>
                                <a href="###" class="ticket">
                                    <i class="iconfont icon-ticket"></i>订单<b id="ticket-num"><%=today_orders_count %></b>笔
                                </a>
                                <a href="###" class="orderlist">
                                    <i class="iconfont icon-order-list"></i>客户<b id="B1"><%=today_customers_count %></b>人
                                </a>
                            </div>
                        </div>
                        <div class="account-middle">
                            <h2><span>订单走势</span>
                            </h2>
                            <div style="background-color: transparent;width:235px;" id="container" class="account-preview-charts ui-ctrl ui-echart">
                            </div>

                            <div class="account-preview-summary">
                                <p>消费金额：</p>
                                <p>
                                    <span class="money">￥</span>
                                    <label data-ctrl-view-context="e" data-ctrl-id="costSummary" id="ctrl-e-costSummary" data-ui-type="Label" data-ui-id="costSummary" class="cost-summary ui-ctrl ui-label"><%=orders_Tendency[2] %></label>
                                </p>
                                <p class="account-detail">
                                    <a class="account-detail-button" href="###">
                                        <i class="iconfont icon-arrow-right"></i>订单管理</a>
                                </p>
                            </div>
                        </div>
                        <div class="account-right">
                            <div class="product account-section">
                                <a href="###" class="product-name" style="margin: 5px 0px 30px 10px">
                                    <i class="iconfont icon-bcm"></i>
                                    <b>数据监控</b>
                                    <div id="container1" style="width: 190px; height: 100px; margin: 0 auto"></div>
                                </a>
                                <div class="product-spec" id="bcm-spec">
                                    <a class="spec-row" href="###">
                                        <span class="spec-key">
                                            <i class="iconfont icon-status normal"></i>正常
                                        </span>
                                        <span class="spec-value" id="bcmNormal">0</span>
                                    </a>
                                    <a class="spec-row" href="###">
                                        <span class="spec-key">
                                            <i class="iconfont icon-status warning"></i>数据不足
                                        </span>
                                        <span class="spec-value" id="bcmWarning">0</span>
                                    </a>
                                    <a class="spec-row" href="###">
                                        <span class="spec-key">
                                            <i class="iconfont icon-status error"></i>异常
                                        </span>
                                        <span class="spec-value" id="bcmError">0</span>
                                    </a>
                                </div>
                            </div>
                            <div class="product account-section last">
                                <a href="###" class="product-name">
                                    <i class="iconfont icon-bss"></i>
                                    <b>账户安全</b>
                                </a>
                                <a class="high" href="###" id="safe-level">
                                    <span>安全等级</span>
                                    <span id="safe-level-label">高</span>
                                    <div class="level-wrap">
                                        <div class="level-inner">
                                        </div>
                                    </div>
                                </a>
                            </div>
                        </div>
                    </div>


                    <!--已开通服务-->
                    <div class="pro-wrap" id="active">
                        <h2>
                            <i class="iconfont icon-ok active-icon"></i><span>已开通服务</span> - <span class="region">Micro channel public number</span>
                        </h2>

                        <%=open_appList %>
                    </div>
                    <!--/已开通服务-->


                    <div class="pro-wrap" id="deactive">
                    <h2>
                        <i class="iconfont icon-waiting deactive-icon"></i><span>BI智能报表分析</span>
                    </h2>
                    <div class="deactive-table-wrap">
                        <div class="product-row">
                            <div class="product">
                                <h3>订单报表分析-(Report)
                                </h3>
                                <div>

                                    <div class="product-inner-row">
                                        <a href="#" class="un-product-name">
                                            <i class="iconfont icon-bcc"></i>当前公众号订单报表统计</a>
                                        <span class="product-links">
                                            <a href="javascript:ShowReport('当前公众号订单报表统计','statistics/reportapp.aspx');"><i class="iconfont icon-buy"></i>查看</a>
                                        </span>
                                    </div>

                                    <div class="product-inner-row">
                                        <a href="#" class="un-product-name">
                                            <i class="iconfont icon-blb"></i>商户旗下所有订单报表统计</a>
                                        <span class="product-links">
                                            <a href="javascript:ShowReport('商户旗下所有订单报表统计','statistics/reportapplist.aspx');"><i class="iconfont icon-buy"></i>查看</a>
                                        </span>
                                    </div>

                                </div>
                            </div>

                            <div class="product">
                                <h3>订单走势分析-(Trend)
                                </h3>
                                <div>

                                    <div class="product-inner-row">
                                        <a href="#" class="un-product-name">
                                            <i class="iconfont icon-cds"></i>当前公众号订单走势分析</a>
                                        <span class="product-links">
                                            <a href="javascript:ShowReport('当前公众号订单走势分析','statistics/trendapp.aspx');"><i class="iconfont icon-buy"></i>查看</a>
                                        </span>
                                    </div>

                                    <div class="product-inner-row">
                                        <a href="#" class="un-product-name">
                                            <i class="iconfont icon-cdn"></i>商户旗下所有订单走势分析</a>
                                        <span class="product-links">
                                            <a href="javascript:ShowReport('商户旗下所有订单走势分析','statistics/trendapplist.aspx');"><i class="iconfont icon-buy"></i>查看</a>
                                        </span>
                                    </div>

                                </div>
                            </div> 
                            <div class="product">

                                <h3>商品销量环比分析-(Quarter-On-Quarter)
                                </h3>
                                <div>

                                    <div class="product-inner-row">
                                        <a href="#" class="un-product-name">
                                            <i class="iconfont icon-rds"></i>当前公众号商品销量环比分析</a>
                                        <span class="product-links">
                                            <a href="javascript:ShowReport('当前公众号商品销量环比分析','statistics/quarterapp.aspx');"><i class="iconfont icon-buy"></i>查看</a>
                                        </span>
                                    </div>

                                    <div class="product-inner-row">
                                        <a href="#" class="un-product-name">
                                            <i class="iconfont icon-scs"></i>商户旗下所有商品销量环比分析</a>
                                        <span class="product-links">
                                            <a href="javascript:ShowReport('商户旗下所有商品销量环比分析','statistics/quarterapplist.aspx');"><i class="iconfont icon-buy"></i>查看</a>
                                        </span>
                                    </div>

                                </div>
                            </div>

                        </div>



                        <div class="product-row">
                            <div class="product">
                                <h3>客户数据分析-(Customer)
                                </h3>
                                <div>

                                    <div class="product-inner-row">
                                        <a href="#" class="un-product-name">
                                            <i class="iconfont icon-bmr"></i>当前公众号客户数据分析</a>
                                        <span class="product-links">
                                            <a href="javascript:ShowReport('当前公众号客户数据分析','statistics/customerapp.aspx');"><i class="iconfont icon-buy"></i>查看</a>
                                        </span>
                                    </div>

                                    <div class="product-inner-row">
                                        <a href="#" class="un-product-name">
                                            <i class="iconfont icon-mct"></i>商户旗下所有客户数据分析</a>
                                        <span class="product-links">
                                            <a href="javascript:ShowReport('商户旗下所有客户数据分析','statistics/customerapplist.aspx');"><i class="iconfont icon-buy"></i>查看</a>
                                        </span>
                                    </div>
                                </div>
                            </div>

                            <div class="product">

                                <h3>客户来源分析-(Source)
                                </h3>
                                <div>

                                    <div class="product-inner-row">
                                        <a href="#" class="un-product-name">
                                            <i class="iconfont icon-bae"></i>当前公众号客户来源分析</a>
                                        <span class="product-links">
                                            <a href="javascript:ShowReport('当前公众号客户来源分析','statistics/sourceapp.aspx');"><i class="iconfont icon-buy"></i>查看</a>
                                        </span>
                                    </div>

                                    <div class="product-inner-row">
                                        <a href="#" class="un-product-name">
                                            <i class="iconfont icon-ses"></i>商户旗下所有客户来源分析</a>
                                        <span class="product-links">
                                            <a href="javascript:ShowReport('商户旗下所有客户来源分析','statistics/sourceapplist.aspx');"><i class="iconfont icon-buy"></i>查看</a>
                                        </span>
                                    </div>

                                    <div class="product-inner-row">
                                        <a href="#" class="un-product-name">
                                            <i class="iconfont icon-sms"></i>客户转化率综合分析</a>
                                        <span class="product-links">
                                            <a href="javascript:ShowReport('客户转化率综合分析','statistics/sourcerates.aspx');"><i class="iconfont icon-buy"></i>查看</a>
                                        </span>
                                    </div>


                                </div>
                            </div>


                            <div class="product">
                                <h3>月销量趋势图-(Month)
                                </h3>
                                <div>

                                    <div class="product-inner-row">
                                        <a href="#" class="un-product-name">
                                            <i class="iconfont icon-qss"></i>当前公众号月销量综合分析</a>
                                        <span class="product-links">
                                            <a href="javascript:ShowReport('当前公众号月销量综合分析','statistics/monthapp.aspx');"><i class="iconfont icon-buy"></i>查看</a>
                                        </span>
                                    </div>

                                    <div class="product-inner-row">
                                        <a href="#" class="un-product-name">
                                            <i class="iconfont icon-wmt"></i>商户旗下月销量趋势分析</a>
                                        <span class="product-links">
                                            <a href="javascript:ShowReport('商户旗下月销量趋势分析','statistics/monthapplist.aspx');"><i class="iconfont icon-buy"></i>查看</a>
                                        </span>
                                    </div>
                                </div>
                            </div>


                        </div>

                    </div>
                </div>


                </div>
            </div>
        </div>
    </div>
</body>
</html>
