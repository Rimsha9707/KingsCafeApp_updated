using KingsCafeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KingsCafeApp.Views.Workers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Worker_Profile : ContentPage
    {
        public Worker_Profile(User u)
        {
            try
            {
                InitializeComponent();
                LoadingInd.IsRunning = true;
                txtUserName.Text = u.Name;
                txtUserEmail.Text = u.Email;
                txtUserStatus.Text = u.Status;
                txtUserImage.Source = u.Image;
                lblUserType.Text = u.Type;
                LoadingInd.IsRunning = false;
                
            }
            catch (Exception ex)
            {
                LoadingInd.IsRunning = false;
                DisplayAlert("Error", "Something went wrong, please try again later. \nError: " + ex.Message, "Ok");
            }
            
        }

    }
}