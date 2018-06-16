using Saver.DataAccess.Credentials;
using Saver.DataAccess.Factories.Interfaces;
using Saver.DataAccess.Interfaces;
using Saver.DataAccess.Objects.MySQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.DataAccess.Factories
{
    /// <summary>
    /// Creates MySQL Data Access object on behalf of consumers
    /// </summary>
    public class MySQLDataAccessFactory : IDataAccessFactory
    {
        private IConnectionStringProvider connectionStringProvider;

        /// <summary>
        /// Creates a new MySQL Data Access Factory
        /// </summary>
        /// <param name="connectionStringProvider">The provider of any connection strings required</param>
        public MySQLDataAccessFactory(IConnectionStringProvider connectionStringProvider)
        {
            this.connectionStringProvider = connectionStringProvider ?? throw new ArgumentNullException(nameof(connectionStringProvider));
        }

        /// <summary>
        /// Returns the data access object for which this factory has
        /// been created to produce. In this case a MySQLDataAccess instance
        /// </summary>
        /// <param name="environment">The environment in to which we are looking to connection</param>
        /// <returns>The data access</returns>
        public IDataAccess GetDataAccess(string environment)
        {
            string connectionString = connectionStringProvider.GetConnectionString(environment);
            return new MySQLDataAccess(connectionString);
        }
    }
}