using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using DataReaderMapper.Internal;
using Microsoft.CSharp.RuntimeBinder;
using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

namespace DataReaderMapper.Mappers
{
    public abstract class DynamicMapper : IObjectMapper
    {
        public abstract bool IsMatch(ResolutionContext context);

        public object Map(ResolutionContext context, IMappingEngineRunner mapper)
        {
            var source = context.SourceValue;
            var destination = mapper.CreateObject(context);
            foreach (var member in MembersToMap(source, destination))
            {
                object sourceMemberValue;
                try
                {
                    sourceMemberValue = GetSourceMember(member, source);
                }
                catch (RuntimeBinderException)
                {
                    continue;
                }
                var destinationMemberValue = ReflectionHelper.Map(member, sourceMemberValue);
                SetDestinationMember(member, destination, destinationMemberValue);
            }
            return destination;
        }

        protected abstract IEnumerable<MemberInfo> MembersToMap(object source, object destination);

        protected abstract object GetSourceMember(MemberInfo member, object target);

        protected abstract void SetDestinationMember(MemberInfo member, object target, object value);

        protected object GetDynamically(MemberInfo member, object target)
        {
            var binder = Binder.GetMember(CSharpBinderFlags.None, member.Name, ReflectionHelper.GetMemberType(member),
                new[] {CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)});
            var callsite = CallSite<Func<CallSite, object, object>>.Create(binder);
            return callsite.Target(callsite, target);
        }

        protected void SetDynamically(MemberInfo member, object target, object value)
        {
            var binder = Binder.SetMember(CSharpBinderFlags.None, member.Name, ReflectionHelper.GetMemberType(member),
                new[]
                {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                });
            var callsite = CallSite<Func<CallSite, object, object, object>>.Create(binder);
            callsite.Target(callsite, target, value);
        }
    }

    public class FromDynamicMapper : DynamicMapper
    {
        public override bool IsMatch(ResolutionContext context)
        {
            return ReflectionHelper.IsDynamic(context.SourceValue) &&
                   !ReflectionHelper.IsDynamic(context.DestinationType);
        }

        protected override IEnumerable<MemberInfo> MembersToMap(object source, object destination)
        {
            return new TypeDetails(destination.GetType()).PublicWriteAccessors;
        }

        protected override object GetSourceMember(MemberInfo member, object target)
        {
            return GetDynamically(member, target);
        }

        protected override void SetDestinationMember(MemberInfo member, object target, object value)
        {
            ReflectionHelper.SetMemberValue(member, target, value);
        }
    }

    public class ToDynamicMapper : DynamicMapper
    {
        public override bool IsMatch(ResolutionContext context)
        {
            return ReflectionHelper.IsDynamic(context.DestinationType) &&
                   !ReflectionHelper.IsDynamic(context.SourceValue);
        }

        protected override IEnumerable<MemberInfo> MembersToMap(object source, object destination)
        {
            return new TypeDetails(source.GetType()).PublicReadAccessors;
        }

        protected override object GetSourceMember(MemberInfo member, object target)
        {
            return ReflectionHelper.GetMemberValue(member, target);
        }

        protected override void SetDestinationMember(MemberInfo member, object target, object value)
        {
            SetDynamically(member, target, value);
        }
    }
}