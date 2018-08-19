using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Saver.DataAccess.Interfaces;
using Saver.Repositories.Implementations.Milestone;
using Saver.Repositories.Interfaces;
using Saver.Repositories.Services.Interfaces;
using Saver.Repositories.Tests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Repositories.Implementations.Milestone.Implementations.Milestones
{
    /// <summary>
    /// Tests that we are able to interact with the database
    /// via the Milestone Repository interface
    /// </summary>
    [TestClass]
    public class MilestoneRepositoryTests : SqlRepositoryTestBase
    {
        /// <summary>
        /// Tests that we can return all known milestones
        /// from the database
        /// </summary>
        [TestMethod]
        public void ShouldReturnAllKnownMilestones()
        {
            //Arrange
            string expectedSQLStatement = "SELECT * FROM saver.Milestones ORDER BY id ASC;";
            IEnumerable<Model.Milestone> expectedMilestones = new List<Model.Milestone>()
            {
                new Model.Milestone(1, 100, "Testing 1", null),
                new Model.Milestone(1, 200, "Testing 2", null),
                new Model.Milestone(1, 300, "Testing 3", null),
                new Model.Milestone(1, 400, "Testing 4", null),
                new Model.Milestone(1, 500, "Testing 5", null)
            };

            //Setup the mocks
            mockDataAccess.Setup(da => da.ExecuteQuery<Model.Milestone>(It.Is<string>(val => val.Equals(expectedSQLStatement)))).Returns(expectedMilestones);
            mockSqlStringService.Setup(ss => ss.GetSqlFromResource(It.IsAny<string>())).Returns(expectedSQLStatement);

            IMilestoneRepository repository = new MilestoneRepository(mockDataAccess.Object, mockSqlStringService.Object);

            //Act
            IEnumerable<Model.Milestone> returnedMilestones = repository.GetAll();

            //Assert
            returnedMilestones.Should().NotBeNull();
            returnedMilestones.Count().Should().Be(5);
            returnedMilestones.Should().Contain(expectedMilestones);
        }



        /// <summary>
        /// Tests that we can return a known milestone by ID
        /// </summary>
        [TestMethod]
        public void ShouldReturnASingleMilestoneById()
        {
            //Arrange
            string expectedSQLStatement = "SELECT * FROM saver.Milestones WHERE Id = @Id;";
            Model.Milestone expectedMilestone = new Model.Milestone(1, 100, "Testing 1", null);

            //Setup the mocks
            mockDataAccess.Setup
            (
                da => da.ExecuteQuery<Model.Milestone>
                (
                    It.Is<string>(val => val.Equals(expectedSQLStatement)),
                    It.Is<Dictionary<string, object>>(val => (int)val["Id"] == expectedMilestone.Id)
                )
            )
            .Returns(new Model.Milestone[] { expectedMilestone });
            mockSqlStringService.Setup(ss => ss.GetSqlFromResource(It.IsAny<string>())).Returns(expectedSQLStatement);

            IMilestoneRepository repository = new MilestoneRepository(mockDataAccess.Object, mockSqlStringService.Object);

            //Act
            Model.Milestone returnedMilestone = repository.Get(expectedMilestone.Id);

            //Assert
            returnedMilestone.Should().NotBeNull();
            returnedMilestone.Should().BeEquivalentTo(expectedMilestone);
        }


        /// <summary>
        /// Tests that we can return all known milestones
        /// from the database for a given goal
        /// </summary>
        [TestMethod]
        public void ShouldReturnAllKnownMilestonesForSpecificGoal()
        {
            //Arrange
            int goalId = 1;
            string expectedSQLStatement = "SELECT * FROM saver.Milestones WHERE goalid = @GoalId ORDER BY id ASC;";
            IEnumerable<Model.Milestone> expectedMilestones = new List<Model.Milestone>()
            {
                new Model.Milestone(1, 100, "Testing 1", null),
                new Model.Milestone(1, 200, "Testing 2", null),
                new Model.Milestone(1, 300, "Testing 3", null),
                new Model.Milestone(1, 400, "Testing 4", null),
                new Model.Milestone(1, 500, "Testing 5", null)
            };

            //Setup the mocks
            mockDataAccess.Setup(da => da.ExecuteQuery<Model.Milestone>
                (
                    It.Is<string>(val => val.Equals(expectedSQLStatement)),
                    It.Is<Dictionary<string, object>>(val => (int)val["GoalId"] == goalId)
                )
            ).Returns(expectedMilestones);
            mockSqlStringService.Setup(ss => ss.GetSqlFromResource(It.IsAny<string>())).Returns(expectedSQLStatement);

            IMilestoneRepository repository = new MilestoneRepository(mockDataAccess.Object, mockSqlStringService.Object);

            //Act
            IEnumerable<Model.Milestone> returnedMilestones = repository.GetForGoal(goalId);

            //Assert
            returnedMilestones.Should().NotBeNull();
            returnedMilestones.Count().Should().Be(5);
            returnedMilestones.Should().Contain(expectedMilestones);
        }

        /// <summary>
        /// Tests that given a milestone and a goal Id that we attempt
        /// to insert that goal in to the database
        /// </summary>
        [TestMethod()]
        public void ShouldCreateASingleMilestoneForAKnownGoal()
        {
            const string expectedDescription = "Testing a milestone";
            double expectedTarget = 230d;
            DateTime expectedDateMet = DateTime.Now;
            int expectedGoalId = 1;
            int expectedMilestoneId = 10;
            Model.Milestone milestone = new Model.Milestone(expectedTarget, expectedDescription, expectedDateMet);
            Model.Milestone expectedResultandMilestone = new Model.Milestone(expectedMilestoneId, expectedTarget, expectedDescription, expectedDateMet);
            string expectedSqlStatement = "INSERT INTO saver.Milestone (target, description, datemet, goalid) VALUES (@Target, @Description, @DateMet, @GoalId);";

            mockDataAccess.Setup
            (
                da => da.ExecuteQuery<Model.Milestone>
                (
                    It.Is<string>(val => val.Equals(expectedSqlStatement)),
                    It.Is<Dictionary<string, object>>
                    (
                        val => (int)val["GoalId"] == expectedGoalId
                            && (string)val["Description"] == expectedDescription
                            && (double)val["Target"] == expectedTarget
                            && (DateTime?)val["DateMet"] == expectedDateMet
                    )
                )
            ).Returns(new List<Model.Milestone>() { expectedResultandMilestone });
            mockSqlStringService.Setup(ss => ss.GetSqlFromResource(It.IsAny<string>())).Returns(expectedSqlStatement);

            IMilestoneRepository repository = new MilestoneRepository(mockDataAccess.Object, mockSqlStringService.Object);

            //Act
            Model.Milestone returnedMilestone = repository.CreateForGoal(milestone, expectedGoalId);

            //Assert
            returnedMilestone.Should().NotBeNull();
            returnedMilestone.Id.Should().Be(expectedMilestoneId);
            returnedMilestone.Should().BeEquivalentTo(expectedResultandMilestone);
        }

        /// <summary>
        /// Tests that we are able to create multiple milestones
        /// on the system for the same goal
        /// </summary>
        [TestMethod()]
        public void ShouldCreateMultipleMilestonesForAGivenGoalId()
        {
            int expectedGoalId = 1;
            List<Model.Milestone> milestones = new List<Model.Milestone>()
            {
                new Model.Milestone(100, "Testing", null),
                new Model.Milestone(200, "Testing", null),
                new Model.Milestone(300, "Testing", null)
            };
            List<Model.Milestone> createdMilestones = new List<Model.Milestone>()
            {
                new Model.Milestone(1, 100, "Testing", null),
                new Model.Milestone(2, 200, "Testing", null),
                new Model.Milestone(3, 300, "Testing", null)
            };
            string expectedSqlStatement = "INSERT INTO saver.Milestone (target, description, datemet, goalid) VALUES (@Target, @Description, @DateMet, @GoalId);";

            mockDataAccess.Setup
            (
                da => da.ExecuteWithGenericParameters
                (
                    It.Is<string>(val => val.Equals(expectedSqlStatement)),
                    It.IsAny<object>()
                )
            ).Returns(createdMilestones.Count);

            mockDataAccess.Setup
            (
                da => da.ExecuteQueryWithGenericParameterType<Model.Milestone>
                (
                    It.Is<string>(val => val.Equals(expectedSqlStatement)),
                    It.IsAny<object>()
                )
            ).Returns(createdMilestones);
            mockSqlStringService.Setup(ss => ss.GetSqlFromResource(It.IsAny<string>())).Returns(expectedSqlStatement);

            IMilestoneRepository repository = new MilestoneRepository(mockDataAccess.Object, mockSqlStringService.Object);

            //Act
            IEnumerable<Model.Milestone> returnedMilestones = repository.CreateMultipleForGoal(milestones, expectedGoalId);

            //Assert
            returnedMilestones.Should().NotBeNull();
            returnedMilestones.Count().Should().Be(createdMilestones.Count);
            returnedMilestones.Select(s => s.Id).Should().Contain(createdMilestones.Select(s => s.Id));
        }

        /// <summary>
        /// Tests that we can update a milestone with information given its ID
        /// </summary>
        [TestMethod()]
        public void ShouldUpdateMilestoneWithNewInformation()
        {
            int expectedMilestoneId = 2;
            Model.Milestone updatedMilestone = new Model.Milestone(2, 250d, null, null);
            string expectedSqlStatement = "UPDATE saver.milestones SET target = @Target, DateMet = @DateMet, Description = @Description WHERE Id = @Id";
            mockDataAccess.Setup
            (
                da => da.ExecuteQueryWithGenericParameterType<Model.Milestone>
                (
                    It.Is<string>(val => val.Equals(expectedSqlStatement)),
                    It.IsAny<Model.Milestone>()
                )
            ).Returns(new Model.Milestone[] { updatedMilestone });
            mockSqlStringService.Setup(ss => ss.GetSqlFromResource(It.IsAny<string>())).Returns(expectedSqlStatement);


            IMilestoneRepository repository = new MilestoneRepository(mockDataAccess.Object, mockSqlStringService.Object);
            
            //Act
            Model.Milestone milestone = repository.Update(expectedMilestoneId, updatedMilestone);

            //Assert
            milestone.Should().NotBeNull();
            milestone.Id.Should().Be(expectedMilestoneId);
            milestone.Should().BeEquivalentTo(updatedMilestone);
        }

        /// <summary>
        /// Tests that we're able to effectively delete a milestone from the 
        /// system given an id
        /// </summary>
        [TestMethod()]
        public void ShouldDeleteAMilestoneWithAGivenIDAndReturn()
        {
            int milestoneId = 2;
            Model.Milestone deletedMilestone = new Model.Milestone(2, 250d, null, null);
            string expectedSqlStatement = "DELETE FROM saver.milestons... etc";
            mockDataAccess.Setup
            (
                da => da.ExecuteQueryWithGenericParameterType<Model.Milestone>
                (
                    It.Is<string>(val => val.Equals(expectedSqlStatement)),
                    It.IsAny<object>()
                )
            ).Returns(new Model.Milestone[] { deletedMilestone });
            mockSqlStringService.Setup(ss => ss.GetSqlFromResource(It.IsAny<string>())).Returns(expectedSqlStatement);


            IMilestoneRepository repository = new MilestoneRepository(mockDataAccess.Object, mockSqlStringService.Object);

            //Act
            Model.Milestone milestone = repository.Delete(milestoneId);

            //Assert
            milestone.Should().NotBeNull();
            milestone.Id.Should().Be(milestoneId);
            milestone.Should().BeEquivalentTo(deletedMilestone);
        }
    }
}