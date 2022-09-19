using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace KingsCafeApp.Models
{
    public class Category
    {
        public int CatID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
    }
}
