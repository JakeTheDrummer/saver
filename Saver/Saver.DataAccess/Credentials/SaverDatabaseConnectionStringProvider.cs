using System;
using System.Collections.Generic;
using System.Configuration;
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
            //Make sure it exists
            ConnectionStringSettings connectionString = WebConfigurationManager.ConnectionStrings[environment];
            if (connectionString == null)
                throw new ArgumentOutOfRangeException(nameof(environment), $"Please ensure that the environment {environment} is provided in the web.config file");
            
            return connectionString.ConnectionString;
        }
    }
}