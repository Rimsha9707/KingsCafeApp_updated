using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using KingsCafeApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Views.Customer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Customization : ContentPage
    {
        public Customization()
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

        }

        async void LoadData()
        {
            var data = (await App.firebaseDatabase.Child("Ingredient").OnceAsync<Ingredient>()).Select(x => new Ingredient
            {
                IngredientID = x.Object.IngredientID,
                Name = x.Object.Name,
                Image = x.Object.Image,
                Price = x.Object.Price,
                Type = x.Object.Type,
            }).ToList();

            DataList.ItemsSource = data;
        }
      

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new StartPage();
        }

        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.Customer.CartPage());
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            var item = btn.CommandParameter as Models.FoodItem;


            int Quantity = 0;
            var QtyRaw = await DisplayActionSheet("Select quantity", "Close", "", "1", "2", "3", "4", "5", "6", "7", "10", "15");
            if (QtyRaw != "Close" && QtyRaw != null)
            {
                Quantity = int.Parse(QtyRaw);
            }
            else
            {
                await DisplayAlert("Message", "Please select at least 1 quantity.", "Ok");
                return;
            }

            int index = -1;
            for (int i = 0; i < App.Cart.Count; i++)
            {
                if (item.ItemID == App.Cart[i].ItemFID)
                {
                    index = i;
                    var ques = await DisplayAlert("Message", item.Name + " is already entered into cart. Do you want to update the quantity of already entered item?", "Yes", "No");
                    if (ques)
                    {
                        App.Cart[index].Quantity += Quantity;
                        await DisplayAlert("Message", item.Name + " quantity updated successfully... ", "Ok");

                    }
                }
            }

            if (index == -1)
            {
                App.Cart.Add(new OrderDetail { ItemFID = item.ItemID, Quantity = Quantity });
                await DisplayAlert("Message", item.Name + " is added into cart... ", "Ok");

            }
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            var item = btn.CommandParameter as Models.FoodItem;
            await DisplayAlert("Category details :",
                "Name : " + item.Name +
                "\n Sale Price : " + item.SalePrice,
                "Ok"
                );
        }
       
        
        private void DataList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             
        }

        //private void DataList1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var item = e.CurrentSelection.FirstOrDefault() as Category;
        //    LoadData(item.CatID);
        //}
    }
   

}