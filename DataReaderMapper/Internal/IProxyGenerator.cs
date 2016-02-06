using System;

namespace DataReaderMapper.Internal
{
    public interface IProxyGenerator
    {
        Type GetProxyType(Type interfaceType);
    }
}