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
            this._startPage = startPage;
        }

        public void Entry_Completed(object sender, EventArgs e)
        {
            var usrNme = userLogin.Text;
            var pssWrd = userPassword.Text;
            bool adminLoggedIn = false;

            if (usrNme == "Admin" && pssWrd == "Admin")
            {
                // Her skal den sætte Admin = true, og logge ind på scanpage (lav polymorphism)
                DisplayAlert("Admin Login Correct", "Your password was correct", "Ok,");
                adminLoggedIn = true;
            }

            else
            {
                DisplayAlert("Error!", "Wrong password, please try again", "Ok");
            }

        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

      

    }
}