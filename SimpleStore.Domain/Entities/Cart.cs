using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public void AddItem(Product product, int quantity)
        {
            CartLine line = lineCollection
                .Where(w => w.Product.ProductId == product.ProductId)
                .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine
                    {
                        Product = product,
                        Quantity = quantity
                    });

            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(r => r.Product.ProductId == product.ProductId);
        }
        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(s => s.Product.Price * s.Quantity);
        }
        public void Clear()
        {
            lineCollection.Clear();
        }
        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }
    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
