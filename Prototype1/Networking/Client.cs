using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using Model;
using System.Threading.Tasks;

namespace Networking
{
    public class Client
    {
        private Socket Sender;
        private string ip = "192.168.1.81";
        private readonly int FlagMessageSize = 25;
        private readonly int HandlerSize = 15;
        private readonly long MessageSize = 536870912; // 512 MB
        private readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };


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
            // Send signal to upload
            Sender.Send(Encoding.UTF8.GetBytes(CommunicationFlag.VerificationUpload.ToString()));

            // Receive confirmation to upload
            byte[] bytes = new byte[HandlerSize]; // Fits longest CommunicationHandler with some change
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

        public async Task<Tuple<VerificationPartition, CommunicationHandler>> DownloadVerificationPartitionasync(Central_Controller.Client client)
        {
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
            Sender.Send(Encoding.UTF8.GetBytes(CommunicationFlag.PartitionRequest.ToString()));

            byte[] handlerBytes = new byte[MessageSize];
            int handlerBytesRec = Sender.Receive(handlerBytes);
            string serverResponse = Encoding.UTF8.GetString(handlerBytes, 0, handlerBytesRec);

            if (serverResponse != CommunicationHandler.Success.ToString())
            {
                handler = CommunicationHandler.Error;
                ClientShutdown();
                VerificationPartition emptyPartition = null;
                return Tuple.Create(emptyPartition, handler);
            }

            Sender.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(client, Settings)));

            // Incoming data from server
            byte[] bytes = new byte[MessageSize];
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
            // Send signal to upload
            Sender.Send(Encoding.UTF8.GetBytes(CommunicationFlag.PartitionUpload.ToString()));

            // Receive confirmation to upload
            byte[] bytes = new byte[HandlerSize]; // Fits longest CommunicationHandler with some change
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
            bytes = new byte[FlagMessageSize];
            // Receive signal to dispose data and close socket
            bytesRec = Sender.Receive(bytes);
            serverFlag = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (serverFlag == CommunicationFlag.ConversationCompleted.ToString())
            {
                ClientShutdown();
            }
            else
            {
                ClientShutdown();
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
            Sender.Send(Encoding.UTF8.GetBytes(CommunicationFlag.PartitionRequest.ToString()));

            byte[] handlerBytes = new byte[MessageSize]; 
            int handlerBytesRec = Sender.Receive(handlerBytes);
            string serverResponse = Encoding.UTF8.GetString(handlerBytes, 0, handlerBytesRec);

            if (serverResponse != CommunicationHandler.Success.ToString())
            {
                handler = CommunicationHandler.Error;
                ClientShutdown();
                Partition emptyPartition = null;
                return Tuple.Create(emptyPartition, handler);
            }

            Sender.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(client, Settings)));

            // Incoming data from server
            byte[] bytes = new byte[MessageSize]; 
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
                IPAddress ipAddress = IPAddress.Parse(ip);
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
            // Send signal to upload
            Sender.Send(Encoding.UTF8.GetBytes(CommunicationFlag.VerificationUpload.ToString()));

            // Receive confirmation to upload
            byte[] bytes = new byte[HandlerSize]; // Fits longest CommunicationHandler with some change
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
            bytes = new byte[FlagMessageSize];
            // Receive signal to dispose data and close socket
            bytesRec = Sender.Receive(bytes);
            serverFlag = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (serverFlag == CommunicationFlag.ConversationCompleted.ToString())
            {
                ClientShutdown();
            }
            else
            {
                ClientShutdown();
                return CommunicationHandler.Error;
            }
            return CommunicationHandler.Success;
        }

        public Tuple<VerificationPartition, CommunicationHandler> DownloadVerificationPartition(Central_Controller.Client client)
        {
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
            Sender.Send(Encoding.UTF8.GetBytes(CommunicationFlag.VerificationRequest.ToString()));

            byte[] handlerBytes = new byte[MessageSize];
            int handlerBytesRec = Sender.Receive(handlerBytes);
            string serverResponse = Encoding.UTF8.GetString(handlerBytes, 0, handlerBytesRec);

            if (serverResponse != CommunicationHandler.Success.ToString())
            {
                handler = CommunicationHandler.Error;
                ClientShutdown();
                VerificationPartition emptyPartition = null;
                return Tuple.Create(emptyPartition, handler);
            }

            Sender.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(client, Settings)));

            // Incoming data from server
            byte[] bytes = new byte[MessageSize];
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
            // Send signal to upload
            Sender.Send(Encoding.UTF8.GetBytes(CommunicationFlag.PartitionUpload.ToString()));

            // Receive confirmation to upload
            byte[] bytes = new byte[HandlerSize]; // Fits longest CommunicationHandler with some change
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
            bytes = new byte[FlagMessageSize];
            // Receive signal to dispose data and close socket
            bytesRec = Sender.Receive(bytes);
            serverFlag = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            if (serverFlag == CommunicationFlag.ConversationCompleted.ToString())
            {
                ClientShutdown();
            }
            else
            {
                ClientShutdown();
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
            Sender.Send(Encoding.UTF8.GetBytes(CommunicationFlag.PartitionRequest.ToString()));

            byte[] handlerBytes = new byte[MessageSize];
            int handlerBytesRec = Sender.Receive(handlerBytes);
            string serverResponse = Encoding.UTF8.GetString(handlerBytes, 0, handlerBytesRec);

            if (serverResponse != CommunicationHandler.Success.ToString())
            {
                handler = CommunicationHandler.Error;
                ClientShutdown();
                Partition emptyPartition = null;
                return Tuple.Create(emptyPartition, handler);
            }

            Sender.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(client, Settings)));

            // Incoming data from server
            byte[] bytes = new byte[MessageSize];
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
                IPAddress ipAddress = IPAddress.Parse(ip);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 6969);

                // Create a TCP/IP socket
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
            string json = JsonConvert.SerializeObject(partition, Settings);
            return Encoding.UTF8.GetBytes(json);
        }

        private Partition DeserializeDataAsPartition(byte[] bytes, int bytesRec)
        {
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            return JsonConvert.DeserializeObject<Partition>(data, Settings);
        }

        private VerificationPartition DeserializeDataAsVerificationPartition(byte[] bytes, int bytesRec)
        {
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            return JsonConvert.DeserializeObject<VerificationPartition>(data, Settings);
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