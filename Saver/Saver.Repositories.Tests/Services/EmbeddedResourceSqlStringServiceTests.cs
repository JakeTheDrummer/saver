using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Saver.Repositories.Services;
using Saver.Repositories.Services.Exceptions;
using Saver.Repositories.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Repositories.Tests.Services
{
    /// <summary>
    /// Tests that we are able to load the embedded resources
    /// stored within a directory on the environment
    /// </summary>
    [TestClass]
    public class EmbeddedResourceSqlStringServiceTests
    {
        /// <summary>
        /// Tests that we are able to load an embedded resource from a file
        /// </summary>
        [TestMethod]
        public void ShouldLoadKnownResourceFromEmbeddedResourceFileAndCacheResult()
        {
            //Arrange
            string knownFile = "GetAllGoals";
            string rootPath = "Resources";
            string expectedSqlStatement = "SELECT * FROM Goals;";

            EmbeddedResourceSqlStringService sqlStringService = new EmbeddedResourceSqlStringService(rootPath);

            //Act
            string sqlString = sqlStringService.GetSqlFromResource(knownFile);

            //Assert
            sqlString.Should().NotBeNull();
            sqlString.Should().Be(expectedSqlStatement);
            sqlStringService.ResourceCache.Should().NotBeNull();
            sqlStringService.ResourceCache.Count.Should().Be(1);
            sqlStringService.ResourceCache.Values.Should().ContainSingle(expectedSqlStatement);
        }

        /// <summary>
        /// Tests that we are able to see an exception when trying to load from a resource that doesn't exist
        /// </summary>
        [TestMethod]
        public void ShouldThrowExceptionLoadingSqlFromNonExistantEmbeddedResource()
        {
            //Arrange
            string nonExistantFile = Guid.NewGuid().ToString();
            string rootPath = "Resources";

            EmbeddedResourceSqlStringService sqlStringService = new EmbeddedResourceSqlStringService(rootPath);

            //Act
            Action failAction = () => sqlStringService.GetSqlFromResource(nonExistantFile);

            //Assert
            failAction.Should().Throw<ResourceNotFoundException>();
            sqlStringService.ResourceCache.Should().BeEmpty();
        }
    }
}
