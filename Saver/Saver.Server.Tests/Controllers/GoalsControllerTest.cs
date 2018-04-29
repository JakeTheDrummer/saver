using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        private const int USERID = 1;
        private const int GOALID = 1;

        /// <summary>
        /// Should test that the server locates the correct
        /// goals for the known user ID
        /// </summary>
        [TestMethod]
        public void Get_ShouldTestAPositiveGetRequestForUserGoals()
        {
            // Arrange
            GoalsController controller = new GoalsController();

            // Act
            IEnumerable<string> result = controller.Get(USERID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

        /// <summary>
        /// Ensures that the server can return a known goal by
        /// user ID and Goal ID
        /// </summary>
        [TestMethod]
        public void Get_ShouldReturnAKnownUserGoalByUserIDAndGoalID()
        {
            // Arrange
            GoalsController controller = new GoalsController();

            // Act
            string result = controller.Get(USERID, GOALID);

            // Assert
            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void Post()
        {
            // Arrange
            GoalsController controller = new GoalsController();

            // Act
            controller.Post("value");

            // Assert
        }

        [TestMethod]
        public void Put()
        {
            // Arrange
            GoalsController controller = new GoalsController();

            // Act
            controller.Put(5, "value");

            // Assert
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            GoalsController controller = new GoalsController();

            // Act
            controller.Delete(5);

            // Assert
        }
    }
}
