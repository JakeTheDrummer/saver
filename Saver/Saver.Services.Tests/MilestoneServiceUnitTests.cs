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
    /// Tests that we can interact appropriately with
    /// the milestone service and repository layers
    /// </summary>
    [TestClass]
    public class MilestoneServiceUnitTests
    {
        private const int EXPECTED_GENERATION_COUNT = 5;
        public readonly Mock<IMilestoneRepository> mockRepository;

        /// <summary>
        /// Creates a new instance of the Milestone Service Unit Tests
        /// </summary>
        public MilestoneServiceUnitTests()
        {
            this.mockRepository = new Mock<IMilestoneRepository>();
        }

        /// <summary>
        /// Tests that we can return a collection of known milestones
        /// in the appropriate order from the service
        /// </summary>
        [TestMethod]
        public void ShouldReturnAKnownCollectionOfOrderedMilestones()
        {
            //Arrange
            IEnumerable<Milestone> expectedMilestones = new List<Milestone>()
            {
                new Milestone(1, 100, "Testing 1", null),
                new Milestone(7, 200, "Testing 2", null),
                new Milestone(2, 300, "Testing 3", null),
                new Milestone(4, 400, "Testing 4", null),
                new Milestone(3, 500, "Testing 5", null)
            };
            IEnumerable<Milestone> expectedOrderedMilestones = expectedMilestones.OrderBy(m => m.Id);
            mockRepository.Setup(r => r.GetAll()).Returns(expectedMilestones);

            IMilestoneService service = new MilestoneService(mockRepository.Object);

            //Act
            IEnumerable<Milestone> milestones = service.GetAllMilestones();

            //Assert
            milestones.Should().NotBeNull();
            milestones.Should().ContainInOrder(expectedOrderedMilestones);
            milestones.Count().Should().Be(expectedOrderedMilestones.Count());
        }

        /// <summary>
        /// Ensures we can return a collection of milestones for a given Goal ID
        /// </summary>
        [TestMethod]
        public void ShouldReturnOrderedMilestonesForAGivenGoalId()
        {
            //Arrange
            int expectedGoalId = 1;
            IEnumerable<Milestone> expectedMilestones = new List<Milestone>()
            {
                new Milestone(1, 100, "Testing 1", null),
                new Milestone(2, 300, "Testing 3", null),
                new Milestone(3, 500, "Testing 5", null)
            };
            IEnumerable<Milestone> expectedOrderedMilestones = expectedMilestones.OrderBy(m => m.Id);
            mockRepository.Setup(r => r.GetForGoal(It.Is<int>(val => val == expectedGoalId))).Returns(expectedMilestones);

            IMilestoneService service = new MilestoneService(mockRepository.Object);

            //Act
            IEnumerable<Milestone> milestones = service.GetMilestonesForGoal(expectedGoalId);

            //Assert
            milestones.Should().NotBeNull();
            milestones.Should().ContainInOrder(expectedOrderedMilestones);
            milestones.Count().Should().Be(expectedOrderedMilestones.Count());
        }

        /// <summary>
        /// Ensures we are able to simply return a milestone given its ID
        /// </summary>
        [TestMethod]
        public void ShouldReturnAMilestoneGivenAKnownId()
        {
            int expectedMilestoneId = 2;
            Milestone expectedMilestone = new Milestone(2, 250d, "Testing", null);

            mockRepository.Setup(r => r.Get(It.Is<int>(val => val == expectedMilestoneId))).Returns(expectedMilestone);

            IMilestoneService service = new MilestoneService(mockRepository.Object);

            //Act
            Milestone milestone = service.GetMilestone(expectedMilestoneId);

            //Assert
            milestone.Should().NotBeNull();
            milestone.Id.Should().Be(expectedMilestoneId);
            milestone.Should().BeEquivalentTo(expectedMilestone);
        }

        /// <summary>
        /// Tests that we generate the milestones for a given
        /// goal and persist these in a persistent data store
        /// </summary>
        [TestMethod]
        public void ShouldGenerateMilestonesForGoal()
        {
            //Arrange
            const int expectedGoalId = 10;
            const string expectedGoalName = "Testing Goal";
            Goal goal = new Goal(expectedGoalId, expectedGoalName, "Test", 100, GoalStatus.Open, false);

            //Set up the mocks
            mockRepository.Setup(r => r.GetForGoal(It.Is<int>(val => val == expectedGoalId))).Returns(new List<Milestone>());
            mockRepository.Setup(r => r.CreateMultipleForGoal(It.IsAny<IEnumerable<Milestone>>(), It.Is<int>(val => val == expectedGoalId)))
            .Returns<IEnumerable<Milestone>, int>
            (
                (unsavedMilestones, goalId) =>
                {
                    int id = 1;
                    foreach (var milestone in unsavedMilestones)
                    {
                        milestone.Id = id;
                        id++;
                    }
                    return unsavedMilestones;
                }
            );
            IMilestoneService service = new MilestoneService(mockRepository.Object);

            //Act
            Milestone[] milestones = service.GenerateMilestones(goal).ToArray();

            //Assert
            milestones.Should().NotBeNull();
            milestones.Count().Should().Be(EXPECTED_GENERATION_COUNT);
            milestones.Select(m => m.Id).Should().BeInAscendingOrder();
            for (int i = 1; i <= EXPECTED_GENERATION_COUNT; i++)
            {
                Milestone testMilestone = milestones[i - 1];
                testMilestone.Target.Should().Be((goal.Target / EXPECTED_GENERATION_COUNT) * i);
                testMilestone.Description.Should().Be($"{goal.Name} - {i}/{EXPECTED_GENERATION_COUNT} completed!");
            }
        }

        /// <summary>
        /// Ensures that we do not allow the automatic generation
        /// of milestones for a goal if they have one or more
        /// </summary>
        [TestMethod]
        public void ShouldNotGenerateMilestonesForGoalIfTheyExistAlready()
        {
            //Arrange
            const int expectedGoalId = 10;
            const string expectedGoalName = "Testing Goal";
            Goal goal = new Goal(expectedGoalId, expectedGoalName, "Test", 100, GoalStatus.Open, false);

            //Set up the mocks
            mockRepository.Setup(r => r.GetForGoal(It.Is<int>(val => val == expectedGoalId)))
                .Returns(new List<Milestone>() { new Milestone(10, 100, "Testing", null) });

            IMilestoneService service = new MilestoneService(mockRepository.Object);

            //Act
            Action failAction = () => service.GenerateMilestones(goal).ToArray();
            failAction.Should().Throw<Exception>();
        }

        /// <summary>
        /// Tests that we can create a milestone for a goal via the service
        /// </summary>
        [TestMethod]
        public void ShouldCreateMilestoneForGoal()
        {
            //Arrange
            const int expectedMilestoneId = 10, expectedGoalId = 13;
            Milestone milestone = new Milestone(100d, "Testing", null);
            Goal goal = new Goal(expectedGoalId, "Testing Goal", "Testing", 1000d, GoalStatus.Open, false);

            //Act

            //Set up the mocks
            mockRepository.Setup(r => r.GetForGoal(It.Is<int>(val => val == expectedGoalId))).Returns(new List<Milestone>());
            mockRepository.Setup(r => r.CreateForGoal(It.IsAny<Milestone>(), It.Is<int>(val => val == expectedGoalId)))
            .Returns<Milestone, int>
            (
                (unsavedMilestone, goalId) =>
                {
                    unsavedMilestone.Id = expectedMilestoneId;
                    return unsavedMilestone;
                }
            );
            IMilestoneService service = new MilestoneService(mockRepository.Object);

            //Act
            Milestone createdMilestone = service.CreateMilestoneForGoal(milestone, goal);

            //Assert
            createdMilestone.Should().NotBeNull();
            createdMilestone.Id.Should().Be(expectedMilestoneId);
            createdMilestone.Should().BeEquivalentTo(milestone);
        }
        
        /// <summary>
        /// Tests that we cannot create a milestone with a target greater than the goal
        /// </summary>
        [TestMethod]
        public void ShouldNotCreateMilestoneForGoalIfTargetExceedsGoalTarget()
        {
            //Arrange
            const int expectedGoalId = 13;
            Milestone milestone = new Milestone(101d, "Testing", null);
            Goal goal = new Goal(expectedGoalId, "Testing Goal", "Testing", 100d, GoalStatus.Open, false);

            //Act

            //Set up the mocks
            IMilestoneService service = new MilestoneService(mockRepository.Object);

            //Act
            Action failAction = () => service.CreateMilestoneForGoal(milestone, goal);

            //Assert
            failAction.Should().Throw<ArgumentException>();
        }

        /// <summary>
        /// Tests that we cannot create a milestone with a target less than zero
        /// </summary>
        [TestMethod]
        public void ShouldNotCreateMilestoneForGoalIfTargetIsZeroOrBelow()
        {
            //Arrange
            const int expectedGoalId = 13;
            Milestone milestone = new Milestone(0d, "Testing", null);
            Goal goal = new Goal(expectedGoalId, "Testing Goal", "Testing", 100d, GoalStatus.Open, false);

            //Act

            //Set up the mocks
            IMilestoneService service = new MilestoneService(mockRepository.Object);

            //Act
            Action failAction = () => service.CreateMilestoneForGoal(milestone, goal);

            //Assert
            failAction.Should().Throw<ArgumentException>();
        }

        /// <summary>
        /// Tests that we cannot create a milestone a matching target
        /// to another milestone that exists already for the goal
        /// </summary>
        [TestMethod]
        public void ShouldNotCreateMilestoneForAGoalWithADuplicateTargetValue()
        {
            //Arrange
            const double collisionTargetValue = 250d;
            const int expectedGoalId = 13;
            Milestone milestone = new Milestone(collisionTargetValue, "Testing", null);
            Goal goal = new Goal(expectedGoalId, "Testing Goal", "Testing", 1000d, GoalStatus.Open, false);

            //Set up the mocks
            mockRepository.Setup(r => r.GetForGoal(It.Is<int>(val => val == expectedGoalId))).Returns
            (
                new List<Milestone>() { new Milestone(15, collisionTargetValue, "Testing", null) }
            );
            IMilestoneService service = new MilestoneService(mockRepository.Object);

            //Act
            Action failAction = () => service.CreateMilestoneForGoal(milestone, goal);

            //Assert
            failAction.Should().Throw<ArgumentException>();
        }

        /// <summary>
        /// Tests that we are able to updat the milestone on the system
        /// with the new infomation required
        /// </summary>
        [TestMethod]
        public void ShouldUpdateMilestoneWithNewInformation()
        {
            int expectedMilestoneId = 10;
            Milestone milestone = new Milestone(100d, "Testing", DateTime.Now);
            Milestone existingMilestone = new Milestone(100d, "Testing", null);

            //Setup the mocks
            mockRepository.Setup(r => r.Get(It.Is<int>(val => val == expectedMilestoneId))).Returns(existingMilestone);
            mockRepository.Setup(r => r.Update(It.Is<int>(val => val == expectedMilestoneId), It.IsAny<Milestone>())).Returns<int, Milestone>
            (
                (id, unsavedMilestone) =>
                {
                    unsavedMilestone.Id = id;
                    return unsavedMilestone;
                }
            );
            IMilestoneService service = new MilestoneService(mockRepository.Object);

            //Act
            Milestone updatedMilestone = service.UpdateMilestone(expectedMilestoneId, milestone);

            //Assert
            updatedMilestone.Should().NotBeNull();
            updatedMilestone.Id.Should().Be(expectedMilestoneId);
            updatedMilestone.Should().BeEquivalentTo(milestone);
        }

        /// <summary>
        /// Ensures that we check a milestone exists before attempt to update it
        /// </summary>
        [TestMethod]
        public void ShouldNotUpdateAMilestoneIfItDoesNotExist()
        {
            int expectedMilestoneId = 10;
            Milestone milestone = new Milestone(100d, "Testing", DateTime.Now);

            //Setup the mocks
            mockRepository.Setup(r => r.Get(It.Is<int>(val => val == expectedMilestoneId))).Returns<Milestone>(null);
            IMilestoneService service = new MilestoneService(mockRepository.Object);

            Action failAction = () => service.UpdateMilestone(expectedMilestoneId, milestone);

            //Assert
            failAction.Should().Throw<ArgumentException>();
        }
        
        /// <summary>
        /// Ensures that we check a milestone has not been completed before we
        /// attempt to update it
        /// </summary>
        [TestMethod]
        public void ShouldNotUpdateAMilestoneOnceItHasBeenCompleted()
        {
            int expectedMilestoneId = 10;
            Milestone milestone = new Milestone(expectedMilestoneId, 100d, "Testing", null);
            Milestone existingMilestone = new Milestone(expectedMilestoneId, 1100d, "Testing", DateTime.Now);

            //Setup the mocks
            mockRepository.Setup(r => r.Get(It.Is<int>(val => val == expectedMilestoneId))).Returns(existingMilestone);
            IMilestoneService service = new MilestoneService(mockRepository.Object);

            Action failAction = () => service.UpdateMilestone(expectedMilestoneId, milestone);

            //Assert
            failAction.Should().Throw<Exception>();
        }

        /// <summary>
        /// Tests that we are able to updat the milestone on the system
        /// with the new infomation required
        /// </summary>
        [TestMethod]
        public void ShouldDeleteMilestoneFromSystemByKnownId()
        {
            int expectedMilestoneId = 10;
            Milestone expectedMilestone = new Milestone(expectedMilestoneId, 100d, "Testing", null);

            //Setup the mocks
            mockRepository.Setup(r => r.Get(It.Is<int>(val => val == expectedMilestoneId))).Returns(expectedMilestone);
            mockRepository.Setup(r => r.Delete(It.Is<int>(val => val == expectedMilestoneId))).Returns(expectedMilestone);
            IMilestoneService service = new MilestoneService(mockRepository.Object);

            //Act
            Milestone removedMilestone = service.DeleteMilestone(expectedMilestoneId);

            //Assert
            removedMilestone.Should().NotBeNull();
            removedMilestone.Id.Should().Be(expectedMilestoneId);
            removedMilestone.Should().BeEquivalentTo(expectedMilestone);
        }

        /// <summary>
        /// Tests that we are not able to delete the milestone from a system that has been completed
        /// </summary>
        [TestMethod]
        public void ShouldNotDeleteMilestoneFromSystemIfItHasBeenCompleted()
        {
            int expectedMilestoneId = 10;
            Milestone expectedMilestone = new Milestone(expectedMilestoneId, 100d, "Testing", DateTime.Now);

            //Setup the mocks
            mockRepository.Setup(r => r.Get(It.Is<int>(val => val == expectedMilestoneId))).Returns(expectedMilestone);
            IMilestoneService service = new MilestoneService(mockRepository.Object);

            //Act
            Action failAction = () => service.DeleteMilestone(expectedMilestoneId);

            //Assert
            failAction.Should().Throw<Exception>();
        }
    }
}
