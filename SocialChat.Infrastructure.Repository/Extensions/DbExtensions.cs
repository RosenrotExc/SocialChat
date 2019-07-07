using Dapper;
using System.Collections.Generic;
using System.Data;

namespace SocialChat.Infrastructure.Data
{
    public static class DbExtensions
    {
        public static IEnumerable<T> QuerySp<T>(this IDbConnection connection, string spName, object param = null) =>
            connection.Query<T>(spName, param, commandType: CommandType.StoredProcedure);

        public static T QueryFirstOrDefaultSp<T>(this IDbConnection connection, string spName, object param = null) =>
            connection.QueryFirstOrDefault<T>(spName, param, commandType: CommandType.StoredProcedure);

        public static T QueryFirstOrDefaultSf<T>(this IDbConnection connection, string spName, object param = null) =>
            connection.QueryFirstOrDefault<T>(spName, param, commandType: CommandType.Text);
    }
}
