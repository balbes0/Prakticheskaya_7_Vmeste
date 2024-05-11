using System.Windows;


namespace Prakticheskaya_7_serv
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        Clienttcpjostkiy tcpClient;
        string clientname;
        public Client(string clientname, string ip)
        {
            InitializeComponent();
            tcpClient = new Clienttcpjostkiy(ip, this, clientname);
            this.clientname = clientname;
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (messageTextBox.Text == "/disconnect")
            {
                this.Close();
            }
            else
            {
                tcpClient.SendMessage($"[{DateTime.Now.ToString("g")}] [{clientname}]: {messageTextBox.Text}");
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string message = $"DISCONNECT_USER: {clientname}";
            tcpClient.SendMessage(message);
            tcpClient.server.Close();
            MainWindow main = new MainWindow();
            main.Show();
        }
    }
}
