namespace Hidistro.SqlDal.Promotions
{
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class GamePrizeDao
    {
        private Database _db = DatabaseFactory.CreateDatabase();

        public bool Create(GamePrizeInfo model)
        {
            string str = "INSERT INTO [Hishop_PromotionGamePrizes]\r\n                    ([GameId]\r\n                    ,[PrizeGrade]\r\n                    ,[PrizeType]\r\n                    ,[GivePoint]\r\n                    ,[GiveCouponId]\r\n                    ,[GiveShopBookId]\r\n                    ,[PrizeCount]\r\n                    ,[PrizeRate]\r\n                    ,[GriveShopBookPicUrl])\r\n                  VALUES\r\n                    ( @GameId\r\n                    ,@PrizeGrade\r\n                    ,@PrizeType\r\n                    ,@GivePoint\r\n                    ,@GiveCouponId\r\n                    ,@GiveShopBookId\r\n                    ,@PrizeCount\r\n                    ,@PrizeRate\r\n                    ,@GriveShopBookPicUrl);";
            DbCommand sqlStringCommand = this._db.GetSqlStringCommand(str);
            this._db.AddInParameter(sqlStringCommand, "@GameId", DbType.Int32, model.GameId);
            this._db.AddInParameter(sqlStringCommand, "@PrizeGrade", DbType.Int32, (int) model.PrizeGrade);
            this._db.AddInParameter(sqlStringCommand, "@PrizeType", DbType.Int32, (int) model.PrizeType);
            this._db.AddInParameter(sqlStringCommand, "@GivePoint", DbType.Decimal, model.GivePoint);
            this._db.AddInParameter(sqlStringCommand, "@GiveCouponId", DbType.String, model.GiveCouponId);
            this._db.AddInParameter(sqlStringCommand, "@GiveShopBookId", DbType.String, model.GiveShopBookId);
            this._db.AddInParameter(sqlStringCommand, "@PrizeCount", DbType.Int32, model.PrizeCount);
            this._db.AddInParameter(sqlStringCommand, "@PrizeRate", DbType.Int32, model.PrizeRate);
            this._db.AddInParameter(sqlStringCommand, "@GriveShopBookPicUrl", DbType.String, model.GriveShopBookPicUrl);
            return (this._db.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public IList<GamePrizeInfo> GetGamePrizeListsByGameId(int gameId)
        {
            string str = "Select * From [Hishop_PromotionGamePrizes] where GameId=@GameId ";
            DbCommand sqlStringCommand = this._db.GetSqlStringCommand(str);
            this._db.AddInParameter(sqlStringCommand, "@GameId", DbType.Int32, gameId);
            using (IDataReader reader = this._db.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<GamePrizeInfo>(reader);
            }
        }

        public GamePrizeInfo GetModelByPrizeGradeAndGameId(PrizeGrade grade, int gameId)
        {
            string str = "Select * From [Hishop_PromotionGamePrizes] where PrizeGrade=@PrizeGrade and GameId=@GameId ";
            DbCommand sqlStringCommand = this._db.GetSqlStringCommand(str);
            this._db.AddInParameter(sqlStringCommand, "@PrizeGrade", DbType.Int32, (int) grade);
            this._db.AddInParameter(sqlStringCommand, "@GameId", DbType.Int32, gameId);
            using (IDataReader reader = this._db.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<GamePrizeInfo>(reader);
            }
        }

        public bool Update(GamePrizeInfo model)
        {
            string str = "Update [Hishop_PromotionGamePrizes] set [PrizeType]=@PrizeType\r\n                    ,[GivePoint]=@GivePoint\r\n                    ,[GiveCouponId]=@GiveCouponId\r\n                    ,[GiveShopBookId]=@GiveShopBookId\r\n                    ,[PrizeCount]=@PrizeCount\r\n                    ,[PrizeRate]=@PrizeRate\r\n                    ,[GriveShopBookPicUrl]=@GriveShopBookPicUrl\r\n                    Where PrizeId=@PrizeId ;";
            DbCommand sqlStringCommand = this._db.GetSqlStringCommand(str);
            this._db.AddInParameter(sqlStringCommand, "@PrizeType", DbType.Int32, (int) model.PrizeType);
            this._db.AddInParameter(sqlStringCommand, "@GivePoint", DbType.Decimal, model.GivePoint);
            this._db.AddInParameter(sqlStringCommand, "@GiveCouponId", DbType.String, model.GiveCouponId);
            this._db.AddInParameter(sqlStringCommand, "@GiveShopBookId", DbType.String, model.GiveShopBookId);
            this._db.AddInParameter(sqlStringCommand, "@PrizeCount", DbType.Int32, model.PrizeCount);
            this._db.AddInParameter(sqlStringCommand, "@PrizeRate", DbType.Int32, model.PrizeRate);
            this._db.AddInParameter(sqlStringCommand, "@GriveShopBookPicUrl", DbType.String, model.GriveShopBookPicUrl);
            this._db.AddInParameter(sqlStringCommand, "@PrizeId", DbType.Int32, model.PrizeId);
            return (this._db.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

