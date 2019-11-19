﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace Prototype1 {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : ContentPage {
        public Page2() {
            InitializeComponent();
            ScanView.Options.TryHarder = true;
            ScanView.Options.PossibleFormats.Add(BarcodeFormat.EAN_13);
            ScanView.Options.PossibleFormats.Add(BarcodeFormat.UPC_A);
            ScanView.Options.TryHarder = true;

        }


        public void Handle_OnScanResult(Result result) {
            Device.BeginInvokeOnMainThread(async () => {
                await DisplayAlert("Scanned result", result.Text, "OK");
            });
        }

        private void SwitchStateChanged(object sender, PropertyChangedEventArgs e) {
            if(Switch.IsToggled) {
                ScanView.Options.PossibleFormats.Clear();
                ScanView.Options.PossibleFormats.Add(BarcodeFormat.CODE_39);
            } else {
                ScanView.Options.PossibleFormats.Clear();
                ScanView.Options.PossibleFormats.Add(BarcodeFormat.EAN_13);
                ScanView.Options.PossibleFormats.Add(BarcodeFormat.UPC_A);
            }

        }
    }
}