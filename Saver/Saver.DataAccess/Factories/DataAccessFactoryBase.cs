using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saver.DataAccess.Credentials;

namespace Saver.DataAccess.Factories
{
    /// <summary>
    /// Provides a base class for the data access factories
    /// </summary>
    public abstract class DataAccessFactoryBase
    {
        protected IConnectionStringProvider connectionStringProvider;

        /// <summary>
        /// Creates a new implementation of the base class for the data access factory
        /// </summary>
        /// <param name="connectionStringProvider">The provider of any connection strings</param>
        protected DataAccessFactoryBase(IConnectionStringProvider connectionStringProvider)
        {
            this.connectionStringProvider = connectionStringProvider;
        }
    }
}
