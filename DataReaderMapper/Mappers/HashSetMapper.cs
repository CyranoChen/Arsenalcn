using System;
using System.Collections.Generic;
using System.Linq;
using DataReaderMapper.Internal;

namespace DataReaderMapper.Mappers
{
    public class HashSetMapper : IObjectMapper
    {
        public object Map(ResolutionContext context, IMappingEngineRunner mapper)
        {
            var genericType = typeof (EnumerableMapper<,>);

            var collectionType = context.DestinationType;
            var elementType = TypeHelper.GetElementType(context.DestinationType);

            var enumerableMapper = genericType.MakeGenericType(collectionType, elementType);

            var objectMapper = (IObjectMapper) Activator.CreateInstance(enumerableMapper);

            return objectMapper.Map(context, mapper);
        }

        public bool IsMatch(ResolutionContext context)
        {
            var isMatch = PrimitiveExtensions.IsEnumerableType(context.SourceType) && IsSetType(context.DestinationType);

            return isMatch;
        }

#if !NETFX_CORE
        private static bool IsSetType(Type type)
        {
            if (TypeExtensions.IsGenericType(type) && type.GetGenericTypeDefinition() == typeof (ISet<>))
            {
                return true;
            }

            var genericInterfaces = type.GetInterfaces().Where(t => TypeExtensions.IsGenericType(t));
            var baseDefinitions = genericInterfaces.Select(t => t.GetGenericTypeDefinition());

            var isCollectionType = baseDefinitions.Any(t => t == typeof (ISet<>));

            return isCollectionType;
        }


        private class EnumerableMapper<TCollection, TElement> : EnumerableMapperBase<TCollection>
            where TCollection : ISet<TElement>
        {
            public override bool IsMatch(ResolutionContext context)
            {
                throw new NotImplementedException();
            }

            protected override void SetElementValue(TCollection destination, object mappedValue, int index)
            {
                destination.Add((TElement) mappedValue);
            }

            protected override void ClearEnumerable(TCollection enumerable)
            {
                enumerable.Clear();
            }

            protected override TCollection CreateDestinationObjectBase(Type destElementType, int sourceLength)
            {
                object collection;

                if (TypeExtensions.IsInterface(typeof (TCollection)))
                {
                    collection = new HashSet<TElement>();
                }
                else
                {
                    collection = ObjectCreator.CreateDefaultValue(typeof (TCollection));
                }

                return (TCollection) collection;
            }
        }

#else

        private static bool IsSetType(Type type)
        {
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(ISet<>))
            {
                return true;
            }

            IEnumerable<Type> genericInterfaces = type.GetTypeInfo().ImplementedInterfaces.Where(t => t.GetTypeInfo().IsGenericType);
            IEnumerable<Type> baseDefinitions = genericInterfaces.Select(t => t.GetGenericTypeDefinition());

            var isCollectionType = baseDefinitions.Any(t => t == typeof(ISet<>));

            return isCollectionType;
        }


        private class EnumerableMapper<TCollection, TElement> : EnumerableMapperBase<TCollection>
            where TCollection : ISet<TElement>
        {
            public override bool IsMatch(ResolutionContext context)
            {
                throw new NotImplementedException();
            }

            protected override void SetElementValue(TCollection destination, object mappedValue, int index)
            {
                destination.Add((TElement)mappedValue);
            }

            protected override void ClearEnumerable(TCollection enumerable)
            {
                enumerable.Clear();
            }

            protected override TCollection CreateDestinationObjectBase(Type destElementType, int sourceLength)
            {
                Object collection;

                if (typeof(TCollection).GetTypeInfo().IsInterface)
                {
                    collection = new HashSet<TElement>();
                }
                else
                {
                    collection = ObjectCreator.CreateDefaultValue(typeof(TCollection));
                }

                return (TCollection)collection;
            }
        }

#endif
    }
}