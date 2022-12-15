using Firebase.Database.Query;
using KingsCafeApp.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Add_User : ContentPage
    {
        private MediaFile _mediaFile;
        public static string PicPath = "category_picker.png";
        public Add_User()
        {
            InitializeComponent();
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


        //================for validations=====================
        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            var EmailPattern = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
            //@ sign used to read special characters as a srting

            if (Regex.IsMatch(e.NewTextValue, EmailPattern))
            {
                lblEmailValidation.IsVisible = true;
                lblEmailValidation.Text = "Valid Email";
                lblEmailValidation.TextColor = Color.Green;
            }

            else
            {
                lblEmailValidation.Text = "InValid Email. Email must contain @ and .com ";
                lblEmailValidation.TextColor = Color.Red;
            }
        }

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            var PasswordPattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])";
            //@ sign used to read special characters as a srting

            if (e.NewTextValue.Length < 8)
            {
                lblPasswordValidation.IsVisible = true;
                lblPasswordValidation.Text = "InValid Password. Your Password must be atleast 8 Characters ";
                lblPasswordValidation.TextColor = Color.Red;

            }


            if (Regex.IsMatch(e.NewTextValue, PasswordPattern))
            {
                lblPasswordValidation.IsVisible = true;
                lblPasswordValidation.Text = "Strong Password.";
                lblEmailValidation.TextColor = Color.Green;
            }


            else
            {

                lblPasswordValidation.Text = "InValid Password! Your Password should contain uppercase letters, lowercase letters,Number(s) and special character [$@$!%*#?&].";
                lblPasswordValidation.TextColor = Color.Red;
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
                    await DisplayAlert("Error", "Please Select required Category and try again", "Ok");
                    return;
                }

                LoadingInd.IsRunning = true;
                int LastID, NewID = 1;

                var LastRecord = (await App.firebaseDatabase.Child("User").OnceAsync<User>()).FirstOrDefault();
                if (LastRecord != null)
                {
                    LastID = (await App.firebaseDatabase.Child("User").OnceAsync<User>()).Max(a => a.Object.UserID);
                    NewID = ++LastID;
                }

                var StoredImageURL = await App.FirebaseStorage
                     .Child("USER_PICTURE")
                     .Child(NewID + "_" + txtUserName.Text + ".jpg")
                     .PutAsync(_mediaFile.GetStream());

                string Utype = ddlUserType.SelectedItem.ToString();
                User u = new User()
                {
                    UserID = NewID,
                    Name = txtUserName.Text,
                    Status = txtUserStatus.Text,
                    Password = txtUserPassword.Text,
                    Email = txtUserEmail.Text,
                    Type = Utype,
                    Image = StoredImageURL
                };

                await App.firebaseDatabase.Child("User").PostAsync(u);
                LoadingInd.IsRunning = false;
                await DisplayAlert("Success", "User is added successfully", "Ok");


            }
            catch (Exception ex)
            {
                LoadingInd.IsRunning = false;
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }
        }
    }
    }
