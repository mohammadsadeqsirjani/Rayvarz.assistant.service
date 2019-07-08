using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rayvarz.inv.assistant.service;
using Rayvarz.inv.assistant.service.Controllers;

namespace Rayvarz.inv.assistant.service.Tests.Controllers
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


        [TestMethod]
        public void AutenticateTest()
        {
            // Arrange
            UserController controller = new UserController();

            // Act
           // var result = controller.Login();

            // Assert
           // Assert.IsNotNull(result);
           // Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
