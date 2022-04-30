using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; }
        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;
                if (Items != null)
                {
                    foreach (var item in Items)
                    {
                        totalPrice += item.price * item.Quantity;
                    }
                }

                return totalPrice;
            }
        }
    }
}
