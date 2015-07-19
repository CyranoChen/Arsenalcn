using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Threading;
using System.Web;

using Arsenalcn.Core.Logger;

namespace Arsenalcn.Core
{
    public abstract class Viewer : IViewer
    {
        private readonly ILog log = new AppLog();

        protected Viewer() { }

        protected Viewer(DataRow dr)
        {
            try
            {
                Contract.Requires(dr != null);

                var attr = (DbSchema)Attribute.GetCustomAttribute(this.GetType(), typeof(DbSchema));

                //this.ID = (TKey)dr[attr.Key];

                foreach (var pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var attrCol = Repository.GetColumnAttr(pi);
                    var type = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;

                    if (attrCol != null)
                    {
                        if (!Convert.IsDBNull(dr[attrCol.Name]))
                        {
                            // SetValue for EnumType
                            if (type.BaseType.Equals(typeof(Enum)))
                            {
                                object value = Enum.Parse(type, dr[attrCol.Name].ToString(), true);

                                pi.SetValue(this, Convert.ChangeType(value, type), null);
                            }
                            else
                            {
                                pi.SetValue(this, Convert.ChangeType(dr[attrCol.Name], type), null);
                            }
                        }
                    }
                    else
                    { continue; }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex, new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
            }
        }

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

        public virtual void Mapper(Object obj)
        {
            try
            {
                foreach (var properInfo in this.GetType()
                    .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    var properInfoOrgin = obj.GetType().GetProperty(properInfo.Name);
                    if (properInfoOrgin != null)
                    {
                        properInfo.SetValue(this, properInfoOrgin.GetValue(obj, null), null);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex, new LogInfo()
                {
                    MethodInstance = MethodBase.GetCurrentMethod(),
                    ThreadInstance = Thread.CurrentThread
                });

                throw;
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
    }
}
