using System;
using System.Collections;
using DataReaderMapper.Internal;

namespace DataReaderMapper.Mappers
{
    public class EnumerableMapper : EnumerableMapperBase<IList>
    {
        public override bool IsMatch(ResolutionContext context)
        {
            // destination type must be IEnumerable interface or a class implementing at least IList 
            return PrimitiveExtensions.IsEnumerableType(context.SourceType) &&
                   (PrimitiveExtensions.IsListType(context.DestinationType) ||
                    DestinationIListTypedAsIEnumerable(context));
        }

        private static bool DestinationIListTypedAsIEnumerable(ResolutionContext context)
        {
            return TypeExtensions.IsInterface(context.DestinationType) &&
                   PrimitiveExtensions.IsEnumerableType(context.DestinationType) &&
                   (context.DestinationValue == null || context.DestinationValue is IList);
        }

        protected override void SetElementValue(IList destination, object mappedValue, int index)
        {
            destination.Add(mappedValue);
        }

        protected override void ClearEnumerable(IList enumerable)
        {
            enumerable.Clear();
        }

        protected override IList CreateDestinationObjectBase(Type destElementType, int sourceLength)
        {
            return ObjectCreator.CreateList(destElementType);
        }
    }
}