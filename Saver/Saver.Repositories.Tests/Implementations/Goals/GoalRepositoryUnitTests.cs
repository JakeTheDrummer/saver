using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Saver.DataAccess.Interfaces;
using Saver.Model;
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
        
        /// <summary>
        /// Tests that we are able to load resources from the SQL
        /// string service and that these are cached for the goal
        /// respository when the instance is accessed.
        /// </summary>
        [TestMethod]
        public void ShouldAddLoadedResourcesForMethodWhenGettingGoalById()
        {
            //Arrange
            int expectedId = 10;
            string expectedSqlStatement = "SELECT * FROM Goals;";
            IEnumerable<Goal> expectedGoals = new List<Goal>()
            {
                new Goal(expectedId, "Testing Goal", "Mock Testing Goal", 100, GoalStatus.Open, false)
            };

            mockDataAccess.Setup(da => da.ExecuteQuery<Goal>(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).Returns(expectedGoals);
            mockSqlStringService.Setup(ss => ss.GetSqlFromResource(It.IsAny<string>())).Returns<string>(val => { return $"{val} - {expectedSqlStatement}"; });
            IGoalRepository repository = new GoalRepository(mockDataAccess.Object, mockSqlStringService.Object);

            //Act
            Goal goal = repository.GetGoal(expectedId);

            //Assert
            goal.Should().NotBeNull();
            goal.Id.Should().Be(expectedId);

            GoalRepository goalRepo = (GoalRepository)repository;
            Dictionary<string, string> loadedResources = goalRepo.LoadedResources[nameof(repository.GetGoal)];
            loadedResources.Should().NotBeNull();
            loadedResources.Count.Should().Be(1);
        }
    }
}
