using System.Data;
using System.Data.Common;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;

namespace contactsapi.DAL.Extensions
{
    public static class DbContextExtension
    {
        public static async Task<IEnumerable<dynamic>> DynamicListFromSqlAsync(this DbContext db, string sqlRaw, Dictionary<string, object> parameters)
        {
            using (DbCommand cmd = db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = sqlRaw;

                if (cmd.Connection?.State != ConnectionState.Open)
                {
                    await cmd.Connection?.OpenAsync();
                }

                foreach (KeyValuePair<string, object> param in parameters)
                {
                    DbParameter dbParameter = cmd.CreateParameter();

                    dbParameter.ParameterName = param.Key;

                    dbParameter.Value = param.Value;

                    cmd.Parameters.Add(dbParameter);
                }

                using (DbDataReader dataReader = await cmd.ExecuteReaderAsync())
                {
                    List<dynamic> result = new List<dynamic>();

                    while (await dataReader.ReadAsync())
                    {
                        dynamic row = new ExpandoObject();

                        for (int fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
                        {
                            ((IDictionary<string, object>)row).Add(dataReader.GetName(fieldCount), dataReader[fieldCount] != null && !dataReader.IsDBNull(fieldCount) ? dataReader[fieldCount] : string.Empty);
                        }

                        result.Add(row);
                    }

                    return result;
                }
            }
        }

        public static async Task<List<Dictionary<string, object>>> ExecuteSqlAsyncDictonaryAsync(this DbContext db, string sqlRaw, Dictionary<string, object> parameters)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            using (DbCommand cmd = db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = sqlRaw;

                if (cmd.Connection?.State != ConnectionState.Open)
                {
                    await cmd.Connection?.OpenAsync();
                }

                foreach (KeyValuePair<string, object> param in parameters)
                {
                    DbParameter dbParameter = cmd.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    dbParameter.Value = param.Value;
                    cmd.Parameters.Add(dbParameter);
                }

                using (DbDataReader dataReader = await cmd.ExecuteReaderAsync())
                {
                    while (await dataReader.ReadAsync())
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();

                        for (int fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
                        {
                            row.Add(dataReader.GetName(fieldCount), dataReader[fieldCount] != null && !dataReader.IsDBNull(fieldCount) ? dataReader[fieldCount] : DBNull.Value);
                        }

                        result.Add(row);
                    }
                }
            }

            return result;
        }
    }
}
