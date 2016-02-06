using System.Reflection;

namespace DataReaderMapper
{
    public interface IMemberGetter : IMemberResolver
    {
        MemberInfo MemberInfo { get; }
        string Name { get; }
        object GetValue(object source);
    }
}