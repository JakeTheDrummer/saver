using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Saver.DataAccess.Interfaces;
using Saver.Repositories.Implementations.Goal;
using Saver.Repositories.Interfaces;
using Saver.Repositories.Services.Interfaces;

namespace Saver.Repositories.Tests.Implementations.Goals
{
    /// <summary>
    /// Tests that the goal repository returns information
    /// from the data layer appropriately
    /// </summary>
    [TestClass]
    public class GoalRepositoryUnitTests
    {
        private readonly Mock<ITypedDataAccess> mockDataAccess;
        private readonly Mock<ISqlStringService> mockSqlStringService;

        /// <summary>
        /// Creates new Goal Repository Unit Tests
        /// </summary>
        public GoalRepositoryUnitTests()
        {
            mockDataAccess = new Mock<ITypedDataAccess>();
            mockSqlStringService = new Mock<ISqlStringService>();
        }

        /// <summary>
        /// Ensures that we are only able to create a repository if
        /// we have been given a data access object
        /// </summary>
        [TestMethod]
        public void ShouldFailToCreateANewGoalRepositoryWithoutDataAccess()
        {
            //Act
            Action failAction = () => new GoalRepository(null, null);

            //Assert
            failAction.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Ensures that we are able to create a repository if
        /// we have been given a data access object
        /// </summary>
        [TestMethod]
        public void ShouldCreateANewGoalRepositoryWithDataAccess()
        {
            //Act
            IGoalRepository repository = new GoalRepository(mockDataAccess.Object, mockSqlStringService.Object);

            //Assert
            repository.Should().NotBeNull();
        }
    }
}
