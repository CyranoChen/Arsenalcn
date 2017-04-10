using System;
using System.Data;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;
using AutoMapper;

namespace iArsenal.Service
{
    public class OrdrPrinting : Order
    {
        public void Init()
        {
            IRepository repo = new Repository();

            var list = repo.Query<OrderItem>(x => x.OrderID == ID)
                .FindAll(x => x.IsActive && Product.Cache.Load(x.ProductGuid) != null);

            if (list.Count > 0)
            {
                var oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.PlayerNumber));
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

                if (OIPlayerNumber != null && OIPlayerName != null)
                {
                    UrlOrderView = "iArsenalOrderView_Printing.aspx";

                    oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.ArsenalFont));
                    if (oiBase != null)
                    {
                        var mapperArsenalFont = new MapperConfiguration(cfg =>
                            cfg.CreateMap<OrderItem, OrdrItmArsenalFont>().AfterMap((s, d) => d.Init()))
                            .CreateMapper();

                        OIArsenalFont = mapperArsenalFont.Map<OrdrItmArsenalFont>(oiBase);
                    }

                    oiBase = list.Find(x => Product.Cache.Load(x.ProductGuid).ProductType.Equals(ProductType.PremiershipPatch));
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
                    throw new Exception("Unable to init Order_Printing.");
                }
            }

            #region Order Status Workflow Info

            var strWorkflow = "{{ \"StatusType\": \"{0}\", \"StatusInfo\": \"{1}\" }}";

            string[] workflowInfo =
            {
                string.Format(strWorkflow, ((int) OrderStatusType.Draft), "未提交"),
                string.Format(strWorkflow, ((int) OrderStatusType.Submitted), "后台审核"),
                string.Format(strWorkflow, ((int) OrderStatusType.Confirmed), "球衣收到"),
                string.Format(strWorkflow, ((int) OrderStatusType.Ordered), "打印中"),
                string.Format(strWorkflow, ((int) OrderStatusType.Delivered), "已发货")
            };

            StatusWorkflowInfo = workflowInfo;

            #endregion
        }


        #region Members and Properties

        public OrdrItmPlayerNumber OIPlayerNumber { get; set; }

        public OrdrItmPlayerName OIPlayerName { get; set; }

        public OrdrItmArsenalFont OIArsenalFont { get; set; }

        public OrdrItmPremiershipPatch OIPremiershipPatch { get; set; }

        public OrdrItmChampionshipPatch OIChampionshipPatch { get; set; }

        #endregion
    }
}
