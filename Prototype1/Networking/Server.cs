using System.Net.Sockets;
using System.Net;
using System.Text;
using Model;
using Newtonsoft.Json;
using Central_Controller.Central_Controller;

namespace Networking
{
    public class Server {
        public Server(Controller controller) {
            Controller = controller;
        }

        private Controller Controller { get; set; }

        private string ip = "192.168.1.81";
        private readonly int FlagMessageSize = 25;
        private readonly long MessageSize = 536870912; // 512 MB
        private Socket Handler;
        private readonly JsonSerializerSettings settings = new JsonSerializerSettings {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };


        public void StartServer() {
            // Get Host IP Address that is used to establish a connection
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1
            // If a host has multiple addresses, you will get a list of addresses
            // Get IP-Address from cmd -> ipconfig IPv4 address from Ethernet adapter.
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 6969);

            // Create a Socket that will use Tcp protocol
            Socket Listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // A Socket must be associated with an endpoint using the Bind method
            Listener.Bind(localEndPoint);

            // Specify how many requests a Socket can listen before it gives Server busy response
            Listener.Listen(15); // Specified wish from StreetAmmo. A total number of 15 people can be

            while (true)
            {
            Handler = Listener.Accept();
            HandleConnection();
            }
        }

        // Handles the connection of the socket.
        private void HandleConnection() {
            // Incoming data from the client
            byte[] bytes = new byte[FlagMessageSize];
            int bytesRec = Handler.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);


            if (data == CommunicationFlag.PartitionRequest.ToString()) {
                Handler.Send(Encoding.UTF8.GetBytes(CommunicationHandler.Success.ToString()));

                byte[] clinetBytes = new byte[1024]; //Size of a CentralController.Client
                bytesRec = Handler.Receive(clinetBytes);
                string Client = Encoding.UTF8.GetString(clinetBytes, 0, bytesRec);
                User device = JsonConvert.DeserializeObject<User>(Client, settings);

                SendPartition(device);

            } else if (data == CommunicationFlag.PartitionUpload.ToString()) {
                AcceptPartitionUpload();
            } else if(data == CommunicationFlag.VerificationRequest.ToString()) {
                byte[] clinetBytes = new byte[1024]; //Size of a CentralController.Client
                bytesRec = Handler.Receive(clinetBytes);
                string Client = Encoding.UTF8.GetString(clinetBytes, 0, bytesRec);
                User device = JsonConvert.DeserializeObject<User>(Client, settings);

                SendVerificationPartition(device);
            } else if(data == CommunicationFlag.VerificationUpload.ToString()) {
                AcceptVerificationPartitionUpload();
            } else {
                CommunicationError();
            }
        }


        private void CommunicationError() {
            Handler.Send(Encoding.ASCII.GetBytes(CommunicationHandler.Error.ToString()));
        }

        private void AcceptPartitionUpload() {
            // Send permision to upload
            Handler.Send(Encoding.UTF8.GetBytes(CommunicationHandler.Accept.ToString()));
            byte[] bytes = new byte[MessageSize];

            // Accept data from client
            int bytesRec = Handler.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            Partition uploadedPartition = JsonConvert.DeserializeObject<Partition>(data, settings);
            Controller.CheckPartition(uploadedPartition);

            // Signal OK to client and shutdown socket
            Handler.Send(Encoding.UTF8.GetBytes(CommunicationFlag.ConversationCompleted.ToString()));
        }

        private void SendPartition(User user) {
            Partition partition = Controller.NextPartition(user);
            // TODO: partition.Locations.Add(new Location("001A01")); 
            // Send partition to client
            string json = JsonConvert.SerializeObject(partition, settings);
            Handler.Send(Encoding.UTF8.GetBytes(json));

            // Recieve callback
            byte[] bytes = new byte[FlagMessageSize];
            int bytesRec = Handler.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (!(data == CommunicationFlag.ConversationCompleted.ToString())) {
                // TODO: DO NOT MARK PARTITION AS InProgress
            }
        }

        private void SendVerificationPartition(User user) {
            VerificationPartition verificationPartition = Controller.CreateVerificationPartition(user);
            // Send partition to client
            string json = JsonConvert.SerializeObject(verificationPartition, settings);
            Handler.Send(Encoding.UTF8.GetBytes(json));

            // Recieve callback
            byte[] bytes = new byte[FlagMessageSize];
            int bytesRec = Handler.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (data != CommunicationFlag.ConversationCompleted.ToString()) {
                // DO NOT MARK PARTITION AS InProgress
            }
        }

        private void AcceptVerificationPartitionUpload() {
            // Send permision to upload
            Handler.Send(Encoding.UTF8.GetBytes(CommunicationHandler.Accept.ToString()));
            byte[] bytes = new byte[MessageSize];

            // Accept data from client
            int bytesRec = Handler.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            VerificationPartition verificationPartition = JsonConvert.DeserializeObject<VerificationPartition>(data, settings);
            // call method to handle data from uploadedPartition

            // Signal OK to client and shutdown socket
            Handler.Send(Encoding.UTF8.GetBytes(CommunicationFlag.ConversationCompleted.ToString()));
        }

        public void ShutdownServer() {
            Handler.Shutdown(SocketShutdown.Both);
            Handler.Close();
        }
    }
}
