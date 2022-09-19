using System;
using System.Collections.Generic;
using System.Text;

namespace KingsCafeApp.ViewModels
{
    class Cart_VM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Picture { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public float Total { get; set; }
    }
}
