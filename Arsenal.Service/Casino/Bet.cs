using System;
using System.Data;
using System.Linq;
using Arsenalcn.Core;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_Bet", Sort = "BetTime DESC")]
    public class Bet : Entity<int>
    {
        // Place Bet of SingleChoice
        public void Place(Guid matchGuid, string selectedOption)
        {
            using (var trans = DapperHelper.MarsConnection.BeginTransaction())
            {
                try
                {
                    IRepository repo = new Repository();

                    #region Get CasinoItem & Check

                    var item = repo.Query<CasinoItem>(x =>
                        x.MatchGuid == matchGuid && x.ItemType == CasinoType.SingleChoice).FirstOrDefault();

                    if (item == null)
                    {
                        throw new Exception("对应投注项不存在(SingleChoice)");
                    }

                    if (item.CloseTime < DateTime.Now)
                    {
                        throw new Exception("已超出投注截止时间");
                    }

                    #endregion

                    #region Get Gambler & Check

                    var gambler = repo.Query<Gambler>(x => x.UserID == UserID)[0];

                    if (gambler == null)
                    {
                        throw new Exception("当前用户不存在博彩帐户(Gambler)");
                    }

                    if (BetAmount == null)
                    {
                        throw new Exception("投注金额无效");
                    }

                    if (BetAmount != null && gambler.Cash < BetAmount.Value)
                    {
                        throw new Exception($"博彩帐户余额不足(博彩币余额: {gambler.Cash.ToString("f2")})");
                    }

                    #endregion

                    #region Get ChoiceOption & Check

                    var choiceOption = repo.Query<ChoiceOption>(x => x.CasinoItemGuid == item.ID)
                        .Find(x => x.OptionName.Equals(selectedOption, StringComparison.OrdinalIgnoreCase));

                    if (choiceOption == null)
                    {
                        throw new Exception("对应投注项不存在(ChoiceOption)");
                    }

                    #endregion

                    #region Get Banker & Check

                    var banker = repo.Single<Banker>(item.BankerID);

                    if (banker == null)
                    {
                        throw new Exception("对应庄家不存在(Banker)");
                    }

                    #endregion

                    //update gambler statistics
                    gambler.Cash -= BetAmount.Value;
                    gambler.TotalBet += BetAmount.Value;
                    banker.Cash += BetAmount.Value;

                    repo.Update(gambler, trans);
                    repo.Update(banker, trans);

                    CasinoItemGuid = item.ID;
                    BetTime = DateTime.Now;
                    BetRate = choiceOption.OptionRate;

                    object key;
                    repo.Insert(this, out key, trans);
                    ID = Convert.ToInt32(key);

                    var betDetail = new BetDetail { BetID = ID };

                    if (selectedOption.ToLower() == "home")
                        betDetail.DetailName = "Home";
                    else if (selectedOption.ToLower() == "away")
                        betDetail.DetailName = "Away";
                    else if (selectedOption.ToLower() == "draw")
                        betDetail.DetailName = "Draw";

                    repo.Insert(betDetail, trans);

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();

                    throw;
                }
            }
        }

        // Place Bet of MatchResult
        public void Place(Guid matchGuid, short resultHome, short resultAway)
        {
            using (var trans = DapperHelper.MarsConnection.BeginTransaction())
            {
                try
                {
                    IRepository repo = new Repository();

                    #region Get CasinoItem & Check

                    var item = repo.Query<CasinoItem>(x =>
                        x.MatchGuid == matchGuid && x.ItemType == CasinoType.MatchResult)[0];

                    if (item == null)
                    {
                        throw new Exception("对应投注项不存在(MatchResult)");
                    }

                    if (item.CloseTime < DateTime.Now)
                    {
                        throw new Exception("已超出投注截止时间");
                    }

                    #endregion

                    #region Get Gambler & Check

                    var gambler = repo.Query<Gambler>(x => x.UserID == UserID)[0];

                    if (gambler == null)
                    {
                        throw new Exception("当前用户不存在博彩帐户(Gambler)");
                    }

                    #endregion

                    #region Get RepeatBet & Check

                    var historyBets = repo.Query<Bet>(x =>
                        x.CasinoItemGuid == item.ID && x.UserID == UserID);

                    if (historyBets.Count > 0)
                    {
                        throw new Exception("已经投过此注，不能重复猜比分");
                    }

                    #endregion

                    CasinoItemGuid = item.ID;
                    BetTime = DateTime.Now;

                    object key;
                    repo.Insert(this, out key, trans);
                    ID = Convert.ToInt32(key);

                    var betDetailHome = new BetDetail
                    {
                        BetID = ID,
                        DetailName = "Home",
                        DetailValue = resultHome.ToString()
                    };


                    repo.Insert(betDetailHome, trans);

                    var betDetailAway = new BetDetail();

                    betDetailAway.BetID = ID;
                    betDetailAway.DetailName = "Away";
                    betDetailAway.DetailValue = resultAway.ToString();

                    repo.Insert(betDetailAway, trans);

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();

                    throw;
                }
            }
        }

        public void ReturnBet()
        {
            using (var trans = DapperHelper.MarsConnection.BeginTransaction())
            {
                try
                {
                    IRepository repo = new Repository();

                    if (BetAmount.HasValue && BetAmount >= 0f)
                    {
                        var betAmount = Convert.ToSingle(BetAmount);

                        var item = repo.Single<CasinoItem>(CasinoItemGuid);
                        if (item == null) { throw new Exception("此投注无对应投注项(CasinoItem)"); }

                        if (item.CloseTime < DateTime.Now)
                        {
                            throw new Exception("已超出投注截止时间");
                        }

                        var banker = repo.Single<Banker>(item.BankerID);
                        if (banker == null) { throw new Exception("此投注无对应庄家(Banker)"); }

                        var gambler = repo.Query<Gambler>(x => x.UserID == UserID)[0];

                        if (gambler == null) { throw new Exception("此投注无对应博彩帐户(Gambler)"); }

                        // do return bet

                        gambler.Cash += betAmount;
                        gambler.TotalBet -= betAmount;
                        banker.Cash -= betAmount;

                        repo.Update(gambler, trans);
                        repo.Update(banker, trans);
                    }

                    var list = repo.Query<BetDetail>(x => x.BetID == ID);

                    list.Delete(trans);

                    repo.Delete(this);

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();

                    throw;
                }
            }
        }

        public static void Clean(IDbTransaction trans = null)
        {
            //DELETE FROM dbo.AcnCasino_Bet WHERE (CasinoItemGuid NOT IN(SELECT CasinoItemGuid FROM dbo.AcnCasino_CasinoItem))
            var sql =
                $@"DELETE FROM {Repository.GetTableAttr<Bet>().Name} WHERE (CasinoItemGuid NOT IN (SELECT CasinoItemGuid FROM {Repository.GetTableAttr<CasinoItem>().Name}))";

            IDapperHelper dapper = new DapperHelper();

            dapper.Execute(sql, trans);
        }

        #region Members and Properties

        [DbColumn("UserID")]
        public int UserID { get; set; }

        [DbColumn("UserName")]
        public string UserName { get; set; }

        [DbColumn("CasinoItemGuid")]
        public Guid CasinoItemGuid { get; set; }

        [DbColumn("BetAmount")]
        public double? BetAmount { get; set; }

        [DbColumn("BetTime")]
        public DateTime BetTime { get; set; }

        [DbColumn("BetRate")]
        public double? BetRate { get; set; }

        [DbColumn("IsWin")]
        public bool? IsWin { get; set; }

        [DbColumn("Earning")]
        public double? Earning { get; set; }

        [DbColumn("EarningDesc")]
        public string EarningDesc { get; set; }

        #endregion
    }
}