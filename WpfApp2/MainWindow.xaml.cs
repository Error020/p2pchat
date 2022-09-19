using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Windows.Threading;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        Socket socket;
        EndPoint epLocal, epRemote;
        byte[] buffer;
        bool isConnected = false;

        public static void method1(Label label1)
        {
        }

        public MainWindow()
        {
            InitializeComponent();
            userip.Text += getLocalIp();
            chatbox.Items.Clear();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        public delegate void UpdateTextCallback(string message);

        public void DoSomething()
        {
            while (true)
            {
                // just to display that the client is updating content
                label1.Dispatcher.Invoke(() => {
                    if (label1.Content.ToString() == "updating")
                    {
                        label1.Content = "...";
                    }
                    else
                    {
                        label1.Content = "updating";
                    }
                });
                
                // update richtextbox and stuff

                Thread.Sleep(2000);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
        }

        private void richTextBox1_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            // Thread test = new Thread(new ThreadStart(TestThread));
            // test.Start();

            new Thread(DoSomething).Start();
        }

        private string getLocalIp()
        {
            //Grabs local ip of clients server
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }

        private void connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isConnected == false && IPAddress.TryParse(partnerip.Text, out IPAddress address) && username.Text != "" && int.TryParse(yourport.Text, out int clientPortCheck) && int.TryParse(partnerport.Text, out int friendsPortCheck))
                {
                    //Grab information
                    string localIp = getLocalIp();
                    string remoteIp = partnerip.Text;
                    int userPort = Convert.ToInt32(yourport.Text);
                    int friendPort = Convert.ToInt32(partnerport.Text);

                    //Setup socket
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    epLocal = new IPEndPoint(IPAddress.Parse(localIp), userPort);
                    socket.Bind(epLocal);

                    //Connects to remote ip
                    epRemote = new IPEndPoint(IPAddress.Parse(remoteIp), friendPort);
                    socket.Connect(epRemote);

                    //listen to specific port
                    buffer = new byte[1500];
                    socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);

                    //Updates connection flags
                    if (isConnected == true)
                    {
                        isConnected = false;
                    }
                    else if (isConnected == false)
                    {
                        isConnected = true;
                    }
                    connect.Content = "Disconnect";
                }
                else if (isConnected == true)
                {
                    //Convert string message to byte
                    ASCIIEncoding aEncoding = new ASCIIEncoding();
                    string disconnectMessage = "{1!ziFEl1.M@)d^d4n7qyRhGYwyZjCVl^#QKD(e)]x/96JB??ce#&xH_XO?5}&L " + username;
                    byte[] sendingMessage = new byte[1500];
                    sendingMessage = aEncoding.GetBytes(disconnectMessage);

                    //Sending the encoded message
                    socket.Send(sendingMessage);
                    chatbox.Items.Add("You have disconnected");

                    //Disconnects & diposes of socket
                    socket.Dispose();
                    connect.Content = "Connect";
                    isConnected = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void send()
        {
            if (clientmessage.Text != "" && socket.IsBound == true)
            {
                //Sends encoded message indicating friend has disconnected
                ASCIIEncoding aEncoding = new ASCIIEncoding();
                byte[] sendingMessage = new byte[1500];
                sendingMessage = aEncoding.GetBytes(username.Text + ": " + clientmessage.Text);
                socket.Send(sendingMessage);

                //Adding to the listbox
                chatbox.Items.Add("Me: " + clientmessage.Text);
                clientmessage.Text = "";
            }
        }

        private void clientmessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                send();
            }
        }

        private void MessageCallBack(IAsyncResult aResult)
        {
            //Async recursive function checking if any messages have been sent to the client
            try
            {
                //Recieves transmission and converts byte[] to string
                byte[] recievedData = new byte[1500];
                recievedData = (byte[])aResult.AsyncState;
                ASCIIEncoding aEncoding = new ASCIIEncoding();
                string recievedMessage = aEncoding.GetString(recievedData);
                int i = recievedMessage.IndexOf('\0');
                if (i >= 0) recievedMessage = recievedMessage.Substring(0, i);

                //Checks if clientDisconnect string
                if (!recievedMessage.Contains("{1!ziFEl1.M@)d^d4n7qyRhGYwyZjCVl^#QKD(e)]x/96JB??ce#&xH_XO?5}&L"))
                {
                    //If recieved message is present multi-thread will delegate a update to chatbox
                    chatbox.Dispatcher.Invoke(
                        DispatcherPriority.Normal,
                        new Action(
                            delegate ()
                            {
                                chatbox.Items.Add(recievedMessage);
                            }
                            )
                        );

                    //clear buffer & rescan
                    buffer = new byte[1500];
                    socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
                }
                else if (recievedMessage.Contains("{1!ziFEl1.M@)d^d4n7qyRhGYwyZjCVl^#QKD(e)]x/96JB??ce#&xH_XO?5}&L"))
                {
                    string nameOfFriend = recievedMessage.Replace("{1!ziFEl1.M@)d^d4n7qyRhGYwyZjCVl^#QKD(e)]x/96JB??ce#&xH_XO?5}&L ", "");

                    //Sends a disconnect message
                    chatbox.Dispatcher.Invoke(
                        DispatcherPriority.Normal,
                        new Action(
                            delegate ()
                            {
                                chatbox.Items.Add(nameOfFriend + " has disconnected");
                            }
                            )
                        );
                }

            }
            catch (Exception ex)
            {
                //If recieved message is present multi-thread will delegate a update to chatbox
                MessageBox.Show("Error: " + ex);
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
    }
}

