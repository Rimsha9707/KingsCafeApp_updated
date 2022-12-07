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
    public partial class Edit_Ingredient : ContentPage
    {

        private MediaFile _mediaFile;
        public static string PicPath= "Ingredient_picker.png";
        public static Ingredient Cat= null;
        public static bool IsNewPicSelected= false;
        public Edit_Ingredient(Ingredient c)
        {
            InitializeComponent();

            Cat = c;
            txtIngName.Text = c.Name;
            txtIngPrice.Text = c.Price.ToString();
            txtIngType.Text = c.Type;
            PreviewPic.Source = c.Image;
           
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
                await DisplayAlert("Message", "Something Went wrong \n" + ex.Message, "OK");

            }
        }

        private async void btnCat_Clicked(object sender, EventArgs e)
        {
            try
            {


                if (string.IsNullOrEmpty(txtIngName.Text)|| string.IsNullOrEmpty(txtIngPrice.Text)||string.IsNullOrEmpty(txtIngType.Text))
                {
                    await DisplayAlert("Error", "Please fill required fields and try again", "Ok");
                    return;
                }

                LoadingInd.IsRunning = true;

                string img = Cat.Image;
             
                if (IsNewPicSelected==true)
                {
                    var StoredImageURL = await App.FirebaseStorage
                    .Child("FOOD_INGREDIENTS_PICTURE")
                    .Child(Cat.IngredientID.ToString() + "_" + txtIngName.Text + ".jpg")
                    .PutAsync(_mediaFile.GetStream());

                    img = StoredImageURL;

                }


                var OldCat = (await App.firebaseDatabase.Child("Ingredient").OnceAsync<Ingredient>()).FirstOrDefault(x => x.Object.IngredientID == Cat.IngredientID);


                OldCat.Object.IngredientID = Cat.IngredientID;
                OldCat.Object.Name = txtIngName.Text;
                OldCat.Object.Type = txtIngType.Text;
                OldCat.Object.Price = (double)decimal.Parse(txtIngPrice.Text);
                OldCat.Object.Image = img;
     

                await App.firebaseDatabase.Child("Ingredient").Child(OldCat.Key).PutAsync(OldCat.Object);
                LoadingInd.IsRunning = false;
                await DisplayAlert("Success", "Ingredient Updated successfully", "Ok");
                await Navigation.PopAsync();

            }
            catch (Exception ex)
            {
                LoadingInd.IsRunning = false;
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }
        }

    }
}