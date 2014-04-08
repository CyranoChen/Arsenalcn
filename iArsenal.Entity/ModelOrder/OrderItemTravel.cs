using System;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

namespace iArsenal.Entity
{
    public class OrderItem_TravelPlan : OrderItemBase
    {
        public OrderItem_TravelPlan() { }

        public OrderItem_TravelPlan(int id)
            : base(id)
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.TravelPlan))
                throw new Exception("The OrderItem is not the type of TravelPlan.");
        }

        public override void Update(SqlTransaction trans = null)
        {
            base.Size = string.Format("{0}|{1}", this.TravelFromDate.ToString("yyyy-MM-dd"), this.TravelToDate.ToString("yyyy-MM-dd"));
            base.Update(trans);
        }

        #region Members and Properties

        public DateTime TravelFromDate
        {
            get
            {
                DateTime _date = DateTime.MinValue;
                String _strDate = base.Size.Split('|')[0];
                if (!string.IsNullOrEmpty(_strDate) && DateTime.TryParse(_strDate, out _date))
                {
                    return _date;
                }
                else
                {
                    throw new Exception("Can't get the TravelFromDate of OrderItem_TravelPlan.Size");
                }
            }
            set
            {
                TravelFromDate = value;
            }
        }

        public DateTime TravelToDate
        {
            get
            {
                DateTime _date = DateTime.MinValue;
                String _strDate = base.Size.Split('|')[1];
                if (!string.IsNullOrEmpty(_strDate) && DateTime.TryParse(_strDate, out _date))
                {
                    return _date;
                }
                else
                {
                    throw new Exception("Can't get the TravelToDate of OrderItem_TravelPlan.Size");
                }
            }
            set
            {
                TravelToDate = value;
            }
        }

        public string[] TravelOption
        {
            get
            {
                if (!string.IsNullOrEmpty(Remark))
                {
                    return Remark.ToUpper().Split('|');
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Remark = string.Join("|", value);
            }
        }

        #endregion
    }

    public class OrderItem_TravelPartner : OrderItemBase
    {
        public OrderItem_TravelPartner() { }

        public OrderItem_TravelPartner(int id)
            : base(id)
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            Product p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.TravelPartner))
                throw new Exception("The OrderItem is not the type of TravelPartner.");
        }

        #region Members and Properties

        public Partner Partner
        {
            get
            {
                if (!string.IsNullOrEmpty(Remark))
                {
                    try
                    {
                        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                        return jsonSerializer.Deserialize<Partner>(Remark);

                    }
                    catch { throw new Exception("Can't get the Partner of OrderItem_TravelPartner.Remark"); }
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value != null)
                {
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    Remark = jsonSerializer.Serialize(value);
                }
                else
                {
                    Remark = string.Empty;
                }
            }
        }

        #endregion
    }
}
