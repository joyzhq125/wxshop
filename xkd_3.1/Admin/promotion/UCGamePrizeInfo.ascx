<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCGamePrizeInfo.ascx.cs"
    Inherits="Hidistro.UI.Web.Admin.promotion.UCGamePrizeInfo" %>
<link rel="stylesheet" href="/Admin/shop/Public/css/dist/component-min.css">
<link rel="stylesheet" href="/Admin/shop/Public/plugins/jbox/jbox-min.css">

<!-- diy css start-->
<%--<link rel="stylesheet" href="/Admin/shop/PublicMob/css/style.css">--%>

<link rel="stylesheet" href="/Admin/shop/Public/plugins/uploadify/uploadify-min.css">

<script src="/Admin/shop/Public/js/dist/underscore.js"></script>
<script src="/Admin/shop/Public/plugins/jbox/jquery.jBox-2.3.min.js"></script>
<%--<script src="/Admin/shop/Public/plugins/zclip/jquery.zclip-min.js"></script>--%>
<script src="/Admin/shop/Public/plugins/uploadify/jquery.uploadify.min.js?ver=940"></script>
<script src="/Admin/shop/Public/js/jquery-ui/jquery-ui.min.js"></script>
<script src="/Admin/shop/Public/js/config.js"></script>

<script src="/Admin/shop/Public/plugins/colorpicker/colorpicker.js"></script>

<script src="/Admin/js/HiShopComPlugin.js"></script>
<script src="/Admin/shop/Public/js/dist/componentadmin-min.js"></script>
<script src="/Admin/js/weiboHelper.js"></script>

<script type="text/javascript">
    $(function () {
        $('.setTab .tabContent').eq(0).show();
        $('.setTab ul.navTab li').click(function () {
            $('.setTab ul.navTab li').removeClass('active');
            $(this).addClass('active');
            $('.setTab .tabContent').hide();
            $('.setTab .tabContent').eq($(this).index()).show();
        })
        SelectPrize();
        $('.selectradio input').change(function () {
            $(this).parents('.tabContent').find('.give').hide();
            var index = $(this)[0].id.substring(2, 3);
            var indexValue = '';
            switch (index) {
                case "0":
                    {
                        indexValue = '一等奖：';
                        break;
                    }
                case "1":
                    {
                        indexValue = '二等奖：';
                        break;
                    }
                case "2":
                    {
                        indexValue = '三等奖：';
                        break;
                    }
                case "3":
                    {
                        indexValue = '普通奖：';
                        break;
                    }
            }
            switch ($(this).val()) {
                case '0':
                    $("#sPrizeGade" + index).html(indexValue + "赠送积分");
                    $(this).parents('.tabContent').find('.giveint').show();
                    break;
                case '1':
                    $("#sPrizeGade" + index).html(indexValue + "赠送优惠劵");
                    $(this).parents('.tabContent').find('.givecop').show();
                    break;
                case '2':
                    $("#sPrizeGade" + index).html(indexValue + "赠送商品");
                    $(this).parents('.tabContent').find('.giveshop').show();
                    break;
                default:
                    return false;
            }
        })

        $("#ShopBookPic").click(function () {
            HiShop.popbox.GoodsAndGroupPicker("goods", function (list) {
                console.log(list.item_id);
            });
        });
    });

    function ShowStep1() {
        $("#step1").show();
        $("#step2").hide();
    }


    function SelectPrize() {
        for (var i = 0; i < 4; i++) {
            switch ($("#prizeTypeValue" + i).val()) {
                case '0':
                    $("#prizeTypeValue" + i).parents('.tabContent').find('.giveint').show();
                    break;
                case '1':
                    $("#prizeTypeValue" + i).parents('.tabContent').find('.givecop').show();
                    $("#prizeTypeValue" + i).parents('.tabContent').find('.giveint').hide();
                    break;
                case '2':
                    $("#prizeTypeValue" + i).parents('.tabContent').find('.giveshop').show();
                    $("#prizeTypeValue" + i).parents('.tabContent').find('.giveint').hide();
                    break;
                default:
                    return false;
            }
        }
    }

    function SelectShopBookId(index) {
        HiShop.popbox.GoodsAndGroupPicker("goods", function (list) {
            $("#imgProduct" + index).attr("src", list.pic);
            $("#txtShopbookId" + index).val(list.item_id);
            $("#txtProductPic" + index).val(list.pic);
        });
    }

</script>
<style type="text/css">
    .goodsearch {
        margin-left: 100px;
        padding: 0 10px;
    }
    .goodsearch input {
        display: inline-block;
        -webkit-box-sizing: border-box;
        -moz-box-sizing: border-box;
        box-sizing: border-box;
        border: 1px solid #ddd;
        width: 350px;
        height: 30px;
        line-height: 30px;
        vertical-align: 0px;
        padding: 0 5px;
        margin: 0 10px;
    }
</style>

    <div class="set-switch resetBorder">
        <p class="mb10 borderSolidB pb5"><strong>奖品设置：</strong></p>
        <div class="setTab">
            <ul class="navTab clearfix">
                <li class="active">一等奖</li>
                <li>二等奖</li>
                <li>三等奖</li>
                <li>普通奖</li>
            </ul>
            <!--奖品信息-->
            <%=PrizeInfoHtml() %>
            <h5 class="winningResults mb20"><strong>中奖结果说明</strong></h5>
            <div class="form-horizontal clearfix">
                <div class="form-group">
                    <label class="col-xs-3 pad resetSize control-label" for="setdate">未中奖说明：</label>
                    <div class="form-inline journal-query col-xs-9">
                        <div class="form-group">

                            <asp:TextBox runat="server" TextMode="MultiLine" class="form-control inputtext"
                                ID="txtNotPrzeDescription" Text="谢谢参与!"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
