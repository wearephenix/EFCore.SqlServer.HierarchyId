using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.InMemory.Storage;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// EntityFrameworkCore.InMemory.HierarchyId extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class InMemoryHierarchyIdServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the services required for HierarchyId support in the in-memory database provider for Entity Framework.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddEntityFrameworkInMemoryHierarchyId(
            this IServiceCollection serviceCollection)
        {
            new EntityFrameworkServicesBuilder(serviceCollection)
                .TryAdd<ITypeMappingSourcePlugin, InMemoryHierarchyIdTypeMappingSourcePlugin>();

            return serviceCollection;
        }
    }
}
