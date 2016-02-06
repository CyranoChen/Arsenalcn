using DataReaderMapper.Internal;

namespace DataReaderMapper.Mappers
{
    public class OpenGenericMapper : IObjectMapper
    {
        public object Map(ResolutionContext context, IMappingEngineRunner mapper)
        {
            var typeMap = mapper.ConfigurationProvider.FindClosedGenericTypeMapFor(context);

            var newContext = context.CreateTypeContext(typeMap, context.SourceValue, context.DestinationValue,
                context.SourceType,
                context.DestinationType);

            return mapper.Map(newContext);
        }

        public bool IsMatch(ResolutionContext context)
        {
            return (context.TypeMap == null
                    && TypeExtensions.IsGenericType(context.SourceType)
                    && TypeExtensions.IsGenericType(context.DestinationType)
                    && (context.SourceType.GetGenericTypeDefinition() != null)
                    && (context.DestinationType.GetGenericTypeDefinition() != null)
                    && context.Engine.ConfigurationProvider.HasOpenGenericTypeMapDefined(context));
        }
    }
}