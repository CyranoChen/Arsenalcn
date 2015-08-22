using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Web;
using System.Linq.Expressions;
using System.Linq;

namespace Arsenalcn.Core
{
    public abstract class Viewer : IViewer
    {
        protected Viewer() { }

        //protected Viewer(DataRow dr)
        //{
        //    try
        //    {
        //        Contract.Requires(dr != null);

        //        foreach (var pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        //        {
        //            var attrCol = Repository.GetColumnAttr(pi);

        //            // skip not db column
        //            if (attrCol == null) { continue; }

        //            var type = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;

        //            // skip IEnumerable property
        //            if (type.BaseType == null) { continue; }

        //            if (type.BaseType.Equals(typeof(Entity<Guid>)) || type.BaseType.Equals(typeof(Entity<int>)))
        //            {
        //                // skip inner object not be included
        //                if (dr.Table.Columns.Contains("@include"))
        //                {
        //                    var includeTypes = dr["@include"].ToString().Split('|');

        //                    if (!includeTypes.Any(x => x.Equals(type.FullName))) { continue; }
        //                }

        //                if (!Convert.IsDBNull(dr[attrCol.Key]))
        //                {
        //                    // new inner object instance
        //                    object instance = Activator.CreateInstance(type);

        //                    // set the primary key of inner object
        //                    var piKey = instance.GetType().GetProperty("ID");

        //                    piKey.SetValue(instance, Convert.ChangeType(dr[attrCol.Key],
        //                        Nullable.GetUnderlyingType(piKey.PropertyType) ?? piKey.PropertyType), null);

        //                    foreach (var piInner in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        //                    {
        //                        var attrColInner = Repository.GetColumnAttr(piInner);

        //                        // skip not db column
        //                        if (attrColInner == null) { continue; }

        //                        var columnName = string.Format("{0}_{1}", attrCol.Name, attrColInner.Name);

        //                        if (dr.Table.Columns.Contains(columnName))
        //                        {
        //                            this.SetPropertyValue(instance, piInner, dr[columnName]);
        //                        }
        //                    }

        //                    pi.SetValue(this, Convert.ChangeType(instance, type), null);
        //                }
        //            }
        //            else
        //            {
        //                if (dr.Table.Columns.Contains(attrCol.Name))
        //                {
        //                    this.SetPropertyValue(this, pi, dr[attrCol.Name]);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(ex, new LogInfo()
        //        {
        //            MethodInstance = MethodBase.GetCurrentMethod(),
        //            ThreadInstance = Thread.CurrentThread
        //        });

        //        throw;
        //    }
        //}

        //private void SetPropertyValue(object instance, PropertyInfo property, object value)
        //{
        //    if (!Convert.IsDBNull(value))
        //    {
        //        var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

        //        // SetValue for EnumType
        //        if (type.BaseType.Equals(typeof(Enum)))
        //        {
        //            object valEnum = Enum.Parse(type, value.ToString(), true);

        //            property.SetValue(instance, Convert.ChangeType(valEnum, type), null);
        //        }
        //        else
        //        {
        //            property.SetValue(instance, Convert.ChangeType(value, type), null);
        //        }
        //    }
        //}

        #region Members and Properties

        [Unique, StringLength(50)]
        public virtual string Key
        {
            get { return _key = _key ?? GenerateKey(); }
            protected set { _key = value; }
        }
        private string _key;

        #endregion

        protected virtual string GenerateKey()
        {
            return KeyGenerator.Generate();
        }

        public override string ToString()
        {
            return Key;
        }

        private static class KeyGenerator
        {
            public static string Generate()
            {
                return Generate(Guid.NewGuid().ToString("D").Substring(24));
            }

            public static string Generate(string input)
            {
                Contract.Requires(!string.IsNullOrWhiteSpace(input));
                return HttpUtility.UrlEncode(input.Replace(" ", "_").Replace("-", "_").Replace("&", "and"));
            }
        }

        public virtual void Many<T>(Expression<Func<T, bool>> whereBy) where T : class, IViewer, new()
        {
            // Get the property which matches IEnumerable<T>
            var property = this.GetType().GetProperties()
                .Where(x => (Nullable.GetUnderlyingType(x.PropertyType) ?? x.PropertyType).Equals(typeof(IEnumerable<T>)))
                .FirstOrDefault();

            //var propertyName = string.Format("List{0}", typeof(T).Name);
            //var property = this.GetType().GetProperty(propertyName, typeof(IEnumerable<T>));

            if (property != null)
            {
                IRepository repo = new Repository();

                var list = repo.Query<T>(whereBy);

                if (list != null && list.Count > 0)
                {
                    property.SetValue(this, list, null);
                }
            }
        }
    }

    public interface IViewer
    {
        /// <summary>
        /// The entity's unique (and URL-safe) public identifier
        /// </summary>
        /// <remarks>
        /// This is the identifier that should be exposed via the web, etc.
        /// </remarks>
        string Key { get; }

        void Many<T>(Expression<Func<T, bool>> whereBy) where T : class, IViewer, new();
    }
}
