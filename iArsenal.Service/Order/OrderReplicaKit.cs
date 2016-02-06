using System;
using System.Linq;
using Arsenalcn.Core;
using AutoMapper;

namespace iArsenal.Service
{
    public class OrdrReplicaKit : Order
    {
        public void Init()
        {
            IRepository repo = new Repository();

            var list = repo.Query<OrderItem>(x => x.OrderID == ID)
                .FindAll(x => x.IsActive && Product.Cache.Load(x.ProductGuid) != null);

            if (list.Any())
            {
                OrderItem oiBase = null;

                oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.ReplicaKitHome));
                if (oiBase != null)
                {
                    var mapperReplicakitHome = new MapperConfiguration(cfg =>
                        cfg.CreateMap<OrderItem, OrdrItmReplicaKitHome>().AfterMap((s, d) => d.Init()))
                        .CreateMapper();

                    OIReplicaKitHome = mapperReplicakitHome.Map<OrdrItmReplicaKitHome>(oiBase);
                }

                oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.ReplicaKitAway));
                if (oiBase != null)
                {
                    var mapperReplicakitAway = new MapperConfiguration(cfg =>
                        cfg.CreateMap<OrderItem, OrdrItmReplicaKitAway>().AfterMap((s, d) => d.Init()))
                        .CreateMapper();

                    OIReplicaKitAway = mapperReplicakitAway.Map<OrdrItmReplicaKitAway>(oiBase);
                }

                oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.ReplicaKitCup));
                if (oiBase != null)
                {
                    var mapperReplicakitCup = new MapperConfiguration(cfg =>
                        cfg.CreateMap<OrderItem, OrdrItmReplicaKitCup>().AfterMap((s, d) => d.Init()))
                        .CreateMapper();

                    OIReplicaKitCup = mapperReplicakitCup.Map<OrdrItmReplicaKitCup>(oiBase);
                }

                if (OIReplicaKitHome != null || OIReplicaKitAway != null || OIReplicaKitCup != null)
                {
                    UrlOrderView = "iArsenalOrderView_ReplicaKit.aspx";

                    oiBase =
                        list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.PlayerNumber));
                    if (oiBase != null)
                    {
                        var mapperPlayerNumber = new MapperConfiguration(cfg =>
                            cfg.CreateMap<OrderItem, OrdrItmPlayerNumber>().AfterMap((s, d) => d.Init()))
                            .CreateMapper();

                        OIPlayerNumber = mapperPlayerNumber.Map<OrdrItmPlayerNumber>(oiBase);
                    }

                    oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.PlayerName));
                    if (oiBase != null)
                    {
                        var mapperPlayerName = new MapperConfiguration(cfg =>
                            cfg.CreateMap<OrderItem, OrdrItmPlayerName>().AfterMap((s, d) => d.Init()))
                            .CreateMapper();

                        OIPlayerName = mapperPlayerName.Map<OrdrItmPlayerName>(oiBase);
                    }

                    oiBase =
                        list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.ArsenalFont));
                    if (oiBase != null)
                    {
                        var mapperArsenalFont = new MapperConfiguration(cfg =>
                            cfg.CreateMap<OrderItem, OrdrItmArsenalFont>().AfterMap((s, d) => d.Init()))
                            .CreateMapper();

                        OIArsenalFont = mapperArsenalFont.Map<OrdrItmArsenalFont>(oiBase);
                    }

                    oiBase =
                        list.Find(
                            x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.PremiershipPatch));
                    if (oiBase != null)
                    {
                        var mapperPremiershipPatch = new MapperConfiguration(cfg =>
                            cfg.CreateMap<OrderItem, OrdrItmPremiershipPatch>().AfterMap((s, d) => d.Init()))
                            .CreateMapper();

                        OIPremiershipPatch = mapperPremiershipPatch.Map<OrdrItmPremiershipPatch>(oiBase);
                    }

                    oiBase =
                        list.Find(
                            x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.ChampionshipPatch));
                    if (oiBase != null)
                    {
                        var mapperChampionshipPatch = new MapperConfiguration(cfg =>
                            cfg.CreateMap<OrderItem, OrdrItmChampionshipPatch>().AfterMap((s, d) => d.Init()))
                            .CreateMapper();

                        OIChampionshipPatch = mapperChampionshipPatch.Map<OrdrItmChampionshipPatch>(oiBase);
                    }
                }
                else
                {
                    throw new Exception("Unable to init Order_ReplicaKit.");
                }
            }

            #region Order Status Workflow Info

            var _strWorkflow = "{{ \"StatusType\": \"{0}\", \"StatusInfo\": \"{1}\" }}";

            string[] _workflowInfo =
            {
                string.Format(_strWorkflow, ((int) OrderStatusType.Draft), "未提交"),
                string.Format(_strWorkflow, ((int) OrderStatusType.Submitted), "审核中"),
                string.Format(_strWorkflow, ((int) OrderStatusType.Confirmed), "已确认"),
                string.Format(_strWorkflow, ((int) OrderStatusType.Ordered), "已下单"),
                string.Format(_strWorkflow, ((int) OrderStatusType.Delivered), "已发货")
            };

            StatusWorkflowInfo = _workflowInfo;

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