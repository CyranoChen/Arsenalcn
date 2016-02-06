using System;
using System.Reflection;

namespace DataReaderMapper.Internal
{
    public interface ISourceToDestinationNameMapper
    {
        MemberInfo GetMatchingMemberInfo(IGetTypeInfoMembers getTypeInfoMembers, TypeDetails typeInfo, Type destType,
            string nameToSearch);
    }
}