﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using System.Web.Mvc;
using SportsStore.WebUI.HtmlHelpers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class Class1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
                {
                    new Product { ProductID = 1, Name = "P1" },
                    new Product { ProductID = 2, Name = "P2" },
                    new Product { ProductID = 3, Name = "P3" },
                    new Product { ProductID = 4, Name = "P4" }
                }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            IEnumerable<Product> result =((ProductsListViewModel)controller.List(null,1).Model).Products;

            Assert.AreEqual(4, result.Count());
            Assert.AreEqual("P1", result.ToArray()[0].Name);
            Assert.AreEqual("P2", result.ToArray()[1].Name);
            Assert.AreEqual("P3", result.ToArray()[2].Name);
            Assert.AreEqual("P4", result.ToArray()[3].Name);
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            HtmlHelper myHelper = null;
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual("<a href=\"Page1\">1</a><a class=\"selected\" href=\"Page2\">2</a><a href=\"Page3\">3</a>", result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 2, Name = "P2" },
                new Product { ProductID = 3, Name = "P3" },
                new Product { ProductID = 4, Name = "P4" },
                new Product { ProductID = 5, Name = "P5" }
            }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.pagesize = 3;
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null,2).Model;

            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(2, pageInfo.CurrentPage);
            Assert.AreEqual(2, pageInfo.TotalPages);
            Assert.AreEqual(3, pageInfo.ItemsPerPage);
            Assert.AreEqual(5, pageInfo.TotalItems);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product{ ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product{ ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product{ ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product{ ProductID = 5, Name = "P5", Category = "Cat3"}
            }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            Product[] result = ((ProductsListViewModel)controller.List("Cat1", 1).Model).Products.ToArray();

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("Cat1", result[0].Category);
            Assert.AreEqual("Cat1", result[1].Category);
        }
    }
}
