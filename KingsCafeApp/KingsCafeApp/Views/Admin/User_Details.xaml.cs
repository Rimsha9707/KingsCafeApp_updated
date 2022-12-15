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
    public partial class User_Details : ContentPage
    {
        public User_Details(User u)
        {
            InitializeComponent();
            LoadingInd.IsRunning = true;
            txtUserName.Text = u.Name;
            txtUserImage.Source = u.Image;
            txtUserEmail.Text = u.Email;
            txtUserStatus.Text = u.Status;
            lblUserType.Text = u.Type;
            LoadingInd.IsRunning = false;
        }
    }
}