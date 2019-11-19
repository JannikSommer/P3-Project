using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace Prototype1 {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ZXingScannerPage {
        public Page1() {
            InitializeComponent();
        }

        public void Handle_OnScanResult(Result result) {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Scanned result", result.Text, "OK");
            });
        }

    }
}