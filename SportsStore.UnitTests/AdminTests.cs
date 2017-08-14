using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1},
                new Product { ProductID = 2},
                new Product { ProductID = 3}
            }.AsQueryable());
            AdminController controller = new AdminController(mock.Object);
            Product[] products = ((IEnumerable<Product>)controller.Index().Model).ToArray();

            Assert.AreEqual(3, products.Length);
        }
    }
}
