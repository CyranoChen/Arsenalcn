using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataReaderMapper.Internal;

namespace DataReaderMapper.Mappers
{
    // So IEnumerable<T> inherits IEnumerable
    // but IDictionary<TKey, TValue> DOES NOT inherit IDictionary
    // Fiddlesticks.
    public class DictionaryMapper : IObjectMapper
    {
        private static readonly Type KvpType = typeof (KeyValuePair<,>);

        public bool IsMatch(ResolutionContext context)
        {
            return (PrimitiveExtensions.IsDictionaryType(context.SourceType) &&
                    PrimitiveExtensions.IsDictionaryType(context.DestinationType));
        }

        public object Map(ResolutionContext context, IMappingEngineRunner mapper)
        {
            if (context.IsSourceValueNull && mapper.ShouldMapSourceCollectionAsNull(context))
            {
                return null;
            }
            var genericSourceDictType = PrimitiveExtensions.GetDictionaryType(context.SourceType);
            var sourceKeyType = genericSourceDictType.GetGenericArguments()[0];
            var sourceValueType = genericSourceDictType.GetGenericArguments()[1];
            var sourceKvpType = KvpType.MakeGenericType(sourceKeyType, sourceValueType);
            var genericDestDictType = PrimitiveExtensions.GetDictionaryType(context.DestinationType);
            var destKeyType = genericDestDictType.GetGenericArguments()[0];
            var destValueType = genericDestDictType.GetGenericArguments()[1];

            var kvpEnumerator = GetKeyValuePairEnumerator(context, sourceKvpType);
            var destDictionary = ObjectCreator.CreateDictionary(context.DestinationType, destKeyType, destValueType);
            var count = 0;
            while (kvpEnumerator.MoveNext())
            {
                var keyValuePair = kvpEnumerator.Current;
                var sourceKey = sourceKvpType.GetProperty("Key").GetValue(keyValuePair, new object[0]);
                var sourceValue = sourceKvpType.GetProperty("Value").GetValue(keyValuePair, new object[0]);

                var keyTypeMap = mapper.ConfigurationProvider.ResolveTypeMap(sourceKey, null, sourceKeyType,
                    destKeyType);
                var valueTypeMap = mapper.ConfigurationProvider.ResolveTypeMap(sourceValue, null, sourceValueType,
                    destValueType);

                var keyContext = context.CreateElementContext(keyTypeMap, sourceKey, sourceKeyType,
                    destKeyType, count);
                var valueContext = context.CreateElementContext(valueTypeMap, sourceValue, sourceValueType,
                    destValueType, count);

                var destKey = mapper.Map(keyContext);
                var destValue = mapper.Map(valueContext);

                genericDestDictType.GetMethod("Add").Invoke(destDictionary, new[] {destKey, destValue});

                count++;
            }

            return destDictionary;
        }

        private static IEnumerator GetKeyValuePairEnumerator(ResolutionContext context, Type sourceKvpType)
        {
            if (context.SourceValue == null)
            {
                return Enumerable.Empty<object>().GetEnumerator();
            }
            var sourceEnumerableValue = (IEnumerable) context.SourceValue;
            var dictionaryEntries =
                sourceEnumerableValue.Cast<object>()
                    .OfType<DictionaryEntry>()
                    .Select(e => Activator.CreateInstance(sourceKvpType, e.Key, e.Value));
            if (dictionaryEntries.Any())
            {
                return dictionaryEntries.GetEnumerator();
            }
            var enumerableKvpType = typeof (IEnumerable<>).MakeGenericType(sourceKvpType);
            if (enumerableKvpType.IsAssignableFrom(sourceEnumerableValue.GetType()))
            {
                return (IEnumerator) enumerableKvpType.GetMethod("GetEnumerator").Invoke(sourceEnumerableValue, null);
            }
            throw new AutoMapperMappingException(context, "Cannot map dictionary type " + context.SourceType);
        }
    }
}