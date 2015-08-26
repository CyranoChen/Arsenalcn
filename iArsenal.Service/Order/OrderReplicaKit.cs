using System;
using Arsenalcn.Core;

namespace iArsenal.Service
{
    public class OrdrReplicaKit : Order
    {
        public OrdrReplicaKit() { }

        public void Init()
        {
            IRepository repo = new Repository();

            var list = repo.Query<OrderItem>(x => x.OrderID == ID && x.IsActive == true)
                .FindAll(x => Product.Cache.Load(x.ProductGuid) != null);

            if (list != null && list.Count > 0)
            {
                OrderItem oiBase = null;

                oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.ReplicaKitHome));
                if (oiBase != null)
                {
                    AutoMapper.Mapper.CreateMap<OrderItem, OrdrItmReplicaKitHome>().AfterMap((s, d) => d.Init());
                    OIReplicaKitHome = AutoMapper.Mapper.Map<OrdrItmReplicaKitHome>(oiBase);
                }

                oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.ReplicaKitAway));
                if (oiBase != null)
                {
                    AutoMapper.Mapper.CreateMap<OrderItem, OrdrItmReplicaKitAway>().AfterMap((s, d) => d.Init());
                    OIReplicaKitAway = AutoMapper.Mapper.Map<OrdrItmReplicaKitAway>(oiBase);
                }

                oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.ReplicaKitCup));
                if (oiBase != null)
                {
                    AutoMapper.Mapper.CreateMap<OrderItem, OrdrItmReplicaKitCup>().AfterMap((s, d) => d.Init());
                    OIReplicaKitCup = AutoMapper.Mapper.Map<OrdrItmReplicaKitCup>(oiBase);
                }

                if (OIReplicaKitHome != null || OIReplicaKitAway != null || OIReplicaKitCup != null)
                {
                    base.UrlOrderView = "iArsenalOrderView_ReplicaKit.aspx";

                    oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.PlayerNumber));
                    if (oiBase != null)
                    {
                        AutoMapper.Mapper.CreateMap<OrderItem, OrdrItmPlayerNumber>().AfterMap((s, d) => d.Init());
                        OIPlayerNumber = AutoMapper.Mapper.Map<OrdrItmPlayerNumber>(oiBase);
                    }

                    oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.PlayerName));
                    if (oiBase != null)
                    {
                        AutoMapper.Mapper.CreateMap<OrderItem, OrdrItmPlayerName>().AfterMap((s, d) => d.Init());
                        OIPlayerName = AutoMapper.Mapper.Map<OrdrItmPlayerName>(oiBase);
                    }

                    oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.ArsenalFont));
                    if (oiBase != null)
                    {
                        AutoMapper.Mapper.CreateMap<OrderItem, OrdrItmArsenalFont>().AfterMap((s, d) => d.Init());
                        OIArsenalFont = AutoMapper.Mapper.Map<OrdrItmArsenalFont>(oiBase);
                    }

                    oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.PremiershipPatch));
                    if (oiBase != null)
                    {
                        AutoMapper.Mapper.CreateMap<OrderItem, OrdrItmPremiershipPatch>().AfterMap((s, d) => d.Init());
                        OIPremiershipPatch = AutoMapper.Mapper.Map<OrdrItmPremiershipPatch>(oiBase);
                    }

                    oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.ChampionshipPatch));
                    if (oiBase != null)
                    {
                        AutoMapper.Mapper.CreateMap<OrderItem, OrdrItmChampionshipPatch>().AfterMap((s, d) => d.Init());
                        OIChampionshipPatch = AutoMapper.Mapper.Map<OrdrItmChampionshipPatch>(oiBase);
                    }
                }
                else
                {
                    throw new Exception("Unable to init Order_ReplicaKit.");
                }
            }

            #region Order Status Workflow Info

            var _strWorkflow = "{{ \"StatusType\": \"{0}\", \"StatusInfo\": \"{1}\" }}";

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

        public OrdrItmReplicaKitAway OIReplicaKitAway { get; set; }

        public OrdrItmReplicaKitCup OIReplicaKitCup { get; set; }

        public OrdrItmPlayerNumber OIPlayerNumber { get; set; }

        public OrdrItmPlayerName OIPlayerName { get; set; }

        public OrdrItmArsenalFont OIArsenalFont { get; set; }

        public OrdrItmPremiershipPatch OIPremiershipPatch { get; set; }

        public OrdrItmChampionshipPatch OIChampionshipPatch { get; set; }

        #endregion

    }
}
