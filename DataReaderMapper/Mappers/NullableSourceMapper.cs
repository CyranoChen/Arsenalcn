using DataReaderMapper.Internal;

namespace DataReaderMapper.Mappers
{
    public class NullableSourceMapper : IObjectMapper
    {
        public object Map(ResolutionContext context, IMappingEngineRunner mapper)
        {
            return context.SourceValue ?? mapper.CreateObject(context);
        }

        public bool IsMatch(ResolutionContext context)
        {
            return PrimitiveExtensions.IsNullableType(context.SourceType) &&
                   !PrimitiveExtensions.IsNullableType(context.DestinationType);
        }
    }
}