using Networking;
using Central_Controller;
using System.Threading;
using WPF_PC;


namespace MVC_Controller {
    public class Mainen
    {
        private Thread NetworkingThread { get; set; }
        private Thread UIThread { get; set; }
        private Controller DelegateController { get; set; } //  Rename
        private MainWindow MainWindow { get; set; }
        

        public void Main() {
            // This is where the good stuff happens.
        }


        private void UpdateUserInterface()
        {

        }


        private void StartServer() // Starts the server on another thread to listen for clients
        {
            Server Server = new Server();
            NetworkingThread = new Thread(new ThreadStart(Server.StartServer));
            NetworkingThread.Start();
        }

        private void StartUI()
        {
            MainWindow = new MainWindow();
        }
    }
}