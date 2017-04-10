using System;
using System.Data;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Service.Casino
{
    [DbSchema("AcnCasino_ChoiceOption", Sort = "CasinoItemGuid, OptionOrder")]
    public class ChoiceOption : Entity<int>
    {
        public static void Clean(IDbTransaction trans = null)
        {
            //DELETE FROM AcnCasino_ChoiceOption WHERE (CasinoItemGuid NOT IN(SELECT CasinoItemGuid FROM AcnCasino_CasinoItem))
            var sql =
                $@"DELETE FROM {Repository.GetTableAttr<ChoiceOption>().Name} 
                     WHERE (CasinoItemGuid NOT IN (SELECT CasinoItemGuid FROM {Repository.GetTableAttr<CasinoItem>().Name}))";

            var dapper = DapperHelper.GetInstance();

            dapper.Execute(sql, trans);
        }

        #region Members and Properties

        [DbColumn("CasinoItemGuid")]
        public Guid CasinoItemGuid { get; set; }

        [DbColumn("OptionName")]
        public string OptionName { get; set; }

        [DbColumn("OptionDisplay")]
        public string OptionDisplay { get; set; }

        [DbColumn("OptionRate")]
        public float OptionRate { get; set; }

        [DbColumn("OptionOrder")]
        public int OptionOrder { get; set; }

        #endregion
    }
}