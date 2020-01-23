using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SAScanApp
{
    public partial class App : Application
    {
        public string Path { get; set; } = Environment.CurrentDirectory + @"\UserData\username.txt";
        private string _userName { get; set; }
        public App()
        {
            InitializeComponent();

            

            

            if (!Environment.CurrentDirectory.Contains(Path))
            {
                MainPage = new NavigationPage(new EnterNamePage(this));
            }

            else
            {
                _userName = File.ReadAllText(Path);
                MainPage = new NavigationPage(new MenuDataHandlerPage(_userName, this));
            }

            
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
