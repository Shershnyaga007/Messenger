using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{

    internal class Program
    {
        static IPEndPoint ServerIP;
        static string ServerPass = null;
        static int MaxUsers;

        public static void Main(string[] args)
        {
            Console.Write("Enter Server IP: ");
            string ip  = Console.ReadLine();

            IPAddress erverIP = IPAddress.Parse(ip);

            Console.Write("Enter Server Port: ");
            string port = Console.ReadLine();
            int serverPort = int.Parse(port);

            ServerIP = new IPEndPoint(erverIP, serverPort);

            Console.Write("Enter Server Password: ");
            ServerPass = Console.ReadLine();

            Console.Write("Enter Max Users: ");
            string maxUsers = Console.ReadLine();
            MaxUsers= int.Parse(maxUsers);

            Server server = new Server(ServerIP, ServerPass, MaxUsers);

            server.InitServer();
        }
    }
}
