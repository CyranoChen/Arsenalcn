using System;

namespace DataReaderMapper.Internal
{
    public interface INullableConverterFactory
    {
        INullableConverter Create(Type nullableType);
    }
}