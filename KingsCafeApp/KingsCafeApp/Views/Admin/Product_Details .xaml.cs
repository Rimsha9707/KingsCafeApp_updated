using KingsCafeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Product_Details : ContentPage
    {
        public Product_Details(FoodItem p)
        {
            InitializeComponent();
            LoadingInd.IsRunning = true;
            txtProName.Text = p.Name;
            txtProImage.Source = p.Image;
            txtProPrice.Text = p.SalePrice.ToString();
            LoadingInd.IsRunning = false;
        }
    }
}