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
    public partial class RiderList : ContentPage
    {
        public static Order o = new Order();
        public RiderList(Order order)
        {

            InitializeComponent();
            try
            {
                LoadingInd.IsRunning = true;
                LoadData();
                LoadingInd.IsRunning = false;
            }
            catch (Exception ex)
            {
                LoadingInd.IsRunning = false;
                DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }
            o = order;

        }

        async void LoadData()
        {
            DataList.ItemsSource = (await App.firebaseDatabase.Child("User").OnceAsync<User>()).Where(x=>x.Object.Type=="Rider").Select(x => new User
            {
                UserID = x.Object.UserID,
                Name = x.Object.Name,
                Email = x.Object.Email,
                Password = x.Object.Password,
                Type = x.Object.Type,
                Status = x.Object.Status,
            }).ToList();
        }

        
        private async void DataList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var selected = e.Item as User;
                var orderToAssign = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).FirstOrDefault(a => a.Object.OrderID == o.OrderID);
                var choice = await DisplayActionSheet("Options", "Close", "Save","Assign Order");

                if (choice == "Assign Order")
                {
                    orderToAssign.Object.AssignedRider = selected.Name;
                    o.Status = "Assigned";
                    await App.firebaseDatabase.Child("Order").Child(orderToAssign.Key).PutAsync(orderToAssign.Object);

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