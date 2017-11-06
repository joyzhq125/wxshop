<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<%@ Import Namespace="Hidistro.ControlPanel.Promotions" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li>
    <div class="outbox">
        <div class="shopInfo clearfix" onclick="javascript:window.location='/ProductDetails.aspx?productId=<%# Eval("ProductId") %>'"
             PrizeGrade="<%#   GameHelper.GetPrizeGradeName(Eval("PrizeGrade").ToString()) %>" PrizeType="<%# Eval("PrizeType") %>"    GiveCouponId="<%# Eval("GiveCouponId") %>" GivePoint="<%# Eval("GivePoint") %>" 
            >
            <div class="img"  >
                <Hi:ListImage ID="ProductImage" runat="server" DataField="ThumbnailUrl100" Width="60" Height="60" />
            </div>
            <div class="imgInfo">
                <div>
                    <p>
                        <%# Eval("ProductName") %>
                    </p>
                    <p><%#   GameHelper.GetPrizeGradeName(Eval("PrizeGrade").ToString()) %>　数量：1</p>
                </div>
            </div>
        </div>
    </div>
    <div class="outbox">
        <div class="prizeInfo">
            <p>中奖编号：<%# Eval("PlayTime","{0:yyyyMMdd}")%>-<%# Eval("PrizeGrade") %>-<%# Eval("LogId")%></p>
            <p>活动名称：<span><%#Eval("GameTitle") %></span><span>[<%# GameHelper.GetGameTypeName(Eval("GameType").ToString()) %>]</span></p>
            <p>中奖时间：<%# Eval("PlayTime","{0:yyyy-MM-dd hh:mm:ss}")%></p>
            <p>发放状态：<%# GameHelper.GetPrizesDeliveStatus(Eval("status").ToString())%></p>
            <div class="btnright" LogId="<%# Eval("LogId") %>" Dstatus="<%# Eval("status") %>" Pid="<%# Eval("Id") %>" >   
        </div>
    </div>

</li>
