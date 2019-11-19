﻿using System;
using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;
using Model;

namespace Networking
{
    public enum CommunicationFlag
    {
        PartitionRequest, UploadRequest, ConversationCompleted
    }
    public enum CommunicationHandler
    {
        Accept, Decline, Error, Success, SocketError, UnknownError
    }
    

    public class Server
    {
        private Socket Handler { get; set; }

        public void StartServer()
        {
            // Get Host IP Address that is used to establish a connection  
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1
            // If a host has multiple addresses, you will get a list of addresses  
            // Get IP-Address from cmd -> ipconfig IPv4 address from Ethernet adapter. 
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 8080);

            try
            {
                // Create a Socket that will use Tcp protocol      
                Socket Listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // A Socket must be associated with an endpoint using the Bind method  
                Listener.Bind(localEndPoint);

                // Specify how many requests a Socket can listen before it gives Server busy response
                Listener.Listen(10);
                while (true) // Accepts connections untill the method ShutdownServer() is called
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
            string data = null;
            byte[] bytes = new byte[1024];
            int bytesRec = Handler.Receive(bytes);
            data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
            Partition partition = new Partition();
            
            if (data == CommunicationFlag.PartitionRequest.ToString())
                SendPartition(partition);
            else if (data == CommunicationFlag.UploadRequest.ToString())
                AcceptPartitionUpload();
            else
                CommunicationError();
        }

        private void CommunicationError()
        {
            Handler.Send(Encoding.ASCII.GetBytes(CommunicationHandler.Error.ToString()));
        }

        private void AcceptPartitionUpload()
        {
            // Send permision to upload
            Handler.Send(Encoding.UTF8.GetBytes(CommunicationHandler.Accept.ToString()));
            Partition uploadedPartition = new Partition();
            byte[] incommingBytes = new byte[1024];

            // Accept data from client
            int bytesRec = Handler.Receive(incommingBytes);
            string data = Encoding.UTF8.GetString(incommingBytes, 0, bytesRec);
            uploadedPartition = JsonSerializer.Deserialize<Partition>(data);

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
            byte[] bytes = null;

            bytes = new byte[1024];
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
