<%@ Page Language="C#" AutoEventWireup="true" CodeFile="center.aspx.cs" Inherits="XKD.Web.Admin.center" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta charset="UTF-8">
    <title>管理首页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <link href="../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../css/first.css" media="all">
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>

    <script type="text/javascript" charset="utf-8" src="../statistics/js/highcharts.js"></script>
    <script type="text/javascript" charset="utf-8" src="../statistics/js/exporting.js"></script>

    <script type="text/javascript">

        function ShowReport(_title, _url) {
            var winDialog = top.dialog({
                title: _title,
                url: _url,
                width: 700,
                height: 450
            }).showModal();
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
                <div style="min-height: 580px;" class="content-wrap">
                    <div class="account-preview">
                        <div class="account-left">
                            <div class="inner-panel">
                                <i class="avatar"></i>
                                <a class="user-name-link" href="###" title="<%=business_name %>">
                                    <span class="user-name"><%=business_name %></span>
                                </a>

                                <a id="overview-verify" href="###" title="通过官方实名认证">
                                    <i class="iconfont icon-verify verify-no"></i>
                                </a>

                                <a id="phoneBound" href="###" title="通过手机认证">
                                    <i class="iconfont icon-verify-mobile verify-no verify-pass"></i>
                                </a>
                                <a id="emailBound" href="###" title="通过邮箱认证">
                                    <i class="iconfont icon-verify-email verify-no"></i>
                                </a>
                                <div class="item cash-balance">
                                    <span>总营业额：</span>
                                    <span class="money">￥</span>
                                    <a href="#" id="cash-label"><%=total_money %></a>
                                    <a class="btn-recharge" style="display: none;" href="###">资料修改</a>
                                </div>
                                <div class="item coupon-balance">
                                    <span>订单数：</span>
                                    <!--  <a id="coupon-num" href="###">0</a>-->
                                    <a id="A2" href="###"><%=total_orders_count %></a>
                                    <span>笔</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                     <span>客户数：</span>
                                    <a id="A1" href="###"><%=total_customers_count %></a>
                                    <span>人</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                    <span>商户数：</span>
                                    <a id="A3" href="###"><%=total_appid_count %></a>
                                    <span>人</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                </div>
                                <div class="item"><a href="###">今日新增：</a></div>
                            </div>
                            <div class="bottom-panel">
                                <a href="###" class="message">
                                    <i class="iconfont icon-ses"></i>订单<b id="mes-num"><%=today_notices_count %></b>笔
                                </a>
                                <a href="###" class="ticket">
                                    <i class="iconfont icon-ticket"></i>客户<b id="ticket-num"><%=today_orders_count %></b>人
                                </a>
                                <a href="###" class="orderlist">
                                    <i class="iconfont icon-order-list"></i>商户<b id="B1"><%=today_customers_count %></b>人
                                </a>
                            </div>
                        </div>
                        <div class="account-middle">
                            <h2><span>订单走势</span>

                            </h2>
                            <div style="background-color: transparent;" id="container" class="account-preview-charts ui-ctrl ui-echart">
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
                                    <div id="container1" style="width: 160px; height: 100px; margin: 0 auto"></div>
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
                                                <i class="iconfont icon-bcc"></i>订单报表统计</a>
                                            <span class="product-links">
                                                <a href="javascript:ShowReport('订单报表统计','statistics/reportapp.aspx');"><i class="iconfont icon-buy"></i>查看</a>
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
                                                <i class="iconfont icon-cds"></i>订单走势分析</a>
                                            <span class="product-links">
                                                <a href="javascript:ShowReport('订单走势分析','statistics/trendapp.aspx');"><i class="iconfont icon-buy"></i>查看</a>
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
                                                <i class="iconfont icon-rds"></i>商品销量环比分析</a>
                                            <span class="product-links">
                                                <a href="javascript:ShowReport('商品销量环比分析','statistics/quarterapp.aspx');"><i class="iconfont icon-buy"></i>查看</a>
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
                                                <i class="iconfont icon-bmr"></i>客户数据分析</a>
                                            <span class="product-links">
                                                <a href="javascript:ShowReport('客户数据分析','statistics/customerapp.aspx');"><i class="iconfont icon-buy"></i>查看</a>
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
                                                <i class="iconfont icon-bae"></i>客户来源分析</a>
                                            <span class="product-links">
                                                <a href="javascript:ShowReport('客户来源分析','statistics/sourceapp.aspx');"><i class="iconfont icon-buy"></i>查看</a>
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
                                                <i class="iconfont icon-qss"></i>月销量综合分析</a>
                                            <span class="product-links">
                                                <a href="javascript:ShowReport('月销量综合分析','statistics/monthapp.aspx');"><i class="iconfont icon-buy"></i>查看</a>
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
