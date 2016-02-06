using System;
using System.Linq.Expressions;

namespace DataReaderMapper.QueryableExtensions
{
    public class ExpressionResolutionResult
    {
        public ExpressionResolutionResult(Expression resolutionExpression, Type type)
        {
            ResolutionExpression = resolutionExpression;
            Type = type;
        }

        public Expression ResolutionExpression { get; private set; }
        public Type Type { get; private set; }
    }
}