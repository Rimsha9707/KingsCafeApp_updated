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
    public partial class CategoryDetails : ContentPage
    {
        public CategoryDetails(Category c)
        {
            InitializeComponent();
            LoadingInd.IsRunning = true;
            FOOD_CATEGORIES_NAME.Text = c.Name;
            FOOD_CATEGORIES_PICTURE.Source = c.Image;
            LoadingInd.IsRunning = false;
        }
    }
}