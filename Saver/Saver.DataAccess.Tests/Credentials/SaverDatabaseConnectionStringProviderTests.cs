using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Saver.DataAccess.Credentials;

namespace Saver.DataAccess.Tests.Credentials
{
    /// <summary>
    /// Tests that we can collect web configurations from the app / web configuration
    /// </summary>
    [TestClass]
    public class SaverDatabaseConnectionStringProviderTests
    {
        private readonly string liveConnectionString;

        /// <summary>
        /// Creates a new class for testing the load of configuration from a web config fsile
        /// </summary>
        public SaverDatabaseConnectionStringProviderTests()
        {
            liveConnectionString = "server=dev-mysql;uid=root;pwd=root;database=saver";
        }

        /// <summary>
        /// Ensures that we can load the connection string from the web config file
        /// </summary>
        [TestMethod]
        public void ShouldGetKnownConnectionStringFromWebConfigurationFile()
        {
            //Arrange
            string environment = "Live";
            IConnectionStringProvider provider = new SaverDatabaseConnectionStringProvider();

            //Act
            string connectionString = provider.GetConnectionString(environment);

            //Assert
            connectionString.Should().NotBeNull();
            connectionString.Should().Be(liveConnectionString);
        }

        /// <summary>
        /// When we attempt to get the connection string from an unlisted environment
        /// we should throw an exception
        /// </summary>
        [TestMethod]
        public void ShouldThrowExceptionWhenGettingConnectionStringFromUnlistedEnvironment()
        {

            //Arrange
            string environment = "Not_An_Environment";
            IConnectionStringProvider provider = new SaverDatabaseConnectionStringProvider();

            //Act
            Action failAction = () => provider.GetConnectionString(environment);

            //Assert
            failAction.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
