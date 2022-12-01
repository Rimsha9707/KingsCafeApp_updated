using System;
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
    public partial class Menu : ContentPage
    {
        public Menu()
        {
            InitializeComponent();
            this.BindingContext = this;
            
        }
            private Timer timer;
        public List<Banner2> Banners2 { get => GetBanners2(); }

        private List<Banner2> GetBanners2()
        {
            var bannerList = new List<Banner2>();
            bannerList.Add(new Banner2 { Image = "menu1.jpeg" });
            bannerList.Add(new Banner2 { Image = "menu2.jpeg" });
            bannerList.Add(new Banner2 { Image = "menu3.jpeg" });
            bannerList.Add(new Banner2 { Image = "menu4.jpeg" });
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

        private void DataList_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }

    public class Banner2
    {
        public string Image { get; set; }
    }

}

