namespace Hidistro.Entities.Promotions
{
    using System;

    public enum CanPrizeError
    {
        可以玩,
        会员等级不在此活动范围内,
        积分不够,
        一天只能玩一次,
        一人只能玩一次,
        一天只能玩两次,
        一人只能玩两次,
        此抽奖活动不在有效期内,
        此抽奖活动还没开始
    }
}

