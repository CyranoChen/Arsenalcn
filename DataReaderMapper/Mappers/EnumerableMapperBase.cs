using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataReaderMapper.Internal;

namespace DataReaderMapper.Mappers
{
    public abstract class EnumerableMapperBase<TEnumerable> : IObjectMapper
        where TEnumerable : IEnumerable
    {
        public object Map(ResolutionContext context, IMappingEngineRunner mapper)
        {
            if (context.IsSourceValueNull && mapper.ShouldMapSourceCollectionAsNull(context))
            {
                return null;
            }

            ICollection<object> enumerableValue = ((IEnumerable) context.SourceValue ?? new object[0])
                .Cast<object>()
                .ToList();

            var sourceElementType = TypeHelper.GetElementType(context.SourceType, enumerableValue);
            var destElementType = TypeHelper.GetElementType(context.DestinationType);

            var sourceLength = enumerableValue.Count;
            var destination = GetOrCreateDestinationObject(context, mapper, destElementType, sourceLength);
            var enumerable = GetEnumerableFor(destination);

            ClearEnumerable(enumerable);

            var i = 0;
            foreach (var item in enumerableValue)
            {
                var newContext = context.CreateElementContext(null, item, sourceElementType, destElementType, i);
                var elementResolutionResult = new ResolutionResult(newContext);

                var typeMap = mapper.ConfigurationProvider.ResolveTypeMap(elementResolutionResult, destElementType);

                var targetSourceType = typeMap != null ? typeMap.SourceType : sourceElementType;
                var targetDestinationType = typeMap != null ? typeMap.DestinationType : destElementType;

                newContext = context.CreateElementContext(typeMap, item, targetSourceType, targetDestinationType, i);

                var mappedValue = mapper.Map(newContext);

                SetElementValue(enumerable, mappedValue, i);

                i++;
            }

            var valueToAssign = destination;
            return valueToAssign;
        }

        public abstract bool IsMatch(ResolutionContext context);

        protected virtual object GetOrCreateDestinationObject(ResolutionContext context, IMappingEngineRunner mapper,
            Type destElementType, int sourceLength)
        {
            if (context.DestinationValue != null)
            {
                // If the source is not an array, assume we can add to it...
                if (!(context.DestinationValue is Array))
                    return context.DestinationValue;

                // If the source is an array, ensure that we have enough room...
                var array = (Array) context.DestinationValue;

                if (array.Length >= sourceLength)
                    return context.DestinationValue;
            }

            return CreateDestinationObject(context, destElementType, sourceLength, mapper);
        }

        protected virtual TEnumerable GetEnumerableFor(object destination)
        {
            return (TEnumerable) destination;
        }

        protected virtual void ClearEnumerable(TEnumerable enumerable)
        {
        }

        protected virtual object CreateDestinationObject(ResolutionContext context, Type destinationElementType,
            int count, IMappingEngineRunner mapper)
        {
            var destinationType = context.DestinationType;

            if (!TypeExtensions.IsInterface(destinationType) && !destinationType.IsArray)
            {
                return mapper.CreateObject(context);
            }
            return CreateDestinationObjectBase(destinationElementType, count);
        }


        protected abstract void SetElementValue(TEnumerable destination, object mappedValue, int index);
        protected abstract TEnumerable CreateDestinationObjectBase(Type destElementType, int sourceLength);
    }
}