using KingsCafeApp.Models;
using KingsCafeApp.ViewModels;
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
            UpdateCartAsync();

            LoadingInd.IsRunning = false;
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

            //==========================EMPTY CART========================================================
            App.Cart=new List<OrderDetail>();

        }
        private async void LoadData(int Id)
        {
            try
            {
            //=========================Customer Detail===================================
            var order = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).FirstOrDefault(x => x.Object.OrderID == id);
            lblCustomer.Text = order.Object.Name;
            lblPhone.Text = order.Object.Phone;
            lblEmail.Text = order.Object.Email;
            lblAddress.Text = order.Object.Address;
            lblDate.Text = order.Object.OrderDate.ToShortDateString();
            lblID.Text = "KC202200-" + order.Object.OrderID.ToString();

            await DisplayAlert("Message", "Order is placed successfully. It will be delivered soon order details are as under", "Ok");
            return;

            }
            catch (Exception ex)
            {
                await DisplayAlert("message", "somthing went wrong. This may be a problem with internet or application. Please ensure that you have a working internet connection and gps enabled. \nerror details : " + ex.Message, "ok");
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}