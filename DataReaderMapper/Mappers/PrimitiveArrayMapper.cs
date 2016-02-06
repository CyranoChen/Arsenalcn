using System;
using DataReaderMapper.Internal;

namespace DataReaderMapper.Mappers
{
    public class PrimitiveArrayMapper : IObjectMapper
    {
        public object Map(ResolutionContext context, IMappingEngineRunner mapper)
        {
            if (context.IsSourceValueNull && mapper.ShouldMapSourceCollectionAsNull(context))
            {
                return null;
            }

            var sourceElementType = TypeHelper.GetElementType(context.SourceType);
            var destElementType = TypeHelper.GetElementType(context.DestinationType);

            var sourceArray = (Array) context.SourceValue ?? ObjectCreator.CreateArray(sourceElementType, 0);

            var sourceLength = sourceArray.Length;
            var destArray = ObjectCreator.CreateArray(destElementType, sourceLength);

            Array.Copy(sourceArray, destArray, sourceLength);

            return destArray;
        }

        public bool IsMatch(ResolutionContext context)
        {
            return IsPrimitiveArrayType(context.DestinationType) &&
                   IsPrimitiveArrayType(context.SourceType) &&
                   (TypeHelper.GetElementType(context.DestinationType)
                       .Equals(TypeHelper.GetElementType(context.SourceType)));
        }

        private bool IsPrimitiveArrayType(Type type)
        {
            if (type.IsArray)
            {
                var elementType = TypeHelper.GetElementType(type);
                return TypeExtensions.IsPrimitive(elementType) || elementType.Equals(typeof (string));
            }

            return false;
        }
    }
}