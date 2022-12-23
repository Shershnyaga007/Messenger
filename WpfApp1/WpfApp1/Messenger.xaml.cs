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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Messenger.xaml
    /// </summary>
    public partial class Messenger : Window
    {

        public Client ServerClient;

        public Messenger()
        {
            InitializeComponent();
            //this.ServerId.Text = $"Server {ServerClient.getServerEndPoint().Address}:{ServerClient.getServerEndPoint().Port}";
        }

        private void SendMessage(object sender, RoutedEventArgs e)
        {
            if (this.Message.Text.Equals(null))
                return;

            ServerClient.SendMessage(this.Message.Text);
            this.Message.Text = null;
        }
    }
}
