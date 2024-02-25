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

        private static readonly object _lock = new object();

        public static void StartServer(int serverPort)
        {
            _listener = new TcpListener(IPAddress.Any, serverPort);
            _listener.Start();

            Thread thread = new Thread(ListenForClients);
            thread.IsBackground = true;
            thread.Start();
        }

        private static void ListenForClients()
        {
            while (true)
            {
                _client = _listener.AcceptTcpClient();
                NewClientConnection?.Invoke();
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

        public static void BroadCast(BackupJobInfo backupJobInfo)
        {
            if (_client != null)
            {
                try
                {
                    NetworkStream stream = _client.GetStream();

                    string serializedData = JsonSerializer.Serialize(backupJobInfo);
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
