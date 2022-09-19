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
    public partial class splashScreen : ContentPage
    {
        public splashScreen()
        {
            InitializeComponent();
            Animation();

        }

        public async void Animation()
        {
            SplashImage.Opacity = 0;

            await Task.WhenAll(
                SplashImage.FadeTo(100,3000),
                SplashImage.ScaleTo(1.3,3000)
                );

            App.Current.MainPage = new StartPage();
        }
    }

}