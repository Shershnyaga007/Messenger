using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Server
    {
        static IPEndPoint ServerIP;
        static string ServerPass = null;
        static int MaxUsers;

        static Socket ServerSocket;
        List<UserBase> Users = new List<UserBase>();

        public Server(IPEndPoint serverIP, string serverPass, int maxUsers) 
        {
            ServerIP = serverIP;
            ServerPass = serverPass;
            MaxUsers = maxUsers;
        }

        // Enabling Server.
        public void InitServer()
        {
            Console.Clear();
            Console.WriteLine("Starting Server...");

            ServerSocket = new Socket(ServerIP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(ServerIP);

            ServerSocket.Listen(MaxUsers);
            Console.Clear();

            Console.WriteLine($"{ServerIP.Address}:{ServerIP.Port}");

            // Enable new user detection, and Listening Server messages.
            Task AcceptClients = StartUserUpdate();
            Task messages = SendServerMessage();

            messages.Wait();
        }

        async Task StartUserUpdate()
        {
            await Task.Run(() => UserUpdate());
        }

        // Listening Server messages.
        async public Task SendServerMessage()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    string message = Console.ReadLine();

                    message = $"Server: {message}";

                    SendMessageToAll(message);
                }
            });
        }

        public void SendMessageToAll(string Message)
        {
            byte[] codedMessage = Encoding.UTF8.GetBytes(Message);

            Console.WriteLine(Message);

            for (int i = 0; i < Users.Count; i++)
            {
                try
                {
                    Users[i].ClientSocket.Send(codedMessage);
                }
                catch (Exception e)
                {
                    string username = Users[i].Username;
                    Users.RemoveAt(i);

                    SendMessageToAll($"{username} leave from the Chat.");
                }
            }
        }

        async Task AcceptMessages(UserBase user)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        byte[] messageReceived = new byte[1024];

                        int byteRecv = user.ClientSocket.Receive(messageReceived);

                        string message = Encoding.UTF8.GetString(messageReceived,
                            0, byteRecv);

                        message = $"{user.Username}: {message}";

                        SendMessageToAll(message);
                    }
                    catch
                    {
                        Users.Remove(user);
                        SendMessageToAll($"{user.Username} leave from the Chat.");
                        throw new TaskCanceledException();
                    }
                }
            });
        }

        // Accept new user connection
        void UserUpdate()
        {
            while (true)
            {
                try
                {
                    Random random = new Random();
                    int NewUserId = random.Next(10000, 99999);
                    Socket clientNew = ServerSocket.Accept();

                    byte[] messageReceived = new byte[1024];
                    int byteRecv = clientNew.Receive(messageReceived);
                    string Pass = Encoding.UTF8.GetString(messageReceived, 0, byteRecv);

                    if (Pass.Equals(ServerPass) || ServerPass.Equals(null))
                    {
                        byte[] pass = Encoding.UTF8.GetBytes("Pass");

                        clientNew.Send(pass);
                    }
                    else
                    {
                        Console.WriteLine($"Pass test {NewUserId} failure.");
                        byte[] pass = Encoding.UTF8.GetBytes("1");

                        clientNew.Send(pass);
                        return;
                    }
                    messageReceived = new byte[1024];

                    byteRecv = clientNew.Receive(messageReceived);

                    string Nickname = Encoding.UTF8.GetString(messageReceived,
                        0, byteRecv);

                    List<string> nicknames = new List<string>();

                    for (int i = 0; i < Users.Count; i++)
                        nicknames.Add(Users[i].Username);


                    if (!nicknames.Contains(Nickname))
                    {
                        UserBase user = new UserBase(Nickname, clientNew);
                        Users.Add(user);

                        byte[] pass = Encoding.UTF8.GetBytes("Pass");

                        clientNew.Send(pass);

                        Task MessagesUpdate = AcceptMessages(user);

                        SendMessageToAll($"User {Nickname} Connected!");
                    }
                    else
                    {
                        byte[] Nope = Encoding.UTF8.GetBytes("1");
                        Console.WriteLine($"Nickname {NewUserId} faulure.");
                        clientNew.Send(Nope);
                    }
                } catch (Exception e)
                {

                }
            }
        }
    }
}
