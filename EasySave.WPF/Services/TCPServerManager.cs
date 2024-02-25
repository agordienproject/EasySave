using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EasySave.Models;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using EasySave.Domain.Models;
using System.ComponentModel;

namespace EasySave.Services
{
    public class TCPServerManager
    {
        private static TcpListener _listener;

        private static TcpClient _client;

        public static event Action NewClientConnection;
        public static event Action<Guid> ExecuteBackupJobEvent;
        public static event Action<Guid> StopBackupJobExecutionEvent;

        public static void StartServer(int serverPort)
        {
            _listener = new TcpListener(IPAddress.Any, serverPort);
            _listener.Start();

            Thread newClientListenerThread = new Thread(ListenForClients);
            newClientListenerThread.IsBackground = true;
            newClientListenerThread.Start();

            Thread receivingThread = new Thread(StartReceiving);
            receivingThread.IsBackground = true;
            receivingThread.Start();
        }

        private static void ListenForClients()
        {
            while (true)
            {
                _client = _listener.AcceptTcpClient();
                NewClientConnection?.Invoke();
            }
        }

        private static void StartReceiving()
        {
            byte[] buffer = new byte[1024];
            StringBuilder receivedData = new StringBuilder();
            
            while (true)
            {
                if (_client != null)
                {
                    try
                    {
                        Stream stream = _client.GetStream();
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);

                        if (bytesRead > 0)
                        {
                            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            receivedData.Append(receivedMessage);

                            string[] splitMessage =  receivedData.ToString().Split(' ');
                            string command = splitMessage[0];
                            Guid guid = Guid.Parse(splitMessage[1]);

                            switch (command)
                            {
                                case "execute":
                                    ExecuteBackupJobEvent.Invoke(guid);
                                    break;
                                case "stop":
                                    StopBackupJobExecutionEvent.Invoke(guid);
                                    break;
                                default:
                                    break;
                            }
                            receivedData.Clear();
                        }
                    }
                    catch (Exception)
                    {
                    
                    }
                }
            }
        }

        public static void BroadCast(List<BackupJobInfo> backupJobs)
        {
            if (_client != null)
            {
                try
                {
                    NetworkStream stream = _client.GetStream();

                    string serializedData = JsonSerializer.Serialize(backupJobs);
                    serializedData = "<SOF>" + serializedData + "<EOF>";

                    byte[] msg = Encoding.UTF8.GetBytes(serializedData);
                    stream.Write(msg, 0, msg.Length);
                }
                catch (Exception ex)
                {

                }
            }
        }

    }
}
