using System;
using System.Net;
using System.Net.Sockets;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WpfApp1
{
    public class Client
    {

        private static IPEndPoint ServerEndPoint;

        private static string ServerPassword;

        private static Socket ClientSocket;
        private static string Nickname;
        private static bool IsStarted = false;
        private static Messenger MessengerWindow;

        public IPEndPoint getServerEndPoint()
        {
            return ServerEndPoint;
        }

        public Client(string ip, int port, string password, string nickname, Messenger messenger) 
        {
            ServerEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            ClientSocket = new Socket(ServerEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            MessengerWindow = messenger;
            Nickname = nickname;
            ServerPassword = password;

        }

        public bool Start()
        {
            try
            {
                ClientSocket.Connect(ServerEndPoint);
                IsStarted= true;
                return true;    
            } catch (Exception e)
            {
                return false;
            }
        }

        public bool SendPassword()
        {
            if (!IsStarted)
                return false;

            if (ServerPassword.Equals(null))
                ServerPassword = "asdaslkjhl12jkhlkj123lkjhsfdjlkh123hljasdhljasdhk123khjlsdf9123hlksdfhlkds1312312321!!!!";

            byte[] CodedStr = Encoding.UTF8.GetBytes(ServerPassword);
            Thread.Sleep(100);
            ClientSocket.Send(CodedStr);
            byte[] messageReceived = new byte[1024];
            int byteRecv = ClientSocket.Receive(messageReceived);
            string ServerMessage = Encoding.UTF8.GetString(messageReceived,
                    0, byteRecv);
            if (ServerMessage.Equals("Pass"))
            {
                return true;
            }
            else
            {
                ClientSocket.Dispose();
                IsStarted = false;
                return false;
            }
        }
        public bool SendUsername()
        {
            if (!IsStarted)
                return false;

            byte[] CodedStr = Encoding.UTF8.GetBytes(Nickname);
            ClientSocket.Send(CodedStr);

            byte[] messageReceived = new byte[1024];

            int byteRecv = ClientSocket.Receive(messageReceived);

            string ServerMessage = Encoding.UTF8.GetString(messageReceived,
                    0, byteRecv);
            if (ServerMessage.Equals("Pass"))
            {
                return true;
            }
            else
            {
                ClientSocket.Dispose();
                IsStarted = false;
                return false;
            }
        }

        public void SendMessage(string Message)
        {
            if (!IsStarted)
                return;

            byte[] CodedStr = Encoding.UTF8.GetBytes(Message);
            ClientSocket.Send(CodedStr);
        }

        async public Task StartUpdate()
        {
            await Task.Run(() => Update());
        }
        private void Update()
        {
            while (true) 
            {
                byte[] messageReceived = new byte[1024];

                int byteRecv = ClientSocket.Receive(messageReceived);

                string ServerMessage = Encoding.UTF8.GetString(messageReceived,
                        0, byteRecv);

                Application.Current.Dispatcher.Invoke(() => {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = ServerMessage;
                    MessengerWindow.Messages.Children.Add(textBlock);
                });
            }
        }
    }
}
