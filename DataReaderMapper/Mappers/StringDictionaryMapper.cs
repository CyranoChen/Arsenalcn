using System.Linq;
using DataReaderMapper.Internal;
using StringDictionary = System.Collections.Generic.IDictionary<string, object>;

namespace DataReaderMapper.Mappers
{
    public class ToStringDictionaryMapper : IObjectMapper
    {
        public bool IsMatch(ResolutionContext context)
        {
            return typeof (StringDictionary).IsAssignableFrom(context.DestinationType);
        }

        public object Map(ResolutionContext context, IMappingEngineRunner mapper)
        {
            var source = context.SourceValue;
            var sourceType = source.GetType();
            var sourceTypeDetails = new TypeDetails(sourceType, _ => true, _ => true);
            var membersDictionary = sourceTypeDetails.PublicReadAccessors.ToDictionary(p => p.Name,
                p => ReflectionHelper.GetMemberValue(p, source));
            var newContext = context.CreateTypeContext(null, membersDictionary, context.DestinationValue,
                membersDictionary.GetType(), context.DestinationType);
            return mapper.Map(newContext);
        }
    }

    public class FromStringDictionaryMapper : IObjectMapper
    {
        public bool IsMatch(ResolutionContext context)
        {
            return context.SourceValue is StringDictionary;
        }

        public object Map(ResolutionContext context, IMappingEngineRunner mapper)
        {
            var dictionary = (StringDictionary) context.SourceValue;
            var destination = mapper.CreateObject(context);
            var destTypeDetails = new TypeDetails(context.DestinationType, _ => true, _ => true);
            var members = from name in dictionary.Keys
                join member in destTypeDetails.PublicWriteAccessors on name equals member.Name
                select member;
            foreach (var member in members)
            {
                var value = ReflectionHelper.Map(member, dictionary[member.Name]);
                ReflectionHelper.SetMemberValue(member, destination, value);
            }
            return destination;
        }
    }
}