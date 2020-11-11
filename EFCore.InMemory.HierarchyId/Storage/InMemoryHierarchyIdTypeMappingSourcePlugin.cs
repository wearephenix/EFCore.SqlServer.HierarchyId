using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.InMemory.Storage
{
    internal class InMemoryHierarchyIdTypeMappingSourcePlugin : ITypeMappingSourcePlugin
    {
        public CoreTypeMapping FindMapping(in TypeMappingInfo mappingInfo)
        {
            var clrType = mappingInfo.ClrType;

            var valueComparer = new ValueComparer<HierarchyId>(
                (w1, w2) => w1.Equals(w2),
                w => w.GetHashCode()
            );

            return typeof(HierarchyId).IsAssignableFrom(clrType)
                ? new InMemoryTypeMapping(clrType, valueComparer, null)
                : null;
        }
    }
}
