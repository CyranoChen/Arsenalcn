using System.Collections.Generic;
using AutoMapper;

namespace Arsenalcn.Core
{
    public static class AutoMapperHelper
    {
        /// <summary>
        ///     类型映射
        /// </summary>
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
            where TSource : class
            where TDestination : class
        {
            if (source == null) return default(TDestination);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());

            var mapper = config.CreateMapper();

            return mapper.Map<TDestination>(source);
        }

        /// <summary>
        ///     集合列表类型映射
        /// </summary>
        public static IEnumerable<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
            where TSource : class
            where TDestination : class
        {
            if (source == null) return default(IEnumerable<TDestination>);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());

            var mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<TDestination>>(source);
        }
    }
}