using System;
using DataReaderMapper.Internal;

namespace DataReaderMapper.Mappers
{
    public class MultidimensionalArrayMapper : EnumerableMapperBase<Array>
    {
        private MultidimensionalArrayFiller filler;

        public override bool IsMatch(ResolutionContext context)
        {
            return context.DestinationType.IsArray && context.DestinationType.GetArrayRank() > 1 &&
                   PrimitiveExtensions.IsEnumerableType(context.SourceType);
        }

        protected override void ClearEnumerable(Array enumerable)
        {
            // no op
        }

        protected override void SetElementValue(Array destination, object mappedValue, int index)
        {
            filler.NewValue(mappedValue);
        }

        protected override Array CreateDestinationObjectBase(Type destElementType, int sourceLength)
        {
            throw new NotImplementedException();
        }

        protected override object GetOrCreateDestinationObject(ResolutionContext context, IMappingEngineRunner mapper,
            Type destElementType, int sourceLength)
        {
            var sourceArray = context.SourceValue as Array;
            if (sourceArray == null)
            {
                return ObjectCreator.CreateArray(destElementType, sourceLength);
            }
            var destinationArray = ObjectCreator.CreateArray(destElementType, sourceArray);
            filler = new MultidimensionalArrayFiller(destinationArray);
            return destinationArray;
        }
    }

    public class MultidimensionalArrayFiller
    {
        private readonly Array destination;
        private readonly int[] indices;

        public MultidimensionalArrayFiller(Array destination)
        {
            indices = new int[destination.Rank];
            this.destination = destination;
        }

        public void NewValue(object value)
        {
            var dimension = destination.Rank - 1;
            var changedDimension = false;
            while (indices[dimension] == destination.GetLength(dimension))
            {
                indices[dimension] = 0;
                dimension--;
                if (dimension < 0)
                {
                    throw new InvalidOperationException("Not enough room in destination array " + destination);
                }
                indices[dimension]++;
                changedDimension = true;
            }
            destination.SetValue(value, indices);
            if (changedDimension)
            {
                indices[dimension + 1]++;
            }
            else
            {
                indices[dimension]++;
            }
        }
    }
}