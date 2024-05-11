using System.Windows;

namespace Prakticheskaya_7_serv
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateChatButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            if (!string.IsNullOrEmpty(username))
            {
                ChatOwnerWindow chatOwnerWindow = new ChatOwnerWindow(username);
                chatOwnerWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Введите имя пользователя.");
            }
        }

        private void ConnectChatButton_Click(object sender, RoutedEventArgs e)
        {
            string ipAddress = ipAddressTextBox.Text;
            string username = usernameTextBox.Text;
            if (!string.IsNullOrEmpty(ipAddress) && !string.IsNullOrEmpty(username))
            {
                //логика для клиента
            }
            else
            {
                MessageBox.Show("Введите IP-адрес чата и имя пользователя.");
            }
        }
    }
}