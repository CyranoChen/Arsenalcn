using System.Linq.Expressions;
using DataReaderMapper.Internal;

namespace DataReaderMapper.QueryableExtensions
{
    public interface IExpressionBinder
    {
        bool IsMatch(PropertyMap propertyMap, TypeMap propertyTypeMap, ExpressionResolutionResult result);

        MemberAssignment Build(IMappingEngine mappingEngine, PropertyMap propertyMap, TypeMap propertyTypeMap,
            ExpressionRequest request, ExpressionResolutionResult result,
            IDictionary<ExpressionRequest, int> typePairCount);
    }
}