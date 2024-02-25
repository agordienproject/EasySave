using EasySave.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleDeportee.Services
{
    public class TCPClientManager
    {
        private static TcpClient _tcpClient;
        private static NetworkStream _stream;

        public static event Action<List<BackupJobInfo>> BackupJobsListReceived;

        public static bool IsConnected => _tcpClient != null && _tcpClient.Connected;

        public static bool ConnectToServer(string serverIP, int port)
        {
            try
            {
                _tcpClient = new TcpClient();

                _tcpClient.Connect(serverIP, port);
                _stream = _tcpClient.GetStream();

                Thread thread = new Thread(StartReceiving);
                thread.IsBackground = true;
                thread.Start();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static void StartReceiving()
        {
            byte[] buffer = new byte[1024];
            StringBuilder receivedData = new StringBuilder();
            string startFlag = "<SOF>";
            string endFlag = "<EOF>";

            while (IsConnected)
            {
                try
                {
                    int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        receivedData.Append(receivedMessage);

                        string dataString = receivedData.ToString();
                        int startIndex = dataString.IndexOf(startFlag);
                        int endIndex = dataString.IndexOf(endFlag);

                        if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
                        {
                            // Extract the content between <SOF> and <EOF>
                            string content = dataString.Substring(startIndex + startFlag.Length, endIndex - startIndex - startFlag.Length);
                            string remainingContent = dataString.Substring(endIndex + endFlag.Length);

                            receivedData = new StringBuilder(remainingContent);

                            if (content.StartsWith("[") && content.EndsWith("]"))
                            {
                                List<BackupJobInfo> backupJobs = JsonSerializer.Deserialize<List<BackupJobInfo>>(content);
                                BackupJobsListReceived?.Invoke(backupJobs);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // Handle exceptions as needed
                }
            }
        }

        public static void SendMessage(string message)
        {
            if (!IsConnected) return;
            byte[] buffer = new byte[1024];
            _stream.Write(buffer, 0, buffer.Length);
        }

        public static void Disconnect()
        {
            if (IsConnected)
            {
                _tcpClient.Close();
                _tcpClient?.Dispose();
            }
        }
    }
}
