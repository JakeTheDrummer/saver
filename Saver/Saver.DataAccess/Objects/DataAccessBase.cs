using Saver.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.DataAccess.Objects
{
    /// <summary>
    /// The base class from which we should inherit
    /// </summary>
    public abstract class DataAccessBase : IDataAccess
    {
        protected IDbConnection connection;

        /// <summary>
        /// Creates a new base data access object with
        /// the given connection string
        /// </summary>
        /// <param name="connectionString">The string with which to connect</param>
        protected DataAccessBase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Returns the connection string we should use
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Opens the connection to the database and executes the
        /// function with the open connection. We then close the
        /// connection and will run the appropriate exceptions
        /// </summary>
        /// <typeparam name="IDbConnectionType">The type of the database connection</typeparam>
        /// <typeparam name="T">The type we are expecting to return</typeparam>
        /// <param name="executeFunction">The function returning the type T</param>
        /// <param name="onConnectErrored">The action to perform if we fail to connect</param>
        /// <param name="onDisconnectErrored">The action to perform if we fail to disconnect</param>
        /// <returns>The result of the execution using the function</returns>
        protected TReturn ExecuteThenClose<TReturn>
        (  
            Func<IDbConnection, TReturn> executeFunction, 
            Action<Exception> onConnectErrored, 
            Action<Exception> onDisconnectErrored
        )
        {
            TReturn returnValue = default(TReturn);
            try
            {
                //Attempt to open the connection
                if (!OpenConnection(onConnectErrored))
                    return returnValue;

                //Execute the required function
                returnValue = executeFunction(connection);
            }
            finally
            {
                //Attempt to disconnect
                CloseConnection(onDisconnectErrored);
            }

            return returnValue;
        }

        /// <summary>
        /// Attempts to open the connection to the database and
        /// runs the appropriate error action when the connection
        /// fails
        /// </summary>
        /// <param name="onConnectErrored">When the connection to the database errors we execute this</param>
        /// <returns>Whether we are connected to the database</returns>
        protected bool OpenConnection(Action<Exception> onConnectErrored)
        {
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                onConnectErrored?.Invoke(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Closes the connection to the database if possible
        /// </summary>
        /// <param name="onDisconnectErrored">The action to run in the case of an error</param>
        /// <returns>Whether we successfully closed the connection</returns>
        protected bool CloseConnection(Action<Exception> onDisconnectErrored)
        {
            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                onDisconnectErrored?.Invoke(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Closes the connection to the database if required
        /// </summary>
        public void Dispose()
        {
            if (connection != null && connection.State.Equals(ConnectionState.Open))
                CloseConnection(null);
        }



        #region Abstract Members

        /// <summary>
        /// Returns a data table by executing the SQL
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <returns>The data table containing the results</returns>
        public abstract DataTable ExecuteDataTable(string sql);

        /// <summary>
        /// Returns a data table by executing the SQL
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="parameters">The parameters to be used in execution</param>
        /// <returns>The data table containing the results</returns>
        public abstract DataTable ExecuteDataTable(string sql, Dictionary<string, object> parameters);

        #endregion
    }
}
