using System;

namespace DataReaderMapper.Internal
{
    public interface INullableConverter
    {
        Type UnderlyingType { get; }
        object ConvertFrom(object value);
    }
}