namespace Hidistro.Entities.Promotions
{
    using System;

    public enum SendCouponResult
    {
        正常领取,
        优惠劵已结束,
        会员等级不在此活动范内,
        已领完,
        此用户已到领取上限,
        其它错误
    }
}

