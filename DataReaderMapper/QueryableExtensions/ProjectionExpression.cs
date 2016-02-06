using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DataReaderMapper.Internal;

namespace DataReaderMapper.QueryableExtensions
{
    using IObjectDictionary = System.Collections.Generic.IDictionary<string, object>;

    public class ProjectionExpression : IProjectionExpression
    {
        private static readonly MethodInfo QueryableSelectMethod = FindQueryableSelectMethod();
        private readonly IMappingEngine _mappingEngine;

        private readonly IQueryable _source;

        public ProjectionExpression(IQueryable source, IMappingEngine mappingEngine)
        {
            _source = source;
            _mappingEngine = mappingEngine;
        }

        public IQueryable<TResult> To<TResult>(object parameters = null)
        {
            return To<TResult>(parameters, new string[0]);
        }

        public IQueryable<TResult> To<TResult>(object parameters = null, params string[] membersToExpand)
        {
            var paramValues = GetParameters(parameters);
            return To<TResult>(paramValues, membersToExpand);
        }

        public IQueryable<TResult> To<TResult>(IObjectDictionary parameters)
        {
            return To<TResult>(parameters, new string[0]);
        }

        public IQueryable<TResult> To<TResult>(IObjectDictionary parameters, params string[] membersToExpand)
        {
            var members = GetMembers(typeof (TResult), membersToExpand);
            return To<TResult>(parameters, members);
        }

        public IQueryable<TResult> To<TResult>(object parameters = null,
            params Expression<Func<TResult, object>>[] membersToExpand)
        {
            return To<TResult>(GetParameters(parameters), GetMembers(membersToExpand));
        }

        public IQueryable<TResult> To<TResult>(IObjectDictionary parameters,
            params Expression<Func<TResult, object>>[] membersToExpand)
        {
            var members = GetMembers(membersToExpand);
            return To<TResult>(parameters, members);
        }

        private static MethodInfo FindQueryableSelectMethod()
        {
            Expression<Func<IQueryable<object>>> select =
                () => default(IQueryable<object>).Select(default(Expression<Func<object, object>>));
            var method = ((MethodCallExpression) select.Body).Method.GetGenericMethodDefinition();
            return method;
        }

        private static IObjectDictionary GetParameters(object parameters)
        {
            return TypeExtensions.GetDeclaredProperties((parameters ?? new object()).GetType())
                .ToDictionary(pi => pi.Name, pi => pi.GetValue(parameters, null));
        }

        private MemberInfo[] GetMembers(Type type, string[] membersToExpand)
        {
            return membersToExpand.Select(m => ReflectionHelper.GetFieldOrProperty(type, m)).ToArray();
        }

        private MemberInfo[] GetMembers<TResult>(Expression<Func<TResult, object>>[] membersToExpand)
        {
            return membersToExpand.Select(ReflectionHelper.GetFieldOrProperty).ToArray();
        }

        private IQueryable<TResult> To<TResult>(IObjectDictionary parameters, MemberInfo[] members)
        {
            var mapExpr = _mappingEngine.CreateMapExpression(_source.ElementType, typeof (TResult), parameters, members);

            return _source.Provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    QueryableSelectMethod.MakeGenericMethod(_source.ElementType, typeof (TResult)),
                    new[] {_source.Expression, Expression.Quote(mapExpr)}
                    )
                );
        }
    }
}