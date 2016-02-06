using System.Linq.Expressions;
using DataReaderMapper.Internal;

namespace DataReaderMapper.QueryableExtensions.Impl
{
    public class AssignableExpressionBinder : IExpressionBinder
    {
        public bool IsMatch(PropertyMap propertyMap, TypeMap propertyTypeMap, ExpressionResolutionResult result)
        {
            return propertyMap.DestinationPropertyType.IsAssignableFrom(result.Type);
        }

        public MemberAssignment Build(IMappingEngine mappingEngine, PropertyMap propertyMap, TypeMap propertyTypeMap,
            ExpressionRequest request, ExpressionResolutionResult result,
            IDictionary<ExpressionRequest, int> typePairCount)
        {
            return BindAssignableExpression(propertyMap, result);
        }

        private static MemberAssignment BindAssignableExpression(PropertyMap propertyMap,
            ExpressionResolutionResult result)
        {
            return Expression.Bind(propertyMap.DestinationProperty.MemberInfo, result.ResolutionExpression);
        }
    }
}