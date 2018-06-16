using Saver.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.DataAccess.Factories.Interfaces
{
    /// <summary>
    /// All data access factories should implement this interface
    /// </summary>
    public interface IDataAccessFactory
    {
        /// <summary>
        /// Returns the data access object for which this factory has
        /// been created to produce
        /// </summary>
        /// <param name="environment">The environment in to which we are looking to connection</param>
        /// <returns>The data access</returns>
        IDataAccess GetDataAccess(string environment);
    }
}
