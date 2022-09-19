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
    public partial class OrderConfirmation : ContentPage
    {
        public static int id;
        public OrderConfirmation(int OrderID)
        {
            InitializeComponent();
            LoadingInd.IsRunning = true;
            id = OrderID;
            LoadData(OrderID);
            LoadingInd.IsRunning = false;
        }
        private async void LoadData(int Id)
        {
            var order = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).FirstOrDefault(x => x.Object.OrderID == Id);

            lblCustomer.Text = order.Object.Name;
            lblDate.Text = order.Object.OrderDate.ToShortDateString();
            lblID.Text = "KC202200-" + order.Object.OrderID.ToString();
        }
    }
}