using System.Windows;
using System.Windows.Input;
using СhatClient.ServiceСhat;

namespace СhatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IServiceChatCallback
    {
        bool isConnected = false;   // Проверка подключение к серверу
        ServiceChatClient client;   // Взаимодействие хоста с его методами
        int ID;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e) { }

        void ConnectUser()      // Подключение к сервису
        {
            if (!isConnected)
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                ID = client.Connect(tbUserName.Text);
                tbUserName.IsEnabled = false;
                bConnDicon.Content = "Disconnect";
                isConnected = true;
            }
        }
        void DisconnectUser()   // Отключение от сервиса
        {
            if (isConnected)
            {
                client.Disconnect(ID);
                client = null;
                tbUserName.IsEnabled = true;
                bConnDicon.Content = "Connect";
                isConnected = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
            }
        }

        public void MsgCallback(string msg) // Сообщение
        {
            lbChat.Items.Add(msg);          // Добавление сообщения в ListBox
            lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count - 1]);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) // Закрытия окна чата
        {
            DisconnectUser();
        }

        private void lbMessage_KeyDown(object sender, KeyEventArgs e)   // Нажатия кнопок
        {
            if(e.Key == Key.Enter)
            {
                if(client != null)
                {
                    client.SendMSG(lbMessage.Text, ID);
                    lbMessage.Text = string.Empty;
                }
            }
        }
    }
}
