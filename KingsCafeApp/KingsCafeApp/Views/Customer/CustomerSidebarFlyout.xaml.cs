using KingsCafeApp.LoginSystem;
using KingsCafeApp.Views.Admin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Views.Customer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerSidebarFlyout : ContentPage
    {
        public ListView ListView;

        public CustomerSidebarFlyout()
        {
            InitializeComponent();

            BindingContext = new CustomerSidebarFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class CustomerSidebarFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<CustomerSidebarFlyoutMenuItem> MenuItems { get; set; }

            public CustomerSidebarFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<CustomerSidebarFlyoutMenuItem>(new[]
                {   
                    new CustomerSidebarFlyoutMenuItem { Id = 0, Icon="Home_icon.png", Title = "Home", TargetType=typeof(CustomerHome) },
                    new CustomerSidebarFlyoutMenuItem { Id = 1, Icon="category_icon.png", Title = "View Categories", TargetType=typeof(FoodCategory) },
                    new CustomerSidebarFlyoutMenuItem { Id = 1, Icon="category_icon.png", Title = "View Products", TargetType=typeof(ProductList) },
                    new CustomerSidebarFlyoutMenuItem { Id = 2,Icon="logo.png", Title = "About", TargetType=typeof(CustomerHome) },
                    new CustomerSidebarFlyoutMenuItem { Id = 2,Icon="ContactUs_icon.png", Title = "Contact us" , TargetType=typeof(CustomerHome)},
                
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}