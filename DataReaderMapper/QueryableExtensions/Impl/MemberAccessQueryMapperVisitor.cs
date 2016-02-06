using System.Linq.Expressions;

namespace DataReaderMapper.QueryableExtensions.Impl
{
    public class MemberAccessQueryMapperVisitor : ExpressionVisitor
    {
        private readonly IMappingEngine _mappingEngine;
        private readonly ExpressionVisitor _rootVisitor;

        public MemberAccessQueryMapperVisitor(ExpressionVisitor rootVisitor, IMappingEngine mappingEngine)
        {
            _rootVisitor = rootVisitor;
            _mappingEngine = mappingEngine;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var parentExpr = _rootVisitor.Visit(node.Expression);
            if (parentExpr != null)
            {
                var propertyMap = _mappingEngine.GetPropertyMap(node.Member, parentExpr.Type);

                var newMember = Expression.MakeMemberAccess(parentExpr, propertyMap.DestinationProperty.MemberInfo);

                return newMember;
            }
            return node;
        }
    }
}