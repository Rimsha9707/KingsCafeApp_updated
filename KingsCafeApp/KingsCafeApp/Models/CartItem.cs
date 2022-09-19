using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KingsCafeApp.Models
{
    public class CartItem
    {
        public FoodItem foodItem {get; set;}
        public int quantity { get; set; }
    }
}