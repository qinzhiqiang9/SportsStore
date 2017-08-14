using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartUnitTests
    {
        [TestMethod]
        public void Can_Add_Cart()
        {
            Product p1 = new Product { ProductID = 1, Price = 100 };
            Product p2 = new Product { ProductID = 1, Price = 150 };
            Product p3 = new Product { ProductID = 1, Price = 210 };

            Cart cart = new Cart();
            cart.Add(p1, 1);

            Assert.AreEqual(1, cart.Items.Count());
            Assert.AreEqual(100, cart.GetTotalAmount());
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name = "P1", Price = 100m, Category = "Apples" }
            }.AsQueryable());

            Cart cart = new Cart();
            CartController controller = new CartController(mock.Object,null);
            controller.Add(cart, 1, null);

            Assert.AreEqual(1, cart.Items.Count());
            Assert.AreEqual(1, cart.Items.ToArray()[0].Product.ProductID);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen() {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name = "P1", Price = 100m, Category = "Apples" }
            }.AsQueryable());

            Cart cart = new Cart();
            CartController controller = new CartController(mock.Object,null);

            RedirectToRouteResult result = controller.Add(cart, 2, "myUrl");

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("myUrl", result.RouteValues["returnUrl"]);
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            Cart cart = new Cart();
            CartController controller = new CartController(null,null);

            CartIndexViewModel result = (CartIndexViewModel)controller.Index(cart, "myUrl").Model;

            Assert.AreEqual(cart, result.Cart);
            Assert.AreEqual("myUrl", result.ReturnUrl);
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart() {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            //mock.Setup(m => m.)

            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null, mock.Object);
            ViewResult result = target.Checkout(cart, shippingDetails);

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(),
                It.IsAny<ShippingDetails>()), Times.Never());

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ ProductID = 1}
            }.AsQueryable());

            AdminController controller = new AdminController(mock.Object);
            Product result = controller.Edit(1).Model as Product;

            Assert.AreNotEqual(null, result);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ ProductID = 1}
            }.AsQueryable());

            AdminController controller = new AdminController(mock.Object);
            Product result = controller.Edit(2).Model as Product;

            Assert.AreEqual(null, result);
        }
    }
}
