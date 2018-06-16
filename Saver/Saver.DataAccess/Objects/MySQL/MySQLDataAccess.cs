﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.DataAccess.Objects.MySQL
{
    /// <summary>
    /// Provides access to the MySQL database
    /// </summary>
    public class MySQLDataAccess : DataAccessBase
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
            DataTable resultTable = ExecuteThenClose<DataTable>
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
    }
}