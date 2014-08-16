using System;
using System.Collections.Generic;

namespace iArsenal.Entity
{
    public class Order_ReplicaKit : OrderBase
    {
        public Order_ReplicaKit() { }

        public Order_ReplicaKit(int id)
            : base(id)
        {
            List<OrderItemBase> oiList = OrderItemBase.GetOrderItems(id).FindAll(oi => oi.IsActive && Product.Cache.Load(oi.ProductGuid) != null);

            if (oiList != null && oiList.Count > 0)
            {
                OrderItemBase oiBase = null;

                oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.ReplicaKitHome));
                if (oiBase != null) { OIReplicaKitHome = new OrderItem_ReplicaKitHome(oiBase.OrderItemID); }

                oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.ReplicaKitAway));
                if (oiBase != null) { OIReplicaKitAway = new OrderItem_ReplicaKitAway(oiBase.OrderItemID); }

                oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.ReplicaKitCup));
                if (oiBase != null) { OIReplicaKitCup = new OrderItem_ReplicaKitCup(oiBase.OrderItemID); }

                if (OIReplicaKitHome != null || OIReplicaKitAway != null || OIReplicaKitCup != null)
                {
                    base.URLOrderView = "iArsenalOrderView_ReplicaKit.aspx";

                    oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.PlayerNumber));
                    if (oiBase != null) { OIPlayerNumber = new OrderItem_PlayerNumber(oiBase.OrderItemID); }

                    oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.PlayerName));
                    if (oiBase != null) { OIPlayerName = new OrderItem_PlayerName(oiBase.OrderItemID); }

                    oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.ArsenalFont));
                    if (oiBase != null) { OIArsenalFont = new OrderItem_ArsenalFont(oiBase.OrderItemID); }

                    oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.PremiershipPatch));
                    if (oiBase != null) { OIPremiershipPatch = new OrderItem_PremiershipPatch(oiBase.OrderItemID); }

                    oiBase = oiList.Find(oi => Product.Cache.Load(oi.ProductGuid).ProductType.Equals(ProductType.ChampionshipPatch));
                    if (oiBase != null) { OIChampionshipPatch = new OrderItem_ChampionshipPatch(oiBase.OrderItemID); }
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

        public OrderItem_ReplicaKitHome OIReplicaKitHome { get; set; }

        public OrderItem_ReplicaKitAway OIReplicaKitAway { get; set; }

        public OrderItem_ReplicaKitCup OIReplicaKitCup { get; set; }

        public OrderItem_PlayerNumber OIPlayerNumber { get; set; }

        public OrderItem_PlayerName OIPlayerName { get; set; }

        public OrderItem_ArsenalFont OIArsenalFont { get; set; }

        public OrderItem_PremiershipPatch OIPremiershipPatch { get; set; }

        public OrderItem_ChampionshipPatch OIChampionshipPatch { get; set; }

        #endregion

    }
}
