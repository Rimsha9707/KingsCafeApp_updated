using KingsCafeApp.LoginSystem;
using KingsCafeApp.Models;
using KingsCafeApp.Views.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class loginUser : ContentPage
    {
        public loginUser()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {


                if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Text))
                {
                    await DisplayAlert("Error", "Please fill required fields and try again", "Ok");
                    return;
                }
                LoadingInd.IsRunning = true;
                var check = (await App.firebaseDatabase.Child("User").OnceAsync<User>()).FirstOrDefault(x => x.Object.Email == txtEmail.Text && x.Object.Password == txtPassword.Text);

                if (check == null)
                {
                    
                    await DisplayAlert("Error", "Invalid Email or Password is incorrect", "ok");
                    LoadingInd.IsRunning = false;
                    return;
                }
                else
                {
                    //await Navigation.PushAsync(new Userlist());

                    App.Current.MainPage = new AdminSidebar();
                    LoadingInd.IsRunning = false;
                }
            }
            catch (Exception ex)
            {
                LoadingInd.IsRunning = false;
                await DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");

            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Register());
        }
    }
}