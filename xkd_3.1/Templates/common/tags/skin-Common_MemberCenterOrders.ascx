<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="orderlist">
    <div class="orderinfo">
        <p>订单编号：<%#Eval("OrderId") %></p>
        <p>订单日期：<%# Eval("OrderDate","{0:d}")%></p>
        <p>订单状态：<Hi:OrderStatusLabel ID="OrderStatusLabel1" Gateway='<%#Eval("Gateway") %>' OrderStatusCode='<%# Eval("OrderStatus") %>' runat="server" /></p>
        <span class="price">￥<em><%# Eval("OrderTotal","{0:F2}")%></em></span>
    </div>
    <asp:Repeater ID="rporderitems" runat="server" DataSource='<%# Eval("OrderItems") %>'>
        <ItemTemplate>
            <div class="orderimg">
                <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailsUrl" />
                <div class="orderimginfo">
                    <a href="<%# Globals.ApplicationPath + "/Vshop/MemberOrderDetails.aspx?OrderId=" + Eval("OrderId") %>">
                        <div class="name bcolor">
                            <%# Eval("ItemDescription")%>
                            <%#Eval("OrderItemsStatus").ToString()=="9"?("<span class='text-danger'>(已退款，金额￥"+decimal.Parse( Eval("ReturnMoney").ToString()).ToString("F2"))+")</span>":"" %>
                            <%#Eval("OrderItemsStatus").ToString()=="10"?("<span class='text-danger'>(已退货，金额￥"+decimal.Parse( Eval("ReturnMoney").ToString()).ToString("F2"))+")</span>":"" %>
                        </div>
                    </a>
                    <div class="specification">
                        <input type="hidden" value="<%# Eval("SkuContent")%><%#Container.ItemIndex %>" />
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="linkbtn">
        <a href='<%# Globals.ApplicationPath + "/Vshop/MyLogistics.aspx?OrderId=" + Eval("OrderId") %> '
            class='btn btn-warning btn-xs <%# ((int)Eval("OrderStatus") == 3 || (int)Eval("OrderStatus") == 5) ? "btn btn-warning btn-xs" : "hide"%>'>查看物流</a>
        <a href='<%# Globals.ApplicationPath + "/Vshop/FinishOrder.aspx?PaymentType=1&OrderId=" + Eval("OrderMarking") %> '
            class='btn btn-danger btn-xs <%# (int)Eval("OrderStatus") == 1&&(int)Eval("PaymentTypeId")!=0&&(string)Eval("GateWay")!="hishop.plugins.payment.bankrequest"&&(string)Eval("GateWay")!="hishop.plugins.payment.podrequest"? "btn btn-danger btn-xs" : "hide"%>'>去付款</a>
        <a href='javascript:void(0)' onclick='FinishOrder(<%#Eval("OrderId") %>)' class='btn btn-warning btn-xs <%# (int)Eval("OrderStatus") == 3 ? "btn btn-danger btn-xs" : "hide"%>'>确认收货</a>

        <a href='<%# Globals.ApplicationPath + "/Vshop/FinishOrder.aspx?OrderId=" + Eval("OrderMarking")+"&onlyHelp=true" %> '
            class='btn btn-warning btn-xs <%# (int)Eval("PaymentTypeId")==99&&(int)Eval("OrderStatus")==1 ? "btn btn-danger btn-xs" : "hide"%>'>线下支付帮助</a>
        <%#(Eval("HasRedPage")).ToString()=="1"?"<a href='/Vshop/GetRedShare.aspx?orderid="+Eval("OrderId")+"' class='btn btn-warning btn-xs btn-danger'>发钱咯</a>":"" %>
    </div>
</div>