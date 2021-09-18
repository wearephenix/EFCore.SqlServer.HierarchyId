using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.InMemory.Properties;
using Microsoft.EntityFrameworkCore.InMemory.Storage;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.InMemory.Infrastructure
{
    internal class InMemoryHierarchyIdOptionsExtension : IDbContextOptionsExtension
    {
        private DbContextOptionsExtensionInfo _info;

        public DbContextOptionsExtensionInfo Info => _info ??= new ExtensionInfo(this);

        public virtual void ApplyServices(IServiceCollection services)
        {
            services.AddEntityFrameworkInMemoryHierarchyId();
        }

        public virtual void Validate(IDbContextOptions options)
        {
            var internalServiceProvider = options.FindExtension<CoreOptionsExtension>()?.InternalServiceProvider;
            if (internalServiceProvider != null)
            {
                using (var scope = internalServiceProvider.CreateScope())
                {
                    if (scope.ServiceProvider.GetService<IEnumerable<ITypeMappingSourcePlugin>>()
                           ?.Any(s => s is InMemoryHierarchyIdTypeMappingSourcePlugin) != true)
                    {
                        throw new InvalidOperationException(Resources.ServicesMissing);
                    }
                }
            }
        }

        private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
        {
            public ExtensionInfo(IDbContextOptionsExtension extension)
                : base(extension)
            {
            }

            private new InMemoryHierarchyIdOptionsExtension Extension
                => (InMemoryHierarchyIdOptionsExtension)base.Extension;

            public override bool IsDatabaseProvider => false;

            public override int GetServiceProviderHashCode() => 0;

            public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
                => true;
            
            public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
            => debugInfo["InMemory:" + nameof(InMemoryHierarchyIdDbContextOptionsBuilderExtensions.UseHierarchyId)] = "1";

            public override string LogFragment => "using HierarchyId ";
        }
    }
}
