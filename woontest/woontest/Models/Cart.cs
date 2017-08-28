using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace woontest.Models
{
    public class Cart
    {
        private List<CartLine> cartLine = new List<CartLine>();
        public IEnumerable<CartLine> Lines { get { return cartLine; } }
        public bool Discount { get; set; }

        public void AddItem(Product producr, int quality, StringBuilder details = null)
        {
            CartLine line = cartLine
                .Where(r => r.Product.Id == producr.Id)
                .FirstOrDefault();

            if (line == null)
            {
                cartLine.Add(new CartLine { Product = producr, Quantity = quality, Details = details });
            }
            else
            {
                line.Quantity += quality;
            }

        }

        public void RemoveItem(Product product)
        {
            cartLine.RemoveAll(r => r.Product.Id == product.Id);
        }

        public decimal TotalCost()
        {
            return cartLine.Sum(t => t.Product.PriceProduct * t.Quantity);
        }

        public void Clear() => cartLine.Clear();

    }

    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public StringBuilder Details { get; set; }
    }
}