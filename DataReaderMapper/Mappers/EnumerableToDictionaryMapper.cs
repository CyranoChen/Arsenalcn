using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataReaderMapper.Internal;

namespace DataReaderMapper.Mappers
{
    public class EnumerableToDictionaryMapper : IObjectMapper
    {
        private static readonly Type KvpType = typeof (KeyValuePair<,>);

        public bool IsMatch(ResolutionContext context)
        {
            return (PrimitiveExtensions.IsDictionaryType(context.DestinationType))
                   && (PrimitiveExtensions.IsEnumerableType(context.SourceType))
                   && (!PrimitiveExtensions.IsDictionaryType(context.SourceType));
        }

        public object Map(ResolutionContext context, IMappingEngineRunner mapper)
        {
            var sourceEnumerableValue = (IEnumerable) context.SourceValue ?? new object[0];
            var enumerableValue = sourceEnumerableValue.Cast<object>();

            var sourceElementType = TypeHelper.GetElementType(context.SourceType, sourceEnumerableValue);
            var genericDestDictType = PrimitiveExtensions.GetDictionaryType(context.DestinationType);
            var destKeyType = genericDestDictType.GetGenericArguments()[0];
            var destValueType = genericDestDictType.GetGenericArguments()[1];
            var destKvpType = KvpType.MakeGenericType(destKeyType, destValueType);

            var destDictionary = ObjectCreator.CreateDictionary(context.DestinationType, destKeyType, destValueType);
            var count = 0;

            foreach (var item in enumerableValue)
            {
                var typeMap = mapper.ConfigurationProvider.ResolveTypeMap(item, null, sourceElementType, destKvpType);

                var targetSourceType = typeMap != null ? typeMap.SourceType : sourceElementType;
                var targetDestinationType = typeMap != null ? typeMap.DestinationType : destKvpType;

                var newContext = context.CreateElementContext(typeMap, item, targetSourceType, targetDestinationType,
                    count);

                var mappedValue = mapper.Map(newContext);
                var keyProperty = mappedValue.GetType().GetProperty("Key");
                var destKey = keyProperty.GetValue(mappedValue, null);

                var valueProperty = mappedValue.GetType().GetProperty("Value");
                var destValue = valueProperty.GetValue(mappedValue, null);

                genericDestDictType.GetMethod("Add").Invoke(destDictionary, new[] {destKey, destValue});

                count++;
            }

            return destDictionary;
        }
    }
}