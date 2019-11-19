﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using Model;
using System.Threading.Tasks;

namespace Networking
{
    public class Client
    {
        private Socket Sender { get; set; }
        private CommunicationHandler CommunicationHandler { get; set; }

        private byte[] FlagMesasge = new byte[1024]; // TODO: Make size fit 

        #region async methods
        public async Task<CommunicationHandler> UploadPartitionAsync(Partition partition)
        {
            CommunicationHandler socketHandler = await StartClientAsync();
            if (socketHandler != CommunicationHandler.Success)
            {
                // A socket error has eccoured
                ClientShutdown();
                return socketHandler;
            }
            FlagMesasge = Encoding.UTF8.GetBytes(CommunicationFlag.UploadRequest.ToString());
            // Send signal to upload
            int bytesSent = Sender.Send(FlagMesasge);

            // Receive confirmation to upload
            byte[] bytes = new byte[64]; // TODO: Make size fit 
            string serverFlag = null;
            int bytesRec = Sender.Receive(bytes);
            serverFlag += Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (serverFlag == CommunicationHandler.Accept.ToString())
                Sender.Send(SerializeDataForTransfer(partition));
            else
                return CommunicationHandler.Error;

            // Receive signal to dispose data and close socket
            bytesRec = Sender.Receive(bytes);
            serverFlag += Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (serverFlag == CommunicationFlag.ConversationCompleted.ToString())
                ClientShutdown();
            else
                return CommunicationHandler.Error;

            return CommunicationHandler.Success;
        }

        public async Task<Partition> DownloadPartitionAsync()
        {
            FlagMesasge = Encoding.UTF8.GetBytes(CommunicationFlag.PartitionRequest.ToString());
            CommunicationHandler socketHandler = await StartClientAsync();
            if (socketHandler != CommunicationHandler.Success)
            {
                // A socket error has eccoured
                CommunicationHandler = socketHandler;
                ClientShutdown();
                return null;
            }
            // Send signal to get partition
            int bytesSent = Sender.Send(FlagMesasge);

            // Incoming data from server
            byte[] bytes = new byte[2048]; // TODO: Make size fit
            int bytesRec = Sender.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (data == CommunicationHandler.Error.ToString())
            {
                // Checks if the request has been received and understood by server. 
                CommunicationHandler = CommunicationHandler.Error;
                ClientShutdown();
                return null;
            }
            else CommunicationHandler = CommunicationHandler.Success;

            Partition partition = DeserializeDataFromTransfer(bytes, bytesRec);

            // Respons to server to close connection
            Sender.Send(Encoding.UTF8.GetBytes(CommunicationFlag.ConversationCompleted.ToString()));

            ClientShutdown();
            return partition;
        }

        private async Task<CommunicationHandler> StartClientAsync()
        {
            try
            {
                // Connect to a Remote server  
                // Get Host IP Address that is used to establish a connection  
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
                // If a host has multiple addresses, you will get a list of addresses  
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 8080);

                // Create a TCP/IP  socket.    
                Sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.    
                try
                {
                    // Connect to Remote EndPoint  
                    Sender.Connect(remoteEP);
                }
                catch (SocketException)
                {
                    return CommunicationHandler.SocketError;
                }
                catch (Exception)
                {
                    return CommunicationHandler.UnknownError;
                }
            }
            catch (Exception)
            {
                return CommunicationHandler.UnknownError;
            }
            return CommunicationHandler.Success;
        }

        #endregion

        #region sync methods 
        public CommunicationHandler UploadPartition(Partition partition)
        {
            CommunicationHandler socketHandler = StartClient();
            if (socketHandler != CommunicationHandler.Success)
            {
                // A socket error has eccoured
                ClientShutdown();
                return socketHandler;
            }
            FlagMesasge = Encoding.UTF8.GetBytes(CommunicationFlag.UploadRequest.ToString());
            // Send signal to upload
            int bytesSent = Sender.Send(FlagMesasge);

            // Receive confirmation to upload
            byte[] bytes = new byte[64]; // TODO: Make size fit 
            string serverFlag = null;
            int bytesRec = Sender.Receive(bytes);
            serverFlag += Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (serverFlag == CommunicationHandler.Accept.ToString())
                Sender.Send(SerializeDataForTransfer(partition));
            else
                return CommunicationHandler.Error;

            // Receive signal to dispose data and close socket
            bytesRec = Sender.Receive(bytes);
            serverFlag += Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (serverFlag == CommunicationFlag.ConversationCompleted.ToString())
                ClientShutdown();
            else
                return CommunicationHandler.Error;

            return CommunicationHandler.Success;
        }

        public Partition DownloadPartition(out CommunicationHandler handler)
        {
            FlagMesasge = Encoding.UTF8.GetBytes(CommunicationFlag.PartitionRequest.ToString());
            CommunicationHandler socketHandler = StartClient();
            if (socketHandler != CommunicationHandler.Success)
            {
                // A socket error has eccoured
                handler = socketHandler;
                ClientShutdown();
                return null;
            }
            // Send signal to get partition
            int bytesSent = Sender.Send(FlagMesasge);

            // Incoming data from server
            byte[] bytes = new byte[2048]; // TODO: Make size fit
            int bytesRec = Sender.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (data == CommunicationHandler.Error.ToString())
            {
                // Checks if the request has been received and understood by server. 
                handler = CommunicationHandler.Error;
                ClientShutdown();
                return null;
            }
            else handler = CommunicationHandler.Success;

            Partition partition = DeserializeDataFromTransfer(bytes, bytesRec);
            
            // Respons to server to close connection
            Sender.Send(Encoding.UTF8.GetBytes(CommunicationFlag.ConversationCompleted.ToString()));

            ClientShutdown();
            return partition;
        }

        private CommunicationHandler StartClient()
        {
            try
            {
                // Connect to a Remote server  
                // Get Host IP Address that is used to establish a connection  
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
                // If a host has multiple addresses, you will get a list of addresses  
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 8080);

                // Create a TCP/IP  socket.    
                Sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.    
                try
                {
                    // Connect to Remote EndPoint  
                    Sender.Connect(remoteEP);
                }
                catch (SocketException)
                {
                    return CommunicationHandler.SocketError;
                }
                catch (Exception)
                {
                    return CommunicationHandler.UnknownError;
                }
            }
            catch (Exception)
            {
                return CommunicationHandler.UnknownError;
            }
            return CommunicationHandler.Success;
        }

        private byte[] SerializeDataForTransfer(Partition partition)
        {
            string json = JsonSerializer.Serialize<Partition>(partition);
            return Encoding.UTF8.GetBytes(json);
        }

        private Partition DeserializeDataFromTransfer(byte[] bytes, int bytesRec)
        {
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            return JsonSerializer.Deserialize<Partition>(data);
        }

        private void ClientShutdown()
        {
            // Release the socket.    
            Sender.Shutdown(SocketShutdown.Both);
            Sender.Close();
        }
        #endregion
    }
}