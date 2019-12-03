using System;
using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;
using Model;
using Central_Controller;
using System.IO;

namespace Networking
{
    public class Server
    {
        private Socket Handler;
        private Cycle Cycle = new Cycle();
        private long MessageSize = 1048576; // 100 MB
        private Controller Controller { get; set; }

        public Server(Controller controller)
        {
            Controller = controller;
        }


        public void StartServer()
        {
            // Get Host IP Address that is used to establish a connection  
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1
            // If a host has multiple addresses, you will get a list of addresses  
            // Get IP-Address from cmd -> ipconfig IPv4 address from Ethernet adapter. 
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = IPAddress.Parse("192.168.1.4");
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
            // Incoming data from the client
            byte[] bytes = new byte[15];
            int bytesRec = Handler.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            Console.WriteLine("Step 1 done");
            if (data == CommunicationFlag.PartitionRequest.ToString())
            {
                bytesRec = Handler.Receive(bytes);
                Console.WriteLine("Step 2 done");

                string Client = Encoding.UTF8.GetString(bytes, 0, bytesRec); // Does this work when the client sends 2 messages in a row?
                Console.WriteLine("Step 3 done");

                var client = JsonSerializer.Deserialize<Central_Controller.Client>(Client);
                Console.WriteLine("Step 4 done");

                SendPartition(Controller.NextPartition(client));
            }
            else if (data == CommunicationFlag.PartitionUpload.ToString()) 
            { 
                AcceptPartitionUpload();
            }
            else if (data == CommunicationFlag.VerificationRequest.ToString())
            {
                VerificationPartition verificationPartition = Controller.CreateVerificationPartition();
                SendVerificationPartition(verificationPartition);
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


        private void CommunicationError()
        {
            Handler.Send(Encoding.ASCII.GetBytes(CommunicationHandler.Error.ToString()));
        }

        private void AcceptPartitionUpload()
        {
            // Send permision to upload
            Handler.Send(Encoding.UTF8.GetBytes(CommunicationHandler.Accept.ToString()));
            byte[] bytes = new byte[MessageSize];

            // Accept data from client
            int bytesRec = Handler.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            Partition uploadedPartition = JsonSerializer.Deserialize<Partition>(data);

            // Signal OK to client and shutdown socket
            Handler.Send(Encoding.UTF8.GetBytes(CommunicationFlag.ConversationCompleted.ToString()));
            Cycle.ReceicePartitionUpload(uploadedPartition);
        }

        public void SendPartition(Partition partition)
        {
            // Send partition to client
            string json = JsonSerializer.Serialize(partition);
            Handler.Send(Encoding.UTF8.GetBytes(json));

            // Recieve callback
            string data = null;
            byte[] bytes = new byte[25];
            int bytesRec = Handler.Receive(bytes);
            data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (!(data == CommunicationFlag.ConversationCompleted.ToString()))
            {
                // DO NOT MARK PARTITION AS InProgress
            }
            ShutdownServer(); // remove after testing!
        }

        private void SendVerificationPartition(VerificationPartition verificationPartition)
        {
            // Send partition to client
            string json = JsonSerializer.Serialize(verificationPartition);
            Handler.Send(Encoding.UTF8.GetBytes(json));

            // Recieve callback
            string data = null;
            byte[] bytes = new byte[25];
            int bytesRec = Handler.Receive(bytes);
            data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (!(data == CommunicationFlag.ConversationCompleted.ToString()))
            {
                // DO NOT MARK PARTITION AS InProgress
            }
        }

        private void AcceptVerificationPartitionUpload()
        {
            // Send permision to upload
            Handler.Send(Encoding.UTF8.GetBytes(CommunicationHandler.Accept.ToString()));
            byte[] bytes = new byte[MessageSize];

            // Accept data from client
            int bytesRec = Handler.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            VerificationPartition verificationPartition = JsonSerializer.Deserialize<VerificationPartition>(data);
            // call method to handle data from uploadedPartition

            // Signal OK to client and shutdown socket
            Handler.Send(Encoding.UTF8.GetBytes(CommunicationFlag.ConversationCompleted.ToString()));
        }    

        public void ShutdownServer()
        {
            Handler.Shutdown(SocketShutdown.Both); 
            Handler.Close();
        }
    }
}
