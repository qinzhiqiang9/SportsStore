using SportsStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Concrete
{
    public class EFProductRepository : IProductsRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Product> Products => context.Products;

        public void SaveProduct(Product product)
        {
            if (product.ProductID == 0)
            {
                // new add
                context.Products.Add(product);
            }
            else
            {
                Product dbEntry = context.Products.Find(product.ProductID);
                dbEntry.Name = product.Name;
                dbEntry.Price = product.Price;
                dbEntry.Category = product.Category;
                dbEntry.Description = product.Description;
            }
            context.SaveChanges();
        }

        public Product DeleteProduct(int productID)
        {
            Product entry = context.Products.Find(productID);
            if (entry != null)
            {
                context.Products.Remove(entry);
                context.SaveChanges();
            }
            return entry;
        }
    }
}
