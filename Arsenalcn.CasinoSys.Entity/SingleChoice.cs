using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Arsenalcn.Common;

namespace Arsenalcn.CasinoSys.Entity
{
    public sealed class SingleChoice : CasinoItem
    {
        internal SingleChoice() { }

        public bool FloatingRate { get; set; }

        public string Result { get; set; }

        public List<ChoiceOption> Options { get; } = new List<ChoiceOption>();

        public override Guid Save(SqlTransaction trans)
        {
            var newGuid = base.Save(trans);

            if (!ItemGuid.HasValue)
            {
                DataAccess.SingleChoice.InsertSingleChoice(newGuid, FloatingRate, trans);

                foreach (var option in Options)
                {
                    option.Insert(newGuid, trans);
                }
            }
            return newGuid;
        }

        protected override void BuildDetail()
        {
            if (ItemGuid != null)
            {
                var dr = DataAccess.SingleChoice.GetSingleChoice(ItemGuid.Value);

                if (dr != null)
                {
                    FloatingRate = Convert.ToBoolean(dr["FloatingRate"]);

                    Result = Convert.IsDBNull(dr["Result"]) ? null : Convert.ToString(dr["Result"]);
                }

                var dt = DataAccess.ChoiceOption.GetChoiceOptions(ItemGuid.Value);

                foreach (DataRow drOption in dt.Rows)
                {
                    var option = new ChoiceOption
                    {
                        CasinoItemGuid = (Guid) drOption["CasinoItemGuid"],
                        OptionName = Convert.ToString(drOption["OptionValue"]),
                        OptionDisplay = Convert.ToString(drOption["OptionDisplay"])
                    };

                    if (Convert.IsDBNull(drOption["OptionRate"]))
                        option.OptionRate = null;
                    else
                        option.OptionRate = Convert.ToSingle(drOption["OptionRate"]);

                    option.OptionOrder = Convert.ToInt32(drOption["OptionOrder"]);

                    Options.Add(option);
                }
            }
        }
    }

    public struct ChoiceOption
    {
        public void Insert(Guid itemGuid, SqlTransaction trans)
        {
            DataAccess.ChoiceOption.InsertChoiceOption(itemGuid, OptionDisplay, OptionName, OptionRate.Value, OptionOrder,
                trans);
        }

        //public static void CleanNoCasinoItemChoiceOption()
        //{
        //    using (var conn = SQLConn.GetConnection())
        //    {
        //        conn.Open();
        //        var trans = conn.BeginTransaction();
        //        try
        //        {
        //            DataAccess.ChoiceOption.CleanChoiceOption(trans);
        //            trans.Commit();
        //        }
        //        catch
        //        {
        //            trans.Rollback();
        //        }

        //        //conn.Close();
        //    }
        //}

        public static float GetOptionTotalBet(Guid itemGuid, string optionValue)
        {
            return DataAccess.ChoiceOption.GetOptionTotalBet(itemGuid, optionValue);
        }

        public static int GetOptionTotalCount(Guid itemGuid, string optionValue)
        {
            return DataAccess.ChoiceOption.GetOptionTotalCount(itemGuid, optionValue);
        }

        public Guid CasinoItemGuid { get; set; }

        public string OptionName { get; set; }

        public string OptionDisplay { get; set; }

        public float? OptionRate { get; set; }

        public int OptionOrder { get; set; }
    }

    public static class MatchChoiceOption
    {
        public static readonly string HomeWinValue = "Home";
        public static readonly string DrawValue = "Draw";
        public static readonly string AwayWinValue = "Away";

        public static void SaveMatchChoiceOption(int id, string optionValue, SqlTransaction trans)
        {
            DataAccess.BetDetail.InsertBetDetail(id, optionValue, null, trans);
        }
    }
}