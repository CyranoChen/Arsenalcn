using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Arsenalcn.Framework.Entity
{
    //[MetadataType(typeof(User.Metadata))]
    public class User : EntityBase
    {
        public User()
        {
            Accounts = new Collection<Account>();
            //Payments = new Collection<Payment>();
            //Selling = new Collection<Auction>();
            //WatchedAuctions = new Collection<Auction>();
        }

        //protected override string GenerateKey()
        //{
        //    if (string.IsNullOrWhiteSpace(Name))
        //        throw new EntityKeyGenerationException(this.GetType(), "User.Name is empty");

        //    return KeyGenerator.Generate(Name);
        //}

        public virtual string DisplayName
        {
            get { return _displayName ?? base.Name; }
            set { _displayName = value; }
        }
        private string _displayName;

        public string Mobile { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }

        //public class Metadata
        //{
        //    //[Required, StringLength(50, MinimumLength = 1)]
        //    //public object Name;

        //    [Required, StringLength(100)]
        //    public object DisplayName;

        //    [Required, StringLength(20, MinimumLength = 10)]
        //    public object Mobile;

        //    [Required, StringLength(100, MinimumLength = 5)]
        //    public object Email;
        //}
    }
}