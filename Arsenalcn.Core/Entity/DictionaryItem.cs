using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Arsenalcn.Core
{
    [DbTable("Arsenalcn_DictionaryItem", Sort = "OrderNum")]
    public class DictionaryItem : Entity<int>
    {
        public DictionaryItem() : base() { }

        public DictionaryItem(DataRow dr) : base(dr) { }

        public static class Cache
        {
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

                DictionaryItemList_Region = repo.Query<DictionaryItem>(x => x.DictionaryID.Equals(108)).ToList();
            }

            public static DictionaryItem Load(int id)
            {
                return DictionaryItemList_Region.Find(x => x.ID.Equals(id));
            }

            public static List<DictionaryItem> DictionaryItemList_Region;
        }

        #region Members and Properties

        [DbColumn("Code")]
        public string Code
        { get; set; }

        [DbColumn("Name")]
        public string Name
        { get; set; }

        [DbColumn("Description")]
        public string Description
        { get; set; }

        [DbColumn("CustomCode")]
        public string CustomCode
        { get; set; }

        [DbColumn("Spell")]
        public string Spell
        { get; set; }

        [DbColumn("ShortSpell")]
        public string ShortSpell
        { get; set; }

        [DbColumn("ParentID")]
        public int ParentID
        { get; set; }

        [DbColumn("OrderNum")]
        public int OrderNum
        { get; set; }

        [DbColumn("DictionaryID")]
        public int DictionaryID
        { get; set; }

        #endregion
    }
}
