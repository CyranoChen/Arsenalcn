using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Web;

using Arsenalcn.Framework.DataAnnotations;

namespace Arsenalcn.Framework.Entity
{
    public interface IEntity
    {
        /// <summary>
        /// The entity's unique (and URL-safe) public identifier
        /// </summary>
        /// <remarks>
        /// This is the identifier that should be exposed via the web, etc.
        /// </remarks>

        //object ID { get; set; }
        string Key { get; }
    }

    public abstract class Entity<TID> : IEntity, IEquatable<Entity<TID>>
        where TID : struct
    {
        [Key]
        public virtual TID ID
        {
            get
            {
                if (_id == null && typeof(TID) == typeof(Guid))
                    _id = Guid.NewGuid();

                return _id == null ? default(TID) : (TID)_id;
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



        //[Timestamp]
        //public byte[] RowVersion
        //{
        //    get;
        //    set;
        //}

        protected virtual string GenerateKey()
        {
            return KeyGenerator.Generate();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Entity<TID>)) return false;
            return Equals((Entity<TID>)obj);
        }

        public bool Equals(Entity<TID> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;

            if (default(TID).Equals(ID) || default(TID).Equals(other.ID))
                return Equals(other._key, _key);

            return other.ID.Equals(ID);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                if (default(TID).Equals(ID))
                    return Key.GetHashCode() * 397;

                return ID.GetHashCode();
            }
        }

        public override string ToString()
        {
            return Key;
        }

        public static bool operator ==(Entity<TID> left, Entity<TID> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<TID> left, Entity<TID> right)
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
}