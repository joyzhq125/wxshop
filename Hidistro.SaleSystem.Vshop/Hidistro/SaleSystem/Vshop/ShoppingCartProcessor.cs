namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Promotions;
    using Hidistro.SqlDal.Sales;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public static class ShoppingCartProcessor
    {
        public static void AddLineItem(string skuId, int quantity, int categoryid, int Templateid, int type = 0, int exchangeId = 0)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (quantity <= 0)
            {
                quantity = 1;
            }
            new ShoppingCartDao().AddLineItem(currentMember, skuId, quantity, categoryid, Templateid, type, exchangeId);
        }

        public static void ClearShoppingCart()
        {
            new ShoppingCartDao().ClearShoppingCart(Globals.GetCurrentMemberUserId());
        }

        public static ShoppingCartInfo GetGroupBuyShoppingCart(int groupbuyId, string productSkuId, int buyAmount)
        {
            ShoppingCartItemInfo info5 = new ShoppingCartItemInfo() ;
            ShoppingCartInfo info = new ShoppingCartInfo();
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            ShoppingCartItemInfo info3 = new ShoppingCartDao().GetCartItemInfo(currentMember, productSkuId, buyAmount, 0);
            if (info3 == null)
            {
                return null;
            }
            GroupBuyInfo groupBuy = GroupBuyBrowser.GetGroupBuy(groupbuyId);
            if (((groupBuy == null) || (groupBuy.StartDate > DateTime.Now)) || (groupBuy.Status != GroupBuyStatus.UnderWay))
            {
                return null;
            }
            int count = groupBuy.Count;
            decimal price = groupBuy.Price;
            info5.SkuId = info3.SkuId;
            info5.ProductId = info3.ProductId;
            info5.SKU = info3.SKU;
            info5.Name = info3.Name;
            info5.MemberPrice = info5.AdjustedPrice = price;
            info5.SkuContent = info3.SkuContent;
            info5.Quantity = info5.ShippQuantity = buyAmount;
            info5.Weight = info3.Weight;
            info5.ThumbnailUrl40 = info3.ThumbnailUrl40;
            info5.ThumbnailUrl60 = info3.ThumbnailUrl60;
            info5.ThumbnailUrl100 = info3.ThumbnailUrl100;
            info.LineItems.Add(info5);
            return info;
        }

        public static List<ShoppingCartInfo> GetListShoppingCart(string productSkuId, int buyAmount)
        {
            List<ShoppingCartInfo> list = new List<ShoppingCartInfo>();
            ShoppingCartInfo item = new ShoppingCartInfo();
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            ShoppingCartItemInfo info3 = new ShoppingCartDao().GetCartItemInfo(currentMember, productSkuId, buyAmount, 0);
            if (info3 == null)
            {
                return null;
            }
            item.TemplateId = info3.FreightTemplateId.ToString();
            item.Amount = info3.SubTotal;
            item.Total = item.Amount;
            item.Exemption = 0M;
            item.ShipCost = 0M;
            item.GetPointNumber = info3.PointNumber * info3.Quantity;
            item.MemberPointNumber = currentMember.Points;
            item.LineItems.Add(info3);
            list.Add(item);
            return list;
        }

        public static List<ShoppingCartInfo> GetOrderSummitCart()
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                return null;
            }
            List<ShoppingCartInfo> orderSummitCart = new ShoppingCartDao().GetOrderSummitCart(currentMember);
            if (orderSummitCart.Count == 0)
            {
                return null;
            }
            return orderSummitCart;
        }

        public static ShoppingCartInfo GetShoppingCart()
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                return null;
            }
            ShoppingCartInfo shoppingCart = new ShoppingCartDao().GetShoppingCart(currentMember);
            if (shoppingCart.LineItems.Count == 0)
            {
                return null;
            }
            return shoppingCart;
        }

        public static ShoppingCartInfo GetShoppingCart(int Templateid)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                return null;
            }
            ShoppingCartInfo shoppingCart = new ShoppingCartDao().GetShoppingCart(currentMember, Templateid);
            if (shoppingCart.LineItems.Count == 0)
            {
                return null;
            }
            return shoppingCart;
        }

        public static ShoppingCartInfo GetShoppingCart(string productSkuId, int buyAmount)
        {
            ShoppingCartInfo info = new ShoppingCartInfo();
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            ShoppingCartItemInfo item = new ShoppingCartDao().GetCartItemInfo(currentMember, productSkuId, buyAmount, 0);
            if (item == null)
            {
                return null;
            }
            info.LineItems.Add(item);
            return info;
        }

        public static List<ShoppingCartInfo> GetShoppingCartAviti(int type = 0)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                return null;
            }
            List<ShoppingCartInfo> shoppingCartAviti = new ShoppingCartDao().GetShoppingCartAviti(currentMember, type);
            if (shoppingCartAviti.Count == 0)
            {
                return null;
            }
            return shoppingCartAviti;
        }

        public static int GetSkuStock(string skuId, int type = 0, int exchangeId = 0)
        {
            int stock = new SkuDao().GetSkuItem(skuId).Stock;
            if (type > 0)
            {
                int productId = int.Parse(skuId.Split(new char[] { '_' })[0]);
                PointExchangeProductInfo productInfo = new PointExChangeDao().GetProductInfo(exchangeId, productId);
                if (productInfo == null)
                {
                    return stock;
                }
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                int eachMaxNumber = 0;
                int num4 = new PointExChangeDao().GetUserProductExchangedCount(exchangeId, productId, currentMember.UserId);
                int productExchangedCount = new PointExChangeDao().GetProductExchangedCount(exchangeId, productId);
                int num6 = ((productInfo.ProductNumber - productExchangedCount) >= 0) ? (productInfo.ProductNumber - productExchangedCount) : 0;
                if (productInfo.EachMaxNumber > 0)
                {
                    if (num4 < productInfo.EachMaxNumber)
                    {
                        if (productInfo.EachMaxNumber <= num6)
                        {
                            eachMaxNumber = productInfo.EachMaxNumber;
                        }
                        else
                        {
                            eachMaxNumber = num6;
                        }
                    }
                    else
                    {
                        eachMaxNumber = 0;
                    }
                }
                else
                {
                    eachMaxNumber = num6;
                }
                if (eachMaxNumber > 0)
                {
                    stock = eachMaxNumber;
                }
            }
            return stock;
        }

        public static void RemoveLineItem(string skuId, int type = 0)
        {
            new ShoppingCartDao().RemoveLineItem(Globals.GetCurrentMemberUserId(), skuId, type);
        }

        public static void UpdateLineItemQuantity(string skuId, int quantity, int type = 0)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (quantity <= 0)
            {
                RemoveLineItem(skuId, 0);
            }
            new ShoppingCartDao().UpdateLineItemQuantity(currentMember, skuId, quantity, type);
        }
    }
}

