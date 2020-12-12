using System.Linq;
using Microsoft.EntityFrameworkCore.SqlServer.Test.Models;
using Xunit;

namespace Microsoft.EntityFrameworkCore.SqlServer
{
    public class DatabaseContextTests
    {
        [Fact]
        public void Should_CreateDatabaseContext()
        {
            // Arrange
            var databaseContext = new AbrahamicContext();

            // Act
            databaseContext.Database.EnsureCreated();

            // Assert
            Assert.NotNull(databaseContext);
            Assert.NotNull(databaseContext.Patriarchy);
            Assert.Equal(16, databaseContext.Patriarchy.Count());
        }

        [Fact]
        public void Should_GetEphraimEntryByName()
        {
            // Arrange
            var databaseContext = new AbrahamicContext();

            // Act
            databaseContext.Database.EnsureCreated();
            var ephraim = databaseContext.Patriarchy.Single(w => w.Name == "Ephraim");

            // Assert
            Assert.Equal(HierarchyId.Parse("/1/1/11.1/"), ephraim.Id);
        }

        [Fact]
        public void Should_GetEphraimEntryById()
        {
            // Arrange
            var databaseContext = new AbrahamicContext();

            // Act
            databaseContext.Database.EnsureCreated();
            var ephraim = databaseContext.Patriarchy.Single(w => w.Id == HierarchyId.Parse("/1/1/11.1/"));

            // Assert
            Assert.Equal("Ephraim", ephraim.Name);
        }
    }
}
