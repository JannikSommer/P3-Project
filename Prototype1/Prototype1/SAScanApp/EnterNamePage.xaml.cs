using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SAScanApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterNamePage : ContentPage
    {
        public string UserName { get; set; }
        private App _appPage { get; set; }

        public EnterNamePage()
        {
            InitializeComponent();
        }

        public EnterNamePage(App appPage) : this()
        {
            _appPage = appPage;
            
        }

        private async void Name_Entered(object sender, EventArgs e)
        {
            //var path = Path.Combine(Environment.CurrentDirectory, @"\UserData\UserName.txt");
            //DirectorySecurity securityRules = new DirectorySecurity();
            //securityRules.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));

            //DirectoryInfo newDir = Directory.CreateDirectory(path, securityRules);

            //var UserName = NameEntry.Text;
            //File.WriteAllText(UserName, path);

            await Navigation.PushAsync(new ScanPage(this));
        }

        private async void AdminLogin_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminLoginPage(this));
        }
    }
}