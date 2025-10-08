using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatApp.Server
{
    public class ServerController
    {
        private ServerModel model;
        private ServerView view;
        private TcpListener listener;
        private static readonly object consoleLock = new object();

        public ServerController(ServerModel m, ServerView v)
        {
            model = m;
            view = v;
        }

        public void StartServer(int port)
        {
            listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();
            view.DisplayMessage($"Server started. Listening on port {port}...");

            TcpClient client = listener.AcceptTcpClient();
            view.DisplayMessage("Client connected!");
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
                            Console.WriteLine($"\r\nClient: {model.Message}");
                            Console.Write("Server: ");
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
                Console.Write("Server: ");
                string reply = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(reply)) continue;

                byte[] data = Encoding.UTF8.GetBytes(reply);
                stream.Write(data, 0, data.Length);
            }
        }
    }
}
