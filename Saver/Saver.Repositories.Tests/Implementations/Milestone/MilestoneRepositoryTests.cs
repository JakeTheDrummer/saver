using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Saver.DataAccess.Interfaces;
using Saver.Repositories.Implementations.Milestone;
using Saver.Repositories.Interfaces;
using Saver.Repositories.Services.Interfaces;
using System;
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
    public class MilestoneRepositoryTests
    {
        private readonly Mock<ITypedDataAccess> mockDataAccess;
        private readonly Mock<ISqlStringService> mockSqlStringService;

        /// <summary>
        /// Creates new Milestone Repository Unit Tests
        /// </summary>
        public MilestoneRepositoryTests()
        {
            mockDataAccess = new Mock<ITypedDataAccess>();
            mockSqlStringService = new Mock<ISqlStringService>();
        }

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

        [TestMethod()]
        public void GetForGoalTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateForGoalTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateMultipleForGoalTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTest()
        {
            Assert.Fail();
        }
    }
}