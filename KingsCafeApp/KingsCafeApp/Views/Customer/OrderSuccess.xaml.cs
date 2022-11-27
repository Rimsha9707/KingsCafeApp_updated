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
            var OrderDetails = (await App.firebaseDatabase.Child("OrderDetail").OnceAsync<OrderDetail>()).Where(x => x.Object.OrderFID == id).ToList();
            List<CartItem> Cart = new List<CartItem>();

            foreach (var item in OrderDetails)
            {
                var food = (await App.firebaseDatabase.Child("FoodItem").OnceAsync<FoodItem>()).FirstOrDefault(x => x.Object.ItemID == item.Object.ItemFID);
                Cart.Add(new CartItem { foodItem = food.Object, quantity = item.Object.Quantity });
            }

            ViewBag.Items = Cart;
            var order = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).FirstOrDefault(x => x.Object.OrderID == id);

           
            await DisplayAlert("Message", "Order has confirmed.It will be delivered soon order details are as under", "Ok");
            return;
           

            //var order = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).FirstOrDefault(x => x.Object.OrderID == Id);

            //lblCustomer.Text = order.Object.Name;
            //lblDate.Text = order.Object.OrderDate.ToShortDateString();
            //lblID.Text = "KC202200-" + order.Object.OrderID.ToString();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}