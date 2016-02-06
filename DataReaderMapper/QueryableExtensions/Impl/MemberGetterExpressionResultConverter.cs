using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DataReaderMapper.QueryableExtensions.Impl
{
    public class MemberGetterExpressionResultConverter : IExpressionResultConverter
    {
        public ExpressionResolutionResult GetExpressionResolutionResult(
            ExpressionResolutionResult expressionResolutionResult, PropertyMap propertyMap, IValueResolver valueResolver)
        {
            var currentChild = expressionResolutionResult.ResolutionExpression;
            Type currentChildType;
            var getter = (IMemberGetter) valueResolver;
            var memberInfo = getter.MemberInfo;

            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                currentChild = Expression.Property(currentChild, propertyInfo);
                currentChildType = propertyInfo.PropertyType;
            }
            else
                currentChildType = currentChild.Type;

            return new ExpressionResolutionResult(currentChild, currentChildType);
        }

        public bool CanGetExpressionResolutionResult(ExpressionResolutionResult expressionResolutionResult,
            IValueResolver valueResolver)
        {
            return valueResolver is IMemberGetter;
        }
    }
}