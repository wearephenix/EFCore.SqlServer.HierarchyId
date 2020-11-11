namespace Microsoft.EntityFrameworkCore.SqlServer.Test.Models
{
    class AbrahamicContext : DbContext
    {
        public DbSet<Patriarch> Patriarchy { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options
                .UseInMemoryDatabase(
                    "HierarchyIdTests",
                    x => x.UseHierarchyId()
                );

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<Patriarch>()
                .HasData(
                    new Patriarch { Id = HierarchyId.GetRoot(), Name = "Abraham" },
                    new Patriarch { Id = HierarchyId.Parse("/1/"), Name = "Isaac" },
                    new Patriarch { Id = HierarchyId.Parse("/1/1/"), Name = "Jacob" },
                    new Patriarch { Id = HierarchyId.Parse("/1/1/1/"), Name = "Reuben" },
                    new Patriarch { Id = HierarchyId.Parse("/1/1/2/"), Name = "Simeon" },
                    new Patriarch { Id = HierarchyId.Parse("/1/1/3/"), Name = "Levi" },
                    new Patriarch { Id = HierarchyId.Parse("/1/1/4/"), Name = "Judah" },
                    new Patriarch { Id = HierarchyId.Parse("/1/1/5/"), Name = "Issachar" },
                    new Patriarch { Id = HierarchyId.Parse("/1/1/6/"), Name = "Zebulun" },
                    new Patriarch { Id = HierarchyId.Parse("/1/1/7/"), Name = "Dan" },
                    new Patriarch { Id = HierarchyId.Parse("/1/1/8/"), Name = "Naphtali" },
                    new Patriarch { Id = HierarchyId.Parse("/1/1/9/"), Name = "Gad" },
                    new Patriarch { Id = HierarchyId.Parse("/1/1/10/"), Name = "Asher" },
                    new Patriarch { Id = HierarchyId.Parse("/1/1/11.1/"), Name = "Ephraim" },
                    new Patriarch { Id = HierarchyId.Parse("/1/1/11.2/"), Name = "Manasseh" },
                    new Patriarch { Id = HierarchyId.Parse("/1/1/12/"), Name = "Benjamin" });
    }
}
