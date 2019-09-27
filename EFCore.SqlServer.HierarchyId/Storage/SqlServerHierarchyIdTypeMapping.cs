using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SqlServer.Storage
{
    internal class SqlServerHierarchyIdTypeMapping : RelationalTypeMapping
    {
        private static readonly MethodInfo _getSqlBytes
            = typeof(SqlDataReader).GetTypeInfo().GetDeclaredMethod(nameof(SqlDataReader.GetSqlBytes));

        private static Action<DbParameter, SqlDbType> _sqlDbTypeSetter;
        private static Action<DbParameter, string> _udtTypeNameSetter;

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
            var type = parameter.GetType();
            LazyInitializer.EnsureInitialized(ref _sqlDbTypeSetter, () => CreateSqlDbTypeAccessor(type));
            LazyInitializer.EnsureInitialized(ref _udtTypeNameSetter, () => CreateUdtTypeNameAccessor(type));

            if (parameter.Value == DBNull.Value)
            {
                parameter.Value = SqlBytes.Null;
            }

            _sqlDbTypeSetter(parameter, SqlDbType.Udt);
            _udtTypeNameSetter(parameter, StoreType);
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

        private static Action<DbParameter, SqlDbType> CreateSqlDbTypeAccessor(Type paramType)
        {
            var paramParam = Expression.Parameter(typeof(DbParameter), "parameter");
            var valueParam = Expression.Parameter(typeof(SqlDbType), "value");

            return Expression.Lambda<Action<DbParameter, SqlDbType>>(
                Expression.Call(
                    Expression.Convert(paramParam, paramType),
                    paramType.GetProperty("SqlDbType").SetMethod,
                    valueParam),
                paramParam,
                valueParam).Compile();
        }

        private static Action<DbParameter, string> CreateUdtTypeNameAccessor(Type paramType)
        {
            var paramParam = Expression.Parameter(typeof(DbParameter), "parameter");
            var valueParam = Expression.Parameter(typeof(string), "value");

            return Expression.Lambda<Action<DbParameter, string>>(
                Expression.Call(
                    Expression.Convert(paramParam, paramType),
                    paramType.GetProperty("UdtTypeName").SetMethod,
                    valueParam),
                paramParam,
                valueParam).Compile();
        }
    }
}
