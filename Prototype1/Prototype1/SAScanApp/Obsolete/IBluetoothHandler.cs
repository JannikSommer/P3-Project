using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SAScanApp
{
    public interface IBluetoothHandler
    {
        Task EnableBluetooth();
        void CloseBluetoothConnection();
        void GetBarcode();
        void Cancel();
        ObservableCollection<string> GetPairedDevices();

    }
}
