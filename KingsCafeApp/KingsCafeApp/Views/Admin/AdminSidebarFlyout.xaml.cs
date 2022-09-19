using KingsCafeApp.LoginSystem;
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
                    new AdminSidebarFlyoutMenuItem { Id = 0, Icon="registration_pic.png", Title = "User List", TargetType=typeof(Userlist) },
                    new AdminSidebarFlyoutMenuItem { Id = 1, Icon="category_icon.png", Title = "Add Category", TargetType=typeof(Add_Category) },
                    new AdminSidebarFlyoutMenuItem { Id = 2, Icon="category_icon.png", Title = "Add Product", TargetType=typeof(Add_Product) },
                    new AdminSidebarFlyoutMenuItem { Id = 3,Icon="category_icon.png", Title = "Manage Category", TargetType=typeof(ManageCategory)},
                    new AdminSidebarFlyoutMenuItem { Id = 4,Icon="category_icon.png", Title = "Manage Product", TargetType=typeof(Manage_Product)},
                    
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