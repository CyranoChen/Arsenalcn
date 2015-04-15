using System;
using System.Collections.Generic;

namespace iArsenal.Entity
{
    public class OrdrReplicaKit : Order
    {
        public OrdrReplicaKit() { }

        public OrdrReplicaKit(int id) : base(id) { this.Init(); }

        private void Init()
        {
            List<OrderItem> oiList = OrderItem.GetOrderItems(OrderID).FindAll(oi => oi.IsActive && Product.Cache.Load(oi.ProductGuid) != null);

            if (oiList != null && oiList.Count > 0)
            {
                OrderItem oiBase = null;

                oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.ReplicaKitHome));
                if (oiBase != null) { OIReplicaKitHome = new OrdrItmReplicaKitHome(oiBase.OrderItemID); }

                oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.ReplicaKitAway));
                if (oiBase != null) { OIReplicaKitAway = new OrdrItemReplicaKitAway(oiBase.OrderItemID); }

                oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.ReplicaKitCup));
                if (oiBase != null) { OIReplicaKitCup = new OrdrItmReplicaKitCup(oiBase.OrderItemID); }

                if (OIReplicaKitHome != null || OIReplicaKitAway != null || OIReplicaKitCup != null)
                {
                    base.URLOrderView = "iArsenalOrderView_ReplicaKit.aspx";

                    oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.PlayerNumber));
                    if (oiBase != null) { OIPlayerNumber = new OrdrItmPlayerNumber(oiBase.OrderItemID); }

                    oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.PlayerName));
                    if (oiBase != null) { OIPlayerName = new OrdrItmPlayerName(oiBase.OrderItemID); }

                    oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.ArsenalFont));
                    if (oiBase != null) { OIArsenalFont = new OrdrItmArsenalFont(oiBase.OrderItemID); }

                    oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.PremiershipPatch));
                    if (oiBase != null) { OIPremiershipPatch = new OrdrItmPremiershipPatch(oiBase.OrderItemID); }

                    oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.ChampionshipPatch));
                    if (oiBase != null) { OIChampionshipPatch = new OrdrItmChampionshipPatch(oiBase.OrderItemID); }
                }
                else
                {
                    throw new Exception("Unable to init Order_ReplicaKit.");
                }
            }

            #region Order Status Workflow Info

            string _strWorkflow = "{{ \"StatusType\": \"{0}\", \"StatusInfo\": \"{1}\" }}";

            string[] _workflowInfo = {
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Draft).ToString(), "未提交"),
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Submitted).ToString(), "审核中"), 
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Confirmed).ToString(), "已确认"), 
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Ordered).ToString(), "已下单"), 
                                      string.Format(_strWorkflow, ((int)OrderStatusType.Delivered).ToString(), "已发货")
                                  };

            base.StatusWorkflowInfo = _workflowInfo;

            #endregion

        }

        #region Members and Properties

        public OrdrItmReplicaKitHome OIReplicaKitHome { get; set; }

        public OrdrItemReplicaKitAway OIReplicaKitAway { get; set; }

        public OrdrItmReplicaKitCup OIReplicaKitCup { get; set; }

        public OrdrItmPlayerNumber OIPlayerNumber { get; set; }

        public OrdrItmPlayerName OIPlayerName { get; set; }

        public OrdrItmArsenalFont OIArsenalFont { get; set; }

        public OrdrItmPremiershipPatch OIPremiershipPatch { get; set; }

        public OrdrItmChampionshipPatch OIChampionshipPatch { get; set; }

        #endregion

    }
}
