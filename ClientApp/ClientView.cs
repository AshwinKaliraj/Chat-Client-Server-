using System;

namespace ChatApp.Client
{
    public class ClientView
    {
        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public string GetMessage()
        {
            Console.Write("Client: ");
            return Console.ReadLine();
        }
    }
}
