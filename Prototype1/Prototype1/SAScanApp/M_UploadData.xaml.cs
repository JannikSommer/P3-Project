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
    public partial class M_UploadData : ContentPage
    {
        private M_StartPage _mStartPage;
        public M_UploadData(M_StartPage mStartPage)
        {
            InitializeComponent();
            _mStartPage = mStartPage;
        }
    }
}