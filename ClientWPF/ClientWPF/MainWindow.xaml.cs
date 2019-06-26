using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

namespace ClientWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _buttonIsStart;
        private TcpClient _tcpClient;
        private Thread _listenThread;
        private Thread _sendThread;
        public MainWindow()
        {
            InitializeComponent();

            _tcpClient = new TcpClient();
        }

        private void ConnectButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _tcpClient.Connect(IPAddress.Parse(ipTextBox.Text), int.Parse(portTextBox.Text));
                _listenThread = new Thread(ListenToServer);
                _listenThread.Start(_tcpClient);
                _buttonIsStart = !_buttonIsStart;
                connectButton.Content = "Stop";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void SendButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _sendThread = new Thread(SendMessage);
                _sendThread.Start(_tcpClient);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SendMessage(object obj)
        {
            TcpClient tcpServer = obj as TcpClient;
            string buffer = "";
            Dispatcher.Invoke(new ThreadStart(() => buffer = cityTextBox.Text));
            _tcpClient.Client.Send(Encoding.ASCII.GetBytes(buffer), SocketFlags.None);
        }

        private void ListenToServer(object obj)
        {
            TcpClient tcpServer = obj as TcpClient;
            byte[] buffer = new byte[4 * 1024];
            while (true)
            {
                var receivedSize = tcpServer.Client.Receive(buffer);
                WriteToLog(Encoding.UTF8.GetString(buffer, 0, receivedSize));
            }
        }

        private void WriteToLog(string text)
        {
            Dispatcher.Invoke(new ThreadStart(() => serverTextBox.AppendText(text + "\r\n")));
        }
    }
}
