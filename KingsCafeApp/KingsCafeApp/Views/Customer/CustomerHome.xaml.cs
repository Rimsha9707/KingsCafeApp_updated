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
            LoadData();
            this.BindingContext = this;
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

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
        //public List<> CollectionsList { get => GetCollections(); }
        public List<Category> TrendsList { get => GetTrends(); }

        private List<Banner> GetBanners()
        {
            var bannerList = new List<Banner>();
            bannerList.Add(new Banner { Heading = "King's Cafe", Message = "King's Cafe", Caption = "Taste K Bhndaarrr", Image = "Slide.jpg" });
            bannerList.Add(new Banner { Heading = "King's Cafe", Message = "Taste K Bhndaarrr", Caption = "Welcome to King's Cafe", Image = "Slidetwo.jpg" });
            bannerList.Add(new Banner { Heading = "King's Cafe", Message = "Your Taste Is Our Goal", Caption = "Taste K Bhndaarrr", Image = "Slidethree.jpg" });
            return bannerList;
        }

        private List<Category> GetTrends()
        {
            var colList = new List<Category>();
            colList.Add(new Category { Image = "heeledShoe.png", Name = "Beige Heeled Shoe", Status = "$109.99" });
            colList.Add(new Category { Image = "dressShoe.png", Name = "Shoe + Addons", Status = "$225.99" });
            return colList;
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

        private void DataList_ItemTapped(object sender, ItemTappedEventArgs e)
        {

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
    
