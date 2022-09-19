
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
namespace KingsCafeApp.Models
{
    public partial class FoodItem
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public int CatFID { get; set; }
        public double SalePrice { get; set; }
        public string Image { get; set; }
        public int Rating { get; set; }
        public string Status { get; set; }
    }
}
