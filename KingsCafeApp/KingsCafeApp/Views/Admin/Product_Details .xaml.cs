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
        public static List<Category> firebaseList = null;
        public Product_Details(FoodItem p)
        {
            InitializeComponent();
            LoadingInd.IsRunning = true;
            txtProName.Text = p.Name;
            txtProImage.Source = p.Image;
            txtProPrice.Text = p.SalePrice.ToString();
            int id = p.CatFID;
            LoadData(id);
            LoadingInd.IsRunning = false;
        }
         async void LoadData(int id)
        {
            firebaseList = (await App.firebaseDatabase.Child("Category").OnceAsync<Category>()).Where(x=>x.Object.CatID==id).Select(x => new Category
            {
                CatID = x.Object.CatID,
                Name = x.Object.Name,
                Image = x.Object.Image,
            }).ToList();

            string refinedList = firebaseList.Select(x => x.Name).FirstOrDefault();
            lblCategory.Text = refinedList;
        }
    }
}