using Firebase.Database;
using Firebase.Storage;
using KingsCafeApp.Models;
using KingsCafeApp.Login;
using KingsCafeApp.Views;
using KingsCafeApp.Views.Admin;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using KingsCafeApp.Views.Workers;

namespace KingsCafeApp
{
    public partial class App : Application
    {
        //public static string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "dbKingsCafeApp.db3");
        //public static SQLiteConnection db = new SQLiteConnection(dbPath);

        //Firebase Connections  ======================================
        public static FirebaseStorage FirebaseStorage = new FirebaseStorage("kingscafeapp.appspot.com");

        public static FirebaseClient firebaseDatabase = new FirebaseClient("https://kingscafeapp-default-rtdb.firebaseio.com/");

        public static List<OrderDetail> Cart = new List<OrderDetail>();

        public static decimal? Total = null;

        public App()
        {
            InitializeComponent();

            MainPage = new splashScreen();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }
         
        protected override void OnResume()
        {
        }
    }
}
