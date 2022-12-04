using KingsCafeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Views.Workers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Worker_Home : ContentPage
    {

        public Worker_Home()
        {
            InitializeComponent();
            LoadData();
        }
        async void LoadData()
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
                Status = x.Object.Status

            }).ToList();
        }
        private void btnStatus_Clicked(object sender, EventArgs e)
        {

        }

        private void DataList_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new StartPage();
        }
    }
}