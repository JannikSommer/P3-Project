using System;
using Android.Bluetooth;
using Java.Util;
using System.Threading.Tasks;
using Java.IO;
using SAScanApp.Droid;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Android;
using Android.App;
using Model;

[assembly: Xamarin.Forms.Dependency(typeof(BluetoothHandler))]
namespace SAScanApp.Droid
{
    public class BluetoothHandler : IBluetoothHandler
    {
        public string Barcode { get; set; }

        private CancellationTokenSource _ct { get; set; }
        private BluetoothDevice Device { get; set; } = null;
        private BluetoothAdapter Adapter { get; set; } = BluetoothAdapter.DefaultAdapter;
        private BluetoothSocket BluetoothSocket { get; set; } = null;


        public async Task EnableBluetooth()
        {
            string name = "CS3070";
            //Thread.Sleep(1000);
            _ct = new CancellationTokenSource();
            while (_ct.IsCancellationRequested == false)
            {
                try
                {
                    Adapter = BluetoothAdapter.DefaultAdapter;

                    if (Adapter == null)
                    {
                        //DisplayAlert("No Bluetooth adapter found.");
                    }

                    if (!Adapter.IsEnabled)
                    {
                        //DisplayAlert("Bluetooth adapter is not enabled.");
                    }

                    foreach (var bd in Adapter.BondedDevices)
                    {
                        if (bd.Name.ToUpper().IndexOf (name.ToUpper()) >= 0)
                        {
                            Device = bd;
                            break;
                        }
                    }

                    if (Device == null)
                    {
                        //DisplayAlert("Named device not found.");
                    }

                    else
                    {
                        UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");

                        if ((int)Android.OS.Build.VERSION.SdkInt >= 10) // Gingerbread 2.3.3 2.3.4
                            BluetoothSocket = Device.CreateInsecureRfcommSocketToServiceRecord(uuid);
                        else
                            BluetoothSocket = Device.CreateRfcommSocketToServiceRecord(uuid);

                        if (BluetoothSocket != null)
                        {
                            //Task.Run ((Func<Task>)loop); /*) => {
                            await BluetoothSocket.ConnectAsync();
                        }
                        else
                        {
                            //DisplayAlert("BthSocket = null");
                        }
                    }


                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("EXCEPTION: " + ex.Message);
                }
            }
        }

        public async void GetBarcode() { 

            if (BluetoothSocket.IsConnected) 
            { 
                var mReader = new InputStreamReader(BluetoothSocket.InputStream);
                var buffer = new BufferedReader(mReader);
                                
                while (_ct.IsCancellationRequested == false) {
                    if (buffer.Ready()) {
                        Barcode = buffer.ToString();
                    } else {                                                       
                        Barcode = await buffer.ReadLineAsync();
                    }            
                    if (Barcode.Length > 0) {
                        MessagingCenter.Send<App, string>((App) Xamarin.Forms.Application.Current, "Barcode", Barcode);
                    }
                }

                buffer.Dispose();

                if (!BluetoothSocket.IsConnected) {
                    //DisplayAlert("Scanner not connected");
                    throw new Exception();
                }
            }
        }

        /// <summary>
        /// Cancel the Reading loop
        /// </summary>
        /// <returns><c>true</c> if this instance cancel ; otherwise, <c>false</c>.</returns>
        public void Cancel()
        {
            if (_ct != null)
            {
                _ct.Cancel();
            }
        }



        public ObservableCollection<string> GetPairedDevices()
        {
            ObservableCollection<string> devices = new ObservableCollection<string>();

            foreach (var bd in Adapter.BondedDevices)
                devices.Add(bd.Name);

            return devices;
        }

        public void CloseBluetoothConnection() {
            if(BluetoothSocket != null) {
                BluetoothSocket.Close();
                Device = null;
                Adapter = null;
            }
        }
    }
}