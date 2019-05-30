using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SqlServer.Storage
{
    internal class SqlServerHierarchyIdTypeMapping : RelationalTypeMapping
    {
        private static readonly MethodInfo _getSqlBytes
            = typeof(SqlDataReader).GetTypeInfo().GetDeclaredMethod(nameof(SqlDataReader.GetSqlBytes));

        public SqlServerHierarchyIdTypeMapping(string storeType, Type clrType)
            : base(CreateRelationalTypeMappingParameters(storeType, clrType))
        {
        }

        private static RelationalTypeMappingParameters CreateRelationalTypeMappingParameters(string storeType, Type clrType)
        {
            return new RelationalTypeMappingParameters(
                new CoreTypeMappingParameters(
                    clrType,
                    new SqlServerHierarchyIdValueConverter()
                    ),
                storeType);
        }

        // needed to implement Clone
        protected SqlServerHierarchyIdTypeMapping(RelationalTypeMappingParameters parameters)
            : base(parameters)
        {
        }

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
        {
            return new SqlServerHierarchyIdTypeMapping(parameters);
        }

        protected override void ConfigureParameter(DbParameter parameter)
        {
            if (parameter.Value == DBNull.Value)
            {
                parameter.Value = SqlBytes.Null;
            }
        }

        public override MethodInfo GetDataReaderMethod()
        {
            return _getSqlBytes;
        }

        protected override string GenerateNonNullSqlLiteral(object value)
        {
            value = Converter.ConvertFromProvider(value);
            return $"'{value}'";
        }
    }
}
