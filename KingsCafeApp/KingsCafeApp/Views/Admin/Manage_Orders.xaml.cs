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
    public partial class Manage_Orders : ContentPage
    {
        public Manage_Orders()
        {
            InitializeComponent();
            LoadingInd.IsRunning = true;
            LoadData();
            LoadingInd.IsRunning = false;
        }
        async void LoadData()
        {
            try
            {
                DataList.ItemsSource = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).Select(x => new Order
                {
                    Address = x.Object.Address,
                    DeliveryDate = x.Object.DeliveryDate,
                    Email = x.Object.Email,
                    OrderDate = x.Object.OrderDate,
                    OrderID = x.Object.OrderID,
                    OrderTime = x.Object.OrderTime,
                    PaymentMethod = x.Object.PaymentMethod,
                    Phone = x.Object.Phone,
                    Name = x.Object.Name,
                    Status = x.Object.Status,
                    AssignedRider = x.Object.AssignedRider

                }).ToList();
                var item = DataList;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new StartPage();
        }
        private async void DataList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var selected = e.Item as Order;


                var item = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).FirstOrDefault(a => a.Object.OrderID == selected.OrderID);
                var choice = await DisplayActionSheet("Options", "Close", "Save", "Send to Delivered", "Send to Proceed");

                if (choice == "Send to Proceed")
                {
                    App.Current.MainPage = new RiderList(item.Object);


                }
                if (choice == "Send to Delivered")
                {
                    item.Object.Status = "Delivered";
                    await App.firebaseDatabase.Child("Order").Child(item.Key).PutAsync(item.Object);
                    await DisplayAlert("Message", item.Object.OrderID + "'s" + " order is Delivered now.", "Ok");
                }
                //if (choice == "Edit")
                //{
                //    await Navigation.PushAsync(new Edit_Product(selected));

                //}

            }

            catch (Exception ex)
            {
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }

        }
    }
}