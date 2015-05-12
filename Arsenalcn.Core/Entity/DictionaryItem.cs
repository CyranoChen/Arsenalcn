using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Arsenalcn.Core
{
    [AttrDbTable("Arsenalcn_DictionaryItem", Sort = "OrderNum")]
    public class DictionaryItem : Entity<long>
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

        [AttrDbColumn("Code")]
        public string Code
        { get; set; }

        [AttrDbColumn("Name")]
        public string Name
        { get; set; }

        [AttrDbColumn("Description")]
        public string Description
        { get; set; }

        [AttrDbColumn("CustomCode")]
        public string CustomCode
        { get; set; }

        [AttrDbColumn("Spell")]
        public string Spell
        { get; set; }

        [AttrDbColumn("ShortSpell")]
        public string ShortSpell
        { get; set; }

        [AttrDbColumn("ParentID")]
        public int ParentID
        { get; set; }

        [AttrDbColumn("OrderNum")]
        public int OrderNum
        { get; set; }

        [AttrDbColumn("DictionaryID")]
        public int DictionaryID
        { get; set; }

        #endregion
    }
}
