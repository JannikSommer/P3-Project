using System;
using Android.Bluetooth;
using Java.Util;
using System.Threading.Tasks;
using Java.IO;
using SAScanApp.Droid;
using System.Threading;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Linq;
using Android.Content;
using Android.App;

[assembly: Xamarin.Forms.Dependency(typeof(BluetoothHandler))]
namespace SAScanApp.Droid
{
    public class BluetoothHandler : IBluetoothHandler
    {
        public string Barcode { get; set; }
        private BluetoothDevice Device { get; set; } = null;
        private BluetoothAdapter Adapter { get; set; }
        private BluetoothSocket Socket { get; set; } = null;
        private char InputData { get; set; }
        private CancellationTokenSource _cancelToken { get; set; }

        public void Start()
        {
            Task.Run(async () => InitializeBluetooth());
        }

        public async Task InitializeBluetooth()
        {
            try
            {
                Adapter = BluetoothAdapter.DefaultAdapter;

                if (Adapter == null)
                {
                    throw new Exception("Bluetooth is not supported by this device, sorry");
                }

                if (!Adapter.IsEnabled)
                {
                    throw new Exception("Please enable bluetooth on the device");
                }

                Device = (from bd in Adapter.BondedDevices
                          where bd.Name == "CS3070"
                          select bd).FirstOrDefault();

                if (Device == null)
                {
                    throw new Exception("Named device not found");
                }

                else
                {

                    Socket = Device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));

                    await Socket.ConnectAsync();
                }

                while (Socket.IsConnected)
                {
                    var Reader = new InputStreamReader(Socket.InputStream);
                    var Buffer = new BufferedReader(Reader);


                    if (Buffer.Ready())
                    {
                        Barcode = await Buffer.ReadLineAsync();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            finally
            {
                if (Socket != null)
                {
                    Socket.Close();
                }

                Device = null;
                Adapter = null;
            }
        }

        public void Cancel()
        {
            if (_cancelToken != null) { 
                _cancelToken.Cancel();
            }
        }

        public ObservableCollection<string> PairedDevices()
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            ObservableCollection<string> devices = new ObservableCollection<string>();

            foreach (var bd in adapter.BondedDevices)
                devices.Add(bd.Name);

            return devices;
        }
    }
}