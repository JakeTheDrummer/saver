using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.DataAccess.Interfaces
{
    /// <summary>
    /// Provides an interface that ensures that we are
    /// able to return a strongly typed collection of
    /// objects from the database.
    /// </summary>
    public interface ITypedDataAccess : IDataAccess
    {
        /// <summary>
        /// Executes the SQL statement and returns the number of rows
        /// that were affected by the query
        /// </summary>
        /// <typeparam name="T">The parameter type of the parameters</typeparam>
        /// <param name="sql">The SQL statement to be processed</param>
        /// <param name="parameters">Any parameters for this request</param>
        /// <returns>The number of rows affected</returns>
        int Execute<T>(string sql, IEnumerable<T> parameters);

        /// <summary>
        /// Executes the SQL statement and returns the number of rows
        /// that were affected by the query
        /// </summary>
        /// <typeparam name="T">The parameter type of the parameters</typeparam>
        /// <param name="sql">The SQL statement to be processed</param>
        /// <param name="parameters">Any parameters for this request</param>
        /// <returns>The number of rows affected</returns>
        int ExecuteWithGenericParameters(string sql, dynamic parameters);

        /// <summary>
        /// Returns the typed objects of type T from the database
        /// using the SQL statement and parameters given
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="sql">The SQL returning the type</param>
        /// <param name="parameters">The parameters that we wish to use in the query</param>
        /// <returns>An enumerable of type T from the data storage</returns>
        IEnumerable<T> ExecuteQueryWithGenericParameterType<T>(string sql, dynamic parameters);

        /// <summary>
        /// Returns the typed objects of type T from the database
        /// using the SQL statement and parameters given
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="sql">The SQL returning the type</param>
        /// <param name="parameters">The parameters that we wish to use in the query</param>
        /// <returns>An enumerable of type T from the data storage</returns>
        IEnumerable<T> ExecuteQuery<T>(string sql, Dictionary<string, object> parameters);

        /// <summary>
        /// Returns the typed objects of type T from the database
        /// using the SQL statement given
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="sql">The SQL returning the type</param>
        /// <returns>An enumerable of type T from the data storage</returns>
        IEnumerable<T> ExecuteQuery<T>(string sql);
    }
}
