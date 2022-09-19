using Firebase.Database.Query;
using KingsCafeApp.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        private MediaFile _mediaFile;
        public static string PicPath = "registration_pic.png";

        public Register()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {


                if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtPhone.Text) || string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Text))
                {
                    await DisplayAlert("Error", "Please fill required fields and try again", "Ok");
                    return;
                }

                if (txtPassword.Text != txtCPassword.Text)
                {
                    await DisplayAlert("Error", "Passwords do not match", "Ok");
                    return;
                }
                var check = (await App.firebaseDatabase.Child("Users").OnceAsync<User>()).FirstOrDefault(x => x.Object.Email == txtEmail.Text);

                if (check != null)
                {
                    await DisplayAlert("Error", "This Email is already registered", "ok");
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
               .Child("Picture")
               .Child(NewID + "_" + txtName.Text + ".jpg")
               .PutAsync(_mediaFile.GetStream());



                User u = new User()
                {
                    UserID = NewID,
                    Name = txtName.Text,
                    Email = txtEmail.Text,
                    Password = txtPassword.Text,
                };

                await App.firebaseDatabase.Child("Users").PostAsync(u);
                LoadingInd.IsRunning = false;

                await DisplayAlert("Success", "Your Account is Registered successfully", "Ok");
                await Navigation.PushAsync(new loginUser());

            }
            catch (Exception ex)
            {
                LoadingInd.IsRunning = false;
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }

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

        //for validations 
        private void txtPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length < 11 || e.NewTextValue.Length > 13)
            {
                lblPhoneValidation.IsVisible = true;
                lblPhoneValidation.Text = "InValid Phone !! Missing digit(s) ";
                lblPhoneValidation.TextColor = Color.Red;
            }
            else
            {
                lblPhoneValidation.Text = "valid phone no.";
                lblPhoneValidation.TextColor = Color.Green;
            }
        }

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
    }
}