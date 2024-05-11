using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace Prakticheskaya_7_serv
{
    class Clienttcpjostkiy
    {
        public Socket server;
        Client chatClientWindow;

        public Clienttcpjostkiy(string ip, Client chatClientWindow, string nameuser)
        {
            try
            {
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Connect(ip, 8888);
                this.chatClientWindow = chatClientWindow;
                SendMessage($"CONNECT_USER: {nameuser}");
                ReceiveMessage();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при подключении к серверу: " + ex.Message);
                chatClientWindow.Close();
            }
        }

        private async Task ReceiveMessage()
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                int bytesRead = await server.ReceiveAsync(bytes, SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes, 0, bytesRead);

                if (message.StartsWith("CONNECT_USER:"))
                {
                    string newuser = $"[{message.Substring(14)}]";
                    if (!chatClientWindow.UsersListBox.Items.Contains(newuser))
                    {
                        chatClientWindow.UsersListBox.Items.Add(newuser);
                    }
                }
                else if (message.StartsWith("DISCONNECT_USER:"))
                {

                }
                else
                {
                    chatClientWindow.MessagesListBox.Items.Add(message);
                }
            }
        }

        public async Task SendMessage(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await server.SendAsync(bytes, SocketFlags.None);
        }
    }
}
