using System;
using System.Web.Script.Serialization;

namespace iArsenal.Service
{
    public class OrdrItmTravelPlan : OrderItem
    {
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
        public new void Init()
        {
            base.Init();

            DateTime date;
            var arrDate = Size.Split('|');

            if (!string.IsNullOrEmpty(arrDate[0]) && DateTime.TryParse(arrDate[0], out date))
            {
                TravelFromDate = date;
            }
            else
            {
                throw new Exception("Can't get the TravelFromDate of OrderItem_TravelPlan.Size");
            }

            if (!string.IsNullOrEmpty(arrDate[1]) && DateTime.TryParse(arrDate[1], out date))
            {
                TravelToDate = date;
            }
            else
            {
                throw new Exception("Can't get the TravelToDate of OrderItem_TravelPlan.Size");
            }

            TravelOption = !string.IsNullOrEmpty(Remark) ? Remark.ToUpper().Split('|') : null;
        }

        public void Place(Member m)
        {
            Size = $"{TravelFromDate.ToString("yyyy-MM-dd")}|{TravelToDate.ToString("yyyy-MM-dd")}";

            Remark = TravelOption != null ? string.Join("|", TravelOption) : string.Empty;

            var product = Product.Cache.Load("iETPL");

            base.Place(m, product);
        }

        #region Members and Properties

        public DateTime TravelFromDate { get; set; }

        public DateTime TravelToDate { get; set; }

        public string[] TravelOption { get; set; }

        #endregion
    }

    public class OrdrItmTravelPlan2015AsiaTrophy : OrdrItmTravelPlan
    {
        public new void Init()
        {
            base.Init();

            bool value;
            if (!string.IsNullOrEmpty(Size) && bool.TryParse(Size, out value))
            {
                IsTicketOnly = value;
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
                catch
                {
                    throw new Exception("Can't get the Partner of OrderItem_TravelPlan.Remark");
                }
            }
            else
            {
                TravelOption = null;
            }
        }

        public void Place(Member m)
        {
            Size = IsTicketOnly.ToString();

            if (TravelOption != null)
            {
                var jsonSerializer = new JavaScriptSerializer();
                Remark = jsonSerializer.Serialize(TravelOption);
            }
            else
            {
                Remark = string.Empty;
            }

            var product = Product.Cache.Load("2015ATPL");

            base.Place(m, product);
        }

        #region Members and Properties

        public bool IsTicketOnly { get; set; }

        public TravelOption TravelOption { get; set; }

        #endregion
    }

    public class OrdrItmTravelPartner : OrderItem
    {
        #region Members and Properties

        public Partner Partner { get; set; }

        #endregion

        public void Init()
        {
            if (!string.IsNullOrEmpty(Remark))
            {
                try
                {
                    var jsonSerializer = new JavaScriptSerializer();
                    Partner = jsonSerializer.Deserialize<Partner>(Remark);
                }
                catch
                {
                    throw new Exception("Can't get the Partner of OrderItem_TravelPartner.Remark");
                }
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

        public override void Place(Member m, Product p)
        {
            if (Partner != null)
            {
                var jsonSerializer = new JavaScriptSerializer();
                Remark = jsonSerializer.Serialize(Partner);
            }
            else
            {
                Remark = string.Empty;
            }

            base.Place(m, p);
        }
    }
}