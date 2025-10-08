using System;

namespace ChatApp.Server
{
    public class ServerView
    {
        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public string GetMessage()
        {
            Console.Write("Server: ");
            return Console.ReadLine();
        }
    }
}
