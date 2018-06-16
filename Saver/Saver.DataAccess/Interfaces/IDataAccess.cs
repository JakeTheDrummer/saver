using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.DataAccess.Interfaces
{
    /// <summary>
    /// All data access providers should inherit this interface
    /// </summary>
    public interface IDataAccess : IDisposable
    {
        /// <summary>
        /// Returns a data table by executing the SQL
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <returns>The data table containing the results</returns>
        DataTable ExecuteDataTable(string sql);

        /// <summary>
        /// Returns a data table by executing the SQL
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="parameters">The parameters to be used in execution</param>
        /// <returns>The data table containing the results</returns>
        DataTable ExecuteDataTable(string sql, Dictionary<string, object> parameters);
    }
}
