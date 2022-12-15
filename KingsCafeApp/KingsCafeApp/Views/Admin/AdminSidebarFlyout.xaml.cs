using KingsCafeApp.LoginSystem;
using KingsCafeApp.Views.AWR;
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

namespace KingsCafeApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminSidebarFlyout : ContentPage
    {
        public ListView ListView;

        public AdminSidebarFlyout()
        {
            InitializeComponent();

            BindingContext = new AdminSidebarFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class AdminSidebarFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<AdminSidebarFlyoutMenuItem> MenuItems { get; set; }

            public AdminSidebarFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<AdminSidebarFlyoutMenuItem>(new[]
                {
                    new AdminSidebarFlyoutMenuItem { Id = 0, Icon="Home_icon.png", Title = "Home", TargetType=typeof(AdminHome) },
                    new AdminSidebarFlyoutMenuItem { Id = 1, Icon="registration_pic.png", Title = "Add User", TargetType=typeof(Add_User) },
                    new AdminSidebarFlyoutMenuItem { Id = 2, Icon="registration_pic.png", Title = "Manage User", TargetType=typeof(Manage_User) },
                    new AdminSidebarFlyoutMenuItem { Id = 3, Icon="category_icon.png", Title = "Add Category", TargetType=typeof(Add_Category) },
                    new AdminSidebarFlyoutMenuItem { Id = 4, Icon="category_icon.png", Title = "Add Product", TargetType=typeof(Add_Product) },
                    new AdminSidebarFlyoutMenuItem { Id = 5, Icon="category_icon.png", Title = "Add Ingredient", TargetType=typeof(Add_Ingredient) },
                    new AdminSidebarFlyoutMenuItem { Id = 6,Icon="category_icon.png", Title = "Manage Category", TargetType=typeof(ManageCategory)},
                    new AdminSidebarFlyoutMenuItem { Id = 7,Icon="category_icon.png", Title = "Manage Product", TargetType=typeof(Manage_Product)},
                    new AdminSidebarFlyoutMenuItem { Id = 8,Icon="category_icon.png", Title = "Manage Ingredient", TargetType=typeof(Manage_Ingredient)},
                    new AdminSidebarFlyoutMenuItem { Id = 9,Icon="category_icon.png", Title = "Manage Orders", TargetType=typeof(Manage_Orders)},
                    
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