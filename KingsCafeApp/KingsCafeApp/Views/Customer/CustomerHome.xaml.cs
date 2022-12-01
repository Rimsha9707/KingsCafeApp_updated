using System;
using KingsCafeApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Views.Customer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerHome : ContentPage
    {
        public CustomerHome()
        {
            InitializeComponent();
            LoadingInd.IsRunning = true;
            LoadData();
            this.BindingContext = this;
            LoadingInd.IsRunning = false;
        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            App.Current.MainPage = new StartPage();
        }

        async void LoadData()
        {
            var data = (await App.firebaseDatabase.Child("Category").OnceAsync<Category>()).Select(x => new Category
            {
                CatID = x.Object.CatID,
                Name = x.Object.Name,
                Image = x.Object.Image,
                Status=x.Object.Status
            }).ToList();
            DataList.ItemsSource = data;
        }
        private Timer timer;
        public List<Banner> Banners { get => GetBanners(); }

        private List<Banner> GetBanners()
        {
            var bannerList = new List<Banner>();
            bannerList.Add(new Banner { Heading = "Welcome To", Message = "King's Cafe", Caption = "Taste K Bhndaarrr", Image = "Slide.jpg" });
            bannerList.Add(new Banner { Heading = "King's Cafe", Message = "Taste Our Food", Caption = "Taste K Bhndaarrr", Image = "Slidetwo.jpg" });
            bannerList.Add(new Banner { Heading = "King's Cafe", Message = "Your Taste Is Our Goal", Caption = "Taste K Bhndaarrr", Image = "Slidethree.jpg" });
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
//========================navigation to products using collection view==================//
        private void DataList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.CurrentSelection.FirstOrDefault() as Category;
            Navigation.PushAsync(new Views.Customer.ProductList(item.CatID));
        }
    }

    public class Banner
    {
        public string Heading { get; set; }
        public string Message { get; set; }
        public string Caption { get; set; }
        public string Image { get; set; }
    }

    public class Product
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string Image { get; set; }
    }
}
    
