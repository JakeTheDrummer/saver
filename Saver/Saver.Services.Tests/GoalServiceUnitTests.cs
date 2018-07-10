using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Saver.Model;
using Saver.Repositories.Interfaces;
using Saver.Services.Implementations;
using Saver.Services.Interfaces;

namespace Saver.Services.Tests
{
    /// <summary>
    /// Tests that we are able to access and deal with
    /// the Goals Service, responsible for interacting
    /// on our behalf with the database and providing
    /// logic to stop us doing stupid things
    /// </summary>
    [TestClass]
    public class GoalServiceUnitTests
    {
        private Mock<IGoalRepository> mockGoalRepository = null;

        /// <summary>
        /// Creates a new Test instance
        /// </summary>
        public GoalServiceUnitTests()
        {
            mockGoalRepository = new Mock<IGoalRepository>();
        }
        /// <summary>
        /// Tests that we cannot create a goals service without a given repository
        /// </summary>
        [TestMethod]
        public void ShouldNotCreateGoalServiceWithoutAGoalRepository()
        {
            //Arrange
            Action failAction = () => new GoalService(null);

            //Assert
            failAction.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Ensures that we are able to return all goals ordered
        /// by the ID of the goals from a known list
        /// </summary>
        [TestMethod]
        public void ShouldReturnAllGoalsOrderedById()
        {
            //Arrange
            IEnumerable<Goal> knownGoals = new List<Goal>()
            {
                new Goal(3, "Testing Goal 3", null, 103, GoalStatus.Open, false),
                new Goal(1, "Testing Goal 1", null, 101, GoalStatus.Open, false),
                new Goal(10, "Testing Goal 3", null, 110, GoalStatus.Cancelled, false),
                new Goal(5, "Testing Goal 5", null, 105, GoalStatus.Complete, true),
            };

            IEnumerable<Goal> orderedExpectedGoals = knownGoals.OrderBy(g => g.Id);

            //Setup the repository
            mockGoalRepository.Setup(gr => gr.GetGoals()).Returns(knownGoals);
            IGoalService goalService = new GoalService(mockGoalRepository.Object);

            //Act
            IEnumerable<Goal> returnedGoals = goalService.GetGoals();

            //Assert
            returnedGoals.Should().NotBeNull();
            returnedGoals.Should().BeInAscendingOrder(g => g.Id);
            IEnumerable<int> orderedIds = orderedExpectedGoals.Select(g => g.Id);
            returnedGoals.Select(g => g.Id).Should().ContainInOrder(orderedIds);
        }
    }
}
