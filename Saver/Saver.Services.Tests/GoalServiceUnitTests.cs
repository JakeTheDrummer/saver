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

        /// <summary>
        /// Tests that we are able to load the goals based on a user ID
        /// </summary>
        [TestMethod]
        public void ShouldGetGoalsForUserBasedOnID()
        {
            //Arrange
            int knownUserId = 1;
            IEnumerable<Goal> knownGoals = new List<Goal>()
            {
                new Goal(3, "Testing Goal A", null, 103, GoalStatus.Open, false),
                new Goal(1, "Testing Goal B", null, 101, GoalStatus.Open, false),
                new Goal(10, "Testing Goal D", null, 110, GoalStatus.Cancelled, false),
                new Goal(5, "Testing Goal C", null, 105, GoalStatus.Complete, true),
            };

            IEnumerable<Goal> orderedExpectedGoals = knownGoals.OrderBy(g => g.Id);

            //Setup the repository
            mockGoalRepository.Setup(gr => gr.GetGoalsForUser(It.Is<int>(v => v == knownUserId))).Returns(knownGoals);
            IGoalService goalService = new GoalService(mockGoalRepository.Object);

            //Act
            IEnumerable<Goal> returnedGoals = goalService.GetGoalsForUser(knownUserId);

            //Assert
            returnedGoals.Should().NotBeNull();
            returnedGoals.Should().BeInAscendingOrder(g => g.Id);
            IEnumerable<int> orderedIds = orderedExpectedGoals.Select(g => g.Id);
            returnedGoals.Select(g => g.Id).Should().ContainInOrder(orderedIds);
        }
        
        /// <summary>
        /// Tests that we are able to insert a record in to the database
        /// using the create function
        /// </summary>
        [TestMethod]
        public void ShouldInsertGoalIntoDatabaseUsingValuesProvided()
        {
            //Arrange
            int expectedId = 151;
            GoalStatus defaultCreateStatus = GoalStatus.Open;
            Goal knownGoal = new Goal(0, "Testing Goal A", null, 100, GoalStatus.Open, false);

            //Setup the repository
            mockGoalRepository.Setup
            (gr => gr.CreateGoalForUser(It.IsAny<int>(), It.Is<Goal>(goal => goal.Equals(knownGoal))))
            .Returns<int, Goal>((userId, goal) =>
            {
                Goal testGoal = new Goal(expectedId, goal.Name, goal.Description, goal.Target, defaultCreateStatus, goal.IsDefault);
                return testGoal;
            });

            IGoalService goalService = new GoalService(mockGoalRepository.Object);

            //Act
            Goal savedGoal = goalService.CreateGoal(1, knownGoal);

            //Assert
            savedGoal.Should().NotBeNull();
            savedGoal.Id.Should().Be(expectedId);
            savedGoal.Name.Should().Be(knownGoal.Name);
            savedGoal.Description.Should().Be(knownGoal.Description);
            savedGoal.Target.Should().Be(knownGoal.Target);
            savedGoal.Status.Should().Be(defaultCreateStatus);
            savedGoal.IsDefault.Should().Be(knownGoal.IsDefault);
        }
    }
}
