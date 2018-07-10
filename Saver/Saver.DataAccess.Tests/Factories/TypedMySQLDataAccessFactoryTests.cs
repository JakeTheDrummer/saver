using System;
using System.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Saver.DataAccess.Credentials;
using Saver.DataAccess.Factories;
using Saver.DataAccess.Factories.Interfaces;
using Saver.DataAccess.Interfaces;
using Saver.DataAccess.Objects.MySQL;

namespace Saver.DataAccess.Tests.Factories
{
    /// <summary>
    /// Tests that we are able to use the typed data access factory for a MySql Instance
    /// </summary>
    [TestClass]
    public class TypedMySQLDataAccessFactoryTests
    {
        private const string connectionString = "server=127.0.0.1;uid=root;pwd=12345;database=test";
        private readonly Mock<IConnectionStringProvider> connectionStringProviderMock;
        private readonly Mock<IDbConnection> connectionMock;

        /// <summary>
        /// Creates a new test class
        /// </summary>
        public TypedMySQLDataAccessFactoryTests()
        {
            connectionMock = new Mock<IDbConnection>();

            //Generate the mock of this
            connectionStringProviderMock = new Mock<IConnectionStringProvider>();
            connectionStringProviderMock.Setup(x => x.GetConnectionString(It.IsAny<string>())).Returns(connectionString);
        }

        /// <summary>
        /// Tests that a Typed MySQL Data Access object is
        /// returned from the factory
        /// </summary>
        [TestMethod]
        public void ShouldCreateAMySQLDataAccess()
        {
            //Arrange
            ITypedDataAccessFactory factory = new TypedMySQLDataAccessFactory(connectionStringProviderMock.Object);

            //Act
            IDataAccess dataAccess = factory.GetTypedDataAccess("Live");

            //Assert
            dataAccess.Should().NotBeNull();
            dataAccess.Should().BeOfType<MySQLDataAccess>();
        }
    }
}
