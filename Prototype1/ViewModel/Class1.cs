using System;
using System.Threading;
using Networking;
using Central_Controller;
using Model;
using WPF_PC;

namespace ViewModel
{
    public class Class1
    {
        public Thread NetworkingThread { get; set; }
        public Controller Controller { get; set; }
        public MainWindow MainWindow { get; set; }

        private void StartServer()
        {
            Server server = new Server();
            NetworkingThread = new Thread(new ThreadStart(server.StartServer));
            NetworkingThread.Start();
        }



    }
}
