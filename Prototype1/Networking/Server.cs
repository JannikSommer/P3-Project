using System;
using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;
using Model;

namespace Networking
{
    public class Server
    {
        private Socket Handler;

        public void StartServer()
        {
            // Get Host IP Address that is used to establish a connection  
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1
            // If a host has multiple addresses, you will get a list of addresses  
            // Get IP-Address from cmd -> ipconfig IPv4 address from Ethernet adapter. 
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = IPAddress.Parse("192.168.0.23");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 8080);

            try
            {
                // Create a Socket that will use Tcp protocol      
                Socket Listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // A Socket must be associated with an endpoint using the Bind method  
                Listener.Bind(localEndPoint);

                // Specify how many requests a Socket can listen before it gives Server busy response
                Listener.Listen(10);
                while (true)
                {
                    Handler = Listener.Accept();
                    HandleConnection();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void HandleConnection() // Handles the connection of the socket. 
        {
            // Incoming data from the client
            byte[] bytes = new byte[15];
            int bytesRec = Handler.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            Partition partition = new Partition();
            
            if (data == CommunicationFlag.PartitionRequest.ToString())
            {
                SendPartition(partition); // get partition from somewhere else
            }
            else if (data == CommunicationFlag.UploadRequest.ToString())
            {
                AcceptPartitionUpload();
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
            byte[] bytes = new byte[1048576];

            // Accept data from client
            int bytesRec = Handler.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            Partition uploadedPartition = JsonSerializer.Deserialize<Partition>(data);
            // call method to handle data from uploadedPartition

            // Signal OK to client and shutdown socket
            Handler.Send(Encoding.UTF8.GetBytes(CommunicationFlag.ConversationCompleted.ToString()));
        }

        private void SendPartition(Partition partition)
        {
            // Send partition to client
            string json = JsonSerializer.Serialize<Partition>(partition);
            Handler.Send(Encoding.UTF8.GetBytes(json));

            // Recieve callback
            string data = null;
            byte[] bytes = new byte[25];
            int bytesRec = Handler.Receive(bytes);
            data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (!(data == CommunicationFlag.ConversationCompleted.ToString()))
            {
                // Handle error
            }
        }

        public void ShutdownServer()
        {
            Handler.Shutdown(SocketShutdown.Both); 
            Handler.Close();
        }
    }
}
