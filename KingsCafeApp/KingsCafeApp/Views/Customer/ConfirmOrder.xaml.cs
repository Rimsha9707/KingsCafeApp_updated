using Plugin.Geolocator;
using KingsCafeApp.Models;
using Plugin.Geolocator.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;
using Firebase.Database.Query;

namespace KingsCafeApp.Views.Customer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmOrder : ContentPage
    {
        public static Xamarin.Forms.Maps.Position Pos = new Xamarin.Forms.Maps.Position();
        public ConfirmOrder()
        {
            InitializeComponent();
            GetLocationMap();
        }

        public async void GetLocationMap()
        {
            try
            {
                await DisplayAlert("Message", "GPS and Internet will be used to access your current location. Please confirm that you have enabled the GPS and Internet.", "Ok");

                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync();

                Xamarin.Forms.Maps.Position TempPos = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
                Pos = TempPos;

                MapView.Pins.Add(new Xamarin.Forms.Maps.Pin { Label = "Your Location", Position = Pos });

                MapView.MoveToRegion(Xamarin.Forms.Maps.MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Xamarin.Forms.Maps.Distance.FromMiles(1)));


            }
            catch (Exception ex)
            {
                await DisplayAlert("Message", "Somthing went wrong. This may be a problem with internet or application. Please ensure that you have a working internet connection and GPS enabled. \nError details : " + ex.Message, "Ok");
            }
        }
        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                //btnConfirm.IsVisible = true;
                //btnCurrentLocation.IsVisible = true;
                //btnProfileLocation.IsVisible = true;

                //txtNewAddress.IsVisible = true;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Message", "Somthing went wrong this may be a problem with internet or application please ensure that you have a working internet connection and GPS enabled. \nError Details : " + ex.Message, "OK");
            }
        }

        private void btnFetch_Clicked_1(object sender, EventArgs e)
        {
            GetLocationMap();
        }

        private async void btncurrentlocation_clicked_1(object sender, EventArgs e)
        {

        }

        private async void btnprofilelocation_clicked(object sender, EventArgs e)
        {
            try
            {

                // creating new id for order ==================================

                int lastid, newid = 1;
                var lastrecord = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).FirstOrDefault();
                if (lastrecord != null)
                {
                    lastid = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).Max(a => a.Object.OrderID);
                    newid = ++lastid;
                }

                // creating new order object for order ==================================

                //Order order = new Order()
                //{
                //    ORDER_ID = newid,
                //    ORDER_DATE = DateTime.Now.Date,
                //    //ordertime = datetime.now.timeofday,
                //    ORDER_STATUS = "pending",
                //    //loctype = "profile",
                //    ORDER_CUSTOMER_ADDRESS = txtNewAddress.Text,
                //    ORDER_CUSTOMER_NAME = txtName.Text,
                //    //userid = app.loggedinuser.userid,
                //};


                // psting order to firebase ==================================


               // await App.firebaseDatabase.Child("Order").PostAsync(order);



                // creating new id for order details ==================================

                int lastid2, newid2 = 1;
                var lastrecord2 = (await App.firebaseDatabase.Child("OrderDetail").OnceAsync<OrderDetail>()).FirstOrDefault();
                if (lastrecord2 != null)
                {
                    lastid2 = (await App.firebaseDatabase.Child("OrderDetail").OnceAsync<OrderDetail>()).Max(a => a.Object.DetailsID);
                    newid2 = ++lastid2;
                }



                foreach (var item in App.Cart)
                {
                    item.DetailsID = newid2;
                    item.OrderFID = newid;

                    await App.firebaseDatabase.Child("OrderDetail").PostAsync(item);

                }


                App.Cart.Clear();

                Helpers.EmailHelper emailHelper = new Helpers.EmailHelper();

                //emailhelper.emailsender(App.loggedinuser.email, "order confirmed", "order confirmed");
                // emailhelper.messagesender(app.loggedinuser.phone,  "order confirmed");



                App.Current.MainPage = new NavigationPage(new Views.Customer.OrderConfirmation(newid));

            }
            catch (Exception ex)
            {
                await DisplayAlert("message", "somthing went wrong. this may be a problem with internet or application. please ensure that you have a working internet connection and gps enabled. \nerror details : " + ex.Message, "ok");
            }
        }

        private async void btnnewaddress_clicked_1(object sender, EventArgs e)
        {

        }

        private async void btnconfirm_clicked(object sender, EventArgs e)
        {

        }

    }

}