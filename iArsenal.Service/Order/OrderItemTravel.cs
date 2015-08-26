using System;
using System.Web.Script.Serialization;

namespace iArsenal.Service
{
    public class OrdrItmTravelPlan : OrderItem
    {
        public OrdrItmTravelPlan() { }

        public void Init()
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            var p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.TravelPlan))
                throw new Exception("The OrderItem is not the type of TravelPlan.");
        }
    }

    public class OrdrItmTravelPlanLondon : OrdrItmTravelPlan
    {
        public OrdrItmTravelPlanLondon() { }

        public new void Init()
        {
            base.Init();

            var _date = DateTime.MinValue;
            var _arrDate = Size.Split('|');

            if (!string.IsNullOrEmpty(_arrDate[0]) && DateTime.TryParse(_arrDate[0], out _date))
            {
                TravelFromDate = _date;
            }
            else
            {
                throw new Exception("Can't get the TravelFromDate of OrderItem_TravelPlan.Size");
            }

            if (!string.IsNullOrEmpty(_arrDate[1]) && DateTime.TryParse(_arrDate[1], out _date))
            {
                TravelToDate = _date;
            }
            else
            {
                throw new Exception("Can't get the TravelToDate of OrderItem_TravelPlan.Size");
            }

            if (!string.IsNullOrEmpty(Remark))
            {
                TravelOption = Remark.ToUpper().Split('|');
            }
            else
            {
                TravelOption = null;
            }
        }

        public void Place(Member m, System.Data.SqlClient.SqlTransaction trans = null)
        {
            this.Size = string.Format("{0}|{1}", TravelFromDate.ToString("yyyy-MM-dd"), TravelToDate.ToString("yyyy-MM-dd"));

            if (TravelOption != null)
            {
                this.Remark = string.Join("|", TravelOption);
            }
            else
            {
                this.Remark = string.Empty;
            }

            var product = Product.Cache.Load("iETPL");

            base.Place(m, product, trans);
        }

        #region Members and Properties

        public DateTime TravelFromDate { get; set; }

        public DateTime TravelToDate { get; set; }

        public string[] TravelOption { get; set; }

        #endregion
    }

    public class OrdrItmTravelPlan2015AsiaTrophy : OrdrItmTravelPlan
    {
        public OrdrItmTravelPlan2015AsiaTrophy() { }

        public new void Init()
        {
            base.Init();

            Boolean _value;
            if (!string.IsNullOrEmpty(Size) && Boolean.TryParse(Size, out _value))
            {
                IsTicketOnly = _value;
            }
            else
            {
                throw new Exception("Can't get IsTicketOnly of OrderItem_TravelPlan.Size");
            }

            if (!string.IsNullOrEmpty(Remark))
            {
                try
                {
                    var jsonSerializer = new JavaScriptSerializer();
                    TravelOption = jsonSerializer.Deserialize<TravelOption>(Remark);

                }
                catch { throw new Exception("Can't get the Partner of OrderItem_TravelPlan.Remark"); }
            }
            else
            {
                TravelOption = null;
            }
        }

        public void Place(Member m, System.Data.SqlClient.SqlTransaction trans = null)
        {
            this.Size = IsTicketOnly.ToString();

            if (TravelOption != null)
            {
                var jsonSerializer = new JavaScriptSerializer();
                this.Remark = jsonSerializer.Serialize(TravelOption);
            }
            else
            {
                this.Remark = string.Empty;
            }

            var product = Product.Cache.Load("2015ATPL");

            base.Place(m, product, trans);
        }

        #region Members and Properties

        public bool IsTicketOnly { get; set; }

        public TravelOption TravelOption { get; set; }

        #endregion
    }

    public class OrdrItmTravelPartner : OrderItem
    {
        public OrdrItmTravelPartner() { }

        public void Init()
        {
            if (!string.IsNullOrEmpty(Remark))
            {
                try
                {
                    var jsonSerializer = new JavaScriptSerializer();
                    Partner = jsonSerializer.Deserialize<Partner>(Remark);

                }
                catch { throw new Exception("Can't get the Partner of OrderItem_TravelPartner.Remark"); }
            }
            else
            {
                throw new Exception("Can't get the Partner of OrderItem_TravelPartner.Remark");
            }

            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            var p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.TravelPartner))
                throw new Exception("The OrderItem is not the type of TravelPartner.");
        }

        public override void Place(Member m, Product p, System.Data.SqlClient.SqlTransaction trans = null)
        {
            if (Partner != null)
            {
                var jsonSerializer = new JavaScriptSerializer();
                this.Remark = jsonSerializer.Serialize(Partner);
            }
            else
            {
                this.Remark = string.Empty;
            }

            base.Place(m, p, trans);
        }

        #region Members and Properties

        public Partner Partner { get; set; }

        #endregion
    }
}


