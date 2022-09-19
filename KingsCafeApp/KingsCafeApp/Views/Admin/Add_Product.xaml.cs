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
    public partial class Add_Product : ContentPage
    {

        private MediaFile _mediaFile;
        public static string PicPath= "category_picker.png";
        public Add_Product()
        {
            InitializeComponent();
            LoadData();
        }

        async void LoadData()
        {
            var firebaseList = (await App.firebaseDatabase.Child("Category").OnceAsync<Category>()).Select(x => new Category
            {
                Name = x.Object.Name,
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
            }
            catch (Exception ex)
            {
                await DisplayAlert("Message", "Something Went wrong \n" + ex.Message, "OK");

            }
        }

        private async void btnPro_Clicked(object sender, EventArgs e)
        {
            try
            {


                if (string.IsNullOrEmpty(txtProName.Text)|| string.IsNullOrEmpty(txtProPrice.Text))
                {
                    await DisplayAlert("Error", "Please fill required fields and try again", "Ok");
                    return;
                }
                if (ddlCat.SelectedItem == null)
                {
                    await DisplayAlert("Error", "Please Select required Category and try again", "Ok");
                    return;
                }

                var check = (await App.firebaseDatabase.Child("FoodItem").OnceAsync<FoodItem>()).FirstOrDefault(x => x.Object.Name ==txtProName.Text);

                if (check != null)
                {
                    await DisplayAlert("Error",check.Object.Name + " is already added", "ok");
                    return;

                }

                LoadingInd.IsRunning = true;
                int LastID, NewID = 1;

                var LastRecord = (await App.firebaseDatabase.Child("FoodItem").OnceAsync<FoodItem>()).FirstOrDefault();
                if (LastRecord != null)
                {
                    LastID = (await App.firebaseDatabase.Child("FoodItem").OnceAsync<FoodItem>()).Max(a => a.Object.ItemID);
                    NewID = ++LastID;
                }

                List<Category> cats = (await App.firebaseDatabase.Child("Category").OnceAsync<Category>()).Select(x => new Category
                {
                    CatID = x.Object.CatID,
                    Name = x.Object.Name,
                }).ToList();

                int selected = cats[ddlCat.SelectedIndex].CatID;

                var StoredImageURL = await App.FirebaseStorage
                    .Child("FOOD_PRODUCT_PICTURE")
                    .Child(NewID+ "_" + txtProName.Text + ".jpg")
                    .PutAsync(_mediaFile.GetStream());



                FoodItem c = new FoodItem()
                {  
                    ItemID= NewID,
                    Name = txtProName.Text,
                    SalePrice = (double)decimal.Parse( txtProPrice.Text),//type casting 
                    Image= StoredImageURL,
                    CatFID=selected,
        };

                await App.firebaseDatabase.Child("FoodItem").PostAsync(c);
                LoadingInd.IsRunning = false;
                await DisplayAlert("Success", "Product is added successfully", "Ok");
              

            }
            catch (Exception ex)
            {
                LoadingInd.IsRunning = false;
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }
        }
    }
}