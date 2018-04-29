using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Saver.Server;
using Saver.Server.Controllers;

namespace Saver.Server.IntegrationTests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
