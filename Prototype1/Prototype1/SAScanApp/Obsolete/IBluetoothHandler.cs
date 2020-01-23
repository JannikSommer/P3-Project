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
        void Start();
        void Cancel();
        ObservableCollection<string> PairedDevices();

    }
}
