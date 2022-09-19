using KingsCafeApp.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Views.Customer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage : ContentPage
    {

        public CartPage()
        {
            InitializeComponent();
            try
            {
             
                UpdateCartAsync();
            }
            catch (Exception ex)
            {
               
                DisplayAlert("Message", "Somthing went wrong. This may be a problem with internet or application. Please ensure that you have a working internet connection and GPS enabled. \nError details : " + ex.Message, "Ok");
            }


        }

        async void UpdateCartAsync()
        {

            List<imageCell_VM> CartItems = new List<imageCell_VM>();
            decimal? Amount = 0;
            foreach (var item in App.Cart)
            {
                var prod = (await App.firebaseDatabase.Child("FoodItem").OnceAsync<Models.FoodItem>()).FirstOrDefault(x => x.Object.ItemID == item.ItemFID);

                decimal? total = (decimal?)(prod.Object.SalePrice * (item.Quantity));
                Amount += total;

                CartItems.Add(new imageCell_VM
                {
                    ID = prod.Object.ItemID,
                    image = prod.Object.Image,
                    Name = prod.Object.Name,
                    Detail = "Rs. " + prod.Object.SalePrice + " X  " + item.Quantity + " = Total Rs. " + total.ToString()
                });
            }

            App.Total = Amount;

            lblTotal.Text = Amount.ToString();
            DataList.ItemsSource = CartItems;

        }


        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (App.Cart.Count < 1)
                {
                    await DisplayAlert("Message", "Cart page is empty. Please add at least one item in cart", "Ok");
                    return;
                }

                var res = await DisplayActionSheet("Select payement method", "Close", "", "Cash on delivery", "Online payment");
                if (res == "Cash on delivery")
                {
                    await Navigation.PushAsync(new Views.Customer.ConfirmOrder());
                }

                if (res == "Online payment")
                {
                    //await Navigation.PushAsync(new Payment());
                }
            }
            catch (Exception ex)
            {

                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }

        }


        private async void btnRemove_Clicked(object sender, EventArgs e)
        {
            try
            {
                ImageButton btn = sender as ImageButton;
                var item = btn.CommandParameter as imageCell_VM;

                for (int i = 0; i < App.Cart.Count; i++)
                {
                    if (App.Cart[i].ItemFID == item.ID)
                    {
                        var res = await DisplayAlert("Question", "Are you sure you want to remove " + item.Name + "?", "Yes", "No");
                        if (res)
                        {
                            App.Cart.RemoveAt(i);
                        }
                    }
                }

                UpdateCartAsync();

            }
            catch (Exception ex)
            {
                await DisplayAlert("Message", "Somthing went wrong. This may be a problem with internet or application. Please ensure that you have a working internet connection and GPS enabled. \nError details : " + ex.Message, "Ok");
            }

        }
    }
}