using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Entities
{
    public class Cart
    {
        private IList<CartItem> _items = new List<CartItem>();

        public void Add(Product product, int quantity)
        {
            CartItem item = _items.Where(i => i.Product.ProductID == product.ProductID)
                .FirstOrDefault();
            if (item == null)
            {
                _items.Add(new CartItem { Product = product, Quantity = quantity });
            }
            else
            {
                item.Quantity += quantity;
            }
        }

        public void Remove(Product product)
        {
            CartItem item = _items.Where(i => i.Product.ProductID == product.ProductID)
                .FirstOrDefault();
            if (item != null)
            {
                _items.Remove(item);
            }
        }

        public void Clear()
        {
            _items.Clear();
        }

        public decimal GetTotalAmount()
        {
            decimal result = 0m;

            foreach (CartItem item in _items)
            {
                result += item.Product.Price * item.Quantity;
            }
            return result;
        }

        public IEnumerable<CartItem> Items { get { return _items.AsEnumerable(); } }
    }

    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
