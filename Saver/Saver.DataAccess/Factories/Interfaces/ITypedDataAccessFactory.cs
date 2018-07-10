using Saver.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.DataAccess.Factories.Interfaces
{
    /// <summary>
    /// All providers of typed data access should
    /// inherit from this interface
    /// </summary>
    public interface ITypedDataAccessFactory
    {
        /// <summary>
        /// Returns a typed MySqlDataAccess object allowing us to interact
        /// with typed data on the MySql instance in the environment given
        /// </summary>
        /// <param name="environment">The environment we wish to interact with</param>
        /// <returns>The typed data access for the MySql instance here</returns>
        ITypedDataAccess GetTypedDataAccess(string environment);
    }
}
