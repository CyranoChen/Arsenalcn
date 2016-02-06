using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataReaderMapper.Internal;

namespace DataReaderMapper
{
    public interface ICtorParamConfigurationExpression<TSource>
    {
        /// <summary>
        ///     Map constructor parameter from member expression
        /// </summary>
        /// <typeparam name="TMember">Member type</typeparam>
        /// <param name="sourceMember">Member expression</param>
        void MapFrom<TMember>(Expression<Func<TSource, TMember>> sourceMember);
    }

    public class CtorParamConfigurationExpression<TSource> : ICtorParamConfigurationExpression<TSource>
    {
        private readonly ConstructorParameterMap _ctorParamMap;

        public CtorParamConfigurationExpression(ConstructorParameterMap ctorParamMap)
        {
            _ctorParamMap = ctorParamMap;
        }

        public void MapFrom<TMember>(Expression<Func<TSource, TMember>> sourceMember)
        {
            var visitor = new MemberInfoFinderVisitor();

            visitor.Visit(sourceMember);

            _ctorParamMap.ResolveUsing(visitor.Members);
        }

        private class MemberInfoFinderVisitor : ExpressionVisitor
        {
            private readonly List<IMemberGetter> _members = new List<IMemberGetter>();

            public IEnumerable<IMemberGetter> Members => _members;

            protected override Expression VisitMember(MemberExpression node)
            {
                _members.Add(ReflectionHelper.ToMemberGetter(node.Member));

                return base.VisitMember(node);
            }
        }
    }
}