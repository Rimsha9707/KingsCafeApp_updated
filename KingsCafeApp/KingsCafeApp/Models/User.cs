using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
namespace KingsCafeApp.Models
{
    public partial class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
    }
}
