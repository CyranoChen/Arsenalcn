﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;
using Arsenalcn.Core.Extension;

namespace Arsenal.Service
{
    [DbSchema("Arsenal_Player", Key = "PlayerGuid", Sort = "IsLegend, IsLoan, SquadNumber, LastName")]
    public class Player : Entity<Guid>
    {
        public static class Cache
        {
            public static List<Player> PlayerList;
            public static List<Player> PlayerListHasSquadNumber;

            public static IEnumerable<int> ColListSquadNumber;
            public static IEnumerable<string> ColListPosition;

            static Cache()
            {
                InitCache();
            }

            public static void RefreshCache()
            {
                InitCache();
            }

            private static void InitCache()
            {
                IRepository repo = new Repository();

                PlayerList = repo.All<Player>();

                PlayerListHasSquadNumber = PlayerList.FindAll(x => x.SquadNumber > 0)
                    .OrderBy(x => x.SquadNumber).ThenBy(x => x.DisplayName).ToList();

                ColListSquadNumber = repo.All<Player>().DistinctOrderBy(x => x.SquadNumber);

                ColListPosition = repo.All<Player>().DistinctOrderBy(x => x.PlayerPosition.ToString());
            }

            public static Player Load(Guid guid)
            {
                return PlayerList.Find(x => x.ID.Equals(guid));
            }

            // HC: Acn Club API
            public static DataRow GetInfo(Guid guid)
            {
                var attr = Repository.GetTableAttr<Player>();

                var sql = $"SELECT * FROM {attr.Name} WHERE {attr.Key} = @key";

                var dapper = DapperHelper.GetInstance();

                return dapper.ExecuteDataTable(sql, new { key = guid })?.Rows[0];
            }
        }

        #region Members and Properties

        [DbColumn("FirstName")]
        public string FirstName { get; set; }

        [DbColumn("LastName")]
        public string LastName { get; set; }

        [DbColumn("DisplayName")]
        public string DisplayName { get; set; }

        [DbColumn("PrintingName")]
        public string PrintingName { get; set; }

        [DbColumn("PlayerPosition")]
        public PlayerPositionType PlayerPosition { get; set; }

        [DbColumn("SquadNumber")]
        public int SquadNumber { get; set; }

        [DbColumn("FaceURL")]
        public string FaceURL { get; set; }

        [DbColumn("PhotoURL")]
        public string PhotoURL { get; set; }

        [DbColumn("Offset")]
        public int Offset { get; set; }

        [DbColumn("IsLegend")]
        public bool IsLegend { get; set; }

        [DbColumn("IsLoan")]
        public bool IsLoan { get; set; }

        [DbColumn("Birthday")]
        public DateTime? Birthday { get; set; }

        [DbColumn("Born")]
        public string Born { get; set; }

        [DbColumn("Starts")]
        public int Starts { get; set; }

        [DbColumn("Subs")]
        public int Subs { get; set; }

        [DbColumn("Apps")]
        public int Apps { get; set; }

        [DbColumn("Goals")]
        public int Goals { get; set; }

        [DbColumn("JoinDate")]
        public DateTime? JoinDate { get; set; }

        [DbColumn("Joined")]
        public string Joined { get; set; }

        [DbColumn("LeftYear")]
        public string LeftYear { get; set; }

        [DbColumn("Debut")]
        public string Debut { get; set; }

        [DbColumn("FirstGoal")]
        public string FirstGoal { get; set; }

        [DbColumn("PreviousClubs")]
        public string PreviousClubs { get; set; }

        [DbColumn("Profile")]
        public string Profile { get; set; }

        #endregion
    }

    public enum PlayerPositionType
    {
        None = 0,
        Goalkeeper = 1,
        Defender = 2,
        Midfielder = 3,
        Forward = 4,
        Coach = 9
    }
}