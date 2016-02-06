using System;

namespace DataReaderMapper
{
    public interface ITypeMapFactory
    {
        TypeMap CreateTypeMap(Type sourceType, Type destinationType, IProfileConfiguration mappingOptions,
            MemberList memberList);
    }
}