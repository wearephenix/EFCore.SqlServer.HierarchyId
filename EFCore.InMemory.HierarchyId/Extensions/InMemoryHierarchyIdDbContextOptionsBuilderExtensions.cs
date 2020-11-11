using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.InMemory.Infrastructure;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// HierarchyId specific extension methods for <see cref="InMemoryDbContextOptionsBuilder"/>.
    /// </summary>
    public static class InMemoryHierarchyIdDbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Enable HierarchyId mappings.
        /// </summary>
        /// <param name="optionsBuilder">The builder being used to configure in-memory database.</param>
        /// <returns>The options builder so that further configuration can be chained.</returns>
        public static InMemoryDbContextOptionsBuilder UseHierarchyId(
           this InMemoryDbContextOptionsBuilder optionsBuilder)
        {
            var optionsBuilderPropertyInfo = optionsBuilder.GetType().GetProperty("OptionsBuilder", BindingFlags.Instance | BindingFlags.NonPublic);
            var coreOptionsBuilder = optionsBuilderPropertyInfo.GetValue(optionsBuilder) as DbContextOptionsBuilder;

            var extension = coreOptionsBuilder.Options.FindExtension<InMemoryHierarchyIdOptionsExtension>()
                ?? new InMemoryHierarchyIdOptionsExtension();

            ((IDbContextOptionsBuilderInfrastructure)coreOptionsBuilder).AddOrUpdateExtension(extension);

            return optionsBuilder;
        }
    }
}
