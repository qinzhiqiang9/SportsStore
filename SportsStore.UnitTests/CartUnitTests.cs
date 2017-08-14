using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
