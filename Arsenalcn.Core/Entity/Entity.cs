using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Arsenalcn.Core
{
    public abstract class Entity<TKey> : Viewer, IEntity where TKey : struct
    {
        protected Entity() : base() { }

        //protected Entity(DataRow dr)
        //    : base(dr)
        //{
        //    var attr = (DbSchema)Attribute.GetCustomAttribute(this.GetType(), typeof(DbSchema));

        //    if (attr != null)
        //    {
        //        this.ID = (TKey)dr[attr.Key];
        //    }
        //}

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

        #endregion

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
                return Equals(other.Key, Key);

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

        public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
        {
            return !Equals(left, right);
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
