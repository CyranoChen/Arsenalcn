using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Arsenalcn.CasinoSys.Entity
{
    public sealed class SingleChoice : CasinoItem
    {
        internal SingleChoice() { }

        public override Guid Save(SqlTransaction trans)
        {
            Guid newGuid = base.Save(trans);

            if (!ItemGuid.HasValue)
            {

                DataAccess.SingleChoice.InsertSingleChoice(newGuid, FloatingRate, trans);

                foreach (ChoiceOption option in Options)
                {
                    option.Insert(newGuid, trans);
                }
            }
            return newGuid;
        }

        protected override void BuildDetail()
        {
            DataRow dr = DataAccess.SingleChoice.GetSingleChoice(ItemGuid.Value);

            if (dr != null)
            {
                FloatingRate = Convert.ToBoolean(dr["FloatingRate"]);

                if (Convert.IsDBNull(dr["Result"]))
                    Result = null;
                else
                    Result = Convert.ToString(dr["Result"]);
            }

            //init options
            DataTable dt = DataAccess.ChoiceOption.GetChoiceOptions(ItemGuid.Value);

            foreach (DataRow drOption in dt.Rows)
            {
                ChoiceOption option = new ChoiceOption();
                option.CasinoItemGuid = (Guid)drOption["CasinoItemGuid"];
                option.OptionValue = Convert.ToString(drOption["OptionValue"]);
                option.OptionDisplay = Convert.ToString(drOption["OptionDisplay"]);

                if (Convert.IsDBNull(drOption["OptionRate"]))
                    option.OptionRate = null;
                else
                    option.OptionRate = Convert.ToSingle(drOption["OptionRate"]);

                option.OrderID = Convert.ToInt32(drOption["OrderID"]);

                _options.Add(option);
            }
        }

        private List<ChoiceOption> _options = new List<ChoiceOption>();

        public bool FloatingRate
        { get; set; }

        public string Result
        { get; set; }

        public List<ChoiceOption> Options
        {
            get
            {
                return _options;
            }
            set
            {
                _options = value;
            }
        }
    }

    public struct ChoiceOption
    {
        public void Insert(Guid itemGuid, SqlTransaction trans)
        {
            DataAccess.ChoiceOption.InsertChoiceOption(itemGuid, OptionDisplay, OptionValue, OptionRate.Value, OrderID, trans);
        }

        public static void CleanNoCasinoItemChoiceOption()
        {
            using (SqlConnection conn = DataAccess.SQLConn.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    DataAccess.ChoiceOption.CleanChoiceOption(trans);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                conn.Close();
            }
        }

        public static float GetOptionTotalBet(Guid itemGuid, string optionValue)
        {
            return DataAccess.ChoiceOption.GetOptionTotalBet(itemGuid, optionValue);
        }

        public static int GetOptionTotalCount(Guid itemGuid, string optionValue)
        {
            return DataAccess.ChoiceOption.GetOptionTotalCount(itemGuid, optionValue);
        }

        public Guid CasinoItemGuid
        { get; set; }

        public string OptionValue
        { get; set; }

        public string OptionDisplay
        { get; set; }

        public float? OptionRate
        { get; set; }

        public int OrderID
        { get; set; }
    }

    public static class MatchChoiceOption
    {
        public static readonly string HomeWinValue = "Home";
        public static readonly string DrawValue = "Draw";
        public static readonly string AwayWinValue = "Away";

        public static void SaveMatchChoiceOption(int betID, string optionValue, SqlTransaction trans)
        {
            DataAccess.BetDetail.InsertBetDetail(betID, optionValue, null, trans);
        }
    }
}
