using System;
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
        private Socket Sender;

        private byte[] FlagMessasge = new byte[25]; // Fits longest CommunicationFlag with some change 

        private long MessageSize = 1048576; // 100 MB


        #region async methods

        public async Task<CommunicationHandler> UploadVerificationPartitionAsync(VerificationPartition verificationPartition)
        {
            CommunicationHandler socketHandler = await StartClientAsync();
            if (socketHandler != CommunicationHandler.Success)
            {
                // A socket error has eccoured
                ClientShutdown();
                return socketHandler;
            }
            FlagMessasge = Encoding.UTF8.GetBytes(CommunicationFlag.VerificationUpload.ToString());
            // Send signal to upload
            int bytesSent = Sender.Send(FlagMessasge);

            // Receive confirmation to upload
            byte[] bytes = new byte[15]; // Fits longest CommunicationHandler with some change
            int bytesRec = Sender.Receive(bytes);
            string serverFlag = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (serverFlag == CommunicationHandler.Accept.ToString())
            {
                Sender.Send(SerializeDataForTransfer(verificationPartition));
            }
            else
            {
                return CommunicationHandler.Error;
            }

            // Receive signal to dispose data and close socket
            bytesRec = Sender.Receive(bytes);
            serverFlag = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (serverFlag == CommunicationFlag.ConversationCompleted.ToString())
            {
                ClientShutdown();
            }
            else
            {
                return CommunicationHandler.Error;
            }
            return CommunicationHandler.Success;
        }

        public async Task<Tuple<VerificationPartition, CommunicationHandler>> DownloadVerificationPartitionasync()
        {
            FlagMessasge = Encoding.UTF8.GetBytes(CommunicationFlag.VerificationRequest.ToString());
            CommunicationHandler handler;
            CommunicationHandler socketHandler = await StartClientAsync();
            if (socketHandler != CommunicationHandler.Success)
            {
                // A socket error has eccoured
                handler = socketHandler;
                ClientShutdown();
                VerificationPartition emptyPartition = null;
                return Tuple.Create(emptyPartition, handler);
            }
            // Send signal to get partition
            int bytesSent = Sender.Send(FlagMessasge);

            // Incoming data from server
            byte[] bytes = new byte[MessageSize]; // TODO: Make size fit. Is 1 MB now
            int bytesRec = Sender.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (data == CommunicationHandler.Error.ToString())
            {
                // Checks if the request has been received and understood by server. 
                handler = CommunicationHandler.Error;
                ClientShutdown();
                VerificationPartition emptyPartition = null;
                return Tuple.Create(emptyPartition, handler);
            }
            else handler = CommunicationHandler.Success;

            VerificationPartition partition = DeserializeDataAsVerificationPartition(bytes, bytesRec);

            // Respons to server to close connection
            Sender.Send(Encoding.UTF8.GetBytes(CommunicationFlag.ConversationCompleted.ToString()));

            ClientShutdown();
            return Tuple.Create(partition, handler);
        }
        
        public async Task<CommunicationHandler> UploadPartitionAsync(Partition partition)
        {
            CommunicationHandler socketHandler = await StartClientAsync();
            if (socketHandler != CommunicationHandler.Success)
            {
                // A socket error has eccoured
                ClientShutdown();
                return socketHandler;
            }
            FlagMessasge = Encoding.UTF8.GetBytes(CommunicationFlag.PartitionUpload.ToString());
            // Send signal to upload
            int bytesSent = Sender.Send(FlagMessasge);

            // Receive confirmation to upload
            byte[] bytes = new byte[15]; // Fits longest CommunicationHandler with some change
            int bytesRec = Sender.Receive(bytes);
            string serverFlag = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (serverFlag == CommunicationHandler.Accept.ToString())
            {
                Sender.Send(SerializeDataForTransfer(partition));
            }
            else
            {
                return CommunicationHandler.Error;
            }

            // Receive signal to dispose data and close socket
            bytesRec = Sender.Receive(bytes);
            serverFlag = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (serverFlag == CommunicationFlag.ConversationCompleted.ToString())
            {
                ClientShutdown();
            }
            else
            {
                return CommunicationHandler.Error;
            }
            return CommunicationHandler.Success;
        }

        public async Task<Tuple<Partition, CommunicationHandler>> DownloadPartitionAsync(Central_Controller.Client client)
        {
            CommunicationHandler handler;
            CommunicationHandler socketHandler = await StartClientAsync();
            if (socketHandler != CommunicationHandler.Success)
            {
                // A socket error has eccoured
                handler = socketHandler;
                ClientShutdown();
                Partition emptyPartition = null;
                return Tuple.Create(emptyPartition, handler);
            }
            // Send signal to get partition
            int bytesSent = Sender.Send(Encoding.UTF8.GetBytes(CommunicationFlag.PartitionRequest.ToString()));
            bytesSent = Sender.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(client)));

            // Incoming data from server
            byte[] bytes = new byte[MessageSize]; // TODO: Make size fit. Is 1 MB now
            int bytesRec = Sender.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (data == CommunicationHandler.Error.ToString())
            {
                // Checks if the request has been received and understood by server. 
                handler = CommunicationHandler.Error;
                ClientShutdown();
                Partition emptyPartition = null;
                return Tuple.Create(emptyPartition, handler);
            }
            else handler = CommunicationHandler.Success;

            Partition partition = DeserializeDataAsPartition(bytes, bytesRec);

            // Respons to server to close connection
            Sender.Send(Encoding.UTF8.GetBytes(CommunicationFlag.ConversationCompleted.ToString()));

            ClientShutdown();
            return Tuple.Create(partition, handler);
        }

        private async Task<CommunicationHandler> StartClientAsync()
        {
            try
            {
                // Connect to a Remote server  
                // Get Host IP Address that is used to establish a connection  
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
                // If a host has multiple addresses, you will get a list of addresses  
                IPHostEntry host = Dns.GetHostEntry("192.168.1.2");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 6969);

                // Create a TCP/IP socket
                Sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.    
                try
                {
                    // Connect to Remote EndPoint  
                    await Sender.ConnectAsync(remoteEP);
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

        public CommunicationHandler UploadVerificationPartition(VerificationPartition verificationPartition)
        {
            CommunicationHandler socketHandler = StartClient();
            if (socketHandler != CommunicationHandler.Success)
            {
                // A socket error has eccoured
                ClientShutdown();
                return socketHandler;
            }
            FlagMessasge = Encoding.UTF8.GetBytes(CommunicationFlag.VerificationUpload.ToString());
            // Send signal to upload
            int bytesSent = Sender.Send(FlagMessasge);

            // Receive confirmation to upload
            byte[] bytes = new byte[15]; // Fits longest CommunicationHandler with some change
            int bytesRec = Sender.Receive(bytes);
            string serverFlag = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (serverFlag == CommunicationHandler.Accept.ToString())
            {
                Sender.Send(SerializeDataForTransfer(verificationPartition));
            }
            else
            {
                return CommunicationHandler.Error;
            }

            // Receive signal to dispose data and close socket
            bytesRec = Sender.Receive(bytes);
            serverFlag += Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (serverFlag == CommunicationFlag.ConversationCompleted.ToString())
            {
                ClientShutdown();
            }
            else
            {
                return CommunicationHandler.Error;
            }

            return CommunicationHandler.Success;
        }

        public Tuple<VerificationPartition, CommunicationHandler> DownloadVerificationPartition()
        {
            FlagMessasge = Encoding.UTF8.GetBytes(CommunicationFlag.VerificationRequest.ToString());
            CommunicationHandler handler;
            CommunicationHandler socketHandler = StartClient();
            if (socketHandler != CommunicationHandler.Success)
            {
                // A socket error has eccoured
                handler = socketHandler;
                ClientShutdown();
                VerificationPartition emptyPartition = null;
                return Tuple.Create(emptyPartition, handler);
            }
            // Send signal to get partition
            int bytesSent = Sender.Send(FlagMessasge);

            // Incoming data from server
            byte[] bytes = new byte[MessageSize]; // TODO: Make size fit
            int bytesRec = Sender.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (data == CommunicationHandler.Error.ToString())
            {
                // Checks if the request has been received and understood by server. 
                handler = CommunicationHandler.Error;
                ClientShutdown();
                VerificationPartition emptyPartition = null;
                return Tuple.Create(emptyPartition, handler);
            }
            else handler = CommunicationHandler.Success;

            VerificationPartition partition = DeserializeDataAsVerificationPartition(bytes, bytesRec);

            // Respons to server to close connection
            Sender.Send(Encoding.UTF8.GetBytes(CommunicationFlag.ConversationCompleted.ToString()));

            ClientShutdown();
            return Tuple.Create(partition, handler);
        }

        public CommunicationHandler UploadPartition(Partition partition)
        {
            CommunicationHandler socketHandler = StartClient();
            if (socketHandler != CommunicationHandler.Success)
            {
                // A socket error has eccoured
                ClientShutdown();
                return socketHandler;
            }
            FlagMessasge = Encoding.UTF8.GetBytes(CommunicationFlag.PartitionUpload.ToString());
            // Send signal to upload
            int bytesSent = Sender.Send(FlagMessasge);

            // Receive confirmation to upload
            byte[] bytes = new byte[15]; // Fits longest CommunicationHandler with some change
            string serverFlag = null;
            int bytesRec = Sender.Receive(bytes);
            serverFlag += Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (serverFlag == CommunicationHandler.Accept.ToString())
            {
                Sender.Send(SerializeDataForTransfer(partition));
            }
            else
            {
                return CommunicationHandler.Error;
            }

            // Receive signal to dispose data and close socket
            bytesRec = Sender.Receive(bytes);
            serverFlag += Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (serverFlag == CommunicationFlag.ConversationCompleted.ToString())
            {
                ClientShutdown();
            }
            else
            {
                return CommunicationHandler.Error;
            }

            return CommunicationHandler.Success;
        }

        public Tuple<Partition, CommunicationHandler> DownloadPartition(Central_Controller.Client client)
        {
            CommunicationHandler handler;
            CommunicationHandler socketHandler = StartClient();
            if (socketHandler != CommunicationHandler.Success)
            {
                // A socket error has eccoured
                handler = socketHandler;
                ClientShutdown();
                Partition emptyPartition = null;
                return Tuple.Create(emptyPartition, handler);
            }
            // Send signal to get partition
            int bytesSent = Sender.Send(Encoding.UTF8.GetBytes(CommunicationFlag.PartitionRequest.ToString()));
            bytesSent = Sender.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(client)));

            // Incoming data from server
            byte[] bytes = new byte[MessageSize]; // TODO: Make size fit
            int bytesRec = Sender.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (data == CommunicationHandler.Error.ToString())
            {
                // Checks if the request has been received and understood by server. 
                handler = CommunicationHandler.Error;
                ClientShutdown();
                Partition emptyPartition = null;
                return Tuple.Create(emptyPartition, handler);
            }
            else handler = CommunicationHandler.Success;

            Partition partition = DeserializeDataAsPartition(bytes, bytesRec);
            
            // Respons to server to close connection
            Sender.Send(Encoding.UTF8.GetBytes(CommunicationFlag.ConversationCompleted.ToString()));

            ClientShutdown();
            return Tuple.Create(partition, handler);
        }

        private CommunicationHandler StartClient()
        {
            try
            {
                // Connect to a Remote server  
                // Get Host IP Address that is used to establish a connection  
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
                // If a host has multiple addresses, you will get a list of addresses  
                IPHostEntry host = Dns.GetHostEntry("192.168.1.2");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 6969);

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

        private byte[] SerializeDataForTransfer(Object partition)
        {
            string json = JsonSerializer.Serialize(partition);
            return Encoding.UTF8.GetBytes(json);
        }

        private Partition DeserializeDataAsPartition(byte[] bytes, int bytesRec)
        {
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            return JsonSerializer.Deserialize<Partition>(data);
        }

        private VerificationPartition DeserializeDataAsVerificationPartition(byte[] bytes, int bytesRec)
        {
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            return JsonSerializer.Deserialize<VerificationPartition>(data);
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