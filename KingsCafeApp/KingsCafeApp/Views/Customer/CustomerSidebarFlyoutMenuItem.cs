using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsCafeApp.Views.Customer
{
    public class CustomerSidebarFlyoutMenuItem
    {
        public CustomerSidebarFlyoutMenuItem()
        {
            TargetType = typeof(CustomerSidebarFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }

        public Type TargetType { get; set; }
    }
}