using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Saver.Model;
using Saver.Repositories.Interfaces;
using Saver.Server;
using Saver.Server.Controllers;

namespace Saver.Server.IntegrationTests.Controllers
{
    /// <summary>
    /// Tests the collection of goal actions in the controller
    /// </summary>
    [TestClass]
    public class GoalsControllerTest
    {
        Mock<IGoalRepository> mockGoalRepository = null;
        private const int USERID = 1;
        private const int GOALID = 1;
        Goal goal = null;

        /// <summary>
        /// Creates a new instance of the tests
        /// </summary>
        public GoalsControllerTest()
        {
            mockGoalRepository = new Mock<IGoalRepository>();
            this.goal = new Goal(3, "Test C", "C", 500, GoalStatus.Open, false);
        }

        /// <summary>
        /// Should test that the server locates the correct
        /// goals for the known user ID
        /// </summary>
        [TestMethod]
        public void Get_ShouldTestAPositiveGetRequestForUserGoals()
        {
            // Arrange
            GoalsController controller = new GoalsController(mockGoalRepository.Object);

            // Act
            IEnumerable<Goal> result = controller.Get(USERID);

            // Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Ensures that the server can return a known goal by
        /// user ID and Goal ID
        /// </summary>
        [TestMethod]
        public void Get_ShouldReturnAKnownUserGoalByUserIDAndGoalID()
        {
            // Arrange
            GoalsController controller = new GoalsController(mockGoalRepository.Object);

            // Act
            Goal result = controller.Get(USERID, GOALID);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Post()
        {
            // Arrange
            GoalsController controller = new GoalsController(mockGoalRepository.Object);

            // Act
            controller.Post(USERID, goal);

            // Assert
        }

        [TestMethod]
        public void Put()
        {
            // Arrange
            GoalsController controller = new GoalsController(mockGoalRepository.Object);

            // Act
            controller.Put(USERID, GOALID, goal);

            // Assert
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            GoalsController controller = new GoalsController(mockGoalRepository.Object);

            // Act
            controller.Delete(USERID, GOALID);

            // Assert
        }
    }
}
