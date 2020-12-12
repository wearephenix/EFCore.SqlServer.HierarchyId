using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.InMemory.Storage
{
    internal class InMemoryHierarchyIdTypeMappingSourcePlugin : ITypeMappingSourcePlugin
    {
        public CoreTypeMapping FindMapping(in TypeMappingInfo mappingInfo)
        {
            var clrType = mappingInfo.ClrType;

            return typeof(HierarchyId).IsAssignableFrom(clrType)
                ? new InMemoryHierarchyIdTypeMapping(clrType)
                : null;
        }
    }
}
