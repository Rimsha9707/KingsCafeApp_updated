using KingsCafeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Views.Workers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Worker_Side : TabbedPage
    {
        User p = new User();
        public Worker_Side()
        {
            InitializeComponent();
            //p = u;            //p = u;
        }

        private async void Profile_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Worker_Profile(p));
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new StartPage();
        }
    }
}