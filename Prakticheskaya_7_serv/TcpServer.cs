using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace Prakticheskaya_7_serv
{
    class TcpServer
    {
        private ChatOwnerWindow _chatOwnerWindow;

        public Socket socket;
        private List<Socket> clients = new List<Socket>();
        public List<string> logs = new List<string>();
        private List<string> NameUsers = new List<string>();

        public TcpServer(ChatOwnerWindow chatOwnerWindow, string ownername)
        {
            _chatOwnerWindow = chatOwnerWindow;

            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8888);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(ipPoint);
                socket.Listen(1000);
                ListenToClients();
                NameUsers.Add(ownername);
                logs.Add($"[{DateTime.Now.ToString("g")}] Создан сервер: [{ownername}]");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании сервера: " + ex.Message);
                _chatOwnerWindow.Close();
            }
        }

        private async Task ListenToClients()
        {
            while (true)
            {
                var client = await socket.AcceptAsync();
                clients.Add(client);
                ReceiveMessage(client);
            }
        }

        private async Task ReceiveMessage(Socket client)
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                int bytesRead = await client.ReceiveAsync(bytes, SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes, 0, bytesRead);

                if (message.StartsWith("CONNECT_USER:"))
                {
                    string newuser = $"[{message.Substring(14)}]";
                    NameUsers.Add(message.Substring(14));
                    _chatOwnerWindow.UsersListBox.Items.Add(newuser);
                    logs.Add($"[{DateTime.Now.ToString("g")}] Новый юзер: {newuser}");
                    foreach (var item in clients)
                    {
                        foreach (var nameuser in NameUsers)
                        {
                            string username = "CONNECT_USER: " + nameuser;
                            await Task.Delay(10);
                            SendMessage(item, username);
                        }
                    }
                }
                else if (message.StartsWith("DISCONNECT_USER: "))
                {
                    string ValueToRemove = message.Substring(17);
                    int indexSocketToDelete = NameUsers.IndexOf(ValueToRemove);
                    clients.RemoveAt(indexSocketToDelete);
                    NameUsers.Remove(ValueToRemove);
                    _chatOwnerWindow.UsersListBox.Items.Remove($"[{ValueToRemove}]");
                    logs.Add($"[{DateTime.Now.ToString("g")}] User disconnected: [{ValueToRemove}]");
                    foreach (var item in clients)
                    {
                        SendMessage(item, message);
                    }
                }
                else
                {
                    _chatOwnerWindow.MessagesListBox.Items.Add(message);
                    foreach (var item in clients)
                    {
                        SendMessage(item, message);
                    }
                }
            }
        }

        private async Task SendMessage(Socket client, string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(bytes, SocketFlags.None);
        }
    }
}
