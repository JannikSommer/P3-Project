using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SAScanApp
{
    public partial class App : Application
    {
        public string Path { get; set; } = Environment.CurrentDirectory + @"\UserData\Username.txt";
        public App()
        {
            InitializeComponent();
            if (File.Exists(Path)){
                MainPage = new NavigationPage(new MainPage());
            }

            else
            {
                MainPage = new NavigationPage(new EnterNamePage(this));
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
