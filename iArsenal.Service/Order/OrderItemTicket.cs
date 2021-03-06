﻿using System;

namespace iArsenal.Service
{
    public class OrdrItmMatchTicket : OrderItem
    {
        public void Init()
        {
            try
            {
                MatchGuid = !string.IsNullOrEmpty(Remark) ? new Guid(Remark) : Guid.Empty;
            }
            catch
            {
                throw new Exception("Can't get the Partner of OrderItem_MatchTicket.Remark.");
            }

            DateTime date;
            if (!string.IsNullOrEmpty(Size) && DateTime.TryParse(Size, out date))
            {
                TravelDate = date;
            }
            else
            {
                throw new Exception("Can't get the TravelDate of OrderItem_MatchTicket.Size.");
            }

            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            var p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.MatchTicket))
                throw new Exception("The OrderItem is not the type of MatchTicket.");
        }

        public override void Place(Member m, Product p)
        {
            Remark = MatchGuid.ToString();
            Size = TravelDate.ToString("yyyy-MM-dd");

            //Product product = Product.Cache.ProductList.Find(p =>
            //    p.ProductType.Equals(ProductType.MatchTicket));

            base.Place(m, p);
        }

        #region Members and Properties

        public Guid MatchGuid { get; set; }

        public DateTime TravelDate { get; set; }

        #endregion
    }

    public class OrdrItm2017TicketBeijing : OrderItem
    {
        public void Init()
        {
            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            var p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.TicketFriendly))
                throw new Exception("The OrderItem is not the type of TicketFriendly.");
        }
    }

    public class OrdrItm2012TicketBeijing : OrderItem
    {
        #region Members and Properties

        public string SeatLevel { get; set; }

        #endregion

        public void Init()
        {
            SeatLevel = Size;

            if (ProductGuid == null)
                throw new Exception("Loading OrderItem failed.");

            var p = Product.Cache.Load(ProductGuid);

            if (!p.ProductType.Equals(ProductType.TicketFriendly))
                throw new Exception("The OrderItem is not the type of TicketFriendly.");
        }

        public override void Place(Member m, Product p)
        {
            Size = SeatLevel;

            base.Place(m, p);
        }
    }
}