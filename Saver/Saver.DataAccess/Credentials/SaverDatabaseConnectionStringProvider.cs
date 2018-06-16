using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Saver.DataAccess.Credentials
{
    /// <summary>
    /// Provides the ability to create / retrieve a connection string
    /// </summary>
    public class SaverDatabaseConnectionStringProvider : IConnectionStringProvider
    {
        /// <summary>
        /// Returns the connection string from the environment in the web.config
        /// </summary>
        /// <param name="environment">The environment / name of the connection string</param>
        /// <returns>The connection string stored in the web configuration</returns>
        public string GetConnectionString(string environment)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings[environment].ConnectionString;
            return connectionString;
        }
    }
}