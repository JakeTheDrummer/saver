using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data;
using Saver.DataAccess.Objects.MySQL;
using Saver.DataAccess.Credentials;
using Saver.DataAccess.Factories.Interfaces;
using Saver.DataAccess.Factories;
using Saver.DataAccess.Interfaces;
using FluentAssertions;

namespace Saver.DataAccess.Tests.Factories
{
    /// <summary>
    /// Summary description for MySQLDataAccessFactoryTests
    /// </summary>
    [TestClass]
    public class MySQLDataAccessFactoryTests
    {
        private const string connectionString = "server=127.0.0.1;uid=root;pwd=12345;database=test";
        private readonly Mock<IConnectionStringProvider> connectionStringProviderMock;
        private readonly Mock<IDbConnection> connectionMock;

        /// <summary>
        /// Creates a new test class
        /// </summary>
        public MySQLDataAccessFactoryTests()
        {
            connectionMock = new Mock<IDbConnection>();

            //Generate the mock of this
            connectionStringProviderMock = new Mock<IConnectionStringProvider>();
            connectionStringProviderMock.Setup(x => x.GetConnectionString(It.IsAny<string>())).Returns(connectionString);
        }

        /// <summary>
        /// Tests that a MySQL Data Access object is
        /// returned from the factory
        /// </summary>
        [TestMethod]
        public void ShouldCreateAMySQLDataAccess()
        {
            //Arrange
            IDataAccessFactory factory = new MySQLDataAccessFactory(connectionStringProviderMock.Object);

            //Act
            IDataAccess dataAccess = factory.GetDataAccess("Live");

            //Assert
            dataAccess.Should().NotBeNull();
            dataAccess.Should().BeOfType<MySQLDataAccess>();
        }
    }
}
