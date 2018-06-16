using System;
using System.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MySql.Data.MySqlClient;
using Saver.DataAccess.Objects.MySQL;

namespace Saver.DataAccess.Tests.Objects
{
    /// <summary>
    /// Tests the functionality of the MySQL Data Access class
    /// </summary>
    [TestClass]
    public class MySQLDataAccessTests
    {
        private const string testConnectionString = "server=127.0.0.1;uid=root;pwd=12345;database=test";

        /// <summary>
        /// Ensures that we capture the creation of the Data Access
        /// with malformed MySQL Connection Strings
        /// </summary>
        [TestMethod]
        public void ShouldCaptureMalformedMySqlConnectionString()
        {
            //Arrange
            string invalidString = "NOT_VALID";
            Action failAction = () => new MySQLDataAccess(invalidString);

            //Act & Assert
            failAction.Should().Throw<Exception>();
        }


        /// <summary>
        /// Simulates a failure to connect to the database
        /// and ensures that we run the appropriate callback
        /// </summary>
        [TestMethod]
        public void ShouldThrowAnExceptionWhenFailingToConnectToMySQLDatabase()
        {
            //Create a failing mock
            Mock<IDbConnection> connectionMock = new Mock<IDbConnection>();
            connectionMock.Setup(x => x.Open()).Throws(new Exception("Unable to connect to the MySQL database instance for some reason"));

            string sql = "SELECT * FROM users;";

            MySQLDataAccess dataAccess = new MySQLDataAccess(testConnectionString, connectionMock.Object);
            Action failAction = () => dataAccess.ExecuteDataTable(sql);


            failAction  .Should()
                        .Throw<Exception>()
                        .WithMessage("Connection to the database was not possible. See inner exception for information.");
        }
    }
}
