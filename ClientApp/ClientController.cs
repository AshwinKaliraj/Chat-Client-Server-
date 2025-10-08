using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatApp.Client
{
    public class ClientController
    {
        private ClientModel model;
        private ClientView view;
        private TcpClient client;
        private static readonly object consoleLock = new object();

        public ClientController(ClientModel m, ClientView v)
        {
            model = m;
            view = v;
        }

        public void StartClient(string hostname, int port)
        {
            client = new TcpClient();
            while (true)
            {
                try
                {
                    client.Connect(hostname, port);
                    break;
                }
                catch
                {
                    Console.WriteLine("Server not ready, retrying in 1 second...");
                    Thread.Sleep(1000);
                }
            }

            view.DisplayMessage("Connected to server!");
            NetworkStream stream = client.GetStream();

            // Thread to receive messages
            Thread receiveThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0) break;

                        model.Message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        lock (consoleLock)
                        {
                            Console.WriteLine($"\r\nServer: {model.Message}");
                            Console.Write("Client: ");
                        }
                    }
                    catch
                    {
                        break;
                    }
                }
            });
            receiveThread.IsBackground = true;
            receiveThread.Start();

            // Main thread to send messages
            while (true)
            {
                Console.Write("Client: ");
                string msg = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(msg)) continue;

                byte[] data = Encoding.UTF8.GetBytes(msg);
                stream.Write(data, 0, data.Length);
            }
        }
    }
}
