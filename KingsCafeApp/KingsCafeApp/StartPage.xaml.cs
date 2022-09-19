using KingsCafeApp.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
            Animation();
        }

        //animation for logo
        public async void Animation()
        {
           
            await image.RotateTo(360, 10000);
            image.Rotation = 0;
        }


        private async void btnGetStarted_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new Views.Customer.CustomerSidebar();
            //moving from one to another without navigation
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new loginUser());
        }

    }
}