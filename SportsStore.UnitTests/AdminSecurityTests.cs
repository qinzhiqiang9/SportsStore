using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.WebUI.Infrastructure.Abstract;
using Moq;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminSecurityTests
    {
        private Mock<IAuthProvider> mock;

        [TestInitialize]
        public void Initialize()
        {
            mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "admin")).Returns(true);
        }

        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            AccountController controller = new AccountController(mock.Object);
            LoginViewModel model = new LoginViewModel { UserName = "admin", Password = "admin" };
            ActionResult result = controller.Login(model, "myUrl");

            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("myUrl", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Cannot_Login_With_Valid_Credentials()
        {
            AccountController controller = new AccountController(mock.Object);
            LoginViewModel model = new LoginViewModel { UserName = "not", Password = "not" };
            ActionResult result = controller.Login(model, "myUrl");

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
            
        }

    }
}
