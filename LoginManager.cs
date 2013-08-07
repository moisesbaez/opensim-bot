using System;
using System.Text;
using System.Threading;
using OpenMetaverse;

namespace FirstBot
{
    static class LoginManager
    {
        private static string gameServer = "http://127.0.0.1:9000/";
        private static Vector3 botLocation = new Vector3(100, 127, 0);

        public static void Connect(string firstName, string lastName, string password, GridClient Client)
        {
            Client.Settings.LOGIN_SERVER = gameServer;
            string startLocation = NetworkManager.StartLocation("OpenSim", (int)botLocation.X, (int)botLocation.Y, (int)botLocation.Z);
            
            if (Client.Network.Login(firstName, lastName, password, "StegBot", startLocation, "1.0"))
            {
                Console.WriteLine("Bot has logged on!\n");
                Thread.Sleep(5000);
            }
            else
            {
                Console.WriteLine("Bot was unable to login: {0}", Client.Network.LoginMessage);
                Console.WriteLine("Press 'Enter' to exit...");
                Console.ReadLine();
            }
        }
    }
}
