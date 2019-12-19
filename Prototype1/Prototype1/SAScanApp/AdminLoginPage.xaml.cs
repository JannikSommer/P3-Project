using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SAScanApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminLoginPage : ContentPage
    {

        private MainPage _mainPage;
        private MenuStartPage _startPage;
        private EnterNamePage _namePage;
        public bool IsAdminLoggedIn { get; set; } = false;

        public AdminLoginPage()
        {
            InitializeComponent();
        }

        public AdminLoginPage(MainPage mainPage)
            : this()
        {
            _mainPage = mainPage;
        }

        public AdminLoginPage(MenuStartPage startPage)
            : this()
        {
            _startPage = startPage;
        }

        public AdminLoginPage(EnterNamePage namePage)
            : this()
        {
            _namePage = namePage;
        }

        public async void Entry_Completed(object sender, EventArgs e)
        {
            var usrNme = userLogin.Text;
            var pssWrd = userPassword.Text;
           
            if (usrNme == "Allan" && pssWrd == "AllansPassword")
            {
                IsAdminLoggedIn = true;

                await Navigation.PushAsync(new AdminPartitionSelection(this));
            }

            else
            {
                await DisplayAlert("Error!", "Wrong password, please try again", "Ok");
            }

        }    

    }
}