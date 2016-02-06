using System;

namespace DataReaderMapper
{
    public interface IMemberResolver : IValueResolver
    {
        Type MemberType { get; }
    }
}