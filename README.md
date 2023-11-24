Microsoft choose not to publish this package for .NET 8, but the code was updated.
-------
So here is our private fork, let's hope that we won't need to update this in the future
-------

EntityFrameworkCore.SqlServer.HierarchyId
========================================

![build status](https://img.shields.io/github/actions/workflow/status/efcore/EFCore.SqlServer.HierarchyId/dotnet.yml?branch=main) [![latest version](https://img.shields.io/nuget/v/EntityFrameworkCore.SqlServer.HierarchyId)](https://www.nuget.org/packages/EntityFrameworkCore.SqlServer.HierarchyId) [![downloads](https://img.shields.io/nuget/dt/EntityFrameworkCore.SqlServer.HierarchyId)](https://www.nuget.org/packages/EntityFrameworkCore.SqlServer.HierarchyId) ![license](https://img.shields.io/github/license/efcore/EFCore.SqlServer.HierarchyId)

Adds hierarchyid support to the SQL Server EF Core provider.

Moved to EF Core
----------------

This project has been merged into [dotnet/efcore](https://github.com/dotnet/efcore). All future bug fixes, enhancements, and releases will be done as part of the main EF Core project.

Installation
------------

The latest stable version is available on [NuGet](https://www.nuget.org/packages/EntityFrameworkCore.SqlServer.HierarchyId).

```sh
dotnet add package EntityFrameworkCore.SqlServer.HierarchyId
```

Compatibility
-------------

The following table show which version of this library to use with which version of EF Core.

| EF Core | Version to use  |
| ------- | --------------- |
| 7.0     | 4.x             |
| 6.0     | 3.x             |
| 3.1     | 1.x             |

Usage
-----

Enable hierarchyid support by calling UseHierarchyId inside UseSqlServer. UseSqlServer is is typically called inside `Startup.ConfigureServices` or `OnConfiguring` of your DbContext type.

```cs
options.UseSqlServer(
    connectionString,
    x => x.UseHierarchyId());
```

Add `HierarchyId` properties to your entity types.

```cs
class Patriarch
{
    public HierarchyId Id { get; set; }
    public string Name { get; set; }
}
```

Insert data.

```cs
dbContext.AddRange(
    new Patriarch { Id = HierarchyId.GetRoot(), Name = "Abraham" },
    new Patriarch { Id = HierarchyId.Parse("/1/"), Name = "Isaac" },
    new Patriarch { Id = HierarchyId.Parse("/1/1/"), Name = "Jacob" });
dbContext.SaveChanges();
```

Query.

```cs
var thirdGeneration = from p in dbContext.Patriarchs
                      where p.Id.GetLevel() == 2
                      select p;
```

Testing
-------
A package for the In-Memory EF Core provider is also available to enable unit testing components that consume HierarchyId data.

```sh
dotnet add package EntityFrameworkCore.InMemory.HierarchyId
```

```cs
options.UseInMemoryDatabase(
    databaseName,
    x => x.UseHierarchyId());
```

See also
--------

* [Hierarchical Data (SQL Server)](https://docs.microsoft.com/sql/relational-databases/hierarchical-data-sql-server)
* [Entity Framework documentation](https://docs.microsoft.com/ef/)
