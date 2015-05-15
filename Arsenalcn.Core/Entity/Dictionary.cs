using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Arsenalcn.Core
{
    [AttrDbTable("Arsenalcn_Dictionary", Sort = "ID")]
    public class Dictionary : Entity<int>
    {
        public Dictionary() : base() { }

        public Dictionary(DataRow dr) : base(dr) { }

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

                DictionaryList = repo.All<Dictionary>().ToList();
            }

            public static Dictionary Load(int id)
            {
                return DictionaryList.Find(x => x.ID.Equals(id));
            }

            public static List<Dictionary> DictionaryList;
        }

        #region Members and Properties

        [AttrDbColumn("Name")]
        public string Name
        { get; set; }

        [AttrDbColumn("DisplayName")]
        public string DisplayName
        { get; set; }

        [AttrDbColumn("StandardLevel")]
        public string StandardLevel
        { get; set; }

        [AttrDbColumn("BusinessField")]
        public string BusinessField
        { get; set; }

        [AttrDbColumn("StandardCode")]
        public string StandardCode
        { get; set; }

        [AttrDbColumn("IsTreeDictionary")]
        public Boolean IsTreeDictionary
        { get; set; }

        [AttrDbColumn("Description")]
        public string Description
        { get; set; }

        #endregion
    }
}
