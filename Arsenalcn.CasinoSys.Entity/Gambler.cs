﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Discuz.Forum;

namespace Arsenalcn.CasinoSys.Entity
{
    public class Gambler
    {
        private Gambler(DataRow dr)
        {
            InitGambler(dr);
        }

        public Gambler(int userId, SqlTransaction trans = null)
        {
            var dr = DataAccess.Gambler.GetGamblerByUserId(userId, trans);

            if (dr != null)
            {
                InitGambler(dr);
            }
            else
            {
                #region Insert new Gambler for new user

                UserID = userId;
                UserName = Users.GetShortUserInfo(userId).Username.Trim();
                Cash = 0f;
                TotalBet = 0f;
                Win = 0;
                Lose = 0;
                RPBonus = null;
                ContestRank = null;
                TotalRank = 0;
                Banker = 0;
                JoinDate = DateTime.Now;
                IsActive = true;
                Description = string.Empty;
                Remark = string.Empty;

                Insert(trans);

                #endregion
            }
        }

        private void InitGambler(DataRow dr)
        {
            if (dr != null)
            {
                GamblerID = Convert.ToInt32(dr["ID"]);
                UserID = Convert.ToInt32(dr["UserID"]);
                UserName = Convert.ToString(dr["UserName"]);
                Cash = Convert.ToSingle(dr["Cash"]);
                TotalBet = Convert.ToSingle(dr["TotalBet"]);
                Win = Convert.ToInt32(dr["Win"]);
                Lose = Convert.ToInt32(dr["Lose"]);

                if (!Convert.IsDBNull(dr["RPBonus"]))
                    RPBonus = Convert.ToInt32(dr["RPBonus"]);
                else
                    RPBonus = null;

                if (!Convert.IsDBNull(dr["ContestRank"]))
                    ContestRank = Convert.ToInt32(dr["ContestRank"]);
                else
                    ContestRank = null;

                TotalRank = Convert.ToInt32(dr["TotalRank"]);

                if (!Convert.IsDBNull(dr["Banker"]))
                    Banker = Convert.ToInt32(dr["Banker"]);
                else
                    Banker = null;

                JoinDate = Convert.ToDateTime(dr["JoinDate"]);
                IsActive = Convert.ToBoolean(dr["IsActive"]);
                Description = dr["Description"].ToString();
                Remark = dr["Remark"].ToString();
            }
            else
                throw new Exception("Unable to init Gambler.");
        }

        //public void Select()
        //{
        //    var dr = DataAccess.Gambler.GetGamblerByID(GamblerID);

        //    if (dr != null)
        //        InitGambler(dr);
        //}

        //public void Select(int userID)
        //{
        //    var dr = DataAccess.Gambler.GetGamblerByUserID(userID);

        //    if (dr != null)
        //        InitGambler(dr);
        //}

        public void Update(SqlTransaction trans = null)
        {
            DataAccess.Gambler.UpdateGambler(GamblerID, UserID, UserName, Cash, TotalBet, Win, Lose, RPBonus,
                ContestRank, TotalRank,
                Banker, JoinDate, IsActive, Description, Remark, trans);
        }

        public void Insert(SqlTransaction trans = null)
        {
            GamblerID = DataAccess.Gambler.InsertGambler(GamblerID, UserID, UserName, Cash, TotalBet, Win, Lose, RPBonus,
                ContestRank, TotalRank,
                Banker, JoinDate, IsActive, Description, Remark, trans);
        }

        //public void Delete(SqlTransaction trans = null)
        //{
        //    DataAccess.Gambler.DeleteGambler(GamblerID, trans);
        //}

        public static List<Gambler> GetGamblers()
        {
            var dt = DataAccess.Gambler.GetGamblers();
            var list = new List<Gambler>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new Gambler(dr));
                }
            }

            return list;
        }

        public static int GetGamblerCount()
        {
            return DataAccess.Gambler.GetGamblerCount();
        }

        public static float GetGamblerTotalBetByUserId(int id, Guid? leagueGuid = null)
        {
            if (leagueGuid.HasValue) { return DataAccess.Gambler.GetGamblerTotalBetByUserID(id, leagueGuid.Value); }

            return DataAccess.Gambler.GetGamblerTotalBetByUserID(id);
        }

        //public static int GetGamblerRPByUserID(int userID, Guid? leagueGuid = null)
        //{
        //    if (leagueGuid.HasValue)
        //        return DataAccess.Gambler.GetGamblerRPByUserID(userID, leagueGuid.Value);
        //    return DataAccess.Gambler.GetGamblerRPByUserID(userID);
        //}

        //public static void GamblerStatistics()
        //{
        //    var listGambler = GetGamblers();
        //    var listCasinoGambler = CasinoGambler.GetCasinoGamblers();

        //    var listCasinoCamblerContest = CasinoGambler.GetCasinoGamblers(ConfigGlobal.DefaultLeagueID);

        //    if (listCasinoCamblerContest != null && listCasinoCamblerContest.Count > 0)
        //    {
        //        listCasinoCamblerContest = CasinoGambler.SortCasinoGambler(listCasinoCamblerContest);
        //    }

        //    if (listGambler != null && listGambler.Count > 0 && listCasinoGambler != null && listCasinoGambler.Count > 0)
        //    {
        //        foreach (var g in listGambler)
        //        {
        //            var cg = listCasinoGambler.Find(
        //                delegate (CasinoGambler casinoGambler) { return casinoGambler.UserID.Equals(g.UserID); });

        //            if (cg != null)
        //                g.InitGambler(cg);

        //            var cgc = listCasinoCamblerContest.Find(
        //                delegate (CasinoGambler casinoGambler) { return casinoGambler.UserID.Equals(g.UserID); });

        //            if (cgc != null)
        //                g.ContestRank = cgc.Rank;
        //            else
        //                g.ContestRank = null;

        //            g.Update();
        //        }
        //    }
        //}

        public static void GamblerStatistics(int id)
        {
            var g = new Gambler(id);
            var cg = CasinoGambler.GetCasinoGamblers().Find(x => x.UserID.Equals(id));

            CasinoGambler cgc = null;

            var listCasinoCamblerContest = CasinoGambler.GetCasinoGamblers(ConfigGlobal.DefaultLeagueID);

            if (listCasinoCamblerContest != null && listCasinoCamblerContest.Count > 0 &&
                listCasinoCamblerContest.Exists(x => x.UserID.Equals(id)))
            {
                listCasinoCamblerContest = CasinoGambler.SortCasinoGambler(listCasinoCamblerContest);

                cgc = listCasinoCamblerContest.Find(x => x.UserID.Equals(id));
            }

            if (cg != null)
            {
                g.InitGambler(cg);

                if (cgc != null)
                    g.ContestRank = cgc.Rank;
                else
                    g.ContestRank = null;
            }

            g.Update();
        }

        private void InitGambler(CasinoGambler cg)
        {
            if (cg != null)
            {
                TotalBet = cg.TotalBet;
                Win = cg.Win;
                Lose = cg.Lose;
                RPBonus = cg.RPBonus;
                TotalRank = cg.Rank;

                // TODO: Add Gambler become Banker
                Banker = null;
            }
        }

        //public static void TopGamblerMonthlyStatistics()
        //{
        //    var iDay = DateTime.Today;

        //    var lastBetTime = DataAccess.Bet.GetLastBetTime();
        //    var sql = string.Empty;

        //    while (!(iDay.Year <= lastBetTime.Year && iDay.Month < lastBetTime.Month))
        //    {
        //        var dtWinner = DataAccess.Rank.GetTopGamblerMonthly(true, iDay);
        //        var dtLoser = DataAccess.Rank.GetTopGamblerMonthly(false, iDay);
        //        var dtRP = DataAccess.Rank.GetTopGamblerMonthly(true, iDay, true);

        //        if (dtWinner != null && dtLoser != null && dtRP != null)
        //        {
        //            var dtRanks = DataAccess.Rank.GetAllRanks(iDay.Year, iDay.Month);

        //            if (dtRanks != null)
        //            {
        //                //insert
        //                DataAccess.Rank.UpdateRank(iDay.Year, iDay.Month, Convert.ToInt32(dtWinner.Rows[0]["UserID"]),
        //                    dtWinner.Rows[0]["UserName"].ToString().Trim(), Convert.ToSingle(dtWinner.Rows[0]["profit"]),
        //                    Convert.ToSingle(dtWinner.Rows[0]["TotalBet"]), Convert.ToInt32(dtLoser.Rows[0]["UserID"]),
        //                    dtLoser.Rows[0]["UserName"].ToString().Trim(), Convert.ToSingle(dtLoser.Rows[0]["profit"]),
        //                    Convert.ToSingle(dtLoser.Rows[0]["TotalBet"]), Convert.ToInt32(dtRP.Rows[0]["UserID"]),
        //                    dtRP.Rows[0]["UserName"].ToString().Trim(), Convert.ToInt32(dtRP.Rows[0]["RPBonus"]));
        //            }
        //            else
        //            {
        //                //update
        //                DataAccess.Rank.InsertRank(iDay.Year, iDay.Month, Convert.ToInt32(dtWinner.Rows[0]["UserID"]),
        //                    dtWinner.Rows[0]["UserName"].ToString().Trim(), Convert.ToSingle(dtWinner.Rows[0]["profit"]),
        //                    Convert.ToSingle(dtWinner.Rows[0]["TotalBet"]), Convert.ToInt32(dtLoser.Rows[0]["UserID"]),
        //                    dtLoser.Rows[0]["UserName"].ToString().Trim(), Convert.ToSingle(dtLoser.Rows[0]["profit"]),
        //                    Convert.ToSingle(dtLoser.Rows[0]["TotalBet"]), Convert.ToInt32(dtRP.Rows[0]["UserID"]),
        //                    dtRP.Rows[0]["UserName"].ToString().Trim(), Convert.ToInt32(dtRP.Rows[0]["RPBonus"]));
        //            }
        //        }
        //        iDay = iDay.AddMonths(-1);
        //    }
        //}

        #region Members and Properties

        public int GamblerID { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public float Cash { get; set; }

        public float TotalBet { get; set; }

        public int Win { get; set; }

        public int Lose { get; set; }

        public int? RPBonus { get; set; }

        public int? ContestRank { get; set; }

        public int TotalRank { get; set; }

        public int? Banker { get; set; }

        public DateTime JoinDate { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }

        public string Remark { get; set; }

        #endregion
    }
}