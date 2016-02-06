using System;
using System.Collections.Generic;
using System.Reflection;
using DataReaderMapper.Internal;
using DataReaderMapper.Mappers;

namespace DataReaderMapper
{
    /// <summary>
    ///     Contains profile-specific configuration
    /// </summary>
    public interface IProfileConfiguration
    {
        IEnumerable<IMemberConfiguration> MemberConfigurations { get; }
        IEnumerable<IConditionalObjectMapper> TypeConfigurations { get; }
        bool ConstructorMappingEnabled { get; set; }
        bool DataReaderMapperYieldReturnEnabled { get; set; }
        IMemberConfiguration DefaultMemberConfig { get; }

        /// <summary>
        ///     Source extension methods included for search
        /// </summary>
        IEnumerable<MethodInfo> SourceExtensionMethods { get; }

        /// <summary>
        ///     Specify which properties should be mapped.
        ///     By default only public properties are mapped.e
        /// </summary>
        Func<PropertyInfo, bool> ShouldMapProperty { get; set; }

        /// <summary>
        ///     Specify which fields should be mapped.
        ///     By default only public fields are mapped.
        /// </summary>
        Func<FieldInfo, bool> ShouldMapField { get; set; }

        IMemberConfiguration AddMemberConfiguration();
        IConditionalObjectMapper AddConditionalObjectMapper();

        void IncludeSourceExtensionMethods(Assembly assembly);
    }
}