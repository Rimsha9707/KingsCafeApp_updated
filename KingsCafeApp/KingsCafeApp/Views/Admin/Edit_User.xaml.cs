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
    public partial class Edit_User : ContentPage
    {
        public static User item = null;
        public static List<User> firebaseList = null;
        public static bool IsNewPicSelected = false;
        private MediaFile _mediaFile;
        public static string PicPath = "category_picker.png";
        public Edit_User(User u)
        {
            InitializeComponent();
            LoadingInd.IsRunning = true;
            item = u;
            lblUserType.Text = u.Type;
            txtUserName.Text = u.Name;
            txtUserEmail.Text = u.Email;
            txtUserPassword.Text = u.Password;
            txtUserStatus.Text = u.Status;
            LoadingInd.IsRunning = false;
            PreviewPic.Source = u.Image;
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
        public async void btnUser_Clicked(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtUserEmail.Text) || string.IsNullOrEmpty(txtUserPassword.Text) || string.IsNullOrEmpty(txtUserStatus.Text))
                {
                    await DisplayAlert("Error", "Please fill required fields and try again", "Ok");
                    return;
                }
                if (ddlUserType.SelectedItem == null)
                {
                    await DisplayAlert("Error", "Please Select required Type and try again", "Ok");
                    return;
                }

                LoadingInd.IsRunning = true;
                string img = item.Image;

                if (IsNewPicSelected == true)
                {
                    var StoredImageURL = await App.FirebaseStorage
                    .Child("USER_PICTURE")
                    .Child(item.UserID.ToString() + "_" + txtUserName.Text + ".jpg")
                    .PutAsync(_mediaFile.GetStream());

                    img = StoredImageURL;

                }

                var OldUser = (await App.firebaseDatabase.Child("User").OnceAsync<User>()).FirstOrDefault(x => x.Object.UserID == item.UserID);

                OldUser.Object.UserID = item.UserID;
                OldUser.Object.Name = txtUserName.Text;
                OldUser.Object.Email = txtUserEmail.Text;
                OldUser.Object.Status = txtUserStatus.Text;
                OldUser.Object.Password = txtUserPassword.Text;
                OldUser.Object.Type = lblUserType.Text;
                OldUser.Object.Image = img;


                await App.firebaseDatabase.Child("User").Child(OldUser.Key).PutAsync(OldUser.Object);
                LoadingInd.IsRunning = false;
                await DisplayAlert("Success", "User Updated successfully", "Ok");
                await Navigation.PopAsync();

            }
            catch (Exception ex)
            {
                LoadingInd.IsRunning = false;
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }
        }

        private void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblUserType.Text=ddlUserType.SelectedItem.ToString();
        }

        private void TapGestureRecognizer_Tapped1(object sender, EventArgs e)
        {
            ddlUserType.IsVisible = true;
        }

    }
}