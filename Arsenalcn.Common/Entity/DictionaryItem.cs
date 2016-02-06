using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenalcn.Common.Entity
{
    public class DictionaryItem
    {
        public DictionaryItem()
        {
        }

        private DictionaryItem(DataRow dr)
        {
            InitDictionaryItem(dr);
        }

        private void InitDictionaryItem(DataRow dr)
        {
            if (dr != null)
            {
                ItemID = Convert.ToInt32(dr["ID"]);
                DictionaryID = Convert.ToInt32(dr["DictionaryID"]);
                Code = dr["Code"].ToString();
                Name = dr["Name"].ToString();
                Description = dr["Description"].ToString();
                CustomCode = dr["CustomCode"].ToString();
                Spell = dr["Spell"].ToString();
                ShortSpell = dr["ShortSpell"].ToString();
                ParentID = Convert.ToInt32(dr["ParentID"]);
                OrderNum = Convert.ToInt32(dr["OrderNum"]);
            }
            else
                throw new Exception("Unable to init DictionaryItem.");
        }

        protected void Select()
        {
            var dr = DataAccess.DictionaryItem.GetDictionaryItemByID(ItemID);

            if (dr != null)
                InitDictionaryItem(dr);
        }

        private void Update()
        {
            DataAccess.DictionaryItem.UpdateDictionaryItem(ItemID, DictionaryID, Code, Name, Description, CustomCode,
                Spell, ShortSpell, ParentID, OrderNum);
        }

        private void Insert()
        {
            DataAccess.DictionaryItem.InsertDictionaryItem(DictionaryID, Code, Name, Description, CustomCode, Spell,
                ShortSpell, ParentID, OrderNum);
        }

        private void Delete()
        {
            DataAccess.DictionaryItem.DeleteDictionaryItem(ItemID);
        }

        protected static List<DictionaryItem> GetDictionaryItems()
        {
            var dt = DataAccess.DictionaryItem.GetDictionaryItems();
            var list = new List<DictionaryItem>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new DictionaryItem(dr));
                }
            }

            return list;
        }

        public static class Cache
        {
            public static List<DictionaryItem> DictionaryItemList_Region;

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
                DictionaryItemList_Region = GetDictionaryItems();
                DictionaryItemList_Region =
                    DictionaryItemList_Region.FindAll(delegate(DictionaryItem di) { return di.DictionaryID == 108; });
                DictionaryItemList_Region.Sort(
                    delegate(DictionaryItem itemA, DictionaryItem itemB) { return itemA.OrderNum - itemB.OrderNum; });
            }

            public static DictionaryItem Load(int id)
            {
                return DictionaryItemList_Region.Find(delegate(DictionaryItem i) { return i.ItemID == id; });
            }
        }

        #region Members and Properties

        public int ItemID { get; set; }

        public int DictionaryID { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CustomCode { get; set; }

        public string Spell { get; set; }

        public string ShortSpell { get; set; }

        public int ParentID { get; set; }

        public int OrderNum { get; set; }

        #endregion
    }
}