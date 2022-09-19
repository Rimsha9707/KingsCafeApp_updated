using Firebase.Database.Query;
using KingsCafeApp.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
    public partial class Edit_Product : ContentPage
    {

        private MediaFile _mediaFile;
        public static string PicPath = "category_picker.png";
        public static FoodItem item = null;
        public static bool IsNewPicSelected = false;
        public static List<Category> firebaseList = null;
        public Edit_Product(FoodItem p)
        {
            InitializeComponent();
            LoadingInd.IsRunning = true;
            LoadData();
            item = p;
            lblCategory.Text = firebaseList.Where(x => x.CatID == p.CatFID).FirstOrDefault().Name;
            txtProName.Text = p.Name;
            txtProPrice.Text = p.SalePrice.ToString();
            PreviewPic.Source = p.Image;
            LoadingInd.IsRunning = false;
        }

        async void LoadData()
        {
          
            firebaseList = (await App.firebaseDatabase.Child("Category").OnceAsync<Category>()).Select(x => new Category
            {
                CatID = x.Object.CatID,
                Name = x.Object.Name,
                Image = x.Object.Image,
            }).ToList();

            var refinedList = firebaseList.Select(x => x.Name).ToList();
            ddlCat.ItemsSource = refinedList;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                var response = await DisplayActionSheet("Select Image Source", "Close", "", "From Gallery", "From Camera");
                if (response == "From Camera")
                {
                    await CrossMedia.Current.Initialize();
                    if (!CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await DisplayAlert("Error", "Phone is not Take Photo Supported", "OK");
                        return;
                    }

                    var mediaOptions = new StoreCameraMediaOptions()
                    {
                        PhotoSize = PhotoSize.Medium
                    };

                    var SelectedImg = await CrossMedia.Current.TakePhotoAsync(mediaOptions);

                    if (SelectedImg == null)
                    {
                        await DisplayAlert("Error", "Error Taking Image...", "OK");
                        return;
                    }

                    _mediaFile = SelectedImg;
                    PicPath = SelectedImg.Path;
                    PreviewPic.Source = SelectedImg.Path;


                }
                if (response == "From Gallery")
                {
                    await CrossMedia.Current.Initialize();
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await DisplayAlert("Error", "Phone is not Pick Photo Supported", "OK");
                        return;
                    }

                    var mediaOptions = new PickMediaOptions()
                    {
                        PhotoSize = PhotoSize.Medium
                    };

                    var SelectedImg = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

                    if (SelectedImg == null)
                    {
                        await DisplayAlert("Error", "Error Picking Image...", "OK");
                        return;
                    }

                    _mediaFile = SelectedImg;
                    PicPath = SelectedImg.Path;
                    PreviewPic.Source = SelectedImg.Path;


                }
                IsNewPicSelected = true;
            }
            catch (Exception ex)
            {
                LoadingInd.IsRunning = false;
                await DisplayAlert("Message", "Something Went wrong \n" + ex.Message, "OK");

            }
        }

        private async void btnPro_Clicked(object sender, EventArgs e)
        {
            try
            {


                if (string.IsNullOrEmpty(txtProName.Text) || string.IsNullOrEmpty(txtProPrice.Text))
                {
                    await DisplayAlert("Error", "Please fill required fields and try again", "Ok");
                    return;
                }
                if (ddlCat.SelectedItem == null)
                {
                    await DisplayAlert("Error", "Please Select required Category and try again", "Ok");
                    return;
                }

                LoadingInd.IsRunning = true;

                string img = item.Image;

                if (IsNewPicSelected == true)
                {
                    var StoredImageURL = await App.FirebaseStorage
                    .Child("FOOD_CATEGORIES_PICTURE")
                    .Child(item.ItemID.ToString() + "_" + txtProName.Text + ".jpg")
                    .PutAsync(_mediaFile.GetStream());

                    img = StoredImageURL;

                }

                var SelectedCatID = firebaseList.Where(x => x.Name == lblCategory.Text).FirstOrDefault().CatID;

                var OldProduct = (await App.firebaseDatabase.Child("FoodItem").OnceAsync<FoodItem>()).FirstOrDefault(x => x.Object.ItemID == item.ItemID);

                OldProduct.Object.ItemID = item.ItemID;
                OldProduct.Object.Name = txtProName.Text;
                OldProduct.Object.CatFID = SelectedCatID;
                OldProduct.Object.Image = img;


                await App.firebaseDatabase.Child("FoodItem").Child(OldProduct.Key).PutAsync(OldProduct.Object);
                LoadingInd.IsRunning = false;
                await DisplayAlert("Success", "Product Updated successfully", "Ok");
                await Navigation.PopAsync();

            }
            catch (Exception ex)
            {
                LoadingInd.IsRunning = false;
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }
        }

        private void ddlCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCategory.Text = ddlCat.SelectedItem.ToString();
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            ddlCat.IsVisible = true;
        }
    }
}