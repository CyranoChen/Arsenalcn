using System;
using System.Collections.Generic;
using System.Linq;
using Arsenalcn.Core;

namespace iArsenal.Service
{
    [DbSchema("iArsenal_Showcase", Sort = "OrderNum, CreateTime DESC")]
    public class Showcase : Entity<int>
    {
        public static class Cache
        {
            public static List<Showcase> ShowcaseList;

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

                ShowcaseList = repo.All<Showcase>().ToList();
            }

            public static Showcase Load(Guid guid)
            {
                return ShowcaseList.Find(x => x.ProductGuid.Equals(guid));
            }

            public static Showcase Load(string code)
            {
                return ShowcaseList.Find(x => x.ProductCode.Equals(code, StringComparison.OrdinalIgnoreCase));
            }
        }

        #region Members and Properties

        [DbColumn("ProductGuid")]
        public Guid ProductGuid { get; set; }

        [DbColumn("ProductCode")]
        public string ProductCode { get; set; }

        [DbColumn("OrderNum")]
        public int OrderNum { get; set; }

        [DbColumn("Category")]
        public ShowcaseCategroyType Category { get; set; }

        [DbColumn("CreateTime")]
        public DateTime CreateTime { get; set; }

        [DbColumn("IsActive")]
        public bool IsActive { get; set; }

        [DbColumn("Remark")]
        public string Remark { get; set; }

        #endregion
    }

    public enum ShowcaseCategroyType
    {
        None = 0
    }

}
