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
    public partial class Ingredient_Details : ContentPage
    {
        public Ingredient_Details(Ingredient i)
        {
            InitializeComponent();
            LoadingInd.IsRunning = true;
            txtIngName.Text = i.Name;
            txtIngPrice.Text = i.Price.ToString();
            txtIngType.Text = i.Type;
            image.Source = i.Image;
            LoadingInd.IsRunning = false;
        }
    }
}