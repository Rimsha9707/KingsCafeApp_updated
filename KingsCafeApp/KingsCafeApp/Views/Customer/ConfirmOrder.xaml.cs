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
using Xamarin.Forms.Xaml;
using Firebase.Database.Query;
using System.Text.RegularExpressions;

namespace KingsCafeApp.Views.Customer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmOrder : ContentPage
    {
        public ConfirmOrder()
        {
            InitializeComponent();
         
        }

        private async void btnConfirm_Clicked(object sender, EventArgs e)
        {
            LoadingInd.IsRunning = true;
            int LastID, NewID = 1;

            var LastRecord = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).FirstOrDefault();
            if (LastRecord != null)
            {
                LastID = (await App.firebaseDatabase.Child("Order").OnceAsync<Order>()).Max(a => a.Object.OrderID);
                NewID = ++LastID;
            }



            Order order = new Order()
            {
                OrderID = NewID,
                Name = txtName.Text,
                Email = txtEmail.Text,
                Address = txtAddress.Text,
                OrderDate = DateTime.Now.Date,
                OrderTime = DateTime.Now.TimeOfDay,
                Status = "Pending"
            };

            await App.firebaseDatabase.Child("Order").PostAsync(order);

            int LastID2, NewID2 = 1;
            var lastrecord2 = (await App.firebaseDatabase.Child("OrderDetail").OnceAsync<OrderDetail>()).FirstOrDefault();
            if (lastrecord2 != null)
            {
                LastID2 = (await App.firebaseDatabase.Child("OrderDetail").OnceAsync<OrderDetail>()).Max(a => a.Object.DetailsID);
                NewID2 = ++LastID2;
            }

            double total = 0;
            
            for (int i = 0; i < App.Cart.Count; i++)
            {
                var prod = (await App.firebaseDatabase.Child("FoodItem").OnceAsync<Models.FoodItem>()).FirstOrDefault(x => x.Object.ItemID == App.Cart[i].ItemFID);

                total = total + (App.Cart[i].Quantity * prod.Object.SalePrice);
                OrderDetail detail = new OrderDetail()
                {
                    OrderFID = NewID,
                    ItemFID = App.Cart[i].ItemFID,
                    Quantity = App.Cart[i].Quantity
                };

                await App.firebaseDatabase.Child("OrderDetail").PostAsync(detail);

            }


            await DisplayAlert("Success", "OrderSaved Successfully", "OK");

            //EMPTY CART======================================================================
            App.Cart=new List<OrderDetail>();


            LoadingInd.IsRunning = false;

        }

        //for validations 
        private void txtPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length < 11 || e.NewTextValue.Length > 13)
            {
                lblPhoneValidation.IsVisible = true;
                lblPhoneValidation.Text = "InValid Phone !! Missing digit(s) ";
                lblPhoneValidation.TextColor = Color.Red;
            }
            else
            {
                lblPhoneValidation.Text = "valid phone no.";
                lblPhoneValidation.TextColor = Color.Green;
            }
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            var EmailPattern = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
            //@ sign used to read special characters as a srting

            if (Regex.IsMatch(e.NewTextValue, EmailPattern))
            {
                lblEmailValidation.IsVisible = true;
                lblEmailValidation.Text = "Valid Email";
                lblEmailValidation.TextColor = Color.Green;
            }

            else
            {
                lblEmailValidation.Text = "InValid Email. Email must contain @ and .com ";
                lblEmailValidation.TextColor = Color.Red;
            }
        }
    }

}