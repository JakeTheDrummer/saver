using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.DataAccess.Credentials
{
    /// <summary>
    /// If we wish to provide connection strings
    /// we should implement this interface
    /// </summary>
    public interface IConnectionStringProvider
    {
        /// <summary>
        /// Returns the connection string for the given environment
        /// </summary>
        /// <param name="environment">The name of the environment</param>
        /// <returns>The connection string for this environment</returns>
        string GetConnectionString(string environment);
    }
}
