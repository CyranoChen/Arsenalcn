using System.Linq.Expressions;
using DataReaderMapper.Internal;
using DataReaderMapper.QueryableExtensions;

namespace DataReaderMapper
{
    /// <summary>
    ///     Main entry point for executing maps
    /// </summary>
    public interface IMappingEngineRunner
    {
        IConfigurationProvider ConfigurationProvider { get; }
        object Map(ResolutionContext context);
        object CreateObject(ResolutionContext context);
        bool ShouldMapSourceValueAsNull(ResolutionContext context);
        bool ShouldMapSourceCollectionAsNull(ResolutionContext context);

        Expression CreateMapExpression(ExpressionRequest request,
            Expression instanceParameter, IDictionary<ExpressionRequest, int> typePairCount);

        LambdaExpression CreateMapExpression(ExpressionRequest request,
            IDictionary<ExpressionRequest, int> typePairCount);
    }
}