using System.Linq;
using System.Reflection;
using DataReaderMapper.Internal;

namespace DataReaderMapper.Mappers
{
    public class ImplicitConversionOperatorMapper : IObjectMapper
    {
        public object Map(ResolutionContext context, IMappingEngineRunner mapper)
        {
            var implicitOperator = GetImplicitConversionOperator(context);

            return implicitOperator.Invoke(null, new[] {context.SourceValue});
        }

        public bool IsMatch(ResolutionContext context)
        {
            var methodInfo = GetImplicitConversionOperator(context);

            return methodInfo != null;
        }

        private static MethodInfo GetImplicitConversionOperator(ResolutionContext context)
        {
            var destinationType = context.DestinationType;
            if (PrimitiveExtensions.IsNullableType(destinationType))
            {
                destinationType = PrimitiveExtensions.GetTypeOfNullable(destinationType);
            }
            var sourceTypeMethod = TypeExtensions.GetDeclaredMethods(context.SourceType)
                .FirstOrDefault(
                    mi => mi.IsPublic && mi.IsStatic && mi.Name == "op_Implicit" && mi.ReturnType == destinationType);

            return sourceTypeMethod ?? destinationType.GetMethod("op_Implicit", new[] {context.SourceType});
        }
    }
}