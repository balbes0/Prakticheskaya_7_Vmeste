using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace Prakticheskaya_7_serv
{
    public partial class ChatOwnerWindow : Window
    {
        private TcpServer tcpServer;
        private Socket clientserver;
        private string ownername;
        public ChatOwnerWindow(string ownername)
        {
            InitializeComponent();
            tcpServer = new TcpServer(this, ownername);
            UsersListBox.Items.Add($"[{ownername}]");
            this.ownername = ownername;

            clientserver = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientserver.Connect("127.0.0.1", 8888);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (messageTextBox.Text == "/disconnect")
            {
                Close();
            }
            else
            {
                string message = $"[{DateTime.Now.ToString("g")}] [{ownername}]: {messageTextBox.Text}";
                SendMessage(message);
            }
        }

        private async Task SendMessage(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await clientserver.SendAsync(bytes, SocketFlags.None);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CheckLogs checkLogs = new CheckLogs(tcpServer.logs);
            checkLogs.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SendMessage("Host disconnected :((");
            clientserver.Close();
            tcpServer.socket.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
