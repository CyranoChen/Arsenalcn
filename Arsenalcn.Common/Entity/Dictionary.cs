using System;
using System.Collections.Generic;
using System.Data;

namespace Arsenalcn.Common.Entity
{
    public class Dictionary
    {
        public Dictionary()
        {
        }

        private Dictionary(DataRow dr)
        {
            InitDictionary(dr);
        }

        private void InitDictionary(DataRow dr)
        {
            if (dr != null)
            {
                DictionaryID = Convert.ToInt32(dr["ID"]);
                Name = dr["Name"].ToString();
                DisplayName = dr["DisplayName"].ToString();
                StandardLevel = dr["StandardLevel"].ToString();
                BusinessField = dr["BusinessField"].ToString();
                StandardCode = dr["StandardCode"].ToString();
                IsTreeDictionary = Convert.ToBoolean(dr["IsTreeDictionary"]);
                Description = dr["Description"].ToString();
            }
            else
                throw new Exception("Unable to init Dictionary.");
        }

        public void Select()
        {
            var dr = DataAccess.Dictionary.GetDictionaryByID(DictionaryID);

            if (dr != null)
                InitDictionary(dr);
        }

        private void Update()
        {
            DataAccess.Dictionary.UpdateDictionary(DictionaryID, Name, DisplayName, StandardLevel, BusinessField,
                StandardCode, IsTreeDictionary, Description);
        }

        private void Insert()
        {
            DataAccess.Dictionary.InsertDictionary(Name, DisplayName, StandardLevel, BusinessField, StandardCode,
                IsTreeDictionary, Description);
        }

        private void Delete()
        {
            DataAccess.Dictionary.DeleteDictionary(DictionaryID);
        }

        public static List<Dictionary> GetDictionaries()
        {
            var dt = DataAccess.Dictionary.GetDictionaries();
            var list = new List<Dictionary>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new Dictionary(dr));
                }
            }

            return list;
        }

        public static class Cache
        {
            public static List<Dictionary> DictionaryList;

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
                DictionaryList = GetDictionaries();
            }

            public static Dictionary Load(int id)
            {
                return DictionaryList.Find(delegate(Dictionary d) { return d.DictionaryID == id; });
            }
        }

        #region Members and Properties

        public int DictionaryID { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string StandardLevel { get; set; }

        public string BusinessField { get; set; }

        public string StandardCode { get; set; }

        public bool IsTreeDictionary { get; set; }

        public string Description { get; set; }

        #endregion
    }
}