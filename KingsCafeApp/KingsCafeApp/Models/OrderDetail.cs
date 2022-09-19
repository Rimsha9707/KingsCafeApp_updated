using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
namespace KingsCafeApp.Models
{
  public partial class OrderDetail
    {
        public int DetailsID { get; set; }
        public int OrderFID { get; set; }
        public int ItemFID { get; set; }
        public int Quantity { get; set; }
    }
}
