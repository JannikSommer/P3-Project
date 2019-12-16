using System;
using Networking;
using Model;
using Central_Controller;
using PrestaSharpAPI;
using System.Threading.Tasks;
using System.Threading;
using StatusController;


namespace MVC_Controller
{
    public class MainController
    {
        private Thread NetworkingThread { get; set; }
        private Server Server { get; set; }
        public Central_Controller.Controller Controller { get; set; }
        public StatusController.Status Status { get; set; }

        public MainController(Central_Controller.Controller controller)
        {
            Status = new StatusController.Status();
            Controller = controller;
        }

        public void InitializeStatus()
        {
            Status.StartStatus();
        }
        
        public void StartServer() 
        {
            Server = new Server(Controller, Status);
            NetworkingThread = new Thread(new ThreadStart(Server.StartServer));
            NetworkingThread.Start();
        }

        public void ServerShutdown() 
        {
            Server.ShutdownServer();
            NetworkingThread.Abort();
        }
    }
}