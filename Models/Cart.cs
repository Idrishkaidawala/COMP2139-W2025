using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartInventoryManagement.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
    }

    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public decimal TotalCost
        {
            get
            {
                decimal total = 0;
                foreach (var item in Items)
                {
                    if (item.Product != null)
                    {
                        total += item.Product.Price * item.Quantity;
                    }
                }
                return total;
            }
        }
    }
}