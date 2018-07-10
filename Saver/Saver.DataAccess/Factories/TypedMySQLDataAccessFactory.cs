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
    /// Creates Typed MySQL Data Access object on behalf of consumers
    /// </summary>
    public class TypedMySQLDataAccessFactory : DataAccessFactoryBase, ITypedDataAccessFactory
    {
        /// <summary>
        /// Creates a new MySQL Data Access Factory
        /// </summary>
        /// <param name="connectionStringProvider">The provider of any connection strings required</param>
        public TypedMySQLDataAccessFactory(IConnectionStringProvider connectionStringProvider)
            : base(connectionStringProvider)
        {
        }

        /// <summary>
        /// Returns a typed MySqlDataAccess object allowing us to interact
        /// with typed data on the MySql instance in the environment given
        /// </summary>
        /// <param name="environment">The environment we wish to interact with</param>
        /// <returns>The typed data access for the MySql instance here</returns>
        public ITypedDataAccess GetTypedDataAccess(string environment)
        {
            string connectionString = connectionStringProvider.GetConnectionString(environment);
            return new MySQLDataAccess(connectionString);
        }
    }
}