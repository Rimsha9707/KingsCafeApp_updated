using Firebase.Database.Query;
using KingsCafeApp.Models;
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
        public Add_User()
        {
            InitializeComponent();
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

                //var check = (await App.firebaseDatabase.Child("User").OnceAsync<User>()).FirstOrDefault(x => x.Object.Name == txtUserName.Text);

                //if (check != null)
                //{
                //    await DisplayAlert("Error", check.Object.Name + " is already added", "ok");
                //    return;

                //}

                LoadingInd.IsRunning = true;
                int LastID, NewID = 1;

                var LastRecord = (await App.firebaseDatabase.Child("User").OnceAsync<User>()).FirstOrDefault();
                if (LastRecord != null)
                {
                    LastID = (await App.firebaseDatabase.Child("User").OnceAsync<User>()).Max(a => a.Object.UserID);
                    NewID = ++LastID;
                }
                string Utype = ddlUserType.SelectedItem.ToString();
                User u = new User()
                {
                    UserID = NewID,
                    Name = txtUserName.Text,
                    Status = txtUserStatus.Text,
                    Password = txtUserPassword.Text,
                    Email = txtUserEmail.Text,
                    Type = Utype,
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
