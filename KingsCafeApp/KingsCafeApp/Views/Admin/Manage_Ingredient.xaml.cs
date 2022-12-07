using Firebase.Database.Query;
using KingsCafeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Manage_Ingredient : ContentPage
    {
        public Manage_Ingredient()
        {
            InitializeComponent();

        }

        //OnAppearing is overrided fuction which we use here
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                LoadingInd.IsRunning = true;
                LoadData();
                LoadingInd.IsRunning = false;
            }
            catch (Exception ex)
            {
                LoadingInd.IsRunning = false;
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }
        }

        async void LoadData()
        {
            DataList.ItemsSource = (await App.firebaseDatabase.Child("Ingredient").OnceAsync<Ingredient>()).Select(x => new Ingredient
            {
                IngredientID = x.Object.IngredientID,
                Name = x.Object.Name,
                Type = x.Object.Type,
                Price = x.Object.Price,
                Image=x.Object.Image,
            }).ToList();
        }

       
        private async void DataList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {

                var selected = e.Item as Ingredient;
                var item = (await App.firebaseDatabase.Child("Ingredient").OnceAsync<Ingredient>()).FirstOrDefault(a => a.Object.IngredientID == selected.IngredientID);
                var choice = await DisplayActionSheet("Options", "Close", "Delete", "View", "Edit");

                if (choice == "View")
                {
                    await Navigation.PushAsync(new Ingredient_Details(selected));
                }
                if (choice == "Delete")
                {
                    var q = await DisplayAlert("Confirmation", "Are you sure you want to delete " + item.Object.Name + "?", "Yes", "No");
                    if (q)
                    {
                        await App.firebaseDatabase.Child("Ingredient").Child(item.Key).DeleteAsync();
                        LoadData();
                        await DisplayAlert("Message", item.Object.Name + "'s" + " Ingredient is deleted permanently.", "Ok");
                    }

                }
                if (choice == "Edit")
                {
                    await Navigation.PushAsync(new Edit_Ingredient(selected));

                }

            }

            catch (Exception ex)
            {

                LoadingInd.IsRunning = false;
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new StartPage();
        }
    }
}