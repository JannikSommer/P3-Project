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

[assembly: Xamarin.Forms.Dependency(typeof(BluetoothHandler))]
namespace SAScanApp.Droid
{
    public class BluetoothHandler : IBluetoothHandler
    {

        private CancellationTokenSource _ct { get; set; }

        public async Task enableBluetooth()
        {
            BluetoothDevice device = null;
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            BluetoothSocket BluetoothSocket = null;

            //Thread.Sleep(1000);
            _ct = new CancellationTokenSource();
            while (_ct.IsCancellationRequested == false)
            {
                try
                {
                    adapter = BluetoothAdapter.DefaultAdapter;

                    if (adapter == null)
                    {
                        //DisplayAlert("No Bluetooth adapter found.");
                    }

                    if (!adapter.IsEnabled)
                    {
                        //DisplayAlert("Bluetooth adapter is not enabled.");
                    }

                    foreach (var bd in adapter.BondedDevices)
                    {

                        if (bd.Name.Contains("3070"))
                        {
                            device = bd;
                        }
                    }

                    if (device == null)
                    {
                        //DisplayAlert("Named device not found.");
                    }

                    else
                    {
                        UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");

                        if ((int)Android.OS.Build.VERSION.SdkInt >= 10) // Gingerbread 2.3.3 2.3.4
                            BluetoothSocket = device.CreateInsecureRfcommSocketToServiceRecord(uuid);
                        else
                            BluetoothSocket = device.CreateRfcommSocketToServiceRecord(uuid);

                        if (BluetoothSocket != null)
                        {
                            //Task.Run ((Func<Task>)loop); /*) => {
                            await BluetoothSocket.ConnectAsync();


                            if (BluetoothSocket.IsConnected)
                            {
                                string barcode = null;
                                var mReader = new InputStreamReader(BluetoothSocket.InputStream);
                                var buffer = new BufferedReader(mReader);
                                
                                while (_ct.IsCancellationRequested == false)
                                {
                                    if (buffer.Ready())
                                    {
                                        barcode = buffer.ToString();
                                    }

                                    else
                                    {
                                                                                
                                        barcode = await buffer.ReadLineAsync();
                                    }
                                    
                                    if (barcode.Length > 0)
                                    {

                                        MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "Barcode", barcode);
                                    }

                                }

                                if (!BluetoothSocket.IsConnected)
                                {
                                    //DisplayAlert("Scanner not connected");
                                    throw new Exception();
                                }
                            }

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

                finally
                {
                    if (BluetoothSocket != null)
                        BluetoothSocket.Close();
                    device = null;
                    adapter = null;
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
                System.Diagnostics.Debug.WriteLine("Send a cancel to task!");
                _ct.Cancel();
            }
        }



        public ObservableCollection<string> getPairedDevices()
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            ObservableCollection<string> devices = new ObservableCollection<string>();

            foreach (var bd in adapter.BondedDevices)
                devices.Add(bd.Name);

            return devices;
        }



    }
}