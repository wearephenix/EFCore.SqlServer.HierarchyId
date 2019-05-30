using System;
using System.Linq;
using Bricelam.EntityFrameworkCore.Test.Models;
using Xunit;

namespace Bricelam.EntityFrameworkCore
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
        public void GetLevel_works()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetLevel() == 0
                select p.Name);

            Assert.Equal(
                condense(@"SELECT [p].[Name] FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = 0"),
                condense(_db.Sql));

            Assert.Equal(new[] { "Abraham" }, results);
        }

        [Fact]
        public void IsDescendantOf_works()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetLevel() == 3
                select p.Id.IsDescendantOf(p.Id.GetAncestor(1)));

            Assert.Equal(
                condense(@"SELECT [p].[Id].IsDescendantOf([p].[Id].GetAncestor(1)) FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = 3"),
                condense(_db.Sql));

            Assert.All(results, b => Assert.True(b));
        }

        [Fact]
        public void GetAncestor_0_works()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetLevel() == 0
                select p.Id.GetAncestor(0));

            Assert.Equal(
                condense(@"SELECT [p].[Id].GetAncestor(0) FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = 0"),
                condense(_db.Sql));

            Assert.All(results, h => Assert.Equal(HierarchyId.GetRoot(), h));
        }

        [Fact]
        public void GetAncestor_1_works()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetLevel() == 1
                select p.Id.GetAncestor(1));

            Assert.Equal(
                condense(@"SELECT [p].[Id].GetAncestor(1) FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = 1"),
                condense(_db.Sql));

            Assert.All(results, h => Assert.Equal(HierarchyId.GetRoot(), h));
        }

        [Fact]
        public void GetAncestor_2_works()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetLevel() == 2
                select p.Id.GetAncestor(2));

            Assert.Equal(
                condense(@"SELECT [p].[Id].GetAncestor(2) FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = 2"),
                condense(_db.Sql));

            Assert.All(results, h => Assert.Equal(HierarchyId.GetRoot(), h));
        }

        [Fact]
        public void GetAncestor_3_works()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetLevel() == 3
                select p.Id.GetAncestor(3));

            Assert.Equal(
                condense(@"SELECT [p].[Id].GetAncestor(3) FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = 3"),
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
                condense(@"SELECT [p].[Id].GetAncestor(1) FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = 0"),
                condense(_db.Sql));

            Assert.Equal(new HierarchyId[] { null }, results);
        }

        [Fact]
        public void ToString_works()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetLevel() == 1
                select p.Id.ToString());

            Assert.Equal(
                condense(@"SELECT [p].[Id].ToString() FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = 1"),
                condense(_db.Sql));

            Assert.Equal(new[] { "/1/" }, results);
        }

        [Fact]
        public void GetDescendent_works()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetLevel() == 0
                select p.Id.GetDescendant(null, null));

            Assert.Equal(
                condense(@"SELECT [p].[Id].GetDescendant(NULL, NULL) FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = 0"),
                condense(_db.Sql));

            Assert.Equal(new[] { HierarchyId.Parse("/1/") }, results);
        }

        //[Fact]
        //public void GetRoot_works()
        //{
        //    var results = Enumerable.ToList(
        //        from p in _db.Patriarchy
        //        where p.Id == HierarchyId.GetRoot()
        //        select p.Name);

        //    Assert.Equal(
        //        condense(@"SELECT [p].[Name] FROM [Patriarchy] AS [p] WHERE [p].[Id] = hierarchyid::GetRoot()"),
        //        condense(_db.Sql));

        //    Assert.Equal(new[] { "Abraham" }, results);
        //}

        [Fact]
        public void Parse_works()
        {
            var results = Enumerable.ToList(
                from p in _db.Patriarchy
                where p.Id.GetLevel() == 0
                select HierarchyId.Parse(p.Id.GetDescendant(null, null).ToString()));

            Assert.Equal(
                condense(@"SELECT hierarchyid::Parse([p].[Id].GetDescendant(NULL, NULL).ToString()) FROM [Patriarchy] AS [p] WHERE [p].[Id].GetLevel() = 0"),
                condense(_db.Sql));

            Assert.Equal(new[] { HierarchyId.Parse("/1/") }, results);
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
