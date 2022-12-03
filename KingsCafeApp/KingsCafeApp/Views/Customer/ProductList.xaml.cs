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
    public partial class ProductList : ContentPage
    {
        public  ProductList(int id)
        {
           
            InitializeComponent();
            try
            {
                LoadingInd.IsRunning = true;
                LoadData(id);
                LoadData1();
                LoadingInd.IsRunning = false;
                this.BindingContext = this;
               
            }
            catch (Exception ex)
            {
                LoadingInd.IsRunning = false;
                 DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }

        }

        async void LoadData(int id)
        {
            var data = (await App.firebaseDatabase.Child("FoodItem").OnceAsync<FoodItem>()).Where(x => x.Object.CatFID == id).Select(x => new FoodItem
            {
                ItemID = x.Object.ItemID,
                Name = x.Object.Name,
                Image = x.Object.Image,
                SalePrice = x.Object.SalePrice,
            }).ToList();

            DataList.ItemsSource = data;
        }
        async void LoadData1()
        {
            DataList1.ItemsSource = (await App.firebaseDatabase.Child("Category").OnceAsync<Category>()).Select(x => new Category
            {
                CatID = x.Object.CatID,
                Name = x.Object.Name,
                Image = x.Object.Image,
            }).ToList();
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
        private Timer timer;
        public List<Banner3> Banners3 { get => GetBanners3(); }

        private List<Banner3> GetBanners3()
        {
            var bannerList = new List<Banner3>();
            bannerList.Add(new Banner3 { Image = "Image31.jpg" });
            bannerList.Add(new Banner3 { Image = "Image32.jpg" });
            bannerList.Add(new Banner3 { Image = "Image33.jpg" });
            return bannerList;
        }
        protected override void OnAppearing()
        {
            timer = new Timer(TimeSpan.FromSeconds(5).TotalMilliseconds) { AutoReset = true, Enabled = true };
            timer.Elapsed += Timer_Elapsed;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            timer?.Dispose();
            base.OnDisappearing();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {

                if (cvBanners.Position == 2)
                {
                    cvBanners.Position = 0;
                    return;
                }

                cvBanners.Position += 1;
            });
        }
        private void DataList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             
        }

        private void ToolbarItem_Clicked_2(object sender, EventArgs e)
        {

        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {

        }
    }
    public class Banner3
    {
        public string Image { get; set; }
    }

}