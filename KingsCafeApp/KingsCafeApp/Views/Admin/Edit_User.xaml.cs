using Firebase.Database.Query;
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
    public partial class Edit_User : ContentPage
    {
        public static User item = null;
        public static List<User> firebaseList = null;
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

                var OldUser = (await App.firebaseDatabase.Child("User").OnceAsync<User>()).FirstOrDefault(x => x.Object.UserID == item.UserID);

                OldUser.Object.UserID = item.UserID;
                OldUser.Object.Name = txtUserName.Text;
                OldUser.Object.Email = txtUserEmail.Text;
                OldUser.Object.Status = txtUserStatus.Text;
                OldUser.Object.Password = txtUserPassword.Text;
                OldUser.Object.Type = lblUserType.Text;


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

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ddlUserType.IsVisible = true;
        }

    }
}