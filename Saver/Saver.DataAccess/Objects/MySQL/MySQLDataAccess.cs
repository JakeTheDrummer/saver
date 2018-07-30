using Dapper;
using MySql.Data.MySqlClient;
using Saver.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Saver.DataAccess.Objects.MySQL
{
    /// <summary>
    /// Provides access to the MySQL database
    /// </summary>
    public class MySQLDataAccess : DataAccessBase, ITypedDataAccess
    {
        /// <summary>
        /// Creates a new MySQL Data Access Object
        /// </summary>
        /// <param name="connectionString">The connection string with which to connect to the database</param>
        /// <param name="connection">The connection object we will use</param>
        public MySQLDataAccess(string connectionString)
            : this(connectionString, new MySqlConnection(connectionString))
        {
        }

        /// <summary>
        /// Creates a new MySQL Data Access Object
        /// </summary>
        /// <param name="connectionString">The connection string with which to connect to the database</param>
        /// <param name="connection">The connection object we will use</param>
        public MySQLDataAccess(string connectionString, IDbConnection connection)
            : base(connectionString)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Returns the data table results from the query
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <returns>The resultant data table from the SQL run</returns>
        public override DataTable ExecuteDataTable(string sql)
        {
            //Call the overload
            return ExecuteDataTable(sql, null);
        }

        /// <summary>
        /// Executes the SQL with the given parameters and returns a data table
        /// containing the results of the query
        /// </summary>
        /// <param name="sql">The query to execute</param>
        /// <param name="parameters">The parameters to use to parameterise the query</param>
        /// <returns>The data table that results from the SQL</returns>
        public override DataTable ExecuteDataTable(string sql, Dictionary<string, object> parameters)
        {
            //Ensure we can run something
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentException("Please ensure that SQL is provided to be run", nameof(sql));

            //Call the following code with an open connection
            DataTable resultTable = ExecuteThenClose
            (
                (connection =>
                {
                    //Produce the SQL command
                    MySqlCommand command = new MySqlCommand(sql, (MySqlConnection)connection);
                    if (parameters != null && parameters.Count > 0)
                    {
                        Parameterise(ref command, parameters);
                    }

                    //Execute the SQL and return
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                    DataTable returnTable = new DataTable();
                    dataAdapter.Fill(returnTable);

                    return returnTable;

                }), 
                OnConnectionErrored, OnDisconnectionErrored
            );

            return resultTable;
        }
        
        /// <summary>
        /// Returns the typed objects of type T from the database
        /// using the SQL statement given
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="sql">The SQL returning the type</param>
        /// <returns>An enumerable of type T from the data storage</returns>
        public IEnumerable<T> ExecuteQuery<T>(string sql)
        {
            return ExecuteQuery<T>(sql, null);
        }
        
        /// <summary>
        /// Returns the typed objects of type T from the database
        /// using the SQL statement and parameters given
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="sql">The SQL returning the type</param>
        /// <param name="parameters">The parameters that we wish to use in the query</param>
        /// <returns>An enumerable of type T from the data storage</returns>
        public IEnumerable<T> ExecuteQuery<T>(string sql, Dictionary<string, object> parameters)
        {
            return ExecuteQueryWithGenericParameterType<T>(sql, parameters);
        }

        /// <summary>
        /// Returns the typed objects of type T from the database
        /// using the SQL statement and parameters given
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="sql">The SQL returning the type</param>
        /// <param name="parameters">The parameters that we wish to use in the query</param>
        /// <returns>An enumerable of type T from the data storage</returns>
        public IEnumerable<T> ExecuteQueryWithGenericParameterType<T>(string sql, dynamic queryParameterObjects)
        {
            IEnumerable<T> results = ExecuteThenClose
            (
                (connection =>
                {
                    CommandDefinition command = new CommandDefinition(sql, queryParameterObjects);

                    //Collect the query through dapper
                    IEnumerable<T> queryResult = connection.Query<T>(command);
                    return queryResult;
                }),
                OnConnectionErrored, OnDisconnectionErrored
            );
            return results;
        }

        /// <summary>
        /// Parameterises the command if any parameters have been provided
        /// </summary>
        /// <param name="command">The command that we are parameterising</param>
        /// <param name="parameters">The parameters we should use to parameterise</param>
        private void Parameterise(ref MySqlCommand command, Dictionary<string, object> parameters)
        {
            if (parameters == null)
                return;

            string key = null;
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                //Ensure each key is properly formed
                key = parameter.Key;
                if (!key.StartsWith("@"))
                    key = $"@{key}";

                command.Parameters.AddWithValue(key, parameter.Value);
            }
        }

        /// <summary>
        /// Throws the appropriate exception when we are unable to connect to the database
        /// </summary>
        /// <param name="exception">The exception that was encountered when connecting</param>
        private void OnConnectionErrored(Exception exception)
        {
            throw new Exception("Connection to the database was not possible. See inner exception for information.", exception);
        }

        /// <summary>
        /// Throws the appropriate exception when we are unable to disconnect from the database
        /// </summary>
        /// <param name="exception">The exception that was encountered when disconnecting</param>
        private void OnDisconnectionErrored(Exception exception)
        {
            throw new Exception("Disconnection from the database was not possible. See inner exception for information.", exception);
        }

        /// <summary>
        /// Executes the SQL statement and returns the number of rows
        /// that were affected by the query
        /// </summary>
        /// <typeparam name="TParameterType">The parameter type of the parameters</typeparam>
        /// <param name="sql">The SQL statement to be processed</param>
        /// <param name="parameters">Any parameters for this request</param>
        /// <returns>The number of rows affected</returns>
        public int Execute<TParameterType>(string sql, IEnumerable<TParameterType> parameters)
        {
            return ExecuteWithGenericParameters(sql, parameters);
        }

        /// <summary>
        /// Executes the query with the generic parameters provided
        /// </summary>
        /// <param name="sql">The SQL containing the query to run</param>
        /// <param name="parameters">The parameters to drive the query</param>
        /// <returns>The count of the affected rows</returns>
        public int ExecuteWithGenericParameters(string sql, dynamic parameters)
        {
            int result = ExecuteThenClose
            (
                (connection) =>
                {
                    CommandDefinition command = new CommandDefinition(sql, parameters);

                    //Collect the query through dapper
                    int affectedRows = connection.Execute(command);
                    return affectedRows;
                },
                OnConnectionErrored, OnDisconnectionErrored
            );
            return result;
        }
    }
}
