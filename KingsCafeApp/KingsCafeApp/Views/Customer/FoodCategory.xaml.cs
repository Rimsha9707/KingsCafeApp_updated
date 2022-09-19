using KingsCafeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Views.Customer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FoodCategory : ContentPage
    {
        public FoodCategory()
        {
            InitializeComponent();
            LoadingInd.IsRunning = true;
            LoadData();
            LoadingInd.IsRunning = false;
        }

        async void LoadData()
        {
            DataList.ItemsSource = (await App.firebaseDatabase.Child("Category").OnceAsync<Category>()).Select(x => new Category
            {
                CatID = x.Object.CatID,
                Name = x.Object.Name,
                Image = x.Object.Image,
            }).ToList();
        }

        private void DataList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as Category;
            Navigation.PushAsync(new Views.Customer.ProductList(item.CatID));
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new StartPage();
        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {

        }

        private void ToolbarItem_Clicked_2(object sender, EventArgs e)
        {

        }
    }
}