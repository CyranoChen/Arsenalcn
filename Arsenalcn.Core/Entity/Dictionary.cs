using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Arsenalcn.Core
{
    [DbTable("Arsenalcn_Dictionary", Sort = "ID")]
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

        [DbColumn("Name")]
        public string Name
        { get; set; }

        [DbColumn("DisplayName")]
        public string DisplayName
        { get; set; }

        [DbColumn("StandardLevel")]
        public string StandardLevel
        { get; set; }

        [DbColumn("BusinessField")]
        public string BusinessField
        { get; set; }

        [DbColumn("StandardCode")]
        public string StandardCode
        { get; set; }

        [DbColumn("IsTreeDictionary")]
        public Boolean IsTreeDictionary
        { get; set; }

        [DbColumn("Description")]
        public string Description
        { get; set; }

        #endregion
    }
}
