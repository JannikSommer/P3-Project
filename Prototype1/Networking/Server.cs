using System.Net.Sockets;
using System.Net;
using System.Text;
using Model;
using Newtonsoft.Json;
using StatusController;
using System.Collections.Generic;

namespace Networking
{
    public class Server
    {
        private Socket Handler;
        private string ip = "192.168.0.23";
        private Controller CycleController { get; set; }
        private Status StatusController { get; set; }
        private readonly int FlagMessageSize = 25;
        private readonly long MessageSize = 536870912; // 512 MB
        private readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public Server(Controller cycleController, Status statusController)
        {
            StatusController = statusController;
            CycleController = cycleController;
        }

        public void StartServer()
        {
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

        private void HandleConnection() // Handles the connection of the socket.
        {
            if (StatusController.IsInitialized == true) 
            {
                AcceptStatusUpload();
            }
            else
            {
                // Incoming data from the client
                byte[] bytes = new byte[FlagMessageSize];
                int bytesRec = Handler.Receive(bytes);
                string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);


                if (data == CommunicationFlag.PartitionRequest.ToString())
                {
                    Handler.Send(Encoding.UTF8.GetBytes(CommunicationHandler.Success.ToString()));

                    byte[] clinetBytes = new byte[1024]; //Size of a CentralController.Client
                    bytesRec = Handler.Receive(clinetBytes);
                    string Client = Encoding.UTF8.GetString(clinetBytes, 0, bytesRec);
                    Central_Controller.Client device = JsonConvert.DeserializeObject<Central_Controller.Client>(Client, Settings);

                    SendPartition(device);

                }
                else if (data == CommunicationFlag.PartitionUpload.ToString())
                {
                    AcceptPartitionUpload();
                }
                else if (data == CommunicationFlag.VerificationRequest.ToString())
                {
                    byte[] clinetBytes = new byte[MessageSize]; 
                    bytesRec = Handler.Receive(clinetBytes);
                    string Client = Encoding.UTF8.GetString(clinetBytes, 0, bytesRec);
                    Central_Controller.Client device = JsonConvert.DeserializeObject<Central_Controller.Client>(Client, Settings);

                    SendVerificationPartition(device);
                }
                else if (data == CommunicationFlag.VerificationUpload.ToString())
                {
                    AcceptVerificationPartitionUpload();
                }
                else
                {
                    CommunicationError();
                }
            }
        }

        private void AcceptStatusUpload()
        {
            byte[] bytes = new byte[MessageSize];
            int bytesRec = Handler.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            List<LocationBarcode> locationBacodes = JsonConvert.DeserializeObject<List<LocationBarcode>>(data, Settings);
            StatusController.UpdateCountedLocations(locationBacodes);
            Handler.Send(Encoding.UTF8.GetBytes(CommunicationFlag.ConversationCompleted.ToString()));
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
            Partition uploadedPartition = JsonConvert.DeserializeObject<Partition>(data, Settings);
            CycleController.CheckPartition(uploadedPartition);
            // Signal OK to client and shutdown socket
            Handler.Send(Encoding.UTF8.GetBytes(CommunicationFlag.ConversationCompleted.ToString()));
        }

        private void SendPartition(Central_Controller.Client client)
        {
            Partition partition = CycleController.NextPartition(client);
            // partition.Locations.Add(new Location("001A01")); 
            // Send partition to client
            string json = JsonConvert.SerializeObject(partition, Settings);
            Handler.Send(Encoding.UTF8.GetBytes(json));

            // Recieve callback
            byte[] bytes = new byte[FlagMessageSize];
            int bytesRec = Handler.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (!(data == CommunicationFlag.ConversationCompleted.ToString())) {
                // TODO: DO NOT MARK PARTITION AS InProgress
            }
        }

        private void SendVerificationPartition(Central_Controller.Client client)
        {
            VerificationPartition verificationPartition = CycleController.CreateVerificationPartition(client);
            // Send partition to client
            string json = JsonConvert.SerializeObject(verificationPartition, Settings);
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
            VerificationPartition verificationPartition = JsonConvert.DeserializeObject<VerificationPartition>(data, Settings);
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
