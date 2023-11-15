using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.InMemory.Storage
{
    internal class InMemoryHierarchyIdTypeMapping : CoreTypeMapping
    {
        public InMemoryHierarchyIdTypeMapping(Type clrType)
            : base(new CoreTypeMappingParameters(clrType))
        {
        }

        private InMemoryHierarchyIdTypeMapping(CoreTypeMappingParameters parameters)
            : base(parameters)
        {
        }

        public override CoreTypeMapping WithComposedConverter(
            ValueConverter converter,
            ValueComparer comparer = null,
            ValueComparer keyComparer = null,
            CoreTypeMapping elementMapping = null,
            JsonValueReaderWriter jsonValueReaderWriter = null)
            => new InMemoryHierarchyIdTypeMapping(
                Parameters.WithComposedConverter(
                    converter,
                    comparer,
                    keyComparer,
                    elementMapping,
                    jsonValueReaderWriter));

        protected override CoreTypeMapping Clone(CoreTypeMappingParameters parameters)
            => new InMemoryHierarchyIdTypeMapping(parameters);
    }
}
