using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OpenMetaverse;
using OpenMetaverse.Assets;

namespace FirstBot
{
    class Program
    {
        private static GridClient Client = new GridClient();
        
        private static string firstName = "Botty";
        private static string lastName = "McBotterson";
        private static string password = "123456";

        static void Main(string[] args)
        {
            LoginManager.Connect(firstName, lastName, password, Client);

            Detection.FindObjects(Client);
            Detection.DetectSteg(Client);
            Extraction.ExtractSteg();

            Console.WriteLine("\nFinished Analyzing. Press enter to logout...");
            Console.ReadLine();
            Client.Network.Logout();
        }

    }
}
