using Firebase.Database.Query;
using KingsCafeApp.Models;
using KingsCafeApp.Views.AWR;
using KingsCafeApp.Views.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Views.Riders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Rider_Side : TabbedPage
    {
        User R = new User();
        public Rider_Side(User u)
        {
            InitializeComponent();
            LoadData(u.Name);
            LoadData1(u.Name);
            R = u;
        }

        async void LoadData(string Name)
        {
            DataList.ItemsSource = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).Where(x=>x.Object.Name==Name && x.Object.Status== "Ready").Select(x => new Order
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
        async void LoadData1(string Name)
        {
            DataList.ItemsSource = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).Where(x=>x.Object.Name==Name && x.Object.Status== "Ready").Select(x => new Order
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
                var choice = await DisplayActionSheet("Options", "Close", "", "Deliverd", "Canceled");

                if (choice == "Deliverd")
                {
                    item.Object.Status = "Delivered";
                    await App.firebaseDatabase.Child("Order").Child(item.Key).PutAsync(item.Object);
                    await DisplayAlert("Message", item.Object.OrderID + "'s" + " order is Delivered now.", "Ok");
                }
                if (choice == "Canceled")
                {
                    item.Object.Status = "Canceled";
                    await App.firebaseDatabase.Child("Order").Child(item.Key).PutAsync(item.Object);
                    await DisplayAlert("Message", item.Object.OrderID + " order is Canceled now.", "Ok");
                }

            }

            catch (Exception ex)
            {


                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }
        }

        private void DataList1_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private async void Profile_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new User_Profile(R));
        }
    }
}