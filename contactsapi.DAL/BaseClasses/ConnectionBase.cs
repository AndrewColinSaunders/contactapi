using Coop.CommonLibrary.Connections;
using Coop.CommonLibrary.Helpers;
using Microsoft.Data.SqlClient;
using Shared.Helpers;

namespace contactsapi.DAL.BaseClasses
{
    public class ConnectionBase
    {
        public static string GetDataBaseSettings(string section = "")
        {
            ConnectionHelper connectionHelper = ConnectionSettings.GetConnectionDictionary(section, ApplicationPaths.ApplicationBasePath);

            if (connectionHelper != null)
            {
                SqlConnectionStringBuilder csbConnectionString = new SqlConnectionStringBuilder
                {
                    DataSource = string.IsNullOrWhiteSpace(connectionHelper.CustomSettings["Port"]) ? connectionHelper.CustomSettings["Server"] : string.Format("{0},{1}", connectionHelper.CustomSettings["Server"], connectionHelper.CustomSettings["Port"]),
                };

                bool trusted = Convert.ToBoolean(connectionHelper.CustomSettings["Trusted"]);

                if (!trusted)
                {
                    csbConnectionString.UserID = connectionHelper.CustomSettings["User"];

                    csbConnectionString.Password = connectionHelper.CustomSettings["Password"];
                }

                csbConnectionString.InitialCatalog = connectionHelper.CustomSettings["Database"];

                csbConnectionString.ConnectTimeout = 120;

                csbConnectionString.TrustServerCertificate = true;

                csbConnectionString.Encrypt = true;

                csbConnectionString.IntegratedSecurity = trusted;

                connectionHelper.ConnectionString = csbConnectionString.ConnectionString;

                return csbConnectionString.ToString();
            }

            return string.Empty;
        }
    }
}