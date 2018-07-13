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

        /// <summary>
        /// Should return a null reference when the goal with the ID does not exist
        /// </summary>
        [TestMethod]
        public void ShouldThrowANullReferenceExceptionWhenGoalWithIDDoesNotExist()
        {
            //Arrange
            int expectedGoalID = 1;
            mockGoalRepository.Setup(gr => gr.GetGoal(It.Is<int>(val => val == expectedGoalID))).Returns(null as Goal);
            IGoalService service = new GoalService(mockGoalRepository.Object);

            //Act
            Action failAction = () => service.GetGoal(expectedGoalID);

            //Assert
            failAction.Should().Throw<ArgumentOutOfRangeException>();
        }



        /// <summary>
        /// Tests that we can update an existing goal for a user
        /// </summary>
        [TestMethod]
        public void ShouldUpdateGoalForUserBasedOnGoalIdAndUserId()
        {
            //Arrange
            int expectedId = 1;
            int expectedUserId = 1;
            Goal targetGoal = new Goal(expectedId, "Testing Goal", "Testing Goal", 50, GoalStatus.Open, false);
            Goal expectedGoal = new Goal(expectedId, "Testing Goal", "Testing Goal", 100, GoalStatus.Open, false);

            //Pretend the goal exists
            mockGoalRepository.Setup(gr => gr.GetGoalForUser(It.Is<int>(val => val == expectedId), It.Is<int>(val => val == expectedUserId))).Returns(targetGoal);

            //Pretend the update goals smoothly
            mockGoalRepository.Setup(gr => gr.UpdateGoal(It.Is<int>(val => val == expectedId), It.Is<Goal>(val => val.Equals(expectedGoal)))).Returns(expectedGoal);

            IGoalService goalService = new GoalService(mockGoalRepository.Object);

            //Act
            Goal savedGoal = goalService.UpdateGoal(expectedUserId, expectedId, expectedGoal);
            
            //Assert
            savedGoal.Should().NotBeNull();
            savedGoal.Id.Should().Be(expectedId);
            savedGoal.Name.Should().Be(expectedGoal.Name);
            savedGoal.Description.Should().Be(expectedGoal.Description);
            savedGoal.Target.Should().Be(expectedGoal.Target);
            savedGoal.Status.Should().Be(expectedGoal.Status);
            savedGoal.IsDefault.Should().Be(expectedGoal.IsDefault);
        }

        /// <summary>
        /// Tests that we cannot update an non-existant goal for a user
        /// </summary>
        [TestMethod]
        public void ShouldThrowANullReferenceExceptionWhenGoalDoesNotExistWithUserIdWhenUpdating()
        {
            //Arrange
            int expectedId = 1;
            int expectedUserId = 1;
            Goal expectedGoal = new Goal(expectedId, "Testing Goal", "Testing Goal", 100, GoalStatus.Open, false);

            //Pretend the goal does not exist
            mockGoalRepository.Setup(gr => gr.GetGoalForUser(It.Is<int>(val => val == expectedId), It.Is<int>(val => val == expectedUserId))).Returns(null as Goal);
            IGoalService goalService = new GoalService(mockGoalRepository.Object);

            //Act
            Action failAction = () => goalService.UpdateGoal(expectedUserId, expectedId, expectedGoal);

            //Assert
            failAction.Should().Throw<NullReferenceException>();
        }


        /// <summary>
        /// Tests that we can update an existing goal for a user
        /// </summary>
        [TestMethod]
        public void ShouldDeleteGoalForUserBasedOnGoalIdAndUserId()
        {
            //Arrange
            int expectedId = 1;
            int expectedUserId = 1;
            Goal expectedGoal = new Goal(expectedId, "Testing Goal", "Testing Goal", 100, GoalStatus.Open, false);

            //Pretend the goal exists
            mockGoalRepository.Setup(gr => gr.GetGoalForUser(It.Is<int>(val => val == expectedId), It.Is<int>(val => val == expectedUserId))).Returns(expectedGoal);

            //Pretend the delete goals operation goes smoothly
            mockGoalRepository.Setup(gr => gr.Delete(It.Is<int>(val => val == expectedId))).Returns(expectedGoal);

            IGoalService goalService = new GoalService(mockGoalRepository.Object);

            //Act
            bool goalRemoved = goalService.DeleteGoal(expectedUserId, expectedId);

            //Assert
            goalRemoved.Should().Be(true);
        }

        /// <summary>
        /// Tests that we cannot delete an non-existant goal for a user
        /// </summary>
        [TestMethod]
        public void ShouldThrowANullReferenceExceptionWhenGoalDoesNotExistWithUserIdWhenDeleting()
        {
            //Arrange
            int expectedId = 1;
            int expectedUserId = 1;
            Goal expectedGoal = new Goal(expectedId, "Testing Goal", "Testing Goal", 100, GoalStatus.Open, false);

            //Pretend the goal does not exist
            mockGoalRepository.Setup(gr => gr.GetGoalForUser(It.Is<int>(val => val == expectedId), It.Is<int>(val => val == expectedUserId))).Returns(null as Goal);
            IGoalService goalService = new GoalService(mockGoalRepository.Object);

            //Act
            Action failAction = () => goalService.DeleteGoal(expectedUserId, expectedId);

            //Assert
            failAction.Should().Throw<NullReferenceException>();
        }


        /// <summary>
        /// Tests that we cannot delete an non-existant goal for a user
        /// when performing the delete operation
        /// </summary>
        [TestMethod]
        public void ShouldThrowANullReferenceExceptionDuringDeleteOperation()
        {
            //Arrange
            int expectedId = 1;
            int expectedUserId = 1;
            Goal expectedGoal = new Goal(expectedId, "Testing Goal", "Testing Goal", 100, GoalStatus.Open, false);

            //Pretend the goal does not exist but the delete does not
            mockGoalRepository.Setup(gr => gr.GetGoalForUser(It.Is<int>(val => val == expectedId), It.Is<int>(val => val == expectedUserId))).Returns(expectedGoal);
            mockGoalRepository.Setup(gr => gr.Delete(It.Is<int>(val => val == expectedId))).Returns(null as Goal);

            IGoalService goalService = new GoalService(mockGoalRepository.Object);

            //Act
            Action failAction = () => goalService.DeleteGoal(expectedUserId, expectedId);

            //Assert
            failAction.Should().Throw<NullReferenceException>();
        }
    }
}
