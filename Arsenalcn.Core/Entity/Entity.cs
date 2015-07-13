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
    public abstract class Entity<TKey> : IEntity where TKey : struct
    {
        private readonly ILog log = new AppLog();

        public Entity() { }

        protected Entity(DataRow dr)
        {
            try
            {
                Contract.Requires(dr != null);

                var attr = (DbTable)Attribute.GetCustomAttribute(this.GetType(), typeof(DbTable));

                this.ID = (TKey)dr[attr.Key];

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

        [Key]
        public virtual TKey ID
        {
            get
            {
                if (typeof(TKey).Equals(typeof(Guid)))
                {
                    if (_id == null || (_id != null && _id.Equals(Guid.Empty)))
                    {
                        _id = Guid.NewGuid();
                    }
                }

                return _id == null ? default(TKey) : (TKey)_id;
            }
            set { _id = value; }
        }
        private object _id;

        [Unique, StringLength(50)]
        public virtual string Key
        {
            get { return _key = _key ?? GenerateKey(); }
            protected set { _key = value; }
        }
        private string _key;

        #endregion

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

        protected virtual string GenerateKey()
        {
            return KeyGenerator.Generate();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Entity<TKey>)) return false;
            return Equals((Entity<TKey>)obj);
        }

        public bool Equals(Entity<TKey> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;

            if (default(TKey).Equals(ID) || default(TKey).Equals(other.ID))
                return Equals(other._key, _key);

            return other.ID.Equals(ID);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                if (default(TKey).Equals(ID))
                    return Key.GetHashCode() * 397;

                return ID.GetHashCode();
            }
        }

        public override string ToString()
        {
            return Key;
        }

        public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
        {
            return !Equals(left, right);
        }

        public static class KeyGenerator
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
    }

    public interface IEntity
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
