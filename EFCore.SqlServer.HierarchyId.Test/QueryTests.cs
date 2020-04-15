using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.SqlServer.Test.Models;
using Xunit;

namespace Microsoft.EntityFrameworkCore.SqlServer
{
    public class QueryTests : IDisposable
    {
        private readonly AbrahamicContext _db;

        public QueryTests()
        {
            _db = new AbrahamicContext();
            //_db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
        }

        [Fact]
        public void GetLevel_can_translate()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetLevel() == 0
                select p.Name);

            Assert.Equal(
                condense(@"SELECT [p].[Name] FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = CAST(0 AS smallint)"),
                condense(_db.Sql));

            Assert.Equal(new[] { "Abraham" }, results);
        }

        [Fact]
        public void IsDescendantOf_can_translate()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetLevel() == 3
                select p.Id.IsDescendantOf(p.Id.GetAncestor(1)));

            Assert.Equal(
                condense(@"SELECT [p].[Id].IsDescendantOf([p].[Id].GetAncestor(1)) FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = CAST(3 AS smallint)"),
                condense(_db.Sql));

            Assert.All(results, b => Assert.True(b));
        }

        [Fact]
        public void GetAncestor_0_can_translate()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetLevel() == 0
                select p.Id.GetAncestor(0));

            Assert.Equal(
                condense(@"SELECT [p].[Id].GetAncestor(0) FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = CAST(0  AS smallint)"),
                condense(_db.Sql));

            Assert.All(results, h => Assert.Equal(HierarchyId.GetRoot(), h));
        }

        [Fact]
        public void GetAncestor_1_can_translate()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetLevel() == 1
                select p.Id.GetAncestor(1));

            Assert.Equal(
                condense(@"SELECT [p].[Id].GetAncestor(1) FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = CAST(1 AS smallint)"),
                condense(_db.Sql));

            Assert.All(results, h => Assert.Equal(HierarchyId.GetRoot(), h));
        }

        [Fact]
        public void GetAncestor_2_can_translate()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetLevel() == 2
                select p.Id.GetAncestor(2));

            Assert.Equal(
                condense(@"SELECT [p].[Id].GetAncestor(2) FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = CAST(2 AS smallint)"),
                condense(_db.Sql));

            Assert.All(results, h => Assert.Equal(HierarchyId.GetRoot(), h));
        }

        [Fact]
        public void GetAncestor_3_can_translate()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetLevel() == 3
                select p.Id.GetAncestor(3));

            Assert.Equal(
                condense(@"SELECT [p].[Id].GetAncestor(3) FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = CAST(3 AS smallint)"),
                condense(_db.Sql));

            Assert.All(results, h => Assert.Equal(HierarchyId.GetRoot(), h));
        }

        [Fact]
        public void GetAncestor_of_root_returns_null()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetLevel() == 0
                select p.Id.GetAncestor(1));

            Assert.Equal(
                condense(@"SELECT [p].[Id].GetAncestor(1) FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = CAST(0 AS smallint)"),
                condense(_db.Sql));

            Assert.Equal(new HierarchyId[] { null }, results);
        }

        [Fact]
        public void GetDescendent_can_translate()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetLevel() == 0
                select p.Id.GetDescendant(null, null));

            Assert.Equal(
                condense(@"SELECT [p].[Id].GetDescendant(NULL, NULL) FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = CAST(0 AS smallint)"),
                condense(_db.Sql));

            Assert.Equal(new[] { HierarchyId.Parse("/1/") }, results);
        }

        [Fact]
        public void HierarchyId_can_be_sent_as_parameter()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id == HierarchyId.Parse("/1/")
                select p.Name);

            Assert.Equal(
                condense(@"SELECT [p].[Name] FROM [Patriarchy] AS [p] WHERE [p].[Id] = '/1/'"),
                condense(_db.Sql));

            Assert.Equal(new[] { "Isaac" }, results);
        }

        [Fact]
        public void HierarchyId_get_ancestor_of_level_is_root()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetAncestor(p.Id.GetLevel()) == HierarchyId.GetRoot() // HierarchyId.Parse("/1/") // HierarchyId.Parse(p.Id.ToString()).GetAncestor(HierarchyId.Parse(p.Id.ToString()).GetLevel())
                select p.Name);

            Assert.Equal(
                condense(@"SELECT [p].[Name] FROM [Patriarchy] AS [p] WHERE [p].[Id].GetAncestor(CAST([p].[Id].GetLevel() AS int)) = '/'"),
                condense(_db.Sql));

            var all = Enumerable.ToList(
                from p in _db.Patriarchy
                select p.Name);

            Assert.Equal(all, results);
        }

        [Fact]
        public void HierarchyId_can_call_method_on_parameter()
        {
            var isaac = HierarchyId.Parse("/1/");

            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where isaac.IsDescendantOf(p.Id)
                select p.Name);

            Assert.Equal(
                condense(@"SELECT [p].[Name] FROM [Patriarchy] AS [p] WHERE @__isaac_0.IsDescendantOf([p].[Id]) = CAST(1 AS bit)"),
                condense(_db.Sql));

            Assert.Equal(new[] { "Abraham", "Isaac" }, results);
        }

        [Fact]
        public void ToString_can_translate()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetLevel() == 1
                select p.Id.ToString());

            Assert.Equal(
                condense(@"SELECT [p].[Id].ToString() FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = CAST(1 AS smallint)"),
                condense(_db.Sql));

            Assert.Equal(new[] { "/1/" }, results);
        }

        [Fact]
        public void ToString_can_translate_redux()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where EF.Functions.Like(p.Id.ToString(), "%/1/")
                select p.Name);

            Assert.Equal(
                condense(@"SELECT [p].[Name] FROM [Patriarchy] AS [p] WHERE [p].[Id].ToString() LIKE N'%/1/'"),
                condense(_db.Sql));

            Assert.Equal(new[] { "Isaac", "Jacob", "Reuben" }, results);
        }

        [Fact]
        public void Parse_can_translate()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id == HierarchyId.GetRoot()
                select HierarchyId.Parse(p.Id.ToString()));

            Assert.Equal(
                condense(@"SELECT hierarchyid::Parse([p].[Id].ToString()) FROM [Patriarchy] AS [p] WHERE [p].[Id] = '/'"),
                condense(_db.Sql));

            Assert.Equal(new[] { HierarchyId.Parse("/") }, results);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        // replace whitespace with a single space
        private static string condense(string str)
        {
            var split = str.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", split);
        }
    }
}
