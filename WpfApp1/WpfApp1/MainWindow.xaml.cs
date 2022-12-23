using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Join_To_Server(object sender, RoutedEventArgs e)
        {
            string ip = ServerIp.Text;
            string nickname = ServerNickname.Text;
            string password = ServerPassword.Text;

            if (password == null)
                password = "1232HKLJAKLH!@#hkl123hkjl";

            if (int.TryParse(ServerPort.Text, out int Port)) {
                Messenger messenger = new Messenger();
                Client client = new Client(ip, Port, password, nickname, messenger);
                messenger.ServerClient = client;

                if (client.Start())
                {
                    if (client.SendPassword())
                    {
                        if (client.SendUsername())
                        {
                            Task task = client.StartUpdate();

                            messenger.Show();

                            Hide();
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Username aleready exists!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Password!");
                    }

                }
                else
                {
                    MessageBox.Show("Invalid Server!");
                }
            }
            else
            {
                MessageBox.Show("Invalid Port!");
            } 
        }
    }
}
