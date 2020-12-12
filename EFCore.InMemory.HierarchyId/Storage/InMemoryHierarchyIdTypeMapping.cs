using System;
using Microsoft.EntityFrameworkCore.Storage;
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

        public override CoreTypeMapping Clone(ValueConverter converter)
            => new InMemoryHierarchyIdTypeMapping(Parameters.WithComposedConverter(converter));
    }
}
