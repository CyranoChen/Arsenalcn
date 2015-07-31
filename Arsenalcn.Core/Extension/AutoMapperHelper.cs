using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using AutoMapper;

namespace Arsenalcn.Core
{
    public static class AutoMapperHelper
    {
        /// <summary>
        ///  类型映射
        /// </summary>
        public static T MapTo<T>(this object obj)
        {
            if (obj == null) return default(T);

            Mapper.CreateMap(obj.GetType(), typeof(T));

            return Mapper.Map<T>(obj);
        }

        /// <summary>
        /// 类型映射
        /// </summary>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
            where TSource : class
            where TDestination : class
        {
            if (source == null) return destination;

            Mapper.CreateMap<TSource, TDestination>();

            return Mapper.Map(source, destination);
        }

        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static IEnumerable<TDestination> MapToList<TDestination>(this IEnumerable source)
        {
            foreach (var first in source)
            {
                var type = first.GetType();

                Mapper.CreateMap(type, typeof(TDestination));

                break;
            }

            return Mapper.Map<IEnumerable<TDestination>>(source);
        }

        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static IEnumerable<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            //IEnumerable<T> 类型需要创建元素的映射
            Mapper.CreateMap<TSource, TDestination>();

            return Mapper.Map<IEnumerable<TDestination>>(source);
        }

        ///// <summary>
        ///// DataReader映射
        ///// </summary>
        //public static T DataReaderMapTo<T>(this IDataReader reader)
        //{
        //    CreateDataReaderMap<T>();

        //    return Mapper.Map<IDataReader, T>(reader);
        //}

        /// <summary>
        /// DataReader映射
        /// </summary>
        public static IEnumerable<T> DataReaderMapTo<T>(this IDataReader reader)
            where T : class, IViewer, new()
        {
            Mapper.Reset();

            var mapper = typeof(T).GetMethod("CreateMap",
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            if (mapper != null)
            {
                mapper.Invoke(null, null);
            }
            else
            {
                Mapper.CreateMap<IDataReader, T>();
            }

            return Mapper.Map<IDataReader, IEnumerable<T>>(reader);
        }

        /// <summary>
        /// IDataReader GetValueByColumnName Extension
        /// </summary>
        public static object GetValue(this IDataRecord reader, string colName)
        {
            if (reader != null && !string.IsNullOrEmpty(colName))
            {
                var index = reader.GetOrdinal(colName);

                if (index >= 0)
                {
                    return !reader.IsDBNull(index) ? reader.GetValue(index) : null;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}