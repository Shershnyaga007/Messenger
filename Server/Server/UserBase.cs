using System.Net.Sockets;

namespace Server
{
    public class UserBase
    {
        public string Username;
        public Socket ClientSocket;

        public UserBase(string username, Socket clientSocket)
        {
            this.Username = username;
            this.ClientSocket = clientSocket;
        }
    }
}